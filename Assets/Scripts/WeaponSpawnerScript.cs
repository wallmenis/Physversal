using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSpawnerScript : MonoBehaviour
{
    public GameObject toSpawn;
    public Transform pickableFather;
    public float spawnRate;
    public int numberOfWeapons;
    int nof;
    float posInSecond;
    GameObject newSpawn;

    // Start is called before the first frame update
    void Start()
    {
        nof = numberOfWeapons;
        posInSecond = 0f;
        if (spawnRate <= 0f)
        {
            spawnRate = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (nof > 0 && posInSecond <= 0f)
        {
            nof--;
            newSpawn = Instantiate(toSpawn);
            newSpawn.transform.parent = pickableFather;
            newSpawn.GetComponent<Pickable>().PickablesParent = pickableFather.gameObject;
            newSpawn.transform.position = transform.position;
            posInSecond = 1f / spawnRate;
            Debug.Log(nof);
        }
        posInSecond -= Time.deltaTime;
    }

    public void ResetSpawn()
    {
        nof = numberOfWeapons;
    }
}
