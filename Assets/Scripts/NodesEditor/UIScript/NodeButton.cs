using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeButton : MonoBehaviour,IPointerClickHandler
{
    [NonSerialized]public int X;
    [NonSerialized]public int Y;
    [NonSerialized]public TMP_Text Text;
    [NonSerialized]public Node_Type Type = Node_Type.Walk;
    [NonSerialized]public Image Image;
    private readonly Color _white = new(255, 255, 255, 1f);
    private readonly Color _red =  new(255,0,0,1f);
    void Awake()
    {
        Text = transform.GetChild(0).GetComponent<TMP_Text>();
        Image = gameObject.GetComponent<Image>();
        // gameObject.GetComponent<Button>().onClick.AddListener(OnClickThisButton);
    }
    
    void Update()
    {
        
    }
    
    public void SetNodeInfo(int x, int y, Node_Type type)
    {
        X = x;
        Y = y;
        Type = type;
        UIUpdate();
    }
    
    public void UIUpdate()
    {
        Text.text = $"({X},{Y})";
        switch (Type)
        {
            case Node_Type.Walk:
                Image.color = _white;
                break;
            case Node_Type.Stop:
                Image.color = _red;
                break;
            default:
                print("出现了未知的类型");
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Type = Type == Node_Type.Walk ? Node_Type.Stop : Node_Type.Walk;
        UIUpdate();
    }
}
