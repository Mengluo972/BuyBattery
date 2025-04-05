using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    [SerializeField]private bool inTrigger;
    private iInteractable actionItem;
    [Header("交互键位")]
    public KeyCode actionKey;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inTrigger)
        {
            if (Input.GetKeyDown(actionKey))
            {
                actionItem.TriggerAction();
                inTrigger = false;
                actionItem.inTriggerAnimation(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            inTrigger = true;
            if (actionItem != null)
            {
                actionItem.inTriggerAnimation(false);
            }
            actionItem = other.GetComponent<iInteractable>();
            actionItem.inTriggerAnimation(true);
        }

    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.GetComponent<iInteractable>() == actionItem)
        {
            inTrigger = false;
            actionItem.inTriggerAnimation(false);
        }
        
    }

}
