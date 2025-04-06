using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SaveDataUI : MonoBehaviour
{
    public GameObject SaveDataPrefab;
    public GameObject parentObject;

    public float waitTime;

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

    private async void OnEnable()
    {
        foreach (Transform t in parentObject.transform)
        {
            Destroy(t.gameObject);
        }
        int count = 5;
        int n = 0;
        while (n < count)
        {
            n++;
            if (Time.timeScale != 0)
            {
                await UniTask.Delay((int)(waitTime * 1000));
            }
            AddSaveDataUI();
        }

    }

    private void OnDisable()
    {
        
    }

    public void SaveGame()
    {

    }

    public void AddSaveDataUI()
    {
        GameObject SDUI = Instantiate(SaveDataPrefab, parentObject.transform);
        SDUI.name = $"SaveData{"n"}";

        //下面写读取存档的按钮事件
        //Button targetButton = SDUI.GetComponent<Button>();
    }

}
