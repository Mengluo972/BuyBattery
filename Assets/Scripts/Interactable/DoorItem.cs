using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorItem : MonoBehaviour,IInteractable
{
    public GameObject DoorCollider;

    public void inTriggerAnimation(bool b)
    {
        DoorCollider.SetActive(b);
    }

    public void TriggerAction()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        DoorCollider = transform.Find("DoorCollider").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
