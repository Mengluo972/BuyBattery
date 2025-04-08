// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Unity.VisualScripting;
// using UnityEngine;
// /// <summary>
// /// 弃用
// /// </summary>
// public class RoomNodeManager : MonoBehaviour
// {
//     //使用须知：格子与格子之间的collider请保持一个角色collider的距离，防止重复检测玩家，否则会产生不可预知的bug
//     public int roomMaxX;//房间最大x值，注意这里不是最大x方向长度
//     public int roomMaxY;//房间最大y值，注意这里不是最大y方向长度
//     public GameObject[] roomNodes;//用于引擎添加格子点，请从(0,0)到(maxX,0)到(1,0)到(1,max)的顺序拖入
//     public RoomNode[,] RoomNodes;//二维数组，[x][y]，原点在左下方
//
//     private void Start()
//     {
//         if (roomMaxX==0||roomMaxY==0)
//         {
//             Debug.Log("房间X或Y值为0，请检查数据是否未填写");
//             return;
//         }
//         RoomNodes = new RoomNode[roomMaxX+1, roomMaxY+1];
//         // AStarManager.Instance.Nodes = new AStarNode[roomMaxX+1, roomMaxY+1][1];
//         AStarManagerModified.Instance.Nodes.Add(new AStarNode[roomMaxX,roomMaxY]);//这个地图存在两个小房间
//         AStarManagerModified.Instance.Nodes.Add(new AStarNode[roomMaxX,roomMaxY]);
//         // AStarManager.Instance.Nodes[0] = new AStarNode[roomMaxX, roomMaxY];
//         AStarManagerModified.Instance.mapW.Add(roomMaxX+1);
//         AStarManagerModified.Instance.mapH.Add(roomMaxY+1);
//         int i = 0;
//         for (int y = 0; y <= roomMaxY; y++)
//         {
//             for (int x = 0; x <= roomMaxX; x++)
//             {
//                 RoomNodes[x,y] = roomNodes[i++].GetComponent<RoomNode>();
//                 AStarManagerModified.Instance.Nodes[0][x, y] = new AStarNode(x, y, RoomNodes[x, y].isWalkable ? Node_Type.Walk : Node_Type.Stop);
//             }
//         }
//         
//     }
//     
// }
