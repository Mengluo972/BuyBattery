using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManeger : MonoBehaviour
{
    private bool InBack;
    private bool paused;
    [SerializeField] private GameObject mainMenu;
    private GameObject saveData;
    private GameObject settings;
    private GameObject loading;
    private GameObject deathMenu;
    private GameObject stopMenu;
    private GameObject saveTip;
    private GameObject BG;//BG同时用于判断是否处于游戏中

    public string gameSceneName;
    public string mainMenuSceneName;

    public float saveTipsTime;

    private static GameObject instance;

    // Start is called before the first frame update
    void Start()
    {
        FindGameObject();
        paused = false;

    }

    private void Awake()
    {
        if (instance != null && instance != gameObject)
        {
            Destroy(instance);

        }
        instance = gameObject;
        DontDestroyOnLoad(gameObject);

    }

    private void OnEnable()
    {
        CoffeeMachine.CoffeeSave += () => ShowSaveTip();
    }

    private void OnDisable()
    {
        CoffeeMachine.CoffeeSave -= () => ShowSaveTip();  
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !deathMenu.activeSelf && !BG.activeSelf) { Pause(); }
    }

    private void FindGameObject()
    {
        mainMenu = transform.Find("MainMenu").gameObject;
        saveData = transform.Find("SaveData").gameObject;
        settings = transform.Find("Settings").gameObject;
        loading = transform.Find("Loading").gameObject;
        stopMenu = transform.Find("Stop").gameObject;
        deathMenu = transform.Find("Death").gameObject;
        saveTip = transform.Find("SaveTips").gameObject;
        BG = transform.Find("BG").gameObject;
    }

    public void OpenSaveData()
    {
        WaitForBack(saveData);
    }

    public void OpenSetting()
    {
        WaitForBack(settings);
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        StartCoroutine(LoadScene(gameSceneName));
    }

    public void Pause()
    {
        Debug.Log("先等一下，我问你个事");
        Time.timeScale = Convert.ToInt32(paused);
        paused = !paused;
        stopMenu.SetActive(paused);
    }

    public void Dead()
    {
        deathMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void BackMainMenu()
    {
        deathMenu.SetActive(false);
        BG.SetActive(true);
        StartCoroutine(LoadScene(mainMenuSceneName));
    }



    private async UniTaskVoid WaitForBack(GameObject tagetGO)
    {
        InBack = false;
        mainMenu.SetActive(false);
        tagetGO.SetActive(true);

        Button backButton = tagetGO.transform.Find("BackBTN").GetComponent<Button>();
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() => { InBack = true; });

        await UniTask.WaitUntil(() => InBack || Input.GetKeyDown(KeyCode.Escape));

        mainMenu.SetActive(BG.activeSelf);
        tagetGO.SetActive(false);
    }


    IEnumerator LoadScene(string sceneName)
    {
        Slider slider = loading.GetComponentInChildren<Slider>();

        Debug.Log("表锅开始加载了喔");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        loading.SetActive(true);

        while (!asyncLoad.isDone)
        {
            float persent = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            slider.value = persent;

            if (persent >= 1f)
            {
                Debug.Log("表锅加载成功了喔");
                asyncLoad.allowSceneActivation = true;
                loading.SetActive(false);
                BG.SetActive(false);
                Time.timeScale = 1f;
            }

            yield return null;
        }

    }

    private async UniTaskVoid ShowSaveTip()
    {
        saveTip.SetActive(true);
        float t = Time.time;

        await UniTask.WaitUntil(()=>(Time.time-t>saveTipsTime));

        saveTip.SetActive(false);

    }

}
