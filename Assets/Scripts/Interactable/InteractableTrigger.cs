using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    private bool inTrigger;
    private iInteractable actionItem;

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
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            inTrigger = true;
            actionItem = other.GetComponent<iInteractable>();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inTrigger = false;
    }

}
