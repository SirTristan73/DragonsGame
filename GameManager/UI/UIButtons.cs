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


    [Header("Stattrackers")]
    public TMP_Text _killCounter;
    public TMP_Text _timeCounter;
    public TMP_Text _mainCurrencyTracker;
    public float _totalTIme;


    [Header("Upgrade")]
    public Button _upgradePlayerButton;
    public TMP_Text _upgradeMeter;


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


    [Header("ChoiseMenuButtons")]
    public Button _previousChoise;
    public Button _nextChoise;
    public TMP_Text _statsHPText;
    public TMP_Text _statsSpeedText;
    public TMP_Text _statsUpgradeText;

    //Animation variables
    private const string _click = "Click";
    private float _animationDelay = 1.2f;




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

        if (_lostScreen.activeInHierarchy)
        {
            _lostScreen.SetActive(false);
        }

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

        IsUpgradeAvalible();

        Cursor.visible = true;
        EventSystemCatch(_mainMenuPrimary.gameObject);

    }


    public void SettingsMenu()
    {
        Cursor.visible = true;
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
        Cursor.visible = true;
        EventSystemCatch(_pauseMenuPrimary.gameObject);
    }


    public void ChoiceMenu()
    {
        _mainMenu.SetActive(false);
        _choiceMenu.SetActive(true);
        PlayerChoise.Instance.CheckSkinAvalible();
        SetButtonsInChoice();
        Cursor.visible = true;
        EventSystemCatch(_choicePrimary.gameObject);

    }


    public void LostGameScreen()
    {
        _lostScreen.SetActive(true);
        Cursor.visible = true;
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


    public void RestartButton()
    {
        StartButton();
        GameManager.Instance.ChangeGameState(GameState.Playing);
    }


    public void NextButtonChoise()
    {
        PlayerChoise.Instance.NextSkin();
        SetButtonsInChoice();
    }


    public void PreviousButtonChoise()
    {
        PlayerChoise.Instance.PreviousSkin();
        SetButtonsInChoice();
    }


    public void SetButtonsInChoice()
    {
        _previousChoise.interactable = PlayerChoise.Instance._previousAvalible;
        _nextChoise.interactable = PlayerChoise.Instance._nextAvalible;

        ModelOutOf();
    }


    public void StatsText(float hp, float speed)
    {
        _statsHPText.text = "Health: " + hp.ToString();
        _statsSpeedText.text = "Speed: " + ((speed / PlayerChoise.Instance._avalibleSkins[0].BaseStats._speed)*100).ToString("F0");
        
    }


    public void ModelOutOf()
    {
        _statsUpgradeText.text =
        (PlayerChoise.Instance._currentChoise + 1).ToString()
        + " / "
        + (PlayerChoise.Instance._avalibleSkins.Count).ToString();
    }


    public void CurrencyTrackers()
    {
        _mainCurrencyTracker.text = "Coins: " + Economics.Instance._coins;
    }


    public void UpgradePlayerButton()
    {
        Economics.Instance.PayingProcess();
        IsUpgradeAvalible();
    }


    public void IsUpgradeAvalible()
    {
        if (Economics.Instance.CheckIfNextUpgradeAvalible())
        {
            _upgradePlayerButton.interactable = true;
            _upgradeMeter.text = "Unlock new character";
        }

        else if (Economics.Instance.PlayerUpgradePrice() == 0)
        {
            _upgradePlayerButton.interactable = false;
            _upgradeMeter.text = "Unlocked all characters";
        }

        else if (!Economics.Instance.CheckIfNextUpgradeAvalible())
        {
            _upgradePlayerButton.interactable = false;
            _upgradeMeter.text = Economics.Instance._coins.ToString() + " / " + Economics.Instance.PlayerUpgradePrice().ToString();
        }

        CurrencyTrackers();
        
    }

}
