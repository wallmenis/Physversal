using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public GameObject toSpawn;
    public float spawnRate;
    public int numberOfEnemies;
    int nof;
    float posInSecond;
    GameObject newSpawn;

    // Start is called before the first frame update
    void Start()
    {
        nof = numberOfEnemies;
        posInSecond = 0f;
        if (spawnRate <= 0f)
        {
            spawnRate = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (nof > 0 && posInSecond <=0f)
        {
            nof--;
            newSpawn = Instantiate(toSpawn);
            newSpawn.transform.position = transform.position;
            posInSecond = 1f / spawnRate;
            //Debug.Log(nof);
        }
        posInSecond -= Time.deltaTime;
    }
}
