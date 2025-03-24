using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public TMP_InputField inputFileNameField;//在引擎中初始化
    public TMP_Text fileNameDisplay;//在引擎中初始化
    public Nodes nodes;//在引擎中初始化
    public TMP_Text indexDisplay;//在引擎中初始化

    private string _path;
    private string _fileName;
    private int _startX;
    private int _startY;
    private int _endX;
    private int _endY;
    private List<List<AStarNode>> _operatingNodesData;
    void Start()
    {
        _path = Application.persistentDataPath;
    }

    void Update()
    {
        
    }

    public void OnClickRead()
    {
        _fileName = inputFileNameField.text;
        _path = Application.persistentDataPath + "/" + _fileName + ".xml";
        if (!File.Exists(_path))
        {
            print($"\"{_path}\"文件不存在，请新建文件");
            return;
        }

        using (StreamReader reader = new StreamReader(_path))
        {
            XmlSerializer s = new XmlSerializer(typeof(List<List<AStarNode>>));
            _operatingNodesData = s.Deserialize(reader) as List<List<AStarNode>>;
        }
        fileNameDisplay.text = _fileName+".xml";
        UpdateListDataToDisplay();
    }

    public void OnClickCreate()
    {
        _fileName = inputFileNameField.text;
        _path = Application.persistentDataPath + "/" + _fileName + ".xml";
        if (File.Exists(_path))
        {
            print("文件已存在，请不要重复创建同名文件");
            return;
        }

        List<List<AStarNode>> dataList = new List<List<AStarNode>>();
        //初始创建自动生成10x10可行走网格
        for (int i = 0; i < 10; i++)
        {
            List<AStarNode> list = new List<AStarNode>();
            for (int j = 0; j < 10; j++)
            {
                AStarNode aStarNode = new AStarNode(i,j,Node_Type.Walk);
                list.Add(aStarNode);
            }
            dataList.Add(list);
        }

        using (StreamWriter writer = new StreamWriter(_path))
        {
            XmlSerializer s = new XmlSerializer(typeof(List<List<AStarNode>>));
            s.Serialize(writer,dataList);
        }

        using (StreamReader reader = new StreamReader(_path))
        {
            XmlSerializer s = new XmlSerializer(typeof(List<List<AStarNode>>));
            _operatingNodesData = s.Deserialize(reader) as List<List<AStarNode>>;
        }
        fileNameDisplay.text = _fileName+".xml";
        UpdateListDataToDisplay();
    }

    private void UpdateListDataToDisplay()
    {
        if (_operatingNodesData == null)
        {
            print("数据为空");
            return;
        }
        //indexDisplay
        
        //
        _endX = 9;
        _endY = 9;
        nodes.UpdateNodes(_operatingNodesData,_startX,_startY,_endX,_endY);
        // print("数据更新完成");
    }
}
