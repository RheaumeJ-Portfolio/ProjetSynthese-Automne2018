﻿using System.Net.Configuration;
using Game.Controller;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuPanel;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameHUD;
    [SerializeField] private GameObject levelFinishedPanel;
    [SerializeField] private GameObject LevelSelectPanel;
    [SerializeField] private GameObject creditsPanel;
    private GameController gameController;
    private const string gameCompletedTextString = "Congratulations!";
    private const string deathTextString = "Game Over";


    private void Awake()
    {
        gameController = GetComponent<GameController>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        HideGameOverPanel();
        HidePausePanel();
        HideGameHUD();
        HideLevelFinishedPanel();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == Game.Values.Scenes.Menu)
        {
            DisplayMainMenu();
            HideGameHUD();
        }
        else
        {
            HideMainMenu();
            HideLevelSelectPanel();
            DisplayGameHUD();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        HideGameOverPanel();
        HidePausePanel();
        HideGameHUD();
        DisplayMainMenu();
    }

    public void HideMainMenu()
    {
        mainMenuPanel.SetActive(false);
    }

    public void DisplayMainMenu()
    {
        mainMenuPanel.SetActive(true);
    }

    public void DisplayGameOverPanel()
    {
        GameOverPanel.SetActive(true);
    }

    public void HideGameOverPanel()
    {
        GameOverPanel.SetActive(false);
    }

    public void DisplayPausePanel()
    {
        PauseMenuPanel.SetActive(true);
    }

    public void HidePausePanel()
    {
        PauseMenuPanel.SetActive(false);
    }

    public void DisplayGameHUD()
    {
        gameHUD.SetActive(true);
    }

    public void HideGameHUD()
    {
        gameHUD.SetActive(false);
    }

    public void DisplayLevelFinishedPanel()
    {
        levelFinishedPanel.SetActive(true);
    }

    public void HideLevelFinishedPanel()
    {
        levelFinishedPanel.SetActive(false);
    }

    public void DisplayLevelSelectPanel()
    {
        LevelSelectPanel.SetActive(true);
    }

    public void HideLevelSelectPanel()
    {
        LevelSelectPanel.SetActive(false);
    }

    public void DisplayCreditsPanel()
    {
        creditsPanel.SetActive(true);
    }

    public void HideCreditsPanel()
    {
        creditsPanel.SetActive(false);
    }
}