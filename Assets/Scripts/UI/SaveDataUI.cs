using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataUI : MonoBehaviour
{
    public GameObject SaveDataPrefab;
    public GameObject parentObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            AddSaveDataUI();
        }
    }

    public void SaveGame()
    {

    }

    public void AddSaveDataUI()
    {
        GameObject SDUI = Instantiate(SaveDataPrefab, parentObject.transform);
        SDUI.name = $"SaveData{"n"}";

        //����д��ȡ�浵�İ�ť�¼�
        //Button targetButton = SDUI.GetComponent<Button>();
    }

}
