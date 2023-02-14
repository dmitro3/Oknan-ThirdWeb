using System.Collections;
using System.Collections.Generic;
using Thirdweb;
using Newtonsoft.Json;
using UnityEngine.UI; // needed when accessing text elements
using UnityEngine;

public class BalanceMenu : MonoBehaviour
{
    private ThirdwebSDK sdk;
    public GameObject SuccessPopup;
    public Text responseText;

    void Start()
    {
        sdk = new ThirdwebSDK("goerli");
      
    }

    async public void GetBalance()
    {
        responseText.text = "Loading...";
        CurrencyValue balance = await sdk.wallet.GetBalance();
        responseText.text = "Balance: " + balance.displayValue.Substring(0, 5) + " " + balance.symbol;
        SuccessPopup.SetActive(true);
    }


}
