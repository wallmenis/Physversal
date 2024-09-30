using UnityEngine;

public class Pickable : MonoBehaviour
{
    public GameObject PickablesParent;
    GameObject previousParrent;
    Rigidbody rb;
    Collider col;
    Transform aimTransform;
    Transform pickerTransform;
    //public MeshRenderer mr;
    MeshRenderer mr;
    float primaryFireVal;
    float secondaryFireVal;
    float counterMetricVal;
    // Start is called before the first frame update
    void Start()
    {
        aimTransform = transform;
        primaryFireVal = 0f;
        secondaryFireVal = 0f;
        previousParrent = PickablesParent;
        //previousParrent = null;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        mr = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void doPrimaryFire(float input)
    {
        primaryFireVal = input;
    }
    public void doSecondaryFire(float input)
    {
        secondaryFireVal = input;
    }

    public float getPrimaryFire()
    {
        return primaryFireVal;
    }
    public float getSecondaryFire()
    {
        return secondaryFireVal;
    }

    public void setPickable(Transform camTransform, Vector3 viewmodelPosition, Transform pickerTrans)
    {
        mr.enabled = false;
        transform.gameObject.layer = LayerMask.NameToLayer("ViewModel");
        mr.transform.gameObject.layer = LayerMask.NameToLayer("ViewModel");
        transform.gameObject.tag = "touchedPickable";
        mr.transform.gameObject.tag = "touchedPickable";
        rb.isKinematic = true;
        col.enabled = false;
        transform.SetParent(camTransform);
        aimTransform = camTransform;
        previousParrent = camTransform.gameObject;
        pickerTransform = pickerTrans;
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, new Quaternion(0, 0, 0, 1f).normalized, 90f);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, viewmodelPosition, 15f);
    }

    public void setDropped(Transform camTransform)
    {
        mr.enabled = true;
        transform.gameObject.layer = LayerMask.NameToLayer("Default");
        mr.transform.gameObject.layer = LayerMask.NameToLayer("Default");
        rb.isKinematic = false;
        col.enabled = true;
        aimTransform = transform;
        transform.tag = "thrownPickable";
        mr.transform.tag = "thrownPickable";
        transform.SetParent(PickablesParent.transform);
        //transform.SetParent(null);
        rb.AddForce(camTransform.forward * 20f * rb.mass, ForceMode.Impulse);
        pickerTransform = null;
    }

    public Transform getAimTransform()
    {
        return aimTransform;
    }

    public Transform getPickerTransform()
    {
        return pickerTransform;
    }

    public void setHidden()
    {
        mr.enabled = false;
    }
    public void setShown()
    {
        mr.enabled = true;
    }

    public bool isHidden() 
    {
        return !mr.enabled;
    }

    public GameObject getModel()
    {
        //return Instantiate(transform.GetChild(0).gameObject);
        return transform.GetChild(0).gameObject;
    }


    private void OnCollisionStay(Collision collision)
    {
        //Debug.Log("Touched");
        if(previousParrent!=null && transform.parent!=null)
        {
            if (transform.parent.ToString() != previousParrent.ToString())
            {
                transform.SetParent(PickablesParent.transform);
                //transform.SetParent(null);
                previousParrent = PickablesParent;
                //previousParrent = null;
                transform.tag = "pickable";
                mr.transform.tag = "pickable";
            }
        }
    }
}
