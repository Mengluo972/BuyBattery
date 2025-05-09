using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToFUEnding : MonoBehaviour,IInteractable
{

    private UIManeger uIManeger;
    private GameObject player;
    public int tagetLevel = 6;

    private void Start()
    {
        uIManeger = GameObject.Find("MainPanel").GetComponent<UIManeger>();
        player=GameObject.Find("player").gameObject;
    }

    public void TriggerAction()
    {
        player.tag = "HiddenPlayer";
        uIManeger.LoadLevelScene(tagetLevel);
    }

    public void inTriggerAnimation(bool b)
    {

    }
}
