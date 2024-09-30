using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewModelStuff : MonoBehaviour
{

    public Vector3 viewmodelPosition;
    Vector2 mouseInput;
    Vector2 movementInput;
    public float Sway;
    public float Bobbing;
    PlayerInventory playerInventory;
    PlayerController playerController;
    GameObject selectedItem;
    Camera cam;
    float animPos;
    // Start is called before the first frame update
    void Start()
    {
        animPos = 0;
        mouseInput = new Vector2 (0, 0);
        movementInput = new Vector2 (0, 0);
        playerInventory = GetComponent<PlayerInventory>();
        playerController = GetComponent<PlayerController>();
        cam = transform.GetChild(1).GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseInput = playerController.getLookInput();
        movementInput = playerController.getMovementInput();
        Vector3 vel = playerController.getVelocity();
        float Mvel = playerController.getMaxVelocity();
        selectedItem = playerInventory.getSelectedItem();
        vel = vel / Mvel;
        if (vel.magnitude > 1f)
        {
            vel = vel.normalized;
        }
        if (selectedItem!=null)
        {
            Vector2 cappedMI = mouseInput;
            
            //Debug.Log(Vector3.Angle(selectedItem.transform.forward, cam.transform.forward));
            //if (Vector3.Angle(selectedItem.transform.forward,transform.forward) > 5f && mouseInput.x > 5f)
            if (mouseInput.x > 5f)
            {
                cappedMI.x = 5f;
            //}else if (Vector3.Angle(selectedItem.transform.forward, transform.forward) > 5f && mouseInput.x < -5f)
            }else if (mouseInput.x < -5f)
            {
                cappedMI.x = -5f;
            }

            if(mouseInput.y > 5f)
            {
                cappedMI.y = 5f;
            }else if(mouseInput.y < -5f)
            {
                cappedMI.y = -5f;
            }
            Quaternion temp = new Quaternion(cappedMI.y * Sway, -cappedMI.x * Sway, 0, 1f).normalized;
            Vector3 temp2 = new Vector3(movementInput.y, -vel.y*4f, movementInput.x)*Sway + viewmodelPosition;
            selectedItem.transform.localRotation = Quaternion.RotateTowards(selectedItem.transform.localRotation, temp, 1000f*Sway);
            selectedItem.transform.localPosition = Vector3.MoveTowards(selectedItem.transform.localPosition, temp2, 0.1f* Sway);



            Vector3 tempBobbing = bobbingFunction(animPos, 1f) * Bobbing  + viewmodelPosition;
            selectedItem.transform.localPosition = Vector3.MoveTowards(selectedItem.transform.localPosition, tempBobbing, 1f*Bobbing);

            animPos += Time.deltaTime;
            if (animPos >= 2f)
            {
                animPos = 0f;
            }
        }
    }

    public Vector3 getViewModelPosition()
    {
        return viewmodelPosition;
    }

    private Vector3 bobbingFunction(float input,float reset)
    {
        float lastInput = input;
        if(input>reset)
        {
            lastInput = reset - input;
        }    
        Vector3 result = new Vector3(input - 0.5f, (input - 0.5f) * (input - 0.5f)/2f,0f);
        return result;
    }
}
