using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor.PackageManager;

public class SaveDataUI : MonoBehaviour
{
    //假设第一个存档序号为0；

    public GameObject SaveDataPrefab;
    public GameObject parentObject;
    public UIManeger uIManeger;

    public float waitTime;
    private PropManager propManager;

    private bool _isChooseDelete;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            //AddSaveDataUI();
        }
    }

    private async void OnEnable()
    {
        ShowSaveDatas();

    }

    private void OnDisable()
    {
        _isChooseDelete = false;
    }

    public void SaveGame()
    {
        //todo
    }

    public void StartNewGame()
    {
        int count = 10;
        int n = 0;
        while (n < count)
        {
            PropManager.ReadData(n, out int levelNum, out int caughtTime, out float gameTime);
            if (levelNum == -1) { break; } 
            n++;
        }

        if (n < 10)
        {
            PropManager.CreateNewData(n);
            uIManeger.StartGame();
        }
        else
        {
            _isChooseDelete = true;
            uIManeger.OpenSaveData();
            n = 0;

        }
    }



    public GameObject AddSaveDataUI(int n,int levelNum,int caughtTime,float gameTime)
    {
        GameObject SDUI = Instantiate(SaveDataPrefab, parentObject.transform);
        SDUI.name = $"SaveData{n}";

        SDUI.transform.Find("saveTitle").GetComponent<TMP_Text>().text = $"存档{n}";
        SDUI.transform.Find("saveDescribe").GetComponent<TMP_Text>().text = $"关卡{levelNum}" + "     " + $"被抓捕次数{caughtTime}";

        int gameSec = (int)(gameTime % 60);
        int gameMin = (int)(gameTime / 60);
        SDUI.transform.Find("saveTime").GetComponent<TMP_Text>().text = "游戏时间：" + gameMin + "." + gameSec;

        return SDUI;
    }

    public void ButtonAddLoad(GameObject SDUI, int saveNum,int loadLevel,float gametime)
    {
        Button loadButton = SDUI.GetComponentInChildren<Button>();
        loadButton.onClick.RemoveAllListeners();
        loadButton.onClick.AddListener(() =>
        {
            PropManager.CurrentSaveNum = saveNum;
            uIManeger.LoadLevelScene(loadLevel);
            uIManeger.nowGameTime = gametime;
            uIManeger.nowSaveData = saveNum;
            uIManeger.nowLevel = loadLevel;
            uIManeger.InBack = true;
        });
    }

    public void ButtonAddDelete(GameObject SDUI,int saveNum)
    {
        Button loadButton = SDUI.GetComponentInChildren<Button>();
        loadButton.onClick.RemoveAllListeners();
        loadButton.onClick.AddListener(() => { PropManager.CreateNewData(saveNum);uIManeger.StartGame(); });
    }

    public async void ShowSaveDatas()
    {
        foreach (Transform t in parentObject.transform)
        {
            Destroy(t.gameObject);
        }

        int count = 10;
        int n = 0;

        while (n < count)
        {
            PropManager.ReadData(n, out int levelNum, out int caughtTime, out float gameTime);

            if(levelNum == -1)
            {
                break;
            }

            if (Time.timeScale != 0)
            {
                await UniTask.Delay((int)(waitTime * 1000));
            }

            GameObject SDUI = AddSaveDataUI(n, levelNum, caughtTime, gameTime);
            if (_isChooseDelete)
            {
                ButtonAddDelete(SDUI, n);
            }
            else
            {
                ButtonAddLoad(SDUI, n, levelNum, gameTime);
            }

            n++;
        }
    }

    

}
