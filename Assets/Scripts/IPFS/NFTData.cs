using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using NFTstorage;
using TMPro;
using NFTstorage.ERC721;
using Object = System.Object;
using Random = UnityEngine.Random;

public class NFTData : MonoBehaviour
{
    //This class receives a string of CID from IPFS
    string path;
    
    TheData2 m_Data;
    bool m_DataCaptured;

    public bool DataCaptured
    {
        get => m_DataCaptured;
    }

    public TMP_InputField theCid;

    public void  NFTMetadata()
    {
        StartCoroutine(GetData());
    }

    IEnumerator GetData()
    {
        string cid = theCid.text;
        path = Helper.GenerateGatewayPath("https://" + cid,
               Constants.GatewaysSubdomain[0], true);
        string url = path;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error occured with web request");
            }
            else
            {
                //Convert the NFT MetaData from IPFS to a JSON File
                m_Data = JsonConvert.DeserializeObject<TheData2>(webRequest.downloadHandler.text);
                m_DataCaptured = true;
                Debug.Log("Data captured successfully");
            }
        }
    }

    //Get the name of the NFT
    public string GetName()
    {
        if (m_DataCaptured)
        {
            return m_Data.name;
        }
        else
        {
            return "Nothing";
        }
    }

    //Get the descritption of the NFT
    public string GetDescription()
    {
        if (m_DataCaptured)
        {
            return m_Data.description;
        }
        else
        {
            return "Nothing";
        }
    }

    //Get the image of the NFT
    public string GetImage()
    {
        if (m_DataCaptured)
        {
            return m_Data.external_url;
        }
        else
        {
            return "Nothing";
        }
    }



    //Get the list of the attributes of the NFT
    public List<NFTstorage.ERC721.Attribute> GetAttributes()
    {
        if (m_DataCaptured)
        {
            return m_Data.attributes;
        }
        else
        {
            return null;
        }
    }
    
}



public class TheData2
{    
public string description;
public string external_url;
public string name;
public List<NFTstorage.ERC721.Attribute> attributes;

}
