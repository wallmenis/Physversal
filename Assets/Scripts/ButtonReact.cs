using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//https://docs.unity3d.com/540/Documentation/ScriptReference/UI.Selectable.html
public class ButtonReact : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Button button;
    TextMeshProUGUI text;
    Color originalTextColor;
    public Color newTextColor;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        //originalTextColor = new Color(text.color.r, text.color.g, text.color.b);
        originalTextColor = text.color;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = newTextColor;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = originalTextColor;
    }
}
