using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomNodeManager : MonoBehaviour
{
    public int roomMaxX;//房间最大x值，注意这里不是最大x方向长度
    public int roomMaxY;//房间最大y值，注意这里不是最大y方向长度
    public RoomNode[] roomNodes;//用于引擎添加格子点，请从(0,0)到(maxX,0)到(1,0)到(1,max)的顺序拖入
    public RoomNode[][] RoomNodes;//二维数组，[x][y]，原点在左下方

    private void Start()
    {
        for (int j = 0; j < transform.childCount; j++)
        {
            RoomNode rn = transform.GetChild(j).AddComponent<RoomNode>();
            
        }
        if (roomMaxX==0||roomMaxY==0)
        {
            Debug.Log("房间X或Y值为0，请检查数据是否未填写");
            return;
        }
        int i = 0;
        for (int y = 0; y < roomMaxY; y++)
        {
            for (int x = 0; x < roomMaxX; x++)
            {
                RoomNodes[x][y] = roomNodes[i++];
            }
        }
    }
}
