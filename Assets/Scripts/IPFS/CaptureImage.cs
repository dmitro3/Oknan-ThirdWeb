using UnityEngine.UI;
using System.Collections.Generic;
using NFTstorage;
using UnityEngine;
using TMPro;
using System;


public class CaptureImage : MonoBehaviour
{
    [SerializeField]
    private Button CaptureBtton;
    [SerializeField]
    private GameObject Panel;
    [SerializeField]
    private SetMetaData setMetaData;

    public TMP_Text hashText;


    private string theName = "Oknan Idol";
    private string description = "This is an award for Being a champion in Oknan Royale Game";

    [SerializeField]
    private NFTstorage.ERC721.NftMetaData nFTMetadata;

    
    private NFTstorage.ERC721.Attribute attribute1 = new NFTstorage.ERC721.Attribute();
    private NFTstorage.ERC721.Attribute attribute2 = new NFTstorage.ERC721.Attribute();
    private NFTstorage.ERC721.Attribute attribute3 = new NFTstorage.ERC721.Attribute();

    // Start is called before the first frame update
    void Start()
    {
        CaptureBtton.onClick.AddListener(TakePicture);
    }

    private void TakePicture()
    {
        //Make the panel invisible while capturing the NFT
        Panel.SetActive(false);     
        Debug.Log("Button Clicked");
        StartCoroutine(Helper.TakeScreenShot(CallBackOnCamera));
    }

    private void CallBackOnCamera(byte[] myBytes)
    {
        StartCoroutine(NetworkManager.UploadObject(CallBackOnUpload, myBytes));
    }

    private void CallBackOnUpload(DataResponse dataResponse)
    {
        if (!dataResponse.Success)
        {
            Debug.LogError("Upload Failed");
            Panel.SetActive(true);
            return;

        }
        if (dataResponse.Values != null)
        {
            var path = Helper.GenerateGatewayPath(dataResponse.Values[0].cid, 
                Constants.GatewaysSubdomain[0], true);
           
            Debug.Log("Image found at " + path);

            nFTMetadata.SetIPFS(dataResponse.Values[0].cid);
            nFTMetadata.SetExternalURL(path);
            nFTMetadata.SetDescription(description);
            nFTMetadata.SetName(theName);

            //Intatiates different attributes from the NFTstorage.ERC721.Attribute class
            attribute1.SetAttributes("Green pand Fader", "95");
            attribute2.SetAttributes("Biodervesity Keeper", "25");
            attribute3.SetAttributes("Forester", "13");

            // set the attributes to a list 
            List<NFTstorage.ERC721.Attribute> attributeList = 
                new List<NFTstorage.ERC721.Attribute> {attribute1, attribute2, attribute3 };

            nFTMetadata.SetAttributesList(attributeList);


            byte[] bytes = Helper.ERC721MetaDataToBytes(nFTMetadata);
            StartCoroutine(NetworkManager.UploadObject(CallBackOnMetaData, bytes));

        }

        
        // Make panel visible sfter taking the image
        Panel.SetActive(true);

    }

    private void CallBackOnMetaData(DataResponse dataResponse)
    {

        if (!dataResponse.Success)
        {
            Debug.LogError("Upload Metadata Failed");
            Panel.SetActive(true);
            return;

        }
        if (dataResponse.Values != null)
        {
            var path = Helper.GenerateGatewayPath(dataResponse.Values[0].cid,
                Constants.GatewaysSubdomain[0], true);
            hashText.text = dataResponse.Values[0].cid;
            Debug.Log("Image found at " + path);
            Debug.Log("Uploaded MetaData Successfully");
        }
    }
}
