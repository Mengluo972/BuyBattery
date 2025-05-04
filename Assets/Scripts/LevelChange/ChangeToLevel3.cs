using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeToLevel3 : MonoBehaviour,IInteractable
{

    public void TriggerAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(3);
        }
    }

    public void inTriggerAnimation(bool b)
    {
        
    }
}
