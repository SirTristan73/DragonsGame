using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


using System.Collections.Generic;

public class UIButtons : PersistentSingleton<UIButtons>
{
    [Header("Menu")]
    public GameObject _mainMenu;
    public GameObject _playMode;
    public GameObject _pauseMenu;
    public GameObject _choiceMenu;
    public GameObject _settingsMenu;
    public GameObject _lostScreen;


    // Элементы таймера
    public TMP_Text _playmodeTimer;
    public float _playerTimer = 0;


    // Статтрекер в меин меню
    public TMP_Text _killCounter;
    public TMP_Text _timeCounter;
    public float _totalTIme;


    [Header("EventSystemCatch")]
    public Button _mainMenuPrimary;
    public Button _pauseMenuPrimary;
    public Button _choicePrimary;
    public Button _settingsPrimary;
    public Button _lostPrimary;



    [Header("Animators")]
    public Animator _animator;
    public Animator _animChoice;


    [Header("Settings Elements")]
    public Toggle _fullscreenToggle;


    private const string _click = "Click";
    public float _animationDelay = 3f;




    public void OnButtonClick(string button)
    {
        if (_mainMenu.activeInHierarchy)
        {
            _animator.SetTrigger(_click);
        }

        if (_choiceMenu.activeInHierarchy)
        {
            _animChoice.SetTrigger(_click);
        }

        Invoke(button, _animationDelay);

    }


    public void StartButton()
    {
        LoadingScene.Instance.LoadSceneWithLS(SceneList.MainPlayScene, GameState.StartLevel);
        EventSystemCatch(null);
        TimerReset();
    }


    public void QuitButton()
    {
        GameManager.Instance.SaveGameData();

        TimerReset();
        EventSystemCatch(null);
        LoadingScene.Instance.LoadSceneWithLS(SceneList.MainMenuScene, GameState.Main);
    }


    public void BackButton()
    {
        EventSystemCatch(null);
        UIinMainMenu();
    }


    public void TimerText(float updateListener)
    {
        _playerTimer += updateListener;
        _playmodeTimer.text = _playerTimer.ToString("F1");

    }


    public void TimerReset()
    {
        _playerTimer = 0f;
    }


    public void UIinPlaymode()
    {
        _pauseMenu.SetActive(false);
        _mainMenu.SetActive(false);
        _playMode.SetActive(true);

    }


    public void UIinMainMenu()
    {
        TimeManager.Instance.UnregisterUpdateListener(TimerText);
        TimerReset();
        TimerReset();

        if (_choiceMenu.activeInHierarchy)
        {
            _choiceMenu.SetActive(false);
        }

        if (_playMode.activeInHierarchy)
        {
            _playMode.SetActive(false);
        }

        if (_pauseMenu.activeInHierarchy)
        {
            _pauseMenu.SetActive(false);
        }

        if (_settingsMenu.activeInHierarchy)
        {
            Settings.Instance.OnSettingsLoaded.RemoveListener(UpdateUIFromSettings);
            Settings.Instance.OnSettingsChanged.RemoveListener(UpdateUIFromSettings);

            _settingsMenu.SetActive(false);
        }


        if (_lostScreen.activeInHierarchy)
        {
            _lostScreen.SetActive(false);
        }

        _mainMenu.SetActive(true);


        EventSystemCatch(_mainMenuPrimary.gameObject);


    }


    public void SettingsMenu()
    {

        _mainMenu.SetActive(false);
        _settingsMenu.SetActive(true);
        EventSystemCatch(_settingsPrimary.gameObject);
        Settings.Instance.OnSettingsLoaded.AddListener(UpdateUIFromSettings);
        Settings.Instance.OnSettingsChanged.AddListener(UpdateUIFromSettings);

    }




    public void UpdateUIFromSettings()
    {
        if (_fullscreenToggle != null)
        {
            _fullscreenToggle.onValueChanged.RemoveListener(Settings.Instance.SetFullscreen);
            _fullscreenToggle.isOn = Settings.Instance.CurrentSettings._fullscreenIS;
            _fullscreenToggle.onValueChanged.AddListener(Settings.Instance.SetFullscreen);
        }



    }

    public void UIPauseMenu()
    {
        _playMode.SetActive(true);
        _pauseMenu.SetActive(true);
        EventSystemCatch(_pauseMenuPrimary.gameObject);
    }


    public void ChoiceMenu()
    {
        _mainMenu.SetActive(false);
        _choiceMenu.SetActive(true);

        EventSystemCatch(_choicePrimary.gameObject);

    }


    public void ChoiceSet(int numChsn)
    {
        PlayerChoise.Instance._currentChoise = numChsn;
        PlayerChoise.Instance.ChooseAction();
    }


    public void LostGameScreen()
    {
        _lostScreen.SetActive(true);

        EventSystemCatch(_lostPrimary.gameObject);
    }


    public void EventSystemCatch(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button);

    }

    public void ExitGame()
    {
        GameManager.Instance.ChangeGameState(GameState.Quit);
    }

}
