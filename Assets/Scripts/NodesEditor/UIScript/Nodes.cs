using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes : MonoBehaviour
{
    private List<List<NodeButton>> _nodeButtons;

    private void Start()
    {
        //获取所有节点的索引
        _nodeButtons = new List<List<NodeButton>>(){};
        for (int i = 0; i < transform.childCount; i++)
        {
            List<NodeButton> list = new List<NodeButton>();
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                list.Add(transform.GetChild(i).GetChild(j).GetComponent<NodeButton>());
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
        print("1");
        for (int i = startX; i <= endX; i++)
        {
            for (int j = startY; j <= endY; j++)
            {
                _nodeButtons[i][j].Type = nodes[i][j].Type;
                _nodeButtons[i][j].X = nodes[i][j].x;
                _nodeButtons[i][j].Y = nodes[i][j].y;
                _nodeButtons[i][j].UIUpdate();
                print($"{i}行{j}列的节点信息：{_nodeButtons[i][j].X},{_nodeButtons[i][j].Y},{_nodeButtons[i][j].Type}");
            }
        }
    }
}
