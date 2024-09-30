using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.UIElements;

//https://discussions.unity.com/t/3d-gameobject-anchored-to-the-new-ui-canvas/116499#answer-835244
//https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@7.2/manual/rendering-to-a-render-texture.html

public class WeaponHud : MonoBehaviour
{
    public GameObject player;
    public Camera whCamera;
    public RenderTexture renderTexture;
    public RawImage rawImg;
    public RawImage backdrop;
    public RawImage selection;
    public TextMeshProUGUI hint;
    float farAppart;
    Vector2 ogRawImgSize;
    Vector2 ogbackdropImgSize;
    Vector2 ogselectionImgPos;
    
    PlayerInventory playerInventory;
    List<GameObject> theModelList;

    // Start is called before the first frame update
    void Start()
    {
        ogselectionImgPos = selection.transform.position;
        selection.enabled = false;
        ogbackdropImgSize = backdrop.rectTransform.sizeDelta;
        ogRawImgSize = rawImg.rectTransform.sizeDelta;
        backdrop.enabled = false;
        hint.enabled = false;
        farAppart = 2f;
        playerInventory = player.GetComponent<PlayerInventory>();
        /*
        theModelList = new List<GameObject>();
        foreach (GameObject model in playerInventory.getModelList())
        {
            theModelList.Add(Instantiate(model, transform));
        }
        for (int i = 0; i < theModelList.Count; i++)
        {
            theModelList[i].transform.position = whCamera.transform.position + new Vector3((i*1.0f - (theModelList.Count - 1) / 2f) * farAppart, 0f, 3f);
            theModelList[i].layer = 8;
            theModelList[i].transform.GetChild(0).gameObject.layer = 8;
            theModelList[i].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        }
        float toResize = theModelList.Count;
        if (toResize > 0f)
        {
            backdrop.enabled = true;
            rawImg.rectTransform.sizeDelta = ogRawImgSize * new Vector2(toResize, 1f);
            backdrop.rectTransform.sizeDelta = ogbackdropImgSize * new Vector2(toResize, 1f);
            whCamera.targetTexture = new RenderTexture(renderTexture.width * (int)toResize, renderTexture.height, renderTexture.depth);
            rawImg.texture = whCamera.targetTexture;
        }*/
        InitializeModelList();
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateModelList();
        //https://forum.unity.com/threads/modify-the-width-and-height-of-recttransform.270993/
        
        foreach (GameObject model in theModelList)
        {
            model.transform.RotateAround(model.transform.position,Vector3.up, 100f*Time.deltaTime);
        }

        if(theModelList.Count>0f)
        {
            selection.enabled = true;
            selection.rectTransform.position = ogselectionImgPos + new Vector2((playerInventory.getItemIterator() - (theModelList.Count - 1) / 2f) * 50f, 0f);
        }
        else
        {
            selection.enabled = false;
        }

    }

    private void UpdateModelList()
    {
        if (playerInventory.getItemListChanged())
        {
            EmptyModelList();
            InitializeModelList();
        }
    }

    private void EmptyModelList()
    {
        foreach (GameObject model in theModelList)
        {
            Destroy(model);
        }
    }

    private void InitializeModelList()
    {
        theModelList = new List<GameObject>();
        foreach (GameObject model in playerInventory.getModelList())
        {
            theModelList.Add(Instantiate(model, transform));
        }
        for (int i = 0; i < theModelList.Count; i++)
        {
            theModelList[i].transform.position = whCamera.transform.position + new Vector3((i * 1.0f - (theModelList.Count - 1) / 2f) * farAppart, 0f, 3f);
            theModelList[i].layer = 8;
            theModelList[i].transform.GetChild(0).gameObject.layer = 8;
            theModelList[i].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        }
        float toResize = theModelList.Count;
        Debug.Log(toResize);
        if (toResize > 0)
        {
            hint.enabled = true;
            backdrop.enabled = true;
            rawImg.rectTransform.sizeDelta = ogRawImgSize * new Vector2(toResize, 1f);
            backdrop.rectTransform.sizeDelta = ogbackdropImgSize * new Vector2(toResize, 1f);
            whCamera.targetTexture = new RenderTexture(renderTexture.width * (int)toResize, renderTexture.height, renderTexture.depth);
            rawImg.texture = whCamera.targetTexture;
        }
        else
        {
            hint.enabled = false;
            backdrop.enabled = false;
        }
    }
}
