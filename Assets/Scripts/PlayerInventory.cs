using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerInventory : MonoBehaviour
{
    List<GameObject> items;
    GameObject selectedItem;
    GameObject pickedUpItem;
    GameObject viewedPickable;
    GameObject touchedPickable;
    Transform cameraTransform;
    ViewModelStuff viewmodel;
    int itemIterator;
    public float pickupDistance;
    public int inventorySize;
    bool itemListChanged;
    bool hasPickedUpItem;
    bool isSeeingPickable;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = transform.GetChild(1).GetComponentInChildren<Camera>().transform;
        //pickupDistance = 5f;
        //itemIterator = 0;
        inventorySize = 2;
        selectedItem = null;
        viewmodel = GetComponent<ViewModelStuff>();
        items = new List<GameObject>();
        itemListChanged = false;
        hasPickedUpItem = false;
        isSeeingPickable = false;

    }
    // Update is called once per frame
    void Update()
    {
        isSeeingPickable = false;
        hasPickedUpItem = false;
        itemListChanged = false;
        GameObject tempfo = getFrontObject();
        if (tempfo != null)
        {
            if (tempfo.CompareTag("pickable"))
            {
                isSeeingPickable = true;

            }
        }
        // check for items
        if (Input.GetButtonDown("Interact"))
        {
            if (tempfo != null)
            {
                if (tempfo.CompareTag("pickable"))
                {
                    viewedPickable = tempfo;
                }
            }
            //Debug.Log("Interact
        }

        // Add item
        if (items.Count < inventorySize)
        {
            pickedUpItem = pickUpItem();
            if (pickedUpItem != null)
            {
                hasPickedUpItem = true;
                pickedUpItem.GetComponent<Pickable>().setPickable(cameraTransform, viewmodel.getViewModelPosition(), transform);
                items.Add(pickedUpItem);
                itemListChanged = true;
            }
        }
        
        // inventory management
        if(items.Count>0)
        {
            if (itemIterator >= items.Count)
            {
                itemIterator = 0;
            }
            if (itemIterator < 0)
            {
                itemIterator = items.Count - 1;
            }
            if (selectedItem == null)
            {
                selectedItem = items[itemIterator];
                if (selectedItem.GetComponent<Pickable>().isHidden())
                {
                    selectedItem.GetComponent<Pickable>().setShown();
                }
            }
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
            {
                selectedItem = ChangePicakble(itemIterator + 1, itemIterator);
                //Debug.Log("Up " + selectedItem.name + " " + itemIterator + " " + items.Count);
                itemIterator++;
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
            {
                selectedItem = ChangePicakble(itemIterator - 1, itemIterator);
                //Debug.Log("Down " + selectedItem.name + " " + itemIterator + " " + items.Count);
                itemIterator--;
            }

            float inputP = Input.GetAxis("PrimaryFire");
            float inputS = Input.GetAxis("SecondaryFire");
            selectedItem.GetComponent<Pickable>().doPrimaryFire(inputP);
            selectedItem.GetComponent<Pickable>().doSecondaryFire(inputS);
            if (Input.GetButtonDown("DropItem"))
            {
                //Debug.Log(selectedItem);
                pickedUpItem = null;
                viewedPickable = null;
                touchedPickable = null;
                selectedItem.GetComponent<Pickable>().setDropped(cameraTransform);
                items.Remove(selectedItem);
                itemListChanged = true;
                if (items.Count < 1)
                {
                    selectedItem = null;
                }
                else
                {
                    selectedItem = ChangePicakble(itemIterator - 1, itemIterator);
                }
                itemIterator--;
            }
        }
        else
        {
            itemIterator = -1;
            selectedItem = null;
        }
    }

    GameObject pickUpItem()
    {
        GameObject pickedItem = null;
        pickedItem = touchedPickable;
        if (pickedItem == null)
        {
            pickedItem = viewedPickable;
        }
        if (pickedItem != null)
        {
            if (pickedItem.gameObject.CompareTag("pickable"))
            {
                return pickedItem;
            }
        }
        return null;
    }

    public GameObject getSelectedItem()
    {
        return selectedItem;
    }
    private GameObject getFrontObject()
    {
        GameObject value = null;
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, pickupDistance))
        {
            value = hit.collider.gameObject;
            //Debug.DrawRay(cameraTransform.position, transform.TransformDirection(cameraTransform.forward) * hit.distance, Color.yellow);
            //Debug.Log(value);
        }
        return value;
    }
    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint i in collision.contacts)
        { 
            if (i.otherCollider.gameObject.CompareTag("pickable"))
            {
                //i.otherCollider.gameObject.tag = "touchedPickable";
                touchedPickable = i.otherCollider.gameObject;
            }
        }
    }

    private GameObject ChangePicakble(int pos,int prev)
    {
        int tmpPos=pos;
        if (items.Count > prev)
        {
            items[prev].GetComponent<Pickable>().setHidden();
        }    
        if (pos < 0)
        {
            tmpPos = items.Count - 1;
        }
        if (pos > items.Count - 1)
        {
            tmpPos = 0;
        }       
        items[tmpPos].GetComponent<Pickable>().setShown();
        return items[tmpPos];
    }

    public List<GameObject> getItemList()
    {
        return items;
    }

    public List<GameObject> getModelList()
    {
        List<GameObject> modelList = new List<GameObject>();
        if (items != null)
        {
            foreach (GameObject i in items)
            {
                modelList.Add(i.GetComponent<Pickable>().getModel());
            }
        }
        //Debug.Log(modelList);
        return modelList;
    }

    public int getItemIterator()
    {
        int iter = itemIterator;
        if (items != null)
        {
            if (items.Count - 1 < iter)
            {
                return items.Count - 1;
            }
            if (iter < 0)
            {
                return 0;
            }
        }
        return iter;
    }

    public bool getItemListChanged()
    {
        return itemListChanged;
    }

    public bool getHasPickedUpItem()
    {
        return hasPickedUpItem;
    }

    public bool getIsSeeingPickable()
    {
        return isSeeingPickable;
    }
}


