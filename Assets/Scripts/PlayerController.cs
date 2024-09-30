using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
//using UnityEngine.Windows.Speech;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    Vector2 goto_direction;
    Vector2 look_input;
    Vector3 prev_force_direction;
    Vector3 diff_force_direction;
    Vector3 forward_direction;
    Vector3 force_direction;
    Vector3 avgNormal;
    Vector3 final_direction;
    Vector3 pickable_influence;
    public float velocity;
    public float sensitivity;
    float lookupdown;
    float lookleftright;
    float updown_degrees;
    public float roundness;
    public float maxvelocity;
    public float maxvelocitycrouched;
    float finalmaxvelocity;
    public float jumpforce;
    public float jumpangle;
    public float dynFriction;
    public float statFriction;
    float angle_from_ground;
    public float airAccelleration;

    public PhysicMaterialCombine physMatCombine;
    public GameObject HUD;
    bool jump;
    bool canUncrouch;
    bool isCrouching;
    Rigidbody rb;
    Collider col;
    MeshCollider UnCrouchTrigger;
    Camera cam;
    GameObject Head;
    float pauseMult;
    float jumpAxis;
    PauseRespawnMenu prMenu;

    int collidercount;
    Vector3 phantomVelocity;

    void Start()
    {
        collidercount = 0;
        jumpAxis = 0f;
        phantomVelocity = Vector3.zero;
        pickable_influence = Vector3.zero;
        isCrouching = false;
        canUncrouch = true;
        UnCrouchTrigger = transform.GetChild(2).GetComponent<MeshCollider>();
        look_input = new Vector2(0,0);
        Time.timeScale = 1f;
        rb = GetComponent<Rigidbody>();
        Head = transform.GetChild(1).gameObject;
        cam = Head.GetComponentInChildren<Camera>();
        col = GetComponentInChildren<Collider>();
        /*
        dynFriction = col.sharedMaterial.dynamicFriction;
        statFriction = col.sharedMaterial.staticFriction;
        physMatCombine = col.sharedMaterial.frictionCombine;
        angle_from_ground = 1f;
        final_direction = Vector3.zero;
        airAccelleration = 30f;
        jumpforce = 25f;
        jumpangle = 45f;
        maxvelocity = 5f;
        maxvelocitycrouched = 3f;
        roundness = 1f;
        velocity = 30f;
        sensitivity = 2f;
        */
        updown_degrees = 0;
        jump = false;
        finalmaxvelocity = maxvelocity;
        force_direction = Vector3.zero;
        forward_direction = Head.transform.forward;
        goto_direction = Vector2.zero;
        force_direction = Vector3.zero;
        prev_force_direction = Vector3.zero;
        diff_force_direction = Vector3.zero;
        Cursor.lockState = CursorLockMode.Locked;
        prMenu = HUD.GetComponent<PauseRespawnMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        collidercount = 0;
        if (prMenu.getIsPaused())
        {
            pauseMult = 0f;
        }
        else
        {
            pauseMult = 1f;
        }
        

        lookupdown = sensitivity * Input.GetAxisRaw("Mouse Y") * pauseMult;
        lookleftright = sensitivity * Input.GetAxisRaw("Mouse X") * pauseMult;
        look_input.x = lookleftright;
        look_input.y = lookupdown;
        Head.transform.Rotate(Vector3.up, lookleftright, Space.World);
        //rb.transform.Rotate(Vector3.up, lookleftright, Space.World);
        forward_direction= Quaternion.AngleAxis(lookleftright, Vector3.up) * Vector3.Normalize(forward_direction);
        updown_degrees = Vector3.Angle(forward_direction, Head.transform.forward);
        if (Vector3.Dot(Head.transform.forward,Vector3.down)>0f)
        {
            updown_degrees *= -1f;
        }
        if ((updown_degrees<=90f && lookupdown>0) || (updown_degrees >= -90f && lookupdown < 0))
        {
            Head.transform.Rotate(Vector3.left, lookupdown , Space.Self);
        }

        goto_direction = new Vector2(Input.GetAxis("Vertical") * pauseMult, Input.GetAxis("Horizontal") * pauseMult);
        prev_force_direction = force_direction;
        force_direction = forward_direction * goto_direction.x + Head.transform.right * goto_direction.y;
        force_direction = force_direction * (1f - roundness) + force_direction.normalized * roundness;
        diff_force_direction = force_direction - prev_force_direction;

        col.sharedMaterial.staticFriction = 0f;
        col.sharedMaterial.dynamicFriction = 0f;
        col.sharedMaterial.frictionCombine = PhysicMaterialCombine.Minimum;

        if (goto_direction.y > 0f)
        {
            cam.transform.localRotation = Quaternion.RotateTowards(cam.transform.localRotation, new Quaternion(0, 0, -0.02f, 1f).normalized, 0.15f);
        }
        if (goto_direction.y < 0f)
        {
            cam.transform.localRotation = Quaternion.RotateTowards(cam.transform.localRotation, new Quaternion(0, 0, 0.02f, 1f).normalized, 0.15f);
        }
        if (goto_direction.y == 0f)
        {
            cam.transform.localRotation = Quaternion.RotateTowards(cam.transform.localRotation, new Quaternion(0, 0, 0, 1f).normalized, 0.15f);
        }
        
        if (jump && !prMenu.getIsPaused())
        {
            if (jumpAxis > 0)
            {
                rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
                jump = false;
            }
            if (goto_direction.x == 0f && goto_direction.y == 0f )//|| Mathf.Abs(diff_force_direction.magnitude) > 0f)
            {
                col.sharedMaterial.staticFriction = statFriction;
                col.sharedMaterial.dynamicFriction = dynFriction;
                col.sharedMaterial.frictionCombine = physMatCombine;
            }
        }
        else
        {
            avgNormal = Vector3.zero;
        }

        if (!prMenu.getIsPaused())
        {
            isCrouching = false;
            if (Input.GetAxis("Crouch") > 0f)
            {
                Crouch();
            }
            else if (canUncrouch)
            {
                UnCrouch();
            }
            /*if (!canUncrouch)
            {
                isCrouching = true;
            }  */  
        }

        Vector3 twodvelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        /*twodvelocity = twodvelocity + pickable_influence/rb.mass;
        if(jump || isCrouching)
        {
            pickable_influence = Vector3.zero;
        }
        else if (pickable_influence.magnitude > 0f)
        {
            pickable_influence *= (1f - 10f * Time.deltaTime);
        }*/
        //pickable_influence = Vector3.zero;
        //Debug.DrawRay(transform.position, transform.TransformDirection(force_direction) * 10f, Color.red);
        //Debug.DrawRay(transform.position, transform.TransformDirection(twodvelocity) * 10f, Color.green);
        //twodvelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        

        //float projectedVelocity = Vector3.Dot(twodvelocity, force_direction);
        float projectedVelocity = Vector3.Dot(phantomVelocity, force_direction);
        //float projectedVelocity = Vector3.Dot(twodvelocity.normalized * prev_force_direction.magnitude, force_direction);
        float a_vel = velocity * Time.deltaTime;
        final_direction = force_direction;
        if (jump)
        {
            if (a_vel + projectedVelocity > finalmaxvelocity)
            {
                a_vel = finalmaxvelocity - projectedVelocity;
            }
        }
        else
        {
            a_vel = airAccelleration * Time.deltaTime;
            if (a_vel + projectedVelocity > maxvelocity)
            {
                a_vel = maxvelocity - projectedVelocity;
            }
        }
        final_direction = final_direction * a_vel;

        

        if (twodvelocity.magnitude > 0f)
        {
            if (jump || isCrouching)
            {
                float friction = dynFriction;
                //friction = 1f;

                Vector3 friction_force = (-twodvelocity * friction + diff_force_direction) * Time.deltaTime;
                //Vector3 friction_force = (-phantomVelocity * friction + diff_force_direction) * Time.deltaTime;
                //Vector3 friction_force = (-twodvelocity * friction) * Time.deltaTime;
                if (friction_force.magnitude > finalmaxvelocity)
                {
                    friction_force = friction_force.normalized * finalmaxvelocity * friction;
                }
                //rb.AddForce(friction_force, ForceMode.VelocityChange);
                final_direction = final_direction + friction_force;

                phantomVelocity = twodvelocity;
                //Debug.LogWarning("FrictionForce: " + friction_force);
                //force_direction += diff_force_direction * function(Vector3.Angle(force_direction, twodvelocity));
            }
            else
            {
                float actual_fric = dynFriction;
                if (actual_fric > 1f)
                {
                    actual_fric = 1f;
                }
                else if (actual_fric < 0f)
                {
                    actual_fric = 0f;
                }
                //phantomVelocity -= phantomVelocity * actual_fric * Time.deltaTime;
                /*if (twodvelocity.magnitude > final_direction.magnitude)
                //if (twodvelocity.magnitude > finalmaxvelocity)
                {
                    final_direction = (final_direction + twodvelocity) / twodvelocity.magnitude;
                //final_direction = final_direction - twodvelocity.normalized * finalmaxvelocity;
                }*/
                //force_direction += diff_force_direction * function(Vector3.Angle(force_direction, twodvelocity))/180f;
            }
        }

        //if (Vector3.Dot(pickable_influence, final_direction) > 0f || final_direction.magnitude==0f || twodvelocity.magnitude > finalmaxvelocity)
        //{
        //    pickable_influence = Vector3.zero;
        //}
        //if (twodvelocity.magnitude > final_direction.magnitude)
        //if (twodvelocity.magnitude > finalmaxvelocity)
        //{
        //final_direction = final_direction / twodvelocity.magnitude;
        //final_direction = final_direction - twodvelocity.normalized * finalmaxvelocity;
        //}
        //if (twodvelocity.magnitude > final_direction.magnitude) //Rapid change in force will be penalized and make you surrender to the already existing velocity
        //{
        //    final_direction = final_direction * (-1f + Vector3.Dot(final_direction.normalized, twodvelocity.normalized))/2f;
        //}

        //rb.AddForce(final_direction - pickable_influence/rb.mass, ForceMode.VelocityChange);
        rb.AddForce(final_direction, ForceMode.VelocityChange);
        phantomVelocity += final_direction;


        //pickable_influence = Vector3.zero;


        /*
                RaycastHit hit;
                if (Physics.Raycast(rb.position, Vector3.down, out hit, 1f))
                {
                    //Debug.Log("Down Vec" + hit.collider);
                    jump = true;
                }
                else
                {
                    jump = false;
                }*/
    }

    private void FixedUpdate()
    {
        if(collidercount < 1)
        {
            jump = false;
        }
        jumpAxis = Input.GetAxis("Jump");
    }

    private void OnCollisionStay(Collision collision)
    {
        float avg = 0;
        float max = 0;
        float min = 180f;
        Vector3 avgN = Vector3.zero;
        int c=0;
        Debug.Log(collision.contacts.Length);
        foreach (ContactPoint i in collision.contacts)
        {
            collidercount++;
            Debug.Log(i.otherCollider);
            float angle = Vector3.Angle(Vector3.up, i.normal);
            if (angle < min)
            {
                min = angle;
            }
            
            c++;
            avg += angle;
            if(angle>max)
            {
                max = angle;
            }
            avgN += i.normal;
        }
        if (min < jumpangle)
        {
            jump = true;
        }
        else
        {
            RaycastHit hit;
            if(Physics.Raycast(rb.position,Vector3.down, out hit,1f))
            {
                //Debug.Log("Down Vec" + hit.collider);
                jump = true;
                //jump = false;
            }
            else
            {
                jump = false;
            }
        }
        if (c!=0)
        {
            avg = avg / c;
            avgN = avgN/ c;
            angle_from_ground = avg;
            avgNormal = avgN;
            //Debug.Log(avgN + " " + (1f - angle_from_ground / 180f));
        }
        else
        {
            avgNormal = Vector3.zero;
            angle_from_ground = 0f;
        }
        if(collision.contacts.Length==0)
        {
            jump = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        phantomVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (!jump)
        {
            RaycastHit hit;
            if (Physics.Raycast(rb.position, Vector3.down, out hit, 1f))
            {
                //Debug.Log("Down Vec" + hit.collider);
                //jump = true;
                jump = true;
            }
            else
            {
                jump = false;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (jump)
        {
            RaycastHit hit;
            if (Physics.Raycast(rb.position, Vector3.down, out hit, 1f))
            {
                //Debug.Log("Down Vec" + hit.collider);
                //jump = true;
                jump = true;
            }
            else
            {
                jump = false;
            }
        }
        //jump = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != 7)
        {
            canUncrouch = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("CROUCH " + other.tag);
        canUncrouch = true;
    }
    public Vector2 getLookInput()
    {
        return look_input;
    }
    public Vector2 getMovementInput()
    {
        return goto_direction;
    }

    public Vector3 getVelocity()
    {
        return rb.velocity;
    }
    public float getMaxVelocity()
    {
        return finalmaxvelocity;
    }

    private void Crouch()
    {
        isCrouching = true;
        finalmaxvelocity = maxvelocitycrouched;
        transform.GetChild(0).transform.localScale = new Vector3(1f,0.7f,1f);
    }

    private void UnCrouch()
    {
        isCrouching = false;
        finalmaxvelocity = maxvelocity;
        transform.GetChild(0).transform.localScale = Vector3.one;
    }

    public bool getIfCrouching()
    {
        return isCrouching;
    }

    public bool getIfCanJump()
    {
        return jump;
    }

    public void setPickableInfluence(Vector3 vec)
    {
        pickable_influence = new Vector3(vec.x, 0f, vec.z);
    }

    public void resetVelocity()
    {
        rb.velocity = Vector3.zero;
        phantomVelocity = Vector3.zero;
    }
}
