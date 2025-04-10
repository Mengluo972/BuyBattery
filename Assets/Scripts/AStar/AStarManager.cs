using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 不要给这个类添加新的对外方法，仅提供FindPath方法
/// </summary>
public class AStarManager
{
    private static AStarManager instance;

    public static AStarManager Instance
    {
        get
        {
            if (instance==null)
            {
                instance = new AStarManager();
            }

            return instance;
        } 
    }
    //格子对象容器
    public AStarNode[,] Nodes;
    //地图宽高
    private int mapW;//从1开始，有几个格子就填几
    private int mapH;//从1开始，有几个格子就填几
    //开启列表、关闭列表
    private List<AStarNode> openList = new List<AStarNode>();
    private List<AStarNode> closeList = new List<AStarNode>();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="w">横向格子数量</param>
    /// <param name="h">纵向格子数量</param>
    /// <param name="nodes"></param>
    public void InitMapInfoWithLength(int w, int h,List<List<AStarNode>> nodes)
    {
        mapH = h;
        mapW = w;
        //数组初始化
        Nodes = new AStarNode[w, h];
        //格子声明
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                Nodes[i, j] = nodes[i][j];
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="maxX">X轴格子坐标</param>
    /// <param name="maxY">Y轴格子坐标</param>
    /// <param name="nodes"></param>
    public void InitMapInfoWithPosition(int maxX, int maxY, List<List<AStarNode>> nodes)
    {
        mapH = maxY+1;
        mapW = maxX+1;
        //数组初始化
        Nodes = new AStarNode[maxX+1, maxY+1];
        //格子声明
        for (int i = 0; i <= maxX; i++)
        {
            for (int j = 0; j <= maxY; j++)
            {
                Nodes[i, j] = nodes[i][j];
            }
        }
    }
    //这个向量的坐标一定要是处理后的数据，使用原始数据会出错
    //注意线程安全
    public List<AStarNode> FindPath(Vector2 startPos, Vector2 endPos)
    {
        // Debug.Log($"startPos.x<0结果为：{startPos.x<0} startPos.x>=mapW结果为：{startPos.x>=mapW}");
        // Debug.Log($"endPos.x<0结果为：{endPos.x<0} endPos.x>=mapW结果为：{endPos.x>=mapW}");
        // Debug.Log($"startPos.y<0结果为：{startPos.y<0} startPos.y>=mapH结果为：{startPos.y>=mapH}");
        // Debug.Log($"endPos.y<0结果为：{endPos.y<0} endPos.y>=mapH结果为：{endPos.y>=mapH}");
        if (startPos.x<0||startPos.x>=mapW||endPos.x<0||endPos.x>=mapW||
            startPos.y<0||startPos.x>=mapH||endPos.y<0||endPos.y>=mapH)
        {
            Debug.Log($"起点或终点在地图格子范围外，起点坐标为：{startPos.x},{startPos.y} 终点坐标为：{endPos.x},{endPos.y}");
            // Debug.Log($"startPos.x<0结果为：{startPos.x<0} startPos.x>=mapW结果为：{startPos.x>=mapW}");
            // Debug.Log($"endPos.x<0结果为：{endPos.x<0} endPos.x>=mapW结果为：{endPos.x>=mapW}");
            // Debug.Log($"startPos.y<0结果为：{startPos.y<0} startPos.y>=mapH结果为：{startPos.y>=mapH}");
            // Debug.Log($"endPos.y<0结果为：{endPos.y<0} endPos.y>=mapH结果为：{endPos.y>=mapH}");
            Debug.Log($"地图的范围为：宽度{mapW}*高度{mapH}");
            return null;
        }

        AStarNode start = Nodes[(int)startPos.x, (int)startPos.y];
        AStarNode end = Nodes[(int)endPos.x, (int)endPos.y];
        //起点或终点是否有阻挡
        if (start.Type==Node_Type.Stop||end.Type==Node_Type.Stop)   
        {
            Debug.Log("起点或者终点有阻挡");
            Debug.Log($"起点坐标：{start.x},{start.y} 终点坐标：{end.x},{end.y}");
            if(start.Type==Node_Type.Stop)
                Debug.Log("起点有阻挡");
            if(end.Type==Node_Type.Stop)
                Debug.Log("终点有阻挡");
            Time.timeScale = 0;
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
            AddToOpenList(start.x-1,start.y+1,14,start,end);
            //正上方
            AddToOpenList(start.x,start.y+1,10,start,end);
            //右上方
            AddToOpenList(start.x+1,start.y+1,14,start,end);
            //左方
            AddToOpenList(start.x-1,start.y,10,start,end);
            //右方
            AddToOpenList(start.x+1,start.y,10,start,end);
            //左下方
            AddToOpenList(start.x-1,start.y-1,14,start,end);
            //正下方
            AddToOpenList(start.x,start.y-1,10,start,end);
            //右下方
            AddToOpenList(start.x+1,start.y-1,14,start,end);
            
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

    private void AddToOpenList(int x, int y, int g, AStarNode father, AStarNode end)
    {
        //节点在地图外
        if(x<0||x>=mapW||y<0||y>mapH) return;
        AStarNode node = Nodes[x, y];
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
