using System;
using System.Collections;
using System.Collections.Generic;
using Thirdweb;
using UnityEngine.UI; // needed when accessing text elements
using UnityEngine;

#if UNITY_WEBGL
public class SignMenu : MonoBehaviour
{
    private ThirdwebSDK sdk;
    public GameObject SuccessPopup;
    public Text responseText;
    public string message = "This is a test message to sign";

    void Start()
    {
        sdk = new ThirdwebSDK("goerli");
       
    }

    async public void OnSignMessage()
    {
        responseText.text = "Signing...";
        try {
            var data = await sdk.wallet.Authenticate("example.com");
            responseText.text = "Signature: " + data.payload.address.Substring(0, 6) + "...";
            SuccessPopup.SetActive(true);
        } catch (Exception e) {
            responseText.text = "Auth Error: " + e.Message;
        }
    }

   
}
#endif