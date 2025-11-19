using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DungeonManager : MonoSingleton<DungeonManager>
{
    [SerializeField] DungeonLayoutGenerator _generator;
    [SerializeField] List<int> _dungeonSeedList = new List<int>();
    [SerializeField] List<Vector3> _dungeonExitList = new List<Vector3>();
    [SerializeField] List<Vector3> _dungeonEntranceList = new List<Vector3>();
    [SerializeField] int _floorNumber = 1;
    [SerializeField] int _numberOfFloors = 1;
    bool _isReturningToFloor;
    SpawnManager _spawnManager;
    public int Floor => _floorNumber;
    public Vector3 ExitPosition(int i) => _dungeonExitList[i];

    private void Start()
    {
        _spawnManager = FindFirstObjectByType<SpawnManager>(FindObjectsInactive.Include);
    }

    public void GenerateNextFloor(int floorNumber)
    {
        //===Exit the Dungeon here ====//
        if (floorNumber < 0)
        {
            Debug.Log("Exit Dungeon");
            return;
        }

        //== Determine iF going up to previous floor ==//
        if (floorNumber < _floorNumber)
            _isReturningToFloor = true;

        //== Generate New Floors if Needed ==//
        if (floorNumber >= _dungeonSeedList.Count)
        {
            while (_dungeonSeedList.Count <= floorNumber)
            {
                int seed = Random.Range(0, 10000000);
                if (_dungeonSeedList.Exists(x => x == seed)) 
                    continue;

                _dungeonSeedList.Add(seed);
            }
        }

        //== Set the Seed and Generate ==//
        _generator.SetSeed(_dungeonSeedList[floorNumber]);
        _generator.Generate();

        //== Move Party to Entrance or Exit==//
        var party = FindFirstObjectByType<PartyController>();
        if (_isReturningToFloor)
        {
            party.transform.position = _dungeonExitList[floorNumber]+Vector3.up;
            _isReturningToFloor = false;
        }
        else
        {
            party.transform.position = _dungeonEntranceList[floorNumber]+Vector3.up;
        }

        _floorNumber = floorNumber;

        //== Check if floor is Bottom Then clear Exits ===//
        if (_floorNumber == _numberOfFloors-1)
        {
            var exits = FindObjectsByType<StairsBehavior>(FindObjectsSortMode.None);
            for (int i = exits.Length-1; i >= 0; i--)
            {
                if (exits[i].Stairs == StairsBehavior.StairDirection.Down)
                    Destroy (exits[i].gameObject);
            }
        }

        //== Generate Our Enemies ==//
        _spawnManager.SpawnEnemies();
    }

    public void SetFloorExit(int seed, Vector3 position)
    {
        if (_dungeonSeedList.Exists(x => x == seed))
        {
            int i = _dungeonSeedList.IndexOf(seed);
            if (_dungeonExitList.Count <= i)
                _dungeonExitList.Add(position);
            else
                _dungeonExitList[i] = position;
        }
    }

    public void SetFloorEntrance(int seed, Vector3 position)
    {
        if (_dungeonSeedList.Exists(x => x == seed))
        {
            int i = _dungeonSeedList.IndexOf(seed);
            if (_dungeonEntranceList.Count <= i)
                _dungeonEntranceList.Add(position);
            else
                _dungeonEntranceList[i] = position;
        }
    }

    public void SetFloorCount(int floors)
    {
        if (floors > 0)
            _numberOfFloors = floors;
        else
            Debug.LogWarning("Floor count for Dungeon must be greater than Zero (0).");
    }
}
