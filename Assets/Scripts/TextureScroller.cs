using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    public float speed;
    MeshRenderer mr;
    Material startMaterial;
    Vector2 textureOffset;
    // Start is called before the first frame update
    void Start()
    {
        // https://discussions.unity.com/t/material-versus-shared-material/38446
        mr = GetComponent<MeshRenderer>();
        startMaterial = mr.material;
        textureOffset = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        mr.material.SetTextureOffset("_BaseMap", textureOffset);
        textureOffset += new Vector2(speed * Time.deltaTime, speed * Time.deltaTime);
        if (textureOffset.magnitude > Mathf.Sqrt(2))
        {
            textureOffset = Vector2.zero;
        }
    }
}
