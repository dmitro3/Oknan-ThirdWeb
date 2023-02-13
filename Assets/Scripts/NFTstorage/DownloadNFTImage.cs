using System.Collections;
using UnityEngine.UI;
using NFTstorage;
using UnityEngine;
using System;

public class DownloadNFTImage : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private Image image;
    [SerializeField]
    private string cid;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(DownloadImage);
    }

    private void DownloadImage() 
    {
        string path = Helper.GenerateGatewayPath(cid, Constants.GatewaysSubdomain[0], true);
        StartCoroutine(NetworkManager.DownloadImage(DownloadCompleted, path));
    }

    private void DownloadCompleted(DownloadResponse downloadResponse)
    {
        if (downloadResponse.IsSuccess == true)
        {
            Debug.Log("Download has successful");
            var myTexture = downloadResponse.Texture2D;
            image.sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), 
                new Vector2(0.5f, 0.5f), 100f);
        }
        else
        {
            Debug.LogError("Download has failed");
        }

    }
}
