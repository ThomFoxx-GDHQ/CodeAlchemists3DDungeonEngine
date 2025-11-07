using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DungeonManager : MonoSingleton<DungeonManager>
{
    [SerializeField] DungeonLayoutGenerator _generator;
    [SerializeField] List<int> _dungeonSeedList = new List<int>();
    [SerializeField] List<Vector3> _dungeonExitList = new List<Vector3>();
    [SerializeField] int _floorNumber = 1;

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
        if (floorNumber < 0)
        {
            Debug.Log("Exit Dungeon");
            return;
        }

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

        _generator.SetSeed(_dungeonSeedList[floorNumber]);
        _generator.Generate();

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
}
