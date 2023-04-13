using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonScript : MonoBehaviour
{

    public GameObject[] dungeonMonsters;
    public int numberOfMonsters;

    public List<GameObject> monstersSpawnAreas;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var msa in GameObject.FindGameObjectsWithTag("MonsterSpawnArea"))
        {
            monstersSpawnAreas.Add(msa);
        }

        GenerateDungeonMonsters();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateDungeonMonsters()
    {
        for (int i = 0; i < numberOfMonsters; i++)
        {
            GameObject monster = Instantiate(dungeonMonsters[Random.Range(0, dungeonMonsters.Length)]);
            var areaPosOnList = Random.Range(0, monstersSpawnAreas.Count);
            var areaToSpawn = monstersSpawnAreas[areaPosOnList];
            monstersSpawnAreas.RemoveAt(areaPosOnList);
            monster.transform.position = areaToSpawn.transform.position;
            Destroy(areaToSpawn);
        }
    }

}
