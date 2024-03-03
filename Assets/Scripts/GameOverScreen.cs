//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.SceneManagement;
//using TMPro;

//public class GameOverScreen : MonoBehaviour
//{
//    public static GameOverScreen instance;

//    [Header("Game Over Menu Variables")]
//    public GameObject gameOverScreen, restartButton;
//    public GameObject[] buttons;
//    public CanvasGroup gameOverMenu, creditsMenu;

//    public void ChangeActiveButtons(int buttonToChoose)
//    {
//        EventSystem.current.SetSelectedGameObject(null);
//        EventSystem.current.SetSelectedGameObject(buttons[buttonToChoose]);
//    }

//    public void ShowGameOver()
//    {
//        StartCoroutine(ShowGameOverCo());
//    }

//    public IEnumerator ShowGameOverCo()
//    {
//        gameOverScreen.SetActive(true);
//        gameUI.SetActive(false);
//        gameOverScreen.transform.GetChild(0).gameObject.SetActive(true);
//        isGameOver = true;
//        PauseMenu.instance.canPause = false;

//        EventSystem.current.SetSelectedGameObject(null);
//        EventSystem.current.SetSelectedGameObject(restartButton);


//        //Open the game over menu by calling the function and setting the timescale to 0
//        OpenGameOverMenu();
//        Time.timeScale = 0;
//    }

//    public void RestartGame()
//    {
//        ChangeTimeScale();
//        GameSaveManager.instance.RestoreGame();
//        SceneManager.LoadScene("StarSystem");
//    }

//    //Function for opening the game over menu by setting alpha to 1
//    public void OpenGameOverMenu()
//    {
//        gameOverMenu.alpha = 1;
//        gameOverMenu.blocksRaycasts = true;
//    }

//    //Function for closing the game over menu by setting alpha to 0
//    public void CloseGameOverMenu()
//    {
//        gameOverMenu.alpha = 0;
//        gameOverMenu.blocksRaycasts = false;
//    }

//    public void OpenCredits()
//    {
//        creditsMenu.alpha = 1;
//        creditsMenu.blocksRaycasts = true;
//    }

//    public void CloseCredits()
//    {
//        creditsMenu.alpha = 0;
//        creditsMenu.blocksRaycasts = false;
//    }

//    public void LoadMainMenu()
//    {
//        ChangeTimeScale();
//        GameSaveManager.instance.RestoreGame();
//        SceneManager.LoadScene(mainMenuScene);
//    }

//    public void ChangeTimeScale()
//    {
//        if (Time.timeScale == 0)
//        {
//            Time.timeScale = 1;
//        }
//    }

//    public void PlayButtonSound()
//    {
//    }
//}
