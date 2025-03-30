using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public TMP_InputField inputFileNameField; //在引擎中初始化
    public TMP_Text fileNameDisplay; //在引擎中初始化
    public Nodes nodes; //在引擎中初始化
    public TMP_Text indexDisplay; //在引擎中初始化
    public TMP_InputField inputXField; //在引擎中初始化
    public TMP_InputField inputYField; //在引擎中初始化

    private string _path;
    private string _fileName;
    private int _startX;
    private int _startY;
    private int _endX;
    private int _endY;

    private List<List<AStarNode>> _operatingNodesData;

    // private NodesData _rawNodesData;
    void Start()
    {
        _path = Application.persistentDataPath;
    }

    void Update()
    {
    }

    public void ChangeSingleNodeInfo(int x, int y, Node_Type type)
    {
        if (x > _operatingNodesData.Count - 1 || y > _operatingNodesData[0].Count - 1)
        {
            print("下标超出范围");
            return;
        }

        _operatingNodesData[x][y].Type = type;
        nodes.UpdateNodes(_operatingNodesData, _startX, _startY, _endX, _endY);
    }
    //按钮方法

    public void BugTest()
    {
        for (int i = 0; i < _operatingNodesData.Count; i++)
        {
            for (int j = 0; j < _operatingNodesData[0].Count; j++)
            {
                print($"x:{_operatingNodesData[i][j].x},y:{_operatingNodesData[i][j].y},i:{i},j:{j}");
            }
        }
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

        fileNameDisplay.text = _fileName + ".xml";
        inputXField.text = (_operatingNodesData.Count - 1).ToString();
        inputYField.text = (_operatingNodesData[0].Count - 1).ToString();

        _startX = 0;
        _startY = 0;
        _endX = 9; //X至少为9，所以不做检查
        _endY = 9;
        // print($"最大X下标:{_operatingNodesData.Count - 1},最大Y下标:{_operatingNodesData[0].Count - 1}");
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

        int maxX, maxY;
        maxX = inputXField.text == "" ? 9 : int.Parse(inputXField.text);
        maxY = inputYField.text == "" ? 9 : int.Parse(inputYField.text);
        if (maxX % 10 != 9 || maxY % 10 != 9)
        {
            print("X或Y不符合格式，请按格式输入");
            return;
        }

        for (int i = 0; i <= maxX; i++)
        {
            List<AStarNode> list = new List<AStarNode>();
            for (int j = 0; j <= maxY; j++)
            {
                AStarNode aStarNode = new AStarNode(i, j, Node_Type.Walk);
                list.Add(aStarNode);
            }

            dataList.Add(list);
        }

        using (StreamWriter writer = new StreamWriter(_path))
        {
            XmlSerializer s = new XmlSerializer(typeof(List<List<AStarNode>>));
            s.Serialize(writer, dataList);
        }

        using (StreamReader reader = new StreamReader(_path))
        {
            XmlSerializer s = new XmlSerializer(typeof(List<List<AStarNode>>));
            _operatingNodesData = s.Deserialize(reader) as List<List<AStarNode>>;
        }

        fileNameDisplay.text = _fileName + ".xml";
        UpdateListDataToDisplay();
    }

    public void OnClickNextXPage()
    {
        if (_operatingNodesData.Count - 1 >= _endX + 10)
        {
            _startX += 10;
            _endX += 10;
            UpdateListDataToDisplay();
        }
        else if (_operatingNodesData.Count - 1 < _endX + 10 && _operatingNodesData.Count - 1 > _endX)
        {
            _startX += 10;
            _endX = _operatingNodesData.Count - 1;
            UpdateListDataToDisplay();
        }
        else
        {
            print("已经是X轴最后一页");
        }
    }

    public void OnClickLastXPage()
    {
        if (_startX - 10 >= 0 && _endX == _startX + 9)
        {
            _startX -= 10;
            _endX -= 10;
            UpdateListDataToDisplay();
        }
        else if (_startX - 10 >= 0 && _endX != _startX + 9)
        {
            _startX -= 10;
            _endX = _startX + 9;
            UpdateListDataToDisplay();
        }
        else if (_startX - 10 < 0 && _startX != 0)
        {
            _startX = 0;
            _endX = 9;
            UpdateListDataToDisplay();
        }
        else
        {
            print("已经是X轴第一页");
        }
    }

    // public void OnClickPlusXPage()
    // {
    //     for (int i = 0; i < 10; i++)
    //     {
    //         List<AStarNode> list = new();
    //         for (int j = 0; j < _operatingNodesData[0].Count; j++)
    //         {
    //             list.Add(new AStarNode(_operatingNodesData.Count+i,j,Node_Type.Walk));
    //         }
    //         _operatingNodesData.Add(list);
    //     }
    // }

    public void OnClickUpYPage()
    {
        if (_operatingNodesData[0].Count - 1 >= _endY + 10)
        {
            _startY += 10;
            _endY += 10;
            UpdateListDataToDisplay();
        }
        else if (_operatingNodesData[0].Count - 1 < _endY + 10 && _operatingNodesData[0].Count - 1 > _endY)
        {
            _startY += 10;
            _endY = _operatingNodesData[0].Count - 1;
            UpdateListDataToDisplay();
        }
        else
        {
            print("已经是Y轴最后一页");
        }
    }

    public void OnClickDownYPage()
    {
        if (_startY - 10 >= 0 && _endY == _startY + 9)
        {
            _startY -= 10;
            _endY -= 10;
            UpdateListDataToDisplay();
        }
        else if (_startY - 10 >= 0 && _endY != _startY + 9)
        {
            _startY -= 10;
            _endY = _startY + 9;
            UpdateListDataToDisplay();
        }
        else if (_startY - 10 < 0 && _startY != 0)
        {
            _startY = 0;
            _endY = 9;
            UpdateListDataToDisplay();
        }
        else
        {
            print("已经是Y轴第一页");
        }
    }

    public void OnClickSave()
    {
        // int maxX,maxY;
        // try
        // {
        //     maxX = int.Parse(inputXField.text);
        //     maxY = int.Parse(inputYField.text);
        // }
        // catch (FormatException e)
        // {
        //     // Console.WriteLine(e);
        //     print("X或Y格式错误，请输入整数");
        //     return;
        // }
        // if(maxX%10!=9||maxY%10!=9)
        // {
        //     print("X或Y不符合格式，请按格式输入");
        //     return;
        // }
        // if(maxX>_operatingNodesData.Count-1)
        // {
        //     //扩充X
        //     int count = maxX-_operatingNodesData.Count+1;
        //     for (int i = 0; i < count; i++)
        //     {
        //         List<AStarNode> list = new();
        //         for (int j = 0; j < _operatingNodesData[0].Count; j++)
        //         {
        //             list.Add(new AStarNode(maxX-9+i,j,Node_Type.Walk));
        //         }
        //         _operatingNodesData.Add(list);
        //     }
        // }else if (maxX<_operatingNodesData.Count-1)
        // {
        //     _operatingNodesData.RemoveRange(maxX+1,_operatingNodesData.Count-1-maxX);
        // }
        //
        // if (maxY>_operatingNodesData[0].Count-1)
        // {
        //     //扩充Y
        //     int count = maxY-_operatingNodesData[0].Count+1;
        //     for (int i = 0; i < _operatingNodesData.Count; i++)
        //     {
        //         for (int j = 0; j < count; j++)
        //         {
        //             _operatingNodesData[j].Add(new AStarNode(i,maxY-9+j,Node_Type.Walk));
        //         }
        //     }
        // }else if (maxY < _operatingNodesData[0].Count - 1)
        // {
        //     foreach (var list in _operatingNodesData)
        //     {
        //         int count = list.Count;
        //         for (int i = count-1; i > maxY; i--)
        //         {
        //             list.RemoveAt(i);
        //         }
        //     }
        // }

        using StreamWriter writer = new StreamWriter(_path);
        XmlSerializer s = new XmlSerializer(typeof(List<List<AStarNode>>));
        s.Serialize(writer, _operatingNodesData);
    }

    private void UpdateListDataToDisplay()
    {
        if (_operatingNodesData == null)
        {
            print("数据为空");
            return;
        }
        //indexDisplay

        indexDisplay.text = $"起始x下标:{_startX} 终止x下标:{_endX} \n起始y下标:{_startY} 终止y下标:{_endY}";


        nodes.UpdateNodes(_operatingNodesData, _startX, _startY, _endX, _endY);
        // print("数据更新完成");
    }
}
// public class NodesData
// {
//     public List<List<AStarNode>> Nodes;
//     public int XLength;
//     public int YLength;
//
//     public NodesData()
//     {
//         Nodes = new List<List<AStarNode>>();
//         XLength = Nodes.Count;
//         YLength = Nodes[0].Count;
//     }
//
//     public NodesData(List<List<AStarNode>> nodes)
//     {
//         Nodes = nodes;
//         XLength = Nodes.Count;
//         YLength = Nodes[0].Count;
//     }
// }