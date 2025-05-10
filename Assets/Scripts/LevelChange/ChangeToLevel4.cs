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
            ChangeTip.ChangePlayTips("- ��𣬾�Ȼ�����ܣ����������������� -");
        }
        else
        {
            ChangeTip.ChangePlayTips("- ͨ���ϰ�칫�ң�������û��Ҫȥ�ı�Ҫ�� -");
        }

        await UniTask.WaitUntil(() => !inTrigger);

        buttonTips.SetActive(false);
        ChangeTip.ChangePlayTips("");

    }
}
