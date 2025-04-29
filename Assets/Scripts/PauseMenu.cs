using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        // Make sure cursor is hidden at start
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        // Get reference to the player controller
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        
        // Show cursor when game is paused
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        // Disable player movement and camera control
        if (playerController != null)
        {
            playerController.canMove = false;
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        
        // Hide cursor when game is resumed
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        // Re-enable player movement and camera control
        if (playerController != null)
        {
            playerController.canMove = true;
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
        isPaused = false;
        
        // Make sure cursor is visible when returning to main menu
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
