using System;
using System.Collections;
using System.Collections.Generic;
using NFTstorage;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class NFTDisplayInfo : MonoBehaviour
{
    [SerializeField]
     NFTData M_Data;

    [SerializeField]
    Text CoinsText;

  
    public void Update()
    {
        //If the Metadat from IPFS was retrieved successfully, assign the values to variables
        if (M_Data.DataCaptured)
        {
           

            List<NFTstorage.ERC721.Attribute> nAttributes = new List<NFTstorage.ERC721.Attribute>();
            nAttributes = M_Data.GetAttributes();

            string trait = nAttributes[0].trait_type;
            string value = nAttributes[0].value.ToString();

            //int value1 = int.Parse(value);

            // this will show the level of the attributes
            
            CoinsText.text = "Coins: " + value;


        }

        

    }
    
}
