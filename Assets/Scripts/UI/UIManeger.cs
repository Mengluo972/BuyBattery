using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.ProBuilder;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIManeger : MonoBehaviour
{
    public static event Action youDie;

    private bool isEasterEgg;//标记彩蛋是否触发
    [NonSerialized] public bool InBack;
    [NonSerialized] public bool InGame;
    private bool paused;
    [NonSerialized] public int nowCaughtTime;
    [NonSerialized] public int nowLevel = 0;
    [NonSerialized] public static float nowGameTime;
    [NonSerialized] public int nowSaveData;

    public GameObject mainMenu;
    private GameObject[] saveData;
    private GameObject saveDataAni;
    private GameObject[] settings;
    private GameObject settingsAni;
    private GameObject[] loading;
    private GameObject loadingAni;
    private GameObject SuccessPanel;
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
    public string level1SceneName;
    public string level2SceneName;
    public string level3SceneName;
    public string level4SceneName;
    [Header("输入UI显示时间")]
    public float saveTipsTime;
    public float EasterEggTime;
    public float MTS;
    public float MTSD;
    public float MTNG;
    [Header("设置死亡画面")]
    public DeadUISet deadUISet=new DeadUISet();

    private static GameObject instance;

    // Start is called before the first frame update
    void Start()
    {
        FindGameObject();
        paused = false;
        Cursor.lockState = CursorLockMode.None;
        PlayMainMenuSound();
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
        ShenMiLittleBook.EasterEgg += () => ShowEasterEgg();
        AttackState.DeathEvent += Dead;
    }

    private void OnDisable()
    {
        CoffeeMachine.CoffeeSave -= () => ShowSaveTip();
        ShenMiLittleBook.EasterEgg -= () => ShowEasterEgg();
        AttackState.DeathEvent -= Dead;
    }

    [Header("紫砂键")]
    public KeyCode deathKey;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !deathMenu.activeSelf && !BG.activeSelf) { Pause(); }
        if (Input.GetKeyDown(deathKey)){Dead(EnemyAnimator.colleague);}

        if (Time.timeScale > 0)
        {
            nowGameTime += Time.deltaTime;
        }
        
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
        SuccessPanel = transform.Find("SuccessPanel").gameObject;
    }

    public void OpenSaveData()
    {
        WaitForBack(saveData,MTSD);
        SoundManager.Instance.PlaySFX("", 2, 6);
    }

    public void OpenSetting()
    {
        WaitForBack(settings,MTS);
        SoundManager.Instance.PlaySFX("", 2, 6);
    }

    public void DeathLoad()
    {
        OpenDeathLoad();
        SoundManager.Instance.PlaySFX("", 2, 6);
    }

    public void StartGame()
    {
        SoundManager.Instance.PlaySFX("", 2, 6);
        InBack =true;
        mainMenu.SetActive(false);
        nowLevel = 1;
        nowGameTime = 0;
        nowCaughtTime = 0;
        StartCoroutine(LoadScene(gameSceneName));
    }

    public void Pause()
    {
        SoundManager.Instance.PlaySFX("", 2, 6);
        Debug.Log("先等一下，我问你个事");
        Time.timeScale = Convert.ToInt32(paused);
        paused = !paused;
        stopMenu.SetActive(paused);
    }

    public void Dead(EnemyAnimator aniType)
    {
        deathMenu.SetActive(true);
        ChangeDeadUI(aniType);
        Time.timeScale = 0;
    }

    private void ChangeDeadUI(EnemyAnimator aniType)
    {
        string uIText = deathMenu.transform.Find("describe").gameObject.GetComponent<TMP_Text>().text;
        Sprite uISprite = deathMenu.transform.Find("Image").gameObject.GetComponent<Image>().sprite;
        switch (aniType)
        {
            case EnemyAnimator.boss:
                uIText = deadUISet.bossText;
                uISprite = deadUISet.bossSprite;
                break;
            case EnemyAnimator.colleague:
                uIText = deadUISet.colleagueText;
                uISprite = deadUISet.colleagueSprite;
                break;
            case EnemyAnimator.maneger:
                uIText = deadUISet.manegerText;
                uISprite = deadUISet.manegerSprite;
                break;
            case EnemyAnimator.intern:
                uIText = deadUISet.internText;
                uISprite = deadUISet.internSprite;
                break;
            case EnemyAnimator.guard:
                uIText = deadUISet.guardText;
                uISprite = deadUISet.guardSprite;
                break;

        }
        
        
    }

    public void PlayerWin()
    {
        Time.timeScale = 0;
        oldBG.SetActive(true);
        SuccessPanel.SetActive(true);

        int gameSec = (int)(nowGameTime % 60);
        int gameMin = (int)(nowGameTime / 60);
        SuccessPanel.transform.Find("gameTime").GetComponent<TMP_Text>().text = gameMin + "m" + gameSec + "s";

        SuccessPanel.transform.Find("catchAmount").GetComponent<TMP_Text>().text = $"{nowCaughtTime}";

        SuccessPanel.transform.Find("FuckMoneyAmount").GetComponent<TMP_Text>().text = "";//此处应输入纸钞获取量

    }

    public void BackMainMenu()
    {
        SoundManager.Instance.PlaySFX("", 2, 6);
        SuccessPanel.SetActive(false);
        deathMenu.SetActive(false);
        stopMenu.SetActive(false);
        oldBG.SetActive(true);
        StartCoroutine(LoadScene(mainMenuSceneName));
        //ceneManager.LoadScene(mainMenuSceneName);
    }

    public void Restart()
    {
        SoundManager.Instance.PlaySFX("", 2, 6);
        deathMenu.SetActive(false);
        stopMenu.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    private async UniTaskVoid OpenDeathLoad()
    {
        SoundManager.Instance.PlaySFX("", 2, 6);
        InBack = false;
        saveData[0].SetActive(true);

        Button backButton = saveData[0].transform.Find("BackBTN").GetComponent<Button>();
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() => { InBack = true; SoundManager.Instance.PlaySFX("", 2, 6); });

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
        backButton.onClick.AddListener(() => { InBack = true; SoundManager.Instance.PlaySFX("", 2, 6); });

        await UniTask.WaitUntil(() => InBack || Input.GetKeyDown(KeyCode.Escape));

        PlayableDirector tagetGOAni = tagetGO[1].GetComponent<PlayableDirector>();
        
        tagetGOAni.time = setTime;
        tagetGOAni.playableGraph.GetRootPlayable(0).SetSpeed(-1);
        tagetGOAni.Play();
        tagetGO[0].SetActive(false);

        await UniTask.Delay((int)(setTime * 1000));

        if (nowLevel == 0)
        {
            mainMenu.SetActive(true);
        }
        else
        {
            mainMenu.SetActive(false);
        }

        tagetGO[1].SetActive(false);
        
    }


    IEnumerator LoadScene(string sceneName)
    {
        InGame = true;

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
        else
        {
            deathMenu.SetActive(false);
            stopMenu.SetActive(false);
            oldBG.SetActive(true);
            if (sceneName != mainMenuSceneName)
            {
                loading[0].SetActive(true);
                Slider slider = loading[0].GetComponentInChildren<Slider>();

                yield return StartCoroutine(FakeLoad(slider, 0.4f, 0, 0.7f));//总时间，初始值和终值

                yield return StartCoroutine(FakeLoad(slider, 0.2f, 0.7f, 1f));
            }

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
        oldBG.SetActive(false);
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

    public void LoadLevelScene(int levelNum)
    {
        switch (levelNum)
        {
            case 1:
                StartCoroutine(LoadScene(level1SceneName));
                break;
            case 2:
                StartCoroutine(LoadScene(level2SceneName));
                break;
            case 3:
                StartCoroutine(LoadScene(level3SceneName));
                break;
            case 4:
                StartCoroutine (LoadScene(level4SceneName));
                break;

        }

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

    private async UniTaskVoid ShowEasterEgg()
    {
        //这里显示彩蛋UI
        float t = Time.time;
        Debug.Log("表锅出彩蛋了喔");
        isEasterEgg=true;

        await UniTask.WaitUntil(() => (Time.time - t > EasterEggTime));

        //隐藏彩蛋UI
        Debug.Log("表锅彩蛋冇了喔");

    }

    public async UniTaskVoid PlayMainMenuSound()
    {
        SoundManager.Instance.PlayBGM(SoundType.Main);
        InGame=false;

        await UniTask.WaitUntil(() => (InGame));

        await UniTask.Delay((int)(MTNG*1000));

        SoundManager.Instance.StopBGM();

    }

    [System.Serializable]
    public class DeadUISet
    {
        public Sprite bossSprite;
        public string bossText;
        public Sprite manegerSprite;
        public string manegerText;
        public Sprite colleagueSprite;
        public string colleagueText;
        public Sprite internSprite;
        public string internText;
        public Sprite guardSprite;
        public string guardText;
    }
    


}
