using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ChangeToLevel4 : MonoBehaviour,IInteractable
{
    private UIManeger uIManeger;
    private GameObject player;
    public int tagetLevel = 4;
    private bool elevatorOn = false;

    private GameObject buttonTips;


    // Start is called before the first frame update
    void Start()
    {
        uIManeger = GameObject.Find("MainPanel").GetComponent<UIManeger>();
        player = GameObject.Find("player").gameObject;
        elevatorOn = false;
        buttonTips = transform.Find("ButtonTips").gameObject;
    }

    private void OnEnable()
    {
        MoneyItem.GetMoney += ElevatorOn;
    }

    private void OnDisable()
    {
        MoneyItem.GetMoney -= ElevatorOn;
    }

    private void ElevatorOn()
    {
        elevatorOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerAction()
    {
        if (elevatorOn)
        {
            player.tag = "HiddenPlayer";
            uIManeger.LoadLevelScene(tagetLevel);
        }
        
    }

    public void inTriggerAnimation(bool b)
    {
        AnimateOn();
        inTrigger = b;
    }

    private bool inTrigger;
    private async UniTaskVoid AnimateOn()
    {
        inTrigger = true;
        buttonTips.SetActive(true);
        if (elevatorOn)
        {
            ChangeTip.ChangePlayTips("- 吼吼，竟然不逃跑，而是向我走来了吗 -");
        }
        else
        {
            ChangeTip.ChangePlayTips("- 通往老板办公室，我现在没有要去的必要。 -");
        }

        await UniTask.WaitUntil(() => !inTrigger);

        buttonTips.SetActive(false);
        ChangeTip.ChangePlayTips("");

    }
}
