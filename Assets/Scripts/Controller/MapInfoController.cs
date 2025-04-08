using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.Serialization;
/// <summary>
/// 实现地图信息的读取和寻路管理,所有地图信息也存在该类中，AStarManager只提供FindPath方法
/// </summary>
public class MapInfoController : MonoBehaviour
{
    private static List<List<AStarNode>> mapNodes = new List<List<AStarNode>>();//不会随着脚本的销毁而销毁
    private static List<List<Transform>> mapTransforms = new List<List<Transform>>();//不会随着脚本的销毁而销毁
    private static Transform _originPosition;
    private static int _mapX;
    private static int _mapY;
    
    //下面是临时测试用，具体使用时请使用静态方法
    public string mapName;
    public GameObject nodePrefab;//预制体应当具有Node脚本
    public Transform originPosition;//应放在地图左下角格子处并且是格子的中心点(x.5,0,z.5)
    // public GameObject cube;//障碍物标记
    
    //正式使用时不要用Start读取地图信息
    void Start()
    {
        // LoadSceneData($"MapInfo/{mapName}");
        LoadSceneData(2);
    }

    private static T ParseXML<T>(string xmlContent)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        using StringReader reader = new StringReader(xmlContent);
        return (T)serializer.Deserialize(reader);
    }

    // public void LoadSceneData(string dataName)
    // {
    //     TextAsset xmlFile =  Resources.Load<TextAsset>($"MapInfo/{mapName}");
    //     if (xmlFile!=null)
    //     {
    //         mapNodes = ParseXML<List<List<AStarNode>>>(xmlFile.text);
    //         // print("读取成功");
    //     }
    //     else return;
    //
    //     _mapX = mapNodes.Count-1;
    //     _mapY = mapNodes[0].Count-1;
    //
    //     AStarManager.Instance.InitMapInfoWithPosition(_mapX,_mapY,mapNodes);
    //     
    //     for (int i = 0; i <= _mapX; i++)
    //     {
    //         for (int j = 0; j <= _mapY; j++)
    //         {
    //             Vector3 position = new Vector3(originPosition.position.x+i,0,originPosition.position.z+j);
    //             GameObject nodeGameObject = Instantiate(nodePrefab,position,Quaternion.identity);
    //             Node node = nodeGameObject.GetComponent<Node>();
    //             node.AStarNode = mapNodes[i][j];
    //         }
    //     }
    // }
    
    /// <summary>
    /// 关卡文件名应为LevelBoxX.xml，X为阿拉伯数字
    /// </summary>
    /// <param name="levelNum"></param>
    public static void LoadSceneData(int levelNum)
    {
        TextAsset xmlFile = Resources.Load<TextAsset>($"MapInfo/LevelBox{levelNum}");
        if (xmlFile!=null)
        {
            //清空数据
            mapNodes.Clear();
            mapNodes = ParseXML<List<List<AStarNode>>>(xmlFile.text);
            print("读取成功");
        }
        else
        {
            print("数据文件不存在");
            return;
        }
        
        //考虑到使用了Add方法，这里进行数据清空
        mapTransforms.Clear();
        
        Transform originPos = GameObject.Find("OriginPosition").transform;//应放在地图左下角格子处并且是格子的中心点(x.5,0,z.5)，名字为OriginPosition
        _originPosition = originPos;
        GameObject prefab = Resources.Load<GameObject>("MapInfo/NodePrefab/NodePrefab");//预制体放在Resources/MapInfo/NodePrefab文件夹下，名字为NodePrefab
        //测试用,障碍标记
        GameObject cube = Resources.Load<GameObject>("MapInfo/NodePrefab/Cube");
        
        // int mapX = mapNodes.Count-1;
        // int mapY = mapNodes[0].Count-1;
        
        _mapX = mapNodes.Count-1;
        _mapY = mapNodes[0].Count-1;

        AStarManager.Instance.InitMapInfoWithPosition(_mapX,_mapY,mapNodes);
        
        //与数据存储同步,列优先读取
        for (int i = 0; i <= _mapX; i++)
        {
            List<Transform> list = new List<Transform>();
            for (int j = 0; j <= _mapY; j++)
            {
                Vector3 position = new Vector3(originPos.position.x+i,0,originPos.position.z+j);
                GameObject nodeGameObject = Instantiate(prefab,position,Quaternion.identity);
                list.Add(nodeGameObject.transform);
                Node node = nodeGameObject.GetComponent<Node>();
                node.AStarNode = mapNodes[i][j];
                // Instantiate(cube, position, Quaternion.identity);
                if (node.AStarNode.Type==Node_Type.Stop)
                {
                    Instantiate(cube, position, Quaternion.identity);
                    Debug.Log("检测到障碍物，坐标为："+position);
                }
            }
            mapTransforms.Add(list);
        }
    }
    
    //进行坐标转换，将网格坐标转换为世界坐标
    public static List<Transform> AStarNodeToTransforms(List<AStarNode> path)
    {
        List<Transform> transforms = new List<Transform>();
        foreach (var node in path)
        {
            // print($"节点信息 x:{node.x},y:{node.y}");
            transforms.Add(mapTransforms[node.x][node.y]);
        }

        return transforms;
    }

    public static bool BarrierCheck(List<AStarNode> path)
    {
        return path.All(node => node.Type != Node_Type.Stop);
    }
    void Update()
    {
        
    }
}
