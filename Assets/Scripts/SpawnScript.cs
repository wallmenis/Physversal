using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public PauseRespawnMenu prm;
    public Transform defSpawn;
    public GameTimerScript gts;
    public GameObject mainOBJ;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (mainOBJ != null)
        {
            mainOBJ.SetActive(true);
        }
        prm.SetRespawnPoint(defSpawn.position);
        transform.parent.gameObject.SetActive(false);
        gts.resetTimer();
    }
}
