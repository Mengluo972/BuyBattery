using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    [SerializeField]private bool inTrigger;
    private IInteractable actionItem = null;
    [Header("交互键位")]
    public KeyCode actionKey=KeyCode.E;
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
            actionItem = other.GetComponent<IInteractable>();
            actionItem.inTriggerAnimation(true);
        }

    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.GetComponent<IInteractable>() == actionItem)
        {
            inTrigger = false;
            if (actionItem != null)
            {
                actionItem.inTriggerAnimation(false);
            }
        }
        
    }

}
