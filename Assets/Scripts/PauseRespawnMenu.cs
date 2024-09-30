using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseRespawnMenu : MonoBehaviour
{
    bool isPaused;
    //bool requestForUnpause;
    public GameObject pauseMenu;
    public GameObject killScreen;
    public GameObject canBhop;
    public GameObject settingsMenu;
    public GameObject endMenu;
    public GameObject pickMe;
    public GameObject player;
    public GameTimerScript gts;
    Vector3 respawnPoint;
    Quaternion respawnRotation;
    PlayerController playerController;
    PlayerInventory playerInventory;
    bool blockPause;
    // Start is called before the first frame update
    void Start()
    {
        blockPause = false;
        playerController = player.GetComponent<PlayerController>();
        playerInventory = player.GetComponent<PlayerInventory>();
        isPaused = false;
        respawnPoint = player.transform.position;
        respawnRotation = player.transform.rotation;
        //requestForUnpause = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.deltaTime);
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!blockPause)
            {
                togglePause();
            }
        }
        if (playerController.getIfCrouching())
        {
            canBhop.SetActive(true);

        }
        else
        {
            canBhop.SetActive(false);
        }

        if(playerInventory.getIsSeeingPickable() && playerInventory.getItemList().Count < playerInventory.inventorySize)
        {
            pickMe.SetActive(true);
        }
        else
        {
            pickMe.SetActive(false);
        }

        //if (requestForUnpause)
        //{
        //    pauseMenu.SetActive(false);
        //    isPaused = false;
        //    Time.timeScale = 1f;
        //}
        if (Input.GetMouseButton(0) && Application.isFocused && !isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    
    public bool getIsPaused()
    {
        return isPaused;
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void returnToMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        
    }

    public void togglePause()
    {
        if (isPaused)
        {
            Debug.Log("UNPAUSED");
            //requestForUnpause = true;
            isPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(false);
            gts.startTimer();
        }
        else
        {
            Debug.Log("PAUSED");
            isPaused = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            gts.stopTimer();
            
        }
    }

    public void playerKilled()
    {
        isPaused = true;
        blockPause = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        killScreen.SetActive(true);
        gts.stopTimer();
    }

    public void respawn()
    {
        player.transform.position = respawnPoint;
        player.transform.rotation = respawnRotation;
        isPaused = false;
        blockPause = false;
        Time.timeScale = 1f;
        playerController.resetVelocity();
        killScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        gts.startTimer();
    }

    public void gotorespawn()
    {
        Vector3 tmp = player.GetComponent<Rigidbody>().velocity;
        player.transform.position = respawnPoint;
        player.GetComponent<Rigidbody>().velocity = tmp;
        //player.transform.rotation = respawnRotation;
    }

    public void SetRespawnPoint(Vector3 respPoint)
    {
        respawnPoint = respPoint;
    }

    public void ShowSettings()
    {
        settingsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void ReloadMap()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
        blockPause = false;
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void EndGame()
    {
        returnToMenu();
    }

    public void EndMenu()
    {
        isPaused = true;
        blockPause = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        endMenu.SetActive(true);
        EndMenuScript ems = endMenu.GetComponent<EndMenuScript>();
        ems.displayTimer(gts.getMinutes(), gts.getSeconds(), gts.getMilliseconds());
    }
}
