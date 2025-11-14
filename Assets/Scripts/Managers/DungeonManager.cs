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
    bool _isReturningToFloor;

    public int Floor => _floorNumber;
    public Vector3 ExitPosition(int i) => _dungeonExitList[i];

    private void Start()
    {
        //StartCoroutine(TestGeneration());
        //StartCoroutine(TestGeneration());
    }

    IEnumerator TestGeneration()
    {
        int count = 0;
        while (true)
        {
            count++;
            count %= _dungeonSeedList.Count;
            yield return new WaitForSeconds(2);
            _generator.SetSeed(_dungeonSeedList[count]);
            _generator.Generate();
        }
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
}
