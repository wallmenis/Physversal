using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrigg : MonoBehaviour
{
    public PauseRespawnMenu prm;
    public GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            prm.EndMenu();
            //prm.EndGame();
        }
    }
}
