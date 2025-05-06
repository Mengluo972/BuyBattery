using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeToLevel3 : MonoBehaviour,IInteractable
{
    // private BoxCollider boxCollider;
    private UIManeger uIManeger;
    public int tagetLevel = 3;

    private void Start()
    {
        uIManeger = GameObject.Find("MainPanel").GetComponent<UIManeger>();
    }

    // private void OnTriggerStay(Collider other)
    // {
    //     
    // }

    public void TriggerAction()
    {
        uIManeger.LoadLevelScene(tagetLevel);
    }

    public void inTriggerAnimation(bool b)
    {

    }
}
