using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class DungeonLayoutGenerator : MonoBehaviour
{
    // == Configuration ==
    [Header("Grid")]
    [SerializeField] private int _cols = 48;
    [SerializeField] private int _rows = 32;
    [SerializeField] private float _tilesize;

    [Header("Rooms")]
    [SerializeField] private int _targetRoomCount = 10;
    [SerializeField] private Vector2Int _roomSizeMin = new Vector2Int(4, 4);
    [SerializeField] private Vector2Int _roomSizeMax = new Vector2Int(8, 8);
    [SerializeField] private int _placementAttempts = 200;
    [SerializeField] private int _padding = 1;

    [Header("Prefabs")]
    [SerializeField] private GameObject _floorPrefab;
    [SerializeField] private GameObject _corridorPrefab;
    [SerializeField] private Transform _buildRoot;
    [SerializeField] private GameObject _downObjectPrefab;
    [SerializeField] private GameObject _upObjectPrefab;

    [Header("Randomness")]
    [SerializeField] private int _seed = 12345;

    // == Internal ==
    private System.Random _rng;
    private char[,] _grid; // 'R' is Room, 'C' is Corridor, '\0' is Empty

    private List<Room> _rooms = new List<Room>();

    // == Debugging ==
    private int _roomTileCount = 0;
    private int _corridorTileCount = 0;

    public char[,] Grid => _grid;
    public int Width => _cols;
    public int Height => _rows;
    public float TileSize => _tilesize;

    private struct Room
    {
        public RectInt Rect;
        public Vector2Int Center;

        public Room(RectInt rect)
        {
            this.Rect = rect;
            this.Center = new Vector2Int(rect.x + rect.width / 2, rect.y + rect.height / 2);
        }
    }

    [ContextMenu("Generate Dungeon")]
    public void Generate()
    {
        //Clear Previous Dungeon
        ClearPrevious();
        //Seed our Random
        _rng = new System.Random(_seed);
        //Set up our Grid
        _grid = new char[_cols, _rows];

        //Place The Rooms
        PlaceRooms();
        //Connect the Rooms
        ConnectRooms();
        //Build The Rooms
        RenderTiles();

        //Debug.Log($"Rooms to Corridors - {_roomTileCount}:{_corridorTileCount}");

        FindFirstObjectByType<DungeonDecorator3D>(FindObjectsInactive.Include).BuildDungeon3d();

        GenerateExits();
    }

    private void GenerateExits()
    {
        //var party = FindFirstObjectByType<PartyController>();
        Vector3 pos = new Vector3(_rooms[0].Center.x, 0, _rooms[0].Center.y);
        pos.x += _rooms[0].Rect.x;
        pos.z += _rooms[0].Rect.y;

        pos = EvenOutPosition(pos);
        DungeonManager.Instance.SetFloorEntrance(_seed, pos);

        /*if (party != null)
        {
            party.transform.position = pos;
        }*/

        Instantiate(_upObjectPrefab, pos, Quaternion.identity, _buildRoot);

        pos = new Vector3(_rooms[_rooms.Count - 1].Center.x, 0, _rooms[_rooms.Count - 1].Center.y);
        pos.x += _rooms[_rooms.Count - 1].Rect.x;
        pos.z += _rooms[_rooms.Count - 1].Rect.y;

        pos = EvenOutPosition(pos);

        Instantiate(_downObjectPrefab, pos, Quaternion.identity, _buildRoot);
        DungeonManager.Instance.SetFloorExit(_seed, pos);
    }

    private void ClearPrevious()
    {
        if (_buildRoot == null) return;

        _roomTileCount = 0;
        _corridorTileCount = 0;

        for (int i = _buildRoot.childCount - 1; i >= 0; i--)
        {
/*#if UNITY_EDITOR
            DestroyImmediate(_buildRoot.GetChild(i).gameObject);
#else 
*/            Destroy(_buildRoot.GetChild(i).gameObject);
            //Debug.Log($"Destroying Children in {_buildRoot.name}");
//#endif
        }
    }

    private void PlaceRooms()
    {
        _rooms.Clear();

        int placed = 0;
        for (int attempt = 0; attempt < _placementAttempts && placed < _targetRoomCount; attempt++)
        {
            int w = _rng.Next(_roomSizeMin.x, _roomSizeMax.x + 1);
            int h = _rng.Next(_roomSizeMin.y, _roomSizeMax.y + 1);

            int x = _rng.Next(1, _cols - w - 1);
            int y = _rng.Next(1, _rows - h - 1);

            var rect = new RectInt(x, y, w, h);

            if (OverlapsAny(rect, _padding))
                continue;

            _rooms.Add(new Room(rect));
            CarveRoom(rect);
            placed++;
        }
    }

    private bool OverlapsAny(RectInt candidate, int padding)
    {
        var padded = new RectInt(
            candidate.x - padding,
            candidate.y - padding,
            candidate.width + padding * 2,
            candidate.height + padding * 2
            );

        foreach (var r in _rooms)
        {
            if (padded.Overlaps(r.Rect))
                return true;
        }
        return false;
    }

    private void CarveRoom(RectInt rect)
    {
        for (int x = rect.x; x < rect.x + rect.width; x++)
            for (int y = rect.y; y < rect.y + rect.height; y++)
                if (InBounds(x, y))
                {
                    _grid[x, y] = 'R';
                    _roomTileCount++;
                }
    }

    private bool InBounds(int x, int y)
    {
        return x >= 0 && x < _cols && y >= 0 && y < _rows;
    }

    private void ConnectRooms()
    {
        if (_rooms.Count <= 1) return;

        //Picks a random Sort for the rooms
        SortRooms();

        for (int i = 0; i < _rooms.Count - 1; i++)
        {
            var a = _rooms[i].Center;
            var b = _rooms[i + 1].Center;

            CarveCorridorL(a, b);
        }
    }

    private void SortRooms()
    {
        int sortID = _rng.Next(0, 3);

        switch (sortID)
        {
            case 0:
                // Sort rooms by Center's X
                _rooms.Sort((a, b) => a.Center.x.CompareTo(b.Center.x));
                break;
            case 1:
                // Sort rooms by Center's Y
                _rooms.Sort((a, b) => a.Center.y.CompareTo(b.Center.y));
                break;
            case 2:
                // Sort by Distance
                _rooms = NearestSort(_rooms, 0);
                break;
            default:
                break;
        }
    }

    private List<Room> NearestSort(List<Room> rooms, int startIndex = 0)
    {
        if (rooms == null || rooms.Count == 0)
            return new List<Room>();

        if (startIndex < 0 || startIndex >= rooms.Count)
            startIndex = 0;

        var orderList = new List<Room>(rooms.Count);
        var visited = new bool[rooms.Count];

        int current = startIndex;
        orderList.Add(rooms[current]);
        visited[current] = true;

        for (int step = 1; step < rooms.Count; step++)
        {
            int next = -1;
            float bestDistSqr = float.PositiveInfinity;
            var currentPOS = rooms[current].Center;

            // Find the closest Room
            for (int i = 0; i < rooms.Count; i++)
            {
                if (visited[i]) continue;

                float dSq = (rooms[i].Center - currentPOS).sqrMagnitude;

                if (dSq < bestDistSqr || Mathf.Approximately(dSq, bestDistSqr) && rooms[i].Center.x < rooms[next].Center.x)
                {
                    bestDistSqr = dSq;
                    next = i;
                }
            }
            if (next == -1)
                break;

            visited[next] = true;
            orderList.Add(rooms[next]);
            current = next;
        }

        return orderList;
    }

    private void CarveCorridorL(Vector2Int a, Vector2Int b)
    {
        // == Horizontal Move ==
        // Move Right if B.x > A.x, otherwise move left
        int xStep = (a.x < b.x) ? 1 : -1;

        for (int x = a.x; x != b.x; x += xStep)
        {
            if (InBounds(x, a.y) && _grid[x, a.y] == '\0')
            {
                _grid[x, a.y] = 'C';
                _corridorTileCount++;
            }
        }

        // == Vertical Move ==
        // Move up if B.y > A.y, otherwise move down
        int yStep = (a.y < b.y) ? 1 : -1;

        for (int y = a.y; y != b.y; y += yStep)
        {
            if (InBounds(b.x, y) && _grid[b.x, y] == '\0')
            {
                _grid[b.x, y] = 'C';
                _corridorTileCount++;
            }
        }

        // == Final Tile ==
        //Mark the Destination if it's Empty
        if (InBounds(b.x, b.y) && _grid[b.x, b.y] == '\0')
        {
            _grid[b.x, b.y] = 'C';
            _corridorTileCount++;
        }
    }

    private void RenderTiles()
    {
        if (_floorPrefab == null) return;

        for (int x = 0; x < _cols; x++)
            for (int y =0; y < _rows; y++)
            {
                char c = _grid[x, y];

                if (c == '\0') continue;

                Vector3 worldPos = new Vector3(x * _tilesize, 0, y * _tilesize);

                GameObject prefab = null;
                switch (c)
                {
                    case 'R': prefab = _floorPrefab; break;
                    case 'C': prefab = _corridorPrefab; break;
                }

                if (gameObject != null)
                    Instantiate(prefab, worldPos, Quaternion.identity, _buildRoot);

            }
    }

    public void SetSeed(int seed)
    {
        _seed = seed;
    }

    private Vector3 EvenOutPosition(Vector3 position)
    {
        Vector3 newpos = Vector3.zero;

        if (position.x % 2 != 0)
            newpos.x = position.x + 1;
        else newpos.x = position.x;
        newpos.y = position.y;
        if (position.z % 2 != 0)
            newpos.z = position.z + 1;
        else newpos.z = position.z;

        return newpos;
    }
}
