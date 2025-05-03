using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeToLevel2 : MonoBehaviour
{
    private BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(2);
        }
    }
}
