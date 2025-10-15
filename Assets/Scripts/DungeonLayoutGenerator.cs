using UnityEngine;
using System.Collections.Generic;

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

    [Header("Randomness")]
    [SerializeField] private int _seed = 12345;

    // == Internal ==
    private System.Random _rng;
    private char[,] _grid; // 'R' is Room, 'C' is Corridor, '\0' is Empty

    private readonly List<Room> _rooms = new List<Room>();
    
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
        //Build The Rooms
        RenderTiles();
    }

    private void ClearPrevious()
    {
        if (_buildRoot == null) return;

        for (int i = _buildRoot.childCount - 1; i >= 0; i--)
        {
#if UNITY_EDITOR
            DestroyImmediate(_buildRoot.GetChild(i).gameObject);
#else
            Destroy(_buildRoot.GetChild(i).gameObject);
#endif
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
                    _grid[x, y] = 'R';
    }

    private bool InBounds(int x, int y)
    {
        return x >= 0 && x < _cols && y>= 0 && y < _rows;
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
}
