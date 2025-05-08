using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinterNoise : MonoBehaviour,IDoorControl
{
    private GameObject Noise;
    public bool isOpen;

    public void DoorClose()
    {
        if (isOpen)
        {
            Noise.SetActive(false);
        }
        
    }

    public void DoorOpen()
    {
        if (isOpen)
        {
            Noise.SetActive(true);
        }
        
    }

    public void printerOn()
    {
        isOpen = true;
        DoorOpen();
    }

    public void printerOff() { isOpen = false; DoorClose(); } 


    // Start is called before the first frame update
    void Start()
    {
        Noise = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
