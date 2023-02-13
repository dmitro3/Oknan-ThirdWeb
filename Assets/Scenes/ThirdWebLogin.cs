using UnityEngine;
using Thirdweb;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ThirdWebLogin : MonoBehaviour
    {


   

    private int expirationTime;
    private string account;


        private ThirdwebSDK sdk;
        private int count;
        public Text resultText;

    // Start is called before the first frame update
    void Start()
        {
            sdk = new ThirdwebSDK("goerli");
            InitializeState();
        }

        private void InitializeState()
        {
            
        }

    public void Dropdown_IndexChange(int index)
    {

        if (index == 1) MetamaskLogin();
        if (index == 2) CoinbaseWalletLogin();
        if (index == 3) WalletConnectLogin();
        if (index == 4) MagicAuthLogin();
    }


    public void MetamaskLogin()
    {
        ConnectWallet(WalletProvider.MetaMask);
    }

    public void CoinbaseWalletLogin()
    {
        ConnectWallet(WalletProvider.CoinbaseWallet);
    }

    public void WalletConnectLogin()
    {
        ConnectWallet(WalletProvider.WalletConnect);
    }

    public void MagicAuthLogin()
    {
        // Requires passing a magic.link API key in the SDK options:
        // sdk = new ThirdwebSDK("goerli", new ThirdwebSDK.Options()
        // {
        //     wallet = new ThirdwebSDK.WalletOptions()
        //     {
        //         appName = "Thirdweb SDK Demo",
        //         extras = new Dictionary<string, object>()
        //         {
        //             {"apiKey", "your_api_key"}
        //         }
        //     }
        // });
        ConnectWallet(WalletProvider.MagicAuth);
    }

    public async void DisconnectWallet()
    {
        await sdk.wallet.Disconnect();
        
    }

    private async void ConnectWallet(WalletProvider provider)
    {
       
        resultText.text = "Connecting...";
        try
        {


            account = await sdk.wallet.Connect(new WalletConnection()
            {
                provider = provider,
                chainId = 5 // Switch the wallet Goerli on connection
            });

            while (account == "")
            {
                await new WaitForSeconds(1f);
                account = await sdk.wallet.Connect(new WalletConnection()
                {
                    provider = provider,
                    chainId = 5 // Switch the wallet Goerli on connection
                });
            };

            // save account for next scene
            PlayerPrefs.SetString("Account", account);
            
            // load next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


            //walletInfotext.text = "Connected as: " + address;
        }
        catch (System.Exception e)
        {
           // walletInfotext.text = "Error (see console): " + e.Message;
        }
    }



}

