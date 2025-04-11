using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.ProBuilder;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManeger : MonoBehaviour
{
    public static event Action youDie;

    private bool InBack;
    private bool paused;
    [SerializeField] private GameObject mainMenu;
    private GameObject[] saveData;
    private GameObject saveDataAni;
    private GameObject[] settings;
    private GameObject settingsAni;
    private GameObject[] loading;
    private GameObject loadingAni;
    private GameObject deathMenu;
    private GameObject stopMenu;
    private GameObject saveTip;
    private GameObject BG;//BG同时用于判断是否处于游戏中
    private GameObject oldBG;
    
    //[Header("拖入模型UI")]
    //public GameObject uIModel;
    [Header("输入跳转场景名")]
    public string gameSceneName;
    public string mainMenuSceneName;
    [Header("输入UI显示时间")]
    public float saveTipsTime;
    public float MTS;
    public float MTSD;
    public float MTNG;


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
        AttackState.DeathEvent += Dead;
    }

    private void OnDisable()
    {
        CoffeeMachine.CoffeeSave -= () => ShowSaveTip();
        AttackState.DeathEvent -= Dead;
    }

    [Header("紫砂键")]
    public KeyCode deathKey;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !deathMenu.activeSelf && !BG.activeSelf) { Pause(); }
        if (Input.GetKeyDown(deathKey)){Dead();}
    }

    private void FindGameObject()
    {
        mainMenu = transform.Find("MainMenu").gameObject;
        saveData = new GameObject[2] { transform.Find("SaveData").gameObject, transform.Find("TimeLIne_UI_MtoSD").gameObject };
        settings = new GameObject[2] { transform.Find("Settings").gameObject, transform.Find("TimeLIne_UI_MtoS").gameObject };
        loading = new GameObject[2] { transform.Find("Loading").gameObject, transform.Find("TimeLIne_UI_MtoNG").gameObject };
        stopMenu = transform.Find("Stop").gameObject;
        deathMenu = transform.Find("Death").gameObject;
        saveTip = transform.Find("SaveTips").gameObject;
        BG = transform.Find("UI_BuyButtery_BG").gameObject;
        oldBG = transform.Find("BG").gameObject;
    }

    public void OpenSaveData()
    {
        WaitForBack(saveData,MTSD);
    }

    public void OpenSetting()
    {
        WaitForBack(settings,MTS);
    }

    public void DeathLoad()
    {
        OpenDeathLoad();
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
        stopMenu.SetActive(false);
        oldBG.SetActive(true);
        StartCoroutine(LoadScene(mainMenuSceneName));
        //ceneManager.LoadScene(mainMenuSceneName);
    }

    private async UniTaskVoid OpenDeathLoad()
    {
        InBack = false;
        saveData[0].SetActive(true);

        Button backButton = saveData[0].transform.Find("BackBTN").GetComponent<Button>();
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() => { InBack = true; });

        await UniTask.WaitUntil(() => InBack || Input.GetKeyDown(KeyCode.Escape));

        saveData[0].SetActive(false);
    }

    private async UniTaskVoid WaitForBack(GameObject[] tagetGO,float setTime)
    {
        mainMenu.SetActive(false);
        tagetGO[1].SetActive(true);
        InBack = false;


        await UniTask.Delay((int)(setTime*1000));
        
        tagetGO[0].SetActive(true);
        
        Button backButton = tagetGO[0].transform.Find("BackBTN").GetComponent<Button>();
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() => { InBack = true; });

        await UniTask.WaitUntil(() => InBack || Input.GetKeyDown(KeyCode.Escape));

        PlayableDirector tagetGOAni = tagetGO[1].GetComponent<PlayableDirector>();
        
        tagetGOAni.time = setTime;
        tagetGOAni.playableGraph.GetRootPlayable(0).SetSpeed(-1);
        tagetGOAni.Play();
        tagetGO[0].SetActive(false);

        await UniTask.Delay((int)(setTime * 1000));

        mainMenu.SetActive(BG.activeSelf);
        tagetGO[1].SetActive(false);
        
    }


    IEnumerator LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        if (BG.activeSelf)
        {
            loading[1].SetActive(true);

            yield return new WaitForSeconds(MTNG);

            loading[0].SetActive(true);
            Slider slider = loading[0].GetComponentInChildren<Slider>();

            yield return StartCoroutine(FakeLoad(slider, 0.4f, 0, 0.7f));//总时间，初始值和终值

            yield return StartCoroutine(FakeLoad(slider, 0.2f, 0.7f, 1f));
        }

        Debug.Log("表锅开始加载了喔");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        float persent = Mathf.Clamp01(asyncLoad.progress / 0.9f);
        while (persent < 1f)
        {
            persent = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            yield return null;
        }
        Debug.Log("表锅加载成功了喔");

        asyncLoad.allowSceneActivation = true;
        loading[0].SetActive(false);
        loading[1].SetActive(false);
        BG.SetActive(false);
        
    }

    IEnumerator FakeLoad(Slider slider,float faket,float startValue,float endValue)
    {
        float tt = 0;
        slider.value = startValue;
        while (tt < faket)
        {
            slider.value = Mathf.Lerp(startValue, endValue, tt / faket);
            tt += Time.deltaTime;
            yield return null;
        }
        slider.value = endValue;
    }


    private async UniTaskVoid ShowSaveTip()
    {
        saveTip.SetActive(true);
        float t = Time.time;
        Debug.Log("表锅savetip来了喔？");

        await UniTask.WaitUntil(()=>(Time.time-t>saveTipsTime));

        saveTip.SetActive(false);
        Debug.Log("表锅savetip走了喔？");

    }

}
