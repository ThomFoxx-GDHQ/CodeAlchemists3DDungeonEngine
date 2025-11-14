using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] _enemyPrefabs;
    [SerializeField] int _numberToSpawn;
    Vector3 _spawnPos = Vector3.zero;
    List<Vector2Int> _spawnPoints = new List<Vector2Int>();
    DungeonLayoutGenerator _layout;

    private void Start()
    {
        _layout = FindFirstObjectByType<DungeonLayoutGenerator>();
    }

    [ContextMenu("Spawn Enemies")]
    public void SpawnEnemies()
    {
        SearchGridForRooms();

        for (int i = 0; i < _numberToSpawn; i++)
        {
            int randomPoint = Random.Range(0, _spawnPoints.Count);
            _spawnPos.x = _spawnPoints[randomPoint].x * _layout.TileSize;
            _spawnPos.z = _spawnPoints[randomPoint].y * _layout.TileSize;
            Instantiate(_enemyPrefabs[0], _spawnPos, Quaternion.identity);
        }
    }

    private void SearchGridForRooms()
    {
        _spawnPoints.Clear();
        var grid = _layout.Grid;
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == 'R')
                {
                    _spawnPoints.Add(new Vector2Int(x, y));
                }
            }
        }
    }
}
