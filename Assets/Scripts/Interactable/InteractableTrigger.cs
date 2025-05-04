using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    [SerializeField]private bool inTrigger;
    private IInteractable actionItem = null;
    private GameObject actionGameObject = null;
    private IDoorControl actionDoor = null;
    [Header("交互键位")]
    public KeyCode actionKey=KeyCode.E;

    private PmcPlayerController playerController;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        playerController=GetComponent<PmcPlayerController>();
        animator=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inTrigger && !playerController.IsDisguised && !playerController.IsSafe)//可交互，且不处于装备道具状态
        {
            if (Input.GetKeyDown(actionKey))
            {
                transform.LookAt(actionGameObject.transform);
                transform.localEulerAngles = new Vector3(0f,transform.localEulerAngles.y,0f);

                animator.Play("rig_player|touch");
                playerController._inAction=true;
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
            actionGameObject = other.gameObject;
            actionItem = other.GetComponent<IInteractable>();
            actionItem.inTriggerAnimation(true);
        }
        if (other.CompareTag("Door"))
        {
            actionDoor=other.GetComponent<IDoorControl>();
            actionDoor.DoorOpen();
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
        if (collision.GetComponent<IDoorControl>() == actionDoor)
        {
            if(actionDoor != null)
            {
                actionDoor.DoorClose();
            }

        }
        
    }


}
