using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 道具管理器，控制每次读档后的道具初始化，在引擎中获取所有道具的的索引
/// 管理器中的道具顺序在引擎安排好后就不应打乱，一旦打乱则容易出现读取问题
/// </summary>
public class PropManager : MonoBehaviour
{
    [Header("当前关卡序号（从1开始）")] public int levelNum;
    private Dictionary<string, int> _propListInfo = new();
    [Header("当前场景中的道具，请勿随意中间插入或者打乱顺序")] public List<GameObject> propList = new();
    [Header("玩家")] public Transform player;
    private Save _saveData;
    [Header("当前存档序号，打开某个存档时要更改")]public static int CurrentSaveNum;
    [Header("游戏时长，可全局访问，退出到主菜单式记得清空")]public static float gameTime;
    [Header("被逮次数，可全局访问，退出到主菜单时记得清空")]public static int caughtTime;

    //关卡开始时读取存档
    void Start()
    {
        _propListInfo.Clear();
        // player = GameObject.FindGameObjectWithTag("Player").transform;
        string path = Application.persistentDataPath + "/" + CurrentSaveNum + ".xml";
        //读档
        //存档不存在
        if (!File.Exists(path))
        {
            Debug.Log("存档不存在!");
            return;
        }

        // if (CurrentSaveNum!=levelNum)
        // {
        //     Debug.Log("疑似在跳转关卡前没有正确修改静态变量值");
        //     return;
        // }

        using (StreamReader sr = new StreamReader(path))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Save));
            _saveData = serializer.Deserialize(sr) as Save;
        }

        //玩家进入新的一关
        if (_saveData.LevelNum != levelNum)
        {
            Debug.Log("玩家进入了新的一关");
            for (int i = 0; i < propList.Count; i++)
            {
                _propListInfo.Add(propList[i].name, i);
                propList[i].gameObject.SetActive(true);
                _saveData.PropState.Add(propList[i]);
                _saveData.PropName.Add(propList[i].name);
            }

            _saveData.LevelNum = levelNum;
            _saveData.PlayerPos = player.position;
            SaveData(CurrentSaveNum, _saveData);
            return;
        }

        //存档合规性检查
        if (_saveData.PropState.Count != _saveData.PropName.Count)
        {
            Debug.Log("存档信息出错!道具状态和道具数量不匹配");
            Debug.Log($"道具状态数量为:{_saveData.PropState.Count},道具名称数量为:{_saveData.PropName.Count}");
            return;
        }

        //判空，如果道具列表为空，那么说明该关卡是第一次进入
        if (_saveData.PropState.Count == 0 || _saveData.PropName.Count == 0)
        {
            Debug.Log($"第一次进入第{levelNum}关");
            for (int i = 0; i < propList.Count; i++)
            {
                _propListInfo.Add(propList[i].name, i);
                propList[i].gameObject.SetActive(true);
                _saveData.PropState.Add(propList[i]);
                _saveData.PropName.Add(propList[i].name);
            }

            _saveData.PlayerPos = player.position;
            SaveData(CurrentSaveNum, _saveData);
            return;
        }
        
        CharacterController ctr =player.GetComponent<CharacterController>();
        ctr.enabled = false;
        player.position = _saveData.PlayerPos;
        ctr.enabled = true;
        for (int i = 0; i < _saveData.PropState.Count; i++)
        {
            propList[i].SetActive(_saveData.PropState[i]);
            _propListInfo.Add(propList[i].name, i);
        }


    }

    private IEnumerator ChangePos()
    {
        yield return new WaitForSeconds(2.1f);
        player.position = _saveData.PlayerPos;
        Debug.Log($"更改后player.position={player.position}");
    }

    void Update()
    {
        // gameTime += Time.deltaTime;
    }

    /// <summary>
    /// 删除对应序号的存档
    /// </summary>
    /// <param name="saveNum"></param>
    public static void DeleteData(int saveNum)
    {
        string path = Application.persistentDataPath + "/" + saveNum + ".xml";
        if (!File.Exists(path))
        {
            Debug.Log($"存档{saveNum}不存在");
            return;
        }

        CreateNewData(saveNum);
        //考虑是否需要回调函数，更新UI界面
    }

    /// <summary>
    /// 创建新存档，一旦调用将不可挽回地覆盖对应序号的存档
    /// </summary>
    /// <param name="saveNum">存档序号</param>
    public static void CreateNewData(int saveNum)
    {
        string path = Application.persistentDataPath + "/" + saveNum + ".xml";
        Save saveData = new Save();
        saveData.LevelNum = 1;
        saveData.CaughtTime = 0;
        saveData.GameTime = 0f;
        using (StreamWriter writer = new StreamWriter(path))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Save));
            serializer.Serialize(writer, saveData);
        }

    }
    /// <summary>
    /// 查找数据信息，有三个向外传参，分别是关卡序号，被逮次数，游戏总时长
    /// </summary>
    /// <param name="saveNum">存档序号</param>
    /// <param name="levelNum">关卡序号</param>
    /// <param name="caughtTime">被逮次数</param>
    /// <param name="gameTime">游戏总时长</param>
    public static void ReadData(int saveNum, out int levelNum, out int caughtTime, out float gameTime)
    {
        string path = Application.persistentDataPath + "/" + saveNum + ".xml";
        if (!File.Exists(path))
        {
            Debug.Log($"存档{saveNum}不存在");
            levelNum = -1;
            caughtTime = -1;
            gameTime = -1f;
            return;
        }

        using (StreamReader sr = new StreamReader(path))
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Save));
            Save saveData = xmlSerializer.Deserialize(sr) as Save;
            levelNum = saveData.LevelNum;
            caughtTime = saveData.CaughtTime;
            gameTime = saveData.GameTime;
        }
    }
    /// <summary>
    /// 如果你看到了这行字，说明你大概调用错了方法
    /// </summary>
    /// <param name="saveNum"></param>
    /// <param name="saveData"></param>
    public static void SaveData(int saveNum,Save saveData)
    {
        if (saveData == null)
        {
            Debug.Log("存档数据为空");
            return;
        }

        using (StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/" + saveNum + ".xml"))
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Save));
            xmlSerializer.Serialize(writer, saveData);
        }
    }
    
    /// <summary>
    /// 咖啡机调用
    /// </summary>
    public void SaveGame()
    {
        _saveData.LevelNum = levelNum;
        _saveData.PropState.Clear();
        _saveData.PropName.Clear();
        _saveData.GameTime = gameTime;
        _saveData.CaughtTime = caughtTime;
        _saveData.PlayerPos = player.position;
        for (int i = 0; i < propList.Count; i++)
        {
            _saveData.PropState.Add(propList[i].activeSelf);
            _saveData.PropName.Add(propList[i].name);
        }

        using (StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/" + CurrentSaveNum + ".xml"))
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Save));
            xmlSerializer.Serialize(writer, _saveData);
        }
    }
    
}
public class Save
{
    [Header("关卡序号")] public int LevelNum;
    [Header("被抓次数")] public int CaughtTime;
    [Header("游戏时间")] public float GameTime;
    [Header("关卡中的道具状态")] public List<bool> PropState = new();
    [Header("对应的道具名称，用于校验")] public List<string> PropName = new();

    public Vector3 PlayerPos;
}