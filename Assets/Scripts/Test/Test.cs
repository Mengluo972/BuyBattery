using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    void Start()
    {
        SoundManager.Instance.PlaySFX("door",1,6);
    }

    void Update()
    {
        
    }
}
