using System.Collections;
using System.Collections.Generic;
using System;
using Models;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MintNFT : MonoBehaviour
{

    // Start is called before the first frame update
    private string chain = "ethereum";
    private string network = "goerli"; // mainnet ropsten kovan rinkeby goerli
    private string account;
    public TMP_Text theCID;
    private string cid;
    private string to;
    public GameObject SuccessPopup;
    public TMP_Text responseText;

    async public void MintButtonPK()
    {
        account = PlayerPrefs.GetString("Account"); // the account calling the mint functions      
        to = account;
        cid = theCID.text;
        string type721 = "1155";
        CreateMintModel.Response nftResponse = await EVM.CreateMint(chain, network, account, to, cid, type721);
        // connects to user's browser wallet (metamask) to send a transaction

        try
        {
           // string response = await Web3GL.SendTransactionData(nftResponse.tx.to, nftResponse.tx.value, nftResponse.tx.gasPrice, nftResponse.tx.gasLimit, nftResponse.tx.data);
          //  print("Response: " + response);
            SuccessPopup.SetActive(true);
            responseText.text = "Success!";
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }
}