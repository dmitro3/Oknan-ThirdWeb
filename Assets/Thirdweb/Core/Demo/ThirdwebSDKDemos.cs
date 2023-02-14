using System.Collections.Generic;
using Thirdweb;
using UnityEngine;
using UnityEngine.UI;

public class ThirdwebSDKDemos : MonoBehaviour
{

   
   public NFTData M_Data;

    private ThirdwebSDK sdk;
    private int count;
    public Text walletInfotext;
    public GameObject connectButtonsContainer;
    public GameObject walletInfoContainer;
    public Text resultText;

    public GameObject balanceSuccessPopup;
    public Text balanceResponseText;

    public GameObject signSuccessPopup;
    public Text signResponseText;

    public GameObject ERC721Popup;
    public Text ERC721ResponseText;

    public GameObject ERC1155Popup;
    public Text ERC1155ResponseText;

    void Start()
    {
        sdk = new ThirdwebSDK("goerli");
        InitializeState();
    }

    private void InitializeState()
    {
        connectButtonsContainer.SetActive(true);
        walletInfoContainer.SetActive(false);
    }

    void Update()
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
        connectButtonsContainer.SetActive(true);
        walletInfoContainer.SetActive(false);
    }

    private async void ConnectWallet(WalletProvider provider)
    {

        resultText.text = "Connecting...";
        try
        {
            string address = await sdk.wallet.Connect(new WalletConnection()
            {
                provider = provider,
                chainId = 5 // Switch the wallet Goerli on connection
            });
           
            connectButtonsContainer.SetActive(false);
            walletInfoContainer.SetActive(true);
            walletInfotext.text = address;
        }
        catch (System.Exception e)
        {
            walletInfotext.text = "Error (see console): " + e.Message;
        }
    }

    public async void OnBalanceClick()
    {
        balanceResponseText.text = "Loading...";
        try
        {
            CurrencyValue balance = await sdk.wallet.GetBalance();
            balanceResponseText.text = "Balance: " + balance.displayValue.Substring(0, 5) + " " + balance.symbol;
            balanceSuccessPopup.SetActive(true);
        }
        catch (System.Exception e)
        {
            balanceResponseText.text = "Balance Error: " + e.Message;
        }
       

    }

    public async void OnSignClick()
    {
        signResponseText.text = "Signing...";
        try
        {
            var data = await sdk.wallet.Authenticate("com.Lumever.Oknan");
            signResponseText.text = "Sign: " + data.payload.address.Substring(0, 6) + "...";
            signSuccessPopup.SetActive(true);
        }
        catch (System.Exception e)
        {
            signResponseText.text = "Auth Error: " + e.Message;
        }
    }

    public async void GetERC721()
    {

        try
        {
            // fetch single NFT
            var contract = sdk.GetContract("0x2e01763fA0e15e07294D74B63cE4b526B321E389"); // NFT Drop
            count++;
            ERC721ResponseText.text = "Fetching Token: " + count;
            NFT result = await contract.ERC721.Get(count.ToString());
            ERC721ResponseText.text = result.metadata.name + "\nowned by " + result.owner.Substring(0, 6) + "...";
            ERC721Popup.SetActive(true);
        }
        catch (System.Exception e)
        {
            ERC721ResponseText.text =  e.Message;
        }
       

        // fetch all NFTs
        // resultText.text = "Fetching all NFTs";
        // List<NFT> result = await contract.ERC721.GetAll(new Thirdweb.QueryAllParams() {
        //     start = 0,
        //     count = 10,
        // });
        // resultText.text = "Fetched " + result.Count + " NFTs";
        // for (int i = 0; i < result.Count; i++) {
        //     Debug.Log(result[i].metadata.name + " owned by " + result[i].owner);
        // }

        // custom function call
        // string uri = await contract.Read<string>("tokenURI", count);
        // fetchButton.text = uri;
    }

    public async void GetERC1155()
    {
        try
        {
            var contract = sdk.GetContract("0x86B7df0dc0A790789D8fDE4C604EF8187FF8AD2A");

            // Edition Drop
            // Fetch single NFT
            // count++;
            // resultText.text = "Fetching Token: " + count;
            // NFT result = await contract.ERC1155.Get(count.ToString());
            // resultText.text = result.metadata.name + " (x" + result.supply + ")";

            // fetch all NFTs
            ERC1155ResponseText.text = "Fetching all NFTs";
            List<NFT> result = await contract.ERC1155.GetAll();
            ERC1155ResponseText.text = "Fetched " + result.Count + " NFTs";
            ERC1155Popup.SetActive(true);
        }
        catch (System.Exception e)
        {
            ERC1155ResponseText.text = e.Message;
        }
        

    }

    public async void GetERC20()
    {
        var contract = sdk.GetContract("0xB4870B21f80223696b68798a755478C86ce349bE"); // Token
        resultText.text = "Fetching Token info";
        Currency result = await contract.ERC20.Get();
        CurrencyValue currencyValue = await contract.ERC20.TotalSupply();
        resultText.text = result.name + " (" + currencyValue.displayValue + ")";
    }

    public async void MintERC721()
    {
        resultText.text = "SigMinting... (needs minter role to generate signature)";

        string trait1;
        string value1;
        string trait2;
        string value2;
        string trait3;
        string value3;

        string theName = null;
        string theDescription = null;
        string TheImage = null;

        Dictionary<string, object> myDictionary = new Dictionary<string, object>();

        if (M_Data.DataCaptured)
        {
            theName = M_Data.GetName();
            theDescription = M_Data.GetName();
            TheImage = M_Data.GetImage();

            List<NFTstorage.ERC721.Attribute> nAttributes = new List<NFTstorage.ERC721.Attribute>();
            nAttributes = M_Data.GetAttributes();

            trait1 = nAttributes[0].trait_type;
            value1 = nAttributes[0].value.ToString();
            trait2 = nAttributes[1].trait_type;
            value2 = nAttributes[1].value.ToString();
            trait3 = nAttributes[2].trait_type;
            value3 = nAttributes[2].value.ToString();

            myDictionary.Add(trait1, value1);
            myDictionary.Add(trait2, value2);
            myDictionary.Add(trait3, value3);
        }

        // sig mint
        var contract = sdk.GetContract("0x8bFD00BD1D3A2778BDA12AFddE5E65Cca95082DF"); // NFT Collection
        var meta = new NFTMetadata()
        {
            name = theName,
            description = theDescription,
            image = TheImage,
            properties = myDictionary
    };
        string connectedAddress = await sdk.wallet.GetAddress();
        var payload = new ERC721MintPayload(connectedAddress, meta);
        try
        {
            var p = await contract.ERC721.signature.Generate(payload); // typically generated on the backend
            var result = await contract.ERC721.signature.Mint(p);
            resultText.text = "SigMinted tokenId: " + result.id;
        }
        catch (System.Exception e)
        {
            resultText.text = "Sigmint Failed (see console): " + e.Message;
        }
    }

    public async void MintERC1155()
    {
        Debug.Log("Claim button clicked");
        resultText.text = "Claiming...";

        // claim
        var contract = sdk.GetContract("0x86B7df0dc0A790789D8fDE4C604EF8187FF8AD2A"); // Edition Drop
        var canClaim = await contract.ERC1155.claimConditions.CanClaim("0", 1);
        if (canClaim)
        {
            try
            {
                var result = await contract.ERC1155.Claim("0", 1);
                var newSupply = await contract.ERC1155.TotalSupply("0");
                resultText.text = "Claim successful! New supply: " + newSupply;
            }
            catch (System.Exception e)
            {
                resultText.text = "Claim Failed: " + e.Message;
            }
        }
        else
        {
            resultText.text = "Can't claim";
        }

        // sig mint additional supply
        // var contract = sdk.GetContract("0xdb9AAb1cB8336CCd50aF8aFd7d75769CD19E5FEc"); // Edition
        // var payload = new ERC1155MintAdditionalPayload("0xE79ee09bD47F4F5381dbbACaCff2040f2FbC5803", "1");
        // payload.quantity = 3;
        // var p = await contract.ERC1155.signature.GenerateFromTokenId(payload);
        // var result = await contract.ERC1155.signature.Mint(p);
        // resultText.text = "sigminted tokenId: " + result.id;
    }

    public async void MintERC20()
    {
        resultText.text = "Minting... (needs minter role)";

        // Mint
        var contract = sdk.GetContract("0xB4870B21f80223696b68798a755478C86ce349bE"); // Token
        try
        {
            var result = await contract.ERC20.Mint("1.2");
            resultText.text = "mint successful";
        }
        catch (System.Exception e)
        {
            resultText.text = "Mint failed (see console): " + e.Message;
        }

        // sig mint
        // var contract = sdk.GetContract("0xB4870B21f80223696b68798a755478C86ce349bE"); // Token
        // var payload = new ERC20MintPayload("0xE79ee09bD47F4F5381dbbACaCff2040f2FbC5803", "3.2");
        // var p = await contract.ERC20.signature.Generate(payload);
        // await contract.ERC20.signature.Mint(p);
        // resultText.text = "sigminted currency successfully";
    }

    public async void GetListing()
    {
        resultText.text = "Fetching listing...";

        // fetch listings
        var marketplace = sdk.GetContract("0xC7DBaD01B18403c041132C5e8c7e9a6542C4291A").marketplace; // Marketplace
        var result = await marketplace.GetAllListings();
        resultText.text = "Listing count: " + result.Count + " | " + result[0].asset.name + "(" + result[0].buyoutCurrencyValuePerToken.displayValue + ")";
    }

    public async void BuyListing()
    {
        resultText.text = "Buying...";

        // buy listing
        var marketplace = sdk.GetContract("0xC7DBaD01B18403c041132C5e8c7e9a6542C4291A").marketplace; // Marketplace
        try
        {
            var result = await marketplace.BuyListing("0", 1);
            resultText.text = "NFT bought successfully";
        }
        catch (System.Exception e)
        {
            resultText.text = "Error Buying listing (see console): " + e.Message;
        }
    }

    public async void Deploy()
    {
        resultText.text = "Deploying...";

        // deploy nft collection contract
        try
        {
            var address = await sdk.deployer.DeployNFTCollection(new NFTContractDeployMetadata
            {
                name = "Unity Collection",
                primary_sale_recipient = await sdk.wallet.GetAddress(),
            });
            resultText.text = "Deployed: " + address;
        }
        catch (System.Exception e)
        {
            resultText.text = "Deploy Failed (see console): " + e.Message;
        }
    }

    public async void CustomContract()
    {
        var contract = sdk.GetContract("0x62Cf5485B6C24b707E47C5E0FB2EAe7EbE18EC4c");
        try
        {
            // custom read
            resultText.text = "Fetching contract data...";
            var result = await contract.Read<string>("uri", 0);
            resultText.text = "Read custom token uri: " + result;
            // custom write
            await contract.Write("claimKitten");
            // custom write with transaction overrides
            // await contract.Write("claim", new TransactionRequest
            // {
            //     value = "0.05".ToWei() // 0.05 ETH
            // }, "0xE79ee09bD47F4F5381dbbACaCff2040f2FbC5803", 0, 1);
            resultText.text = "Custom contraact call successful";
        }
        catch (System.Exception e)
        {
            resultText.text = "Error calling contract (see console): " + e.Message;
        }
    }
}
