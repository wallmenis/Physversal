using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3 wishVector;
    Vector3 wishLookDirection;
    Vector3 lookDirection;
    Vector3 gotoDirection;
    Vector3 toUpVector;
    Boolean canJump;
    public float jumpforce;
    public float jumpangle;
    public float speed;
    float pushForce;
    Rigidbody rb;

    void Start()
    {
        gotoDirection = transform.position;
        lookDirection = transform.forward;
        toUpVector = Vector3.zero;
        rb = GetComponent<Rigidbody>();
        wishVector = Vector3.zero;
        wishLookDirection = Vector3.zero;
        canJump = true;
        pushForce = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(Vector3.up*rb.mass*1.5f);

        toUpVector = Vector3.up - transform.up;
        toUpVector.Normalize();
        rb.AddForceAtPosition(toUpVector * Time.deltaTime * 100f*rb.mass ,  Vector3.up);
        if (canJump ) {

            //lookDirection = new Vector3(lookDirection.x, transform.position.y , lookDirection.z);
            //lookDirection.Normalize();
            //transform.LookAt(lookDirection);
            //wishLookDirection = lookDirection - transform.position;
            //wishLookDirection = wishLookDirection.normalized - transform.forward;
            //wishLookDirection.Normalize();
            //wishLookDirection = transform.position - wishLookDirection;
            //wishLookDirection.Normalize();
            
            //Debug.DrawRay(transform.position, wishLookDirection, Color.red);
            
            //rb.AddTorque(wishLookDirection * Time.deltaTime * 0.001f);
            
            //rb.AddForceAtPosition(wishLookDirection * Time.deltaTime * rb.mass, transform.forward);
            
            gotoDirection = new Vector3(gotoDirection.x, transform.position.y, gotoDirection.z);
            
            //gotoDirection.Normalize();
            wishVector = gotoDirection - transform.position;
            wishVector *= pushForce;
            if (rb.velocity.magnitude > 1f)
            {
                wishVector.Normalize();
            }
            Vector3 twodVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (Vector3.Dot(twodVelocity, wishVector)<0f)
            {
                wishVector -= twodVelocity;
            }
            //wishVector.Normalize();
            //Debug.DrawRay(transform.position, wishVector, Color.magenta);
            rb.AddForce(wishVector * Time.deltaTime * speed * rb.mass);
        }
        else
        {
            rb.AddTorque(-rb.angularVelocity);
        }

        //rb.AddForceAtPosition(toUpVector * Time.deltaTime * 100f ,  Vector3.forward);
    }

    void ShouldJump()
    {
        RaycastHit hit;
        if (!Physics.Raycast(rb.position, transform.forward, out hit, 1f))
        {
            Jump();
        }
    }

    public void Jump()
    {
        if (canJump) 
        {
            rb.AddForce(Vector3.up*100f);
        }
    }
    public void StopDirection()
    {
        gotoDirection = transform.position;
        lookDirection = transform.forward;
    }
    public void SetGotoDirection(Vector3 gotodir)
    {
        gotoDirection = gotodir;
    }

    public void SetLookDirection(Vector3 lookdir)
    {
        lookDirection = lookdir;
    }

    private void OnCollisionStay(Collision collision)
    {
        float min = 180f;
        int c = 0;
        foreach (ContactPoint i in collision.contacts)
        {
            float angle = Vector3.Angle(Vector3.up, i.normal);
            if (angle < min)
            {
                min = angle;
            }

            c++;
        }
        if (min < jumpangle)
        {
            canJump = true;
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(rb.position, Vector3.down, out hit, 1f))
            {
                canJump = true;
            }
            else
            {
                canJump = false;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        RaycastHit hit;
        if (Physics.Raycast(rb.position, Vector3.down, out hit, 1f))
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }

    public void SetPushForce(float pf)
    {
        pushForce = pf;
    }
}

