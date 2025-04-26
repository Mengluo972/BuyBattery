using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
/// <summary>
/// 道具管理器，控制每次读档后的道具初始化，在引擎中获取所有道具的的索引
/// 管理器中的道具顺序在引擎安排好后就不应打乱，一旦打乱则容易出现读取问题
/// </summary>
public class PropManager : MonoBehaviour
{
    [Header("当前关卡序号（从1开始）")]public int levelNum;
    private Dictionary<string,int> _propListInfo = new();
    [Header("当前场景中的道具，请勿随意中间插入或者打乱顺序")]public List<GameObject> propList;
    [Header("玩家")]public Transform player;
    private Save _saveData;
    void Start()
    {
        // string path = Application.streamingAssetsPath + "/" + levelNum + ".xml";
        // //读档
        // //存档不存在
        // if (!File.Exists(path))
        // {
        //     //无存档，所有道具激活
        //     foreach (var go in propList)
        //     {
        //         go.SetActive(true);
        //     }
        //     Save saveData = new Save();
        //     
        //     for (int i = 0; i < propList.Count; i++)
        //     {
        //         saveData.PropState.Add(propList[i].activeInHierarchy);
        //         saveData.PropName.Add(propList[i].name);
        //         _propListInfo.Add(propList[i].name, i);
        //     }
        //     saveData.PlayerPos = player.position;
        //     _saveData = saveData;
        //     return;
        // }
        //
        // //存档存在
        // using (StreamReader sr = new StreamReader(path))
        // {
        //     XmlSerializer xmlSerializer = new XmlSerializer(typeof(Save));
        //     _saveData = xmlSerializer.Deserialize(sr) as Save;
        // }
        //
        // if (_saveData.PropState.Count!=_saveData.PropName.Count)
        // {
        //     print("道具状态和道具名称数量不匹配");
        //     return;
        // }
        // _propListInfo.Clear();
        // for (int i = 0; i < _saveData.PropState.Count; i++)
        // {
        //     propList[i].SetActive(_saveData.PropState[i]);
        //     _propListInfo.Add(_saveData.PropName[i], i);
        // }
    }

    public void ReadData()
    {
        string path = Application.streamingAssetsPath + "/" + levelNum + ".xml";
        if (!File.Exists(path))
        {
            //无存档，所有道具激活
            Save saveData = new();
            foreach (var go in propList)
            {
                go.SetActive(true);
                saveData.PropState.Add(go.activeInHierarchy);
                saveData.PropName.Add(go.name);
                _propListInfo.Add(go.name, propList.IndexOf(go));
            }
            saveData.PlayerPos = player.position;
            _saveData = saveData;
            return;
        }
        //存档存在
        using (StreamReader sr = new StreamReader(path))
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Save));
            _saveData = xmlSerializer.Deserialize(sr) as Save;
        }

        if (_saveData.PropState.Count!=_saveData.PropName.Count)
        {
            print("道具状态和道具名称数量不匹配");
            return;
        }
        player.position = _saveData.PlayerPos;
        for (int i = 0; i < _saveData.PropState.Count; i++)
        {
            propList[i].SetActive(_saveData.PropState[i]);
        }
    }
    
    public void SaveData()
    {
        string path = Application.streamingAssetsPath + "/" + levelNum + ".xml";
        _saveData.PropState.Clear();
        _saveData.PropName.Clear();
        _saveData.LevelNum = levelNum;
        _saveData.PlayerPos = player.position;
        foreach (var go in propList)
        {
            _saveData.PropState.Add(go.activeInHierarchy);
            _saveData.PropName.Add(go.name);
        }
        using StreamWriter sw = new StreamWriter(path);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Save)); 
        xmlSerializer.Serialize(sw, _saveData);
    }
    void Update()
    {
        
    }
}

public class Save
{
    [Header("当前关卡")]public int LevelNum;
    [Header("关卡中的道具状态")]public List<bool> PropState = new();
    [Header("对应的道具名称，用于校验")]public List<string> PropName = new();
    public Vector3 PlayerPos;
}
