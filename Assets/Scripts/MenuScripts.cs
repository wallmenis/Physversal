using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScripts : MonoBehaviour
{
    public GameObject settingsMenu;
    // Start is called before the first frame update
    void Start()
    {   

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainLevel");
    }

    public void goToSettings()
    {
        settingsMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void exitGame()
    {
        Application.Quit();
    }
}
