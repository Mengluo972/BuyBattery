using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes : MonoBehaviour
{
    private List<List<NodeButton>> _nodeButtons;
    public UIPanel UIPanel;//引擎中初始化

    private void Start()   
    {
        //获取所有节点的索引
        //行优先存储
        _nodeButtons = new List<List<NodeButton>>();
        for (int i = 0; i < transform.childCount; i++)
        {
            List<NodeButton> list = new List<NodeButton>();
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                NodeButton nodeButton = transform.GetChild(i).GetChild(j).GetComponent<NodeButton>();
                nodeButton.UIPanel = UIPanel;
                list.Add(nodeButton);
            }
            _nodeButtons.Add(list);
        }
    }

    void Update()
    {
        
    }
    
    /// <summary>
    /// 读取数据，刷新UI显示界面
    /// </summary>
    /// <param name="nodes">总数据数组</param>
    /// <param name="startX">起始X下标</param>
    /// <param name="startY">起始Y下标</param>
    /// <param name="endX">末端X下标</param>
    /// <param name="endY">末端Y下标</param>
    public void UpdateNodes(List<List<AStarNode>> nodes,int startX,int startY,int endX,int endY)
    {   
        // print("1");
        if (endX>nodes.Count-1)
        {
            print("X轴超出范围");
            return;
        }
        if (endY>nodes[0].Count-1)
        {
            print("Y轴超出范围");
            return;
        }
        for (int i = startX%10; i <= endX%10; i++)
        {
            for (int j = startY%10; j <= endY%10; j++)
            {
                // print($"正在更改{startX + i},{startY + j}节点");
                // print($"更改为{nodes[startX + i][startY + j].Type}");
                _nodeButtons[j][i].SetNodeInfo(nodes[startX + i][startY + j].x, nodes[startX + i][startY + j].y,
                    nodes[startX + i][startY + j].Type);
            }
        }
    }
}
