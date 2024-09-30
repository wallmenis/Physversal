using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PusherScript : MonoBehaviour
{
    // Start is called before the first frame update
    EnemyMovement em;
    Transform playerTransform;
    public float viewAngle;
    public bool startBlind;
    public bool noPlayer;
    public Transform otherEnemy;
    public float pushForce;

    void Start()
    {
        playerTransform = otherEnemy;
        
        //Debug.Log(playerTransform);
        em = GetComponent<EnemyMovement>();   
        if (!noPlayer)
        {
            playerTransform = GameObject.Find("Player").transform;
        }
        em.SetPushForce(pushForce);
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 playerDir = playerTransform.position - transform.position;
        Debug.DrawRay(transform.position, transform.TransformDirection(playerDir), Color.red);
        RaycastHit hit;
        Physics.Raycast(transform.position, playerDir , out hit, playerDir.magnitude);
        if (hit.collider!=null)
        {
            if (hit.collider.gameObject.tag == "Player" || startBlind)
            {
                //Debug.DrawRay(transform.position, playerDir, Color.green);
                //Debug.Log(playerTransform.position);
                
                em.SetGotoDirection(playerTransform.position);
                em.SetLookDirection(playerTransform.position);
            }
        }
        else
        {
            em.StopDirection();
        }
    }
}
