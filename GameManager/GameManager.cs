using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : PersistentSingleton<GameManager>
{

    public static event Action<GameState> OnPrepareStateChange;
    public static event Action<GameState> OnChangeGameState;

    public GameState State { get; private set; }

    void Start() => ChangeGameState(GameState.SystemsLoaded);


    public void ChangeGameState(GameState newState)
    {
        OnPrepareStateChange?.Invoke(newState);

        State = newState;

        switch (newState)
        {
            case GameState.SystemsLoaded:
                WhenSystemsLoaded();
                break;
            case GameState.Main:
                MainMenuScene();
                break;
            case GameState.StartLevel:
                StartGameScene();
                break;
            case GameState.Playing:
                GameIsPlaying();
                break;
            case GameState.Paused:
                GameIsPaused();
                break;
            case GameState.Lost:
                LostConditions();
                break;
            case GameState.Quit:
                QuitGameProcess();
                break;
        }

        OnChangeGameState?.Invoke(newState);

    }


    private void WhenSystemsLoaded()
    {
        LoadingScene.Instance.LoadSceneWithLS(SceneList.MainMenuScene, GameState.Main);

    }


    private void MainMenuScene()
    {
        Time.timeScale = 1;

        UIButtons.Instance.UIinMainMenu();

        TimeManager.Instance.UnregisterAllListeners();

        LoadGameData();

    }


    private void StartGameScene()
    {
        ChangeGameState(GameState.Playing);
    }


    private void GameIsPaused()
    {
        Time.timeScale = 0;

        UIButtons.Instance.UIPauseMenu();
    }


    private void GameIsPlaying()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        UIButtons.Instance.UIinPlaymode();
    }


    private void QuitGameProcess()
    {
        SaveGameData();
        Application.Quit();
    }


    public void PauseIsPressed()
    {
        if (State == GameState.Playing)
        {
            ChangeGameState(GameState.Paused);
        }
        else
        {
            ChangeGameState(GameState.Playing);
        }
    }


    public void LostConditions()
    {
        Time.timeScale = 0;
        SaveGameData();
        UIButtons.Instance.LostGameScreen();
    }


    public void SaveGameData()
    {
        SaveFile data = SaveManager.Instance.LoadGame();

        data._timePlayed += UIButtons.Instance._playerTimer;
        
        if (EnemyController.SharedInstance != null)
        {
            data._enemyKilled += EnemyController.SharedInstance._enemiesKilled;
        }

        data._currentPlayerModel = PlayerChoise.Instance._currentChoise;

        SaveManager.Instance.SaveGame(data);

    }


    public void LoadGameData()
    {
        SaveFile loadedData = SaveManager.Instance.LoadGame();
        UIButtons.Instance._killCounter.text = "Total kills: " + loadedData._enemyKilled.ToString("F1");
        UIButtons.Instance._timeCounter.text = "Total time: " + loadedData._timePlayed.ToString("F1");
        PlayerChoise.Instance._currentChoise = loadedData._currentPlayerModel;
    }
    
}


    [Serializable]
    public enum GameState
    {
        SystemsLoaded = 0,
        Main = 1,
        StartLevel = 2,
        Playing = 3,
        Paused = 4,
        Lost = 5,
        Quit = 9,
    }