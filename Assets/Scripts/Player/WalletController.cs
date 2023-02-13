using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class WalletController : MonoBehaviour
{
    private GameObject GlobalManager;
    public Text WalletText;
    public Text CoinsText;
    public Text LivesText;
   

    void Awake()
    {
        // finds global object
        GlobalManager = GameObject.FindGameObjectWithTag("Global");
        // sets texts
        WalletText.text = PlayerPrefs.GetString("Account");
        CoinsText.text = "Coins: " + GlobalManager.GetComponent<Global>().globalCoins.ToString();
        LivesText.text = "Lives: " + GlobalManager.GetComponent<Global>().globalLives.ToString();
  
    }

    // used when player collides with tagged objects
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            // checks if player has 0 lives on hit, if greater than 0, subract 1
            if (GlobalManager.GetComponent<Global>().globalLives > 0)
            {
            GlobalManager.GetComponent<Global>().globalLives -= 1;
            LivesText.text = "Lives: " + GlobalManager.GetComponent<Global>().globalLives.ToString();
            FindObjectOfType<AudioManager2>().Play("Cluck");
            SceneManager.LoadScene("Game");
            }
            else
            {
            FindObjectOfType<AudioManager2>().Play("Cluck");
            SceneManager.LoadScene("Game");
            }
        }
    }

    // used when player collides with tagged objects
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        // adds 1 to coin score, displays it and destroys the coin
        if (hit.transform.tag == "Coin")
        {
            FindObjectOfType<AudioManager2>().Play("Coin");
            FindObjectOfType<AudioManager2>().Play("Cluck");
            GlobalManager.GetComponent<Global>().globalCoins += 1;
            CoinsText.text = "Coins: " + GlobalManager.GetComponent<Global>().globalCoins.ToString();
            Destroy(hit.gameObject);
        }

      
    }

  

    
}
