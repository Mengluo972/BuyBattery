using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToLevel4 : MonoBehaviour,IInteractable
{
    private UIManeger uIManeger;
    private GameObject player;
    public int tagetLevel = 4;
    private bool elevatorOn = false;

    // Start is called before the first frame update
    void Start()
    {
        uIManeger = GameObject.Find("MainPanel").GetComponent<UIManeger>();
        player = GameObject.Find("player").gameObject;
        elevatorOn = false;
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
        //throw new System.NotImplementedException();
    }
}
