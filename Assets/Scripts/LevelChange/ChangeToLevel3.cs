using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeToLevel3 : MonoBehaviour,IInteractable
{
    // private BoxCollider boxCollider;
    private UIManeger uIManeger;
    private GameObject player;
    public int tagetLevel = 3;

    private void Start()
    {
        uIManeger = GameObject.Find("MainPanel").GetComponent<UIManeger>();
        player=GameObject.Find("player").gameObject;
    }

    // private void OnTriggerStay(Collider other)
    // {
    //     
    // }

    public void TriggerAction()
    {
        player.tag = "HiddenPlayer";
        uIManeger.LoadLevelScene(tagetLevel);
    }

    public void inTriggerAnimation(bool b)
    {

    }
}
