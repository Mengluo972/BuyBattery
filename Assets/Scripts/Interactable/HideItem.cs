using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;

public class HideItem : MonoBehaviour,IInteractable
{
    public static event Action PlayerHide;
    private bool inTrigger;


    public void TriggerAction()
    {
        SoundManager.Instance.PlaySFX("trashCan",1,6);
        PlayerHide?.Invoke();
    }

    public void inTriggerAnimation(bool b)
    {
        //这里写动画
        AnimateOn();
        inTrigger = b;

    }

    private async UniTaskVoid AnimateOn()
    {
        inTrigger = true;
        Debug.Log("将军走此小道！");

        await UniTask.WaitUntil(() => !inTrigger);

        Debug.Log("陛下何故先降？");

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
