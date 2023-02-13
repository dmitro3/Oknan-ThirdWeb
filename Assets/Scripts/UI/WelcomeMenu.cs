using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oknan
{
    public class WelcomeMenu : MonoBehaviour
    {
        public GameObject WelcomeMenuObject;
       
        // Start is called before the first frame update
        void Awake()
        {
            WelcomeMenuObject.SetActive(true);
            
            Time.timeScale = 0;
        }

        public void CloseWelcomeMenu()
        {
            FindObjectOfType<AudioManager2>().Play("Pop");
            WelcomeMenuObject.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
