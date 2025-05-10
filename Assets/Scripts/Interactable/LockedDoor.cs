using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class LockedDoor : MonoBehaviour,IDoorControl
{
    private int DoorNumber=0;
    public GameObject DoorCollider;
    public bool isLocked=true;
    public Animator ani;
    private bool inTrigger;

    public void DoorOpen()
    {
        if (!isLocked)
        {
            AnimateOn();
            
        }
        else
        {
            ChangeTip.ChangePlayTips("- 不能从这一侧打开 -");
        }
    }

    public void DoorClose()
    {
        inTrigger = false;
        if (isLocked)
        {
            ChangeTip.ChangePlayTips("");
        }
    }


    private void OnEnable()
    {
        KeyItem.DoorUnLock += UnLock;
        
    }

    private void OnDisable()
    {
        KeyItem.DoorUnLock -= UnLock;
    }

    private void UnLock(int Taget)
    {
        if (Taget == DoorNumber)
        {
            isLocked = false;
            DoorCollider.SetActive(false);
        }
    }

    private async UniTaskVoid AnimateOn()
    {
        inTrigger = true;
        Debug.Log("表锅我开门了喔");
        ani.Play("doorGlassBone|doorGlass_open");
        SoundManager.Instance.PlaySFX("door",1,6);
        await UniTask.WaitUntil(() => !inTrigger);

        Debug.Log("表锅我关门了喔");
        ani.Play("doorGlassBone|doorGlass_close");
        SoundManager.Instance.PlaySFX("doorClose",1,6);

    }

    // Start is called before the first frame update
    void Start()
    {
        DoorCollider = transform.Find("DoorCollider").gameObject;
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
