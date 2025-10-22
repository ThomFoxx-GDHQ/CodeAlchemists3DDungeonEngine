using Unity.Mathematics;
using UnityEngine;

public class DungeonDecorator3D : MonoBehaviour
{
    [SerializeField] private DungeonLayoutGenerator _layoutSource;
    [SerializeField] private GameObject _roomWallPrefab;
    [SerializeField] private GameObject _corridorWallPrefab;
    [SerializeField] private GameObject _doorTilePrefab;

    [SerializeField] private Transform _wallsRoot;
    [SerializeField] private Transform _doorsRoot;

    [SerializeField] private float _wallHeight = 2f;
    [SerializeField] private float _wallThickness = .1f;
    [SerializeField] private float _epsilon = 0.01f;

    private char[,] Grid => _layoutSource.Grid;
    private int Width => _layoutSource.Width;
    private int Height => _layoutSource.Height;
    private float TileSize => _layoutSource.TileSize;

    [ContextMenu("Build 3d Dungeon")]
    public void BuildDungeon3d()
    {
        if (_layoutSource == null)
            return;

        //Clear out Walls
        ClearChildren(_wallsRoot);
        //Clear out Doors
        ClearChildren(_doorsRoot);

        //Build Walls and Doors
        BuildWallsAndDoors();
    }

    private void BuildWallsAndDoors()
    {
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                char current = Grid[x, y];
                if (current == '\0') continue;

                Vector3 basePos = new Vector3(x * TileSize, 0, y * TileSize);

                foreach (var dir in _directions)
                {
                    int nx = x + dir.dx;
                    int ny = y + dir.dy;
                    char neighbor = GetCell(nx, ny);

                    if (current == neighbor) continue;

                    if (IsDoorHere(x,y,dir))
                    {
                        if (current == 'C')
                        { } //Spawn Door
                        continue;
                    }

                    if (current == 'R' && neighbor == 'C' && IsDoorHere(nx, ny, Opposite(dir)))
                        continue;

                    SpawnWall(basePos, dir, current);
                }
            }
    }

    private void SpawnWall(Vector3 basePos, Direction dir, char celltype)
    {
        Vector3 center = basePos + dir.world * (TileSize * .5f);
        Quaternion rotation = GetRotation(dir);

        GameObject prefab = (celltype == 'R') ? _roomWallPrefab : _corridorWallPrefab;
        GameObject wall = Instantiate(prefab, center - dir.world * _epsilon, rotation, _wallsRoot);
        wall.transform.localScale = new Vector3(TileSize, _wallHeight, _wallThickness);
    }


    private bool IsDoorHere(int x, int y, Direction d)
    {
        return false;
    }

    // ===Helpers===
    private void ClearChildren(Transform root)
    {
        if (root == null) return;

        for (int i = root.childCount - 1; i >= 0; i--)
        {
#if UNITY_EDITOR
            DestroyImmediate(root.GetChild(i).gameObject);
#else
            Destroy(root.GetChild(i).gameObject);
#endif
        }
    }

    private readonly struct Direction
    {
        public readonly int dx, dy;
        public readonly Vector3 world;
        public readonly Directions id;

        public Direction(int dx, int dy, Vector3 world, Directions id)
        {
            this.dx = dx;
            this.dy = dy;
            this.world = world;
            this.id = id;
        }
    }

    private readonly Direction[] _directions = new[]
    {
        new Direction(0,1,Vector3.forward, Directions.North),
        new Direction(1,0,Vector3.right, Directions.East),
        new Direction(0,-1,Vector3.back, Directions.South),
        new Direction(-1,0,Vector3.left, Directions.West)
    };

    private Direction Opposite(Direction d)
    {
        switch (d.id)
        {
            case Directions.North:
                return _directions[2];
            case Directions.East:
                return _directions[3];
            case Directions.South:
                return _directions[0];
            case Directions.West:
                return _directions[1];
            default:
                return d;
        }
    }

    private char GetCell(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height) return '\0';
        return Grid[x, y];
    }

    private Quaternion GetRotation(Direction dir)
    {
        switch (dir.id)
        {
            case Directions.North:
                return Quaternion.Euler(0, 180, 0);
            case Directions.East:
                return Quaternion.Euler(0, -90, 0);
            case Directions.South:
                return Quaternion.Euler(0, 0, 0);
            case Directions.West:
                return Quaternion.Euler(0, 90, 0);
            default:
                return Quaternion.Euler(0, 0, 0);
        }
    }
}
