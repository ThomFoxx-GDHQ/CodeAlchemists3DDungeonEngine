using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DungeonManager : MonoSingleton<DungeonManager>
{
    [SerializeField] DungeonLayoutGenerator _generator;
    [SerializeField] List<int> _dungeonSeedList = new List<int>();

    private void Start()
    {
        StartCoroutine(TestGeneration());
    }

    IEnumerator TestGeneration()
    {
        int count = 0;
        while (true)
        {
            count++;
            count %= _dungeonSeedList.Count;
            yield return new WaitForSeconds(5);
            _generator.SetSeed(_dungeonSeedList[count]);
            _generator.Generate();
        }
    }
}
