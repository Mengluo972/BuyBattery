using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Serialization;
/// <summary>
/// 实现地图信息的读取和寻路管理
/// </summary>
public class MapInfoController : MonoBehaviour
{
    private static List<List<AStarNode>> mapNodes = new List<List<AStarNode>>();//不会随着脚本的销毁而销毁
    private static List<List<Transform>> mapTransforms = new List<List<Transform>>();//不会随着脚本的销毁而销毁
    public string mapName;
    public GameObject nodePrefab;//预制体应当具有Node脚本
    public Transform originPosition;//应放在地图左下角格子处并且是格子的中心点(x.5,0,z.5)
    private static int _mapX;
    private static int _mapY;
    void Start()
    {
        // LoadSceneData($"MapInfo/{mapName}");
        LoadSceneData(6);
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
    /// 关卡文件名应为LevelX.xml，X为阿拉伯数字
    /// </summary>
    /// <param name="levelNum"></param>
    public static void LoadSceneData(int levelNum)
    {
        TextAsset xmlFile = Resources.Load<TextAsset>($"MapInfo/Level{levelNum}");
        if (xmlFile!=null)
        {
            mapNodes = ParseXML<List<List<AStarNode>>>(xmlFile.text);
            print("读取成功");
        }
        else return;

        Transform originPos = GameObject.Find("OriginPosition").transform;//应放在地图左下角格子处并且是格子的中心点(x.5,0,z.5)，名字为OriginPosition
        GameObject prefab = Resources.Load<GameObject>("MapInfo/NodePrefab/NodePrefab");//预制体放在Resources/MapInfo/NodePrefab文件夹下，名字为NodePrefab
        
        
        // int mapX = mapNodes.Count-1;
        // int mapY = mapNodes[0].Count-1;
        
        _mapX = mapNodes.Count-1;
        _mapY = mapNodes[0].Count-1;

        AStarManager.Instance.InitMapInfoWithPosition(_mapX,_mapY,mapNodes);
        
        for (int i = 0; i <= _mapX; i++)
        {
            for (int j = 0; j <= _mapY; j++)
            {
                Vector3 position = new Vector3(originPos.position.x+i,0,originPos.position.z+j);
                GameObject nodeGameObject = Instantiate(prefab,position,Quaternion.identity);
                Node node = nodeGameObject.GetComponent<Node>();
                node.AStarNode = mapNodes[i][j];
            }
        }
    }
    
    public static List<Transform> AStarNodeToTransforms(List<AStarNode> path)
    {
        List<Transform> transforms = new List<Transform>();
        foreach (var node in path)
        {
            transforms.Add(mapTransforms[node.x][node.y]);
        }

        return transforms;
    }
    
    void Update()
    {
        
    }
}
