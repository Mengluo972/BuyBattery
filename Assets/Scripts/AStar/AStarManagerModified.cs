using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 弃用
/// </summary>
public class AStarManagerModified
{
    private static AStarManagerModified instance;

    public static AStarManagerModified Instance
    {
        get
        {
            if (instance==null)
            {
                instance = new AStarManagerModified();
            }

            return instance;
        } 
    }
    //格子对象容器
    // public AStarNode[,][] Nodes;//地图格子数组，第一个维度为地图坐标，第二个维度为地图列表（同一个地图中多个小地图）
    public List<AStarNode[,]> Nodes = new();
    //地图宽高
    // public int[] mapW;
    public List<int> mapW = new();
    // public int[] mapH;
    public List<int> mapH = new();
    //开启列表、关闭列表
    private List<AStarNode> openList = new();
    private List<AStarNode> closeList = new();

    // public void InitMapInfo(int w, int h)
    // {
    //     mapH = h;
    //     mapW = w;
    //     //数组初始化
    //     Nodes = new AStarNode[w, h];
    //     //格子声明
    //     for (int i = 0; i < w; i++)
    //     {
    //         for (int j = 0; j < h; j++)
    //         {
    //             AStarNode node = new AStarNode(i, j, Random.Range(0, 100) < 20 ? Node_Type.Stop : Node_Type.Walk);
    //             Nodes[i, j] = node;
    //         }
    //     }
    // }

    public List<AStarNode> FindPath(Vector2 startPos, Vector2 endPos,int mapIndex)
    {
        if (startPos.x<0||startPos.x>=mapW[mapIndex]||endPos.x<0||endPos.x>=mapW[mapIndex]||
            startPos.y<0||startPos.x>=mapH[mapIndex]||endPos.y<0||endPos.y>=mapH[mapIndex])
        {
            Debug.Log("起点或终点在地图格子范围外");
            return null;
        }

        AStarNode start = Nodes[mapIndex][(int)startPos.x, (int)startPos.y];
        AStarNode end = Nodes[mapIndex][(int)endPos.x, (int)endPos.y];
        //起点或终点是否有阻挡
        if (start.Type==Node_Type.Stop||end.Type==Node_Type.Stop)   
        {
            Debug.Log("起点或者终点有阻挡");
            return null;
        }
        //清空开启和关闭列表
        closeList.Clear();
        openList.Clear();
        
        //把起点放入关闭列表
        start.father = null;
        start.f = 0;
        start.g = 0;
        start.h = 0;
        closeList.Add(start);
        
        while (true)
        {
            //节点添加
            //左上方
            AddToOpenList(start.x-1,start.y+1,14,start,end,mapIndex);
            //正上方
            AddToOpenList(start.x,start.y+1,10,start,end,mapIndex);
            //右上方
            AddToOpenList(start.x+1,start.y+1,14,start,end,mapIndex);
            //左方
            AddToOpenList(start.x-1,start.y,10,start,end,mapIndex);
            //右方
            AddToOpenList(start.x+1,start.y,10,start,end,mapIndex);
            //左下方
            AddToOpenList(start.x-1,start.y-1,14,start,end,mapIndex);
            //正下方
            AddToOpenList(start.x,start.y-1,10,start,end,mapIndex);
            //右下方
            AddToOpenList(start.x+1,start.y-1,14,start,end,mapIndex);
            
            //是否为死路
            if (openList.Count == 0)
            {
                Debug.Log("是死路");
                return null;
            }
            //对开启列表进行排序，寻找f最小的点
            openList.Sort(SortOpenList);
            
            //消耗最小的点放入关闭列表中
            closeList.Add(openList[0]);
            start = openList[0];
            if (start.x==end.x&&start.y==end.y)
            {
                //该点为终点，寻路结束
                List<AStarNode> path = new List<AStarNode>();
                path.Add(end);
                while (end.father!=null)
                {
                    path.Add(end.father);
                    end = end.father;
                }
                path.Reverse();
                return path;
            }
            openList.RemoveAt(0);
        }
    }

    private int SortOpenList(AStarNode a, AStarNode b)
    {
        if (a.f>b.f)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    private void AddToOpenList(int x, int y, int g, AStarNode father, AStarNode end,int mapIndex)
    {
        //节点在地图外
        if(x<0||x>=mapW[mapIndex]||y<0||y>mapH[mapIndex]) return;
        AStarNode node = Nodes[mapIndex][x, y];
        //可能的情况：节点为空，节点为障碍物，节点已添加到开启列表或者关闭列表中
        if (node == null || node.Type == Node_Type.Stop || closeList.Contains(node) || openList.Contains(node)) return;

        //对节点的f g h 进行计算
        node.father = father;
        node.g = node.father.g + g;
        node.h = Mathf.Abs(end.x - node.x) * 10 + Mathf.Abs(end.y - node.y) * 10;
        node.f = node.g + node.h;
        
        //把节点添加到开启列表
        openList.Add(node);
    }
}
