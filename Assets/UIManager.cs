using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI WaveCountText;
    public TextMeshProUGUI ScoreText;

    public TextMeshProUGUI GameWonScoreText;


    public Image rightSideIndicator;
    public Image leftSideIndicator;
    public Image topSideIndicator;
    public Image bottomSideIndicator;

    public GameObject WaveStartIndicator;
    public GameObject WaveEndIndicator;
    
    [Header("Game Over Menu Variables")]
    public CanvasGroup GameOverScreen;
    public GameObject restartGameOverButton;

    public CanvasGroup GameWonScreen;
    public GameObject restartGameWonButton;

    public CanvasGroup HUD_Screen;

    void Start()
    {
        if(BountyManager.instance == null) { return; }
        BountyManager.instance.WaveStarted += UpdateInfoOnWaveStart;
        BountyManager.instance.UpdateScore += UpdateScore;

        BountyManager.instance.WaveEnded += UpdateInfoOnWaveEnd;

        BountyManager.instance.GameOverEvent += EnableGameOverUI;
        BountyManager.instance.GameWon += EnableGameWonUI;

        BountyManager.instance.TurnOff += Disable;
    }

    public void Disable()
    {
        BountyManager.instance.WaveStarted -= UpdateInfoOnWaveStart;
        BountyManager.instance.UpdateScore -= UpdateScore;

        BountyManager.instance.WaveEnded -= UpdateInfoOnWaveEnd;

        BountyManager.instance.GameOverEvent -= EnableGameOverUI;
        BountyManager.instance.GameWon -= EnableGameWonUI;

        BountyManager.instance.TurnOff -= Disable;
    }

    private void UpdateInfoOnWaveStart(BountyManager.Wave wave)
    {
        UpdateWaveCount(wave.WaveNum);
        IndicateGoals(wave.AsteroidGoals);

        WaveStartIndicator.SetActive(true);
        Invoke(nameof(CloseWaveStart), 2f);
    }

    private void UpdateInfoOnWaveEnd(BountyManager.Wave wave)
    {
        WaveEndIndicator.SetActive(true);
        Invoke(nameof(CloseWaveEnd), 2f);
    }

    void CloseWaveStart()
    {
        WaveStartIndicator.SetActive(false);
        IndicateGoals(null);
    }

    void CloseWaveEnd()
    {
        WaveEndIndicator.SetActive(false);
    }

    void UpdateScore(int score)
    {
        ScoreText.text = $"{score}";
        GameWonScoreText.text = $"Score: {score}";
    }

    void UpdateWaveCount(int waveCount)
    {
        WaveCountText.text = $"{waveCount}";
    }

    void IndicateGoals(List<ObjectSpawner> AsteroidGoals)
    {
        bottomSideIndicator.gameObject.SetActive(false);
        rightSideIndicator.gameObject.SetActive(false);
        topSideIndicator.gameObject.SetActive(false);
        leftSideIndicator.gameObject.SetActive(false);

        if (AsteroidGoals is null) { return; }

        foreach (ObjectSpawner Goal in AsteroidGoals)
        {
            switch(Goal.side)
            {
                case ObjectSpawner.CameraSide.Left:
                    leftSideIndicator.gameObject.SetActive(true);
                    break;
                case ObjectSpawner.CameraSide.Right:
                    rightSideIndicator.gameObject.SetActive(true);
                    break;
                case ObjectSpawner.CameraSide.Up:
                    topSideIndicator.gameObject.SetActive(true);
                    break;
                case ObjectSpawner.CameraSide.Down:
                    bottomSideIndicator.gameObject.SetActive(true);
                    break;
            }
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    void EnableGameOverUI()
    {
        Time.timeScale = 0f;
        GameOverScreen.alpha = 1f;
        GameOverScreen.interactable = true;
        GameOverScreen.blocksRaycasts = true;

        HUD_Screen.alpha = 0f;
        Cursor.visible = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(restartGameOverButton);

        AudioManager.Instance.StopSound(AudioManagerChannels.ThrusterChannel);
        AudioManager.Instance.StopSound(AudioManagerChannels.SideThrusterChannel);
    }
    void EnableGameWonUI()
    {
        Time.timeScale = 0f;
        GameWonScreen.alpha = 1f;
        GameWonScreen.interactable = true;
        GameWonScreen.blocksRaycasts = true;

        HUD_Screen.alpha = 0f;
        Cursor.visible = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(restartGameWonButton);
    }
    public void RestartGame()
    {
        Debug.Log("Restarted!");
        BountyManager.instance.RestartGame();
    }
    
    public void LoadLevel()
    {
        SceneManager.LoadSceneAsync("AsteroidBelt");
    }
}
