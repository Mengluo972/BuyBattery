using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

public class MapInfoController : MonoBehaviour
{
    private List<List<AStarNode>> mapNodes = new List<List<AStarNode>>();
    public string mapName;
    public GameObject nodePrefab;
    public Transform originPosition;//应放在地图左下角格子处并且是格子的中心点(x.5,0,z.5)
    private int _mapX;
    private int _mapY;
    void Start()
    {
        TextAsset xmlFile =  Resources.Load<TextAsset>($"/MapInfo/{mapName}.xml");
        if (xmlFile!=null)
        {
            mapNodes = ParseXML<List<List<AStarNode>>>(xmlFile.text);
        }
        else return;

        _mapX = mapNodes.Count-1;
        _mapY = mapNodes[0].Count-1;

        AStarManager.Instance.InitMapInfoWithPosition(_mapX,_mapY,mapNodes);
        
        for (int i = 0; i <= _mapX; i++)
        {
            for (int j = 0; j <= _mapY; j++)
            {
                Vector3 position = new Vector3(originPosition.position.x+i,0,originPosition.position.z+j);
                GameObject nodeGameObject = Instantiate(nodePrefab,position,Quaternion.identity);
                Node node = nodeGameObject.GetComponent<Node>();
                node.AStarNode = mapNodes[i][j];
            }
        }
        
        
    }

    private T ParseXML<T>(string xmlContent)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        using StringReader reader = new StringReader(xmlContent);
        return (T)serializer.Deserialize(reader);
    }

    void Update()
    {
        
    }
}
