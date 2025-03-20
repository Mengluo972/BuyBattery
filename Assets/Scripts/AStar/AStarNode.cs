using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    public enum Node_Type
    {
        Walk,
        Stop
    }
    public class AstarNode
    {
        //地图坐标
        public int x;
        public int y;
        //寻路消耗
        public int f;
        public int g;
        public int h;

        public AstarNode father;

        public Node_Type Type;

        public AstarNode(int x, int y, Node_Type type)
        {
            this.x = x;
            this.y = y;
            this.Type = type;
        }
    }
}
