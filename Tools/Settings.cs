using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Linq;
using UnityEditor;

public class Settings : PersistentSingleton<Settings>
{
    public UnityEvent OnSettingsLoaded = new UnityEvent();
    public UnityEvent OnSettingsChanged = new UnityEvent();

    public GameSettings CurrentSettings { get; private set; }


    [Header("Fullsceen")]
    public Toggle _isFullscreen;
    private FullScreenMode _selectedFullSceenMode;


    [Header("Resolution")]
    public Dropdown _resDropdown;
    public Dropdown _refreshRateDropdown;


    private List<string> _resOptions = new();
    private List<string> _refreshRateOptions = new();


    //Vitaliy test
    private readonly List<Vector2Int> _avalibleResolutions = new();
    private readonly List<RefreshRate> _avalibleRefreshRates = new();

    public Vector2Int _currentResolution;
    private RefreshRate _currentRefreshRate;

    private Vector2Int _selectedResolution;
    private RefreshRate _selectedRefreshRate;


    [Header("Volume")]
    public AudioMixer _mainMixer;

    private const string _MasterVolumePar = "Master";

    public Slider _masterVolumeSL;



    protected override void Awake()
    {
        base.Awake();

        InitResolutionsAndRefreshRates();

        LoadSettingsFromSaveFile();

        PopulateResolutionsDropdown();
        PopulateRefreshRatesDropdown();

        SetRefreshRateDropdownValueToSaved();
        SetResolutionDropdownValueToSaved();

        ApplyAllSetting();
    }


    public void LoadSettingsFromSaveFile()
    {
        SaveFile loadedSaveFile = SaveManager.Instance.LoadGame();

        CurrentSettings = loadedSaveFile.GameSettings;

        if (_masterVolumeSL != null)
        {
            _masterVolumeSL.onValueChanged.AddListener(SetMasterVolume);
            _masterVolumeSL.value = CurrentSettings._masterVolume;
        }

        if (_resDropdown != null)
        {
            _resDropdown.onValueChanged.AddListener(SetResolutionFromDropdown);
        }

        if (_isFullscreen != null)
        {
            _isFullscreen.onValueChanged.AddListener(SetFullscreen);
            _isFullscreen.isOn = CurrentSettings._fullscreenIS;
        }

        if (_refreshRateDropdown != null)
        {
            _refreshRateDropdown.onValueChanged.AddListener(SetRefreshRateFromDropdown);
        }

        if (CurrentSettings._resolutionIND >= 0 && CurrentSettings._resolutionIND < _avalibleResolutions.Count)
        {
            _selectedResolution = _avalibleResolutions[CurrentSettings._resolutionIND];
        }

        if (CurrentSettings._refreshRateIND >= 0 && CurrentSettings._refreshRateIND < _avalibleRefreshRates.Count)
        {
            _selectedRefreshRate = _avalibleRefreshRates[CurrentSettings._refreshRateIND];
        }
        
    }


    public void ApplyAllSetting()
    {
        if (CurrentSettings == null)
        {
            return;
        }

        ApplyResolutionSettings();

        float volumeToSet = CurrentSettings._masterVolume;

        if (volumeToSet < 0.0001f)
        {
            volumeToSet = 0.0001f;
        }

        _mainMixer.SetFloat(_MasterVolumePar, Mathf.Log10(volumeToSet) * 20);

        OnSettingsChanged.Invoke();

    }


    public void SaveCurrentSettings()
    {
        SaveFile currentSaveFile = SaveManager.Instance.LoadGame();
        currentSaveFile.GameSettings = CurrentSettings;
        SaveManager.Instance.SaveGame(currentSaveFile);
    }


    public void SetMasterVolume(float volume)
    {
        CurrentSettings._masterVolume = volume;

        ApplyAllSetting();

        OnSettingsChanged.Invoke();

        SaveCurrentSettings();
    }


    public void SetFullscreen(bool fullscreen)
    {
        if (fullscreen)
        {
            _selectedFullSceenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            _selectedFullSceenMode = FullScreenMode.Windowed;
        }

        Screen.fullScreenMode = _selectedFullSceenMode;

        CurrentSettings._fullscreenIS = fullscreen;

        ApplyAllSetting();

        OnSettingsChanged.Invoke();

        SaveCurrentSettings();
    }


    public void SetResolutionFromDropdown(int resolutionIndex)
    {
        CurrentSettings._resolutionIND = resolutionIndex;
        _selectedResolution = _avalibleResolutions[resolutionIndex];
        ApplyAllSetting();
        OnSettingsChanged.Invoke();
        SaveCurrentSettings();
    }


    public void SetRefreshRateFromDropdown(int refreshRateIndex)
    {
        CurrentSettings._refreshRateIND = refreshRateIndex;
        _selectedRefreshRate = _avalibleRefreshRates[refreshRateIndex];
        ApplyAllSetting();
        OnSettingsChanged.Invoke();
        SaveCurrentSettings();
    }


    public void PopulateResolutionsDropdown()
    {
        _resDropdown.ClearOptions();
        _refreshRateOptions.Clear();

        int currentResolutionIndex = 0;

        for (int i = 0; i < _avalibleResolutions.Count; i++)
        {
            Vector2Int resolution = _avalibleResolutions[i];
            string option = $"{resolution.x}x{resolution.y}";
            _resOptions.Add(option);
        }

        _resDropdown.AddOptions(_resOptions);
        _resDropdown.value = currentResolutionIndex;
        _resDropdown.RefreshShownValue();

    }


    public void PopulateRefreshRatesDropdown()
    {
        _refreshRateDropdown.ClearOptions();
        _refreshRateOptions.Clear();

        int currentRefreshRateIndex = 0;

        for (int i = 0; i < _avalibleRefreshRates.Count; i++)
        {
            uint refreshRate = _avalibleRefreshRates[i].numerator / _avalibleRefreshRates[i].denominator;
            string option = $"{refreshRate}HZ";
            _refreshRateOptions.Add(option);
        }

        _refreshRateDropdown.AddOptions(_refreshRateOptions);
        _refreshRateDropdown.value = currentRefreshRateIndex;
        _refreshRateDropdown.RefreshShownValue();
    }


    private void InitResolutionsAndRefreshRates()
    {
        _avalibleRefreshRates.Clear();
        _avalibleResolutions.Clear();

        foreach (var resolutions in Screen.resolutions)
        {
            if (!_avalibleRefreshRates.Contains(resolutions.refreshRateRatio))
            {
                _avalibleRefreshRates.Add(resolutions.refreshRateRatio);
            }

            var candidateResolution = new Vector2Int(resolutions.width, resolutions.height);

            if (!_avalibleResolutions.Contains(candidateResolution))
            {
                _avalibleResolutions.Add(candidateResolution);
            }
        }


    }


    private void ApplyResolutionSettings()
    {
        if (_currentResolution != _selectedResolution)
        {
            if (IsRefreshRatesMatch())
            {
                Screen.SetResolution(_selectedResolution.x, _selectedResolution.y, _selectedFullSceenMode, _selectedRefreshRate);
            }

            else
            {
                Screen.SetResolution(_selectedResolution.x, _selectedResolution.y, _selectedFullSceenMode);
            }

            _currentResolution = _selectedResolution;
            _currentRefreshRate = _selectedRefreshRate;

            Debug.Log(_currentResolution);
            Debug.Log(_currentRefreshRate);
        }
    }


    public bool IsRefreshRatesMatch()
    {
        if (_currentRefreshRate.numerator != _selectedRefreshRate.numerator || _currentRefreshRate.denominator != _selectedRefreshRate.denominator)
        {
            return false;
        }

        else return true;
    }


    private void SetResolutionDropdownValueToSaved()
    {
        int savedInd = CurrentSettings._resolutionIND;

        if (savedInd >= 0 && savedInd < _resOptions.Count)
        {
            _resDropdown.value = savedInd;
        }
        else
        {
            int actualCurrentInd = 0;

            for (int i = 0; i < _avalibleResolutions.Count; i++)
            {
                Vector2Int res = _avalibleResolutions[i];
                if (res.x == Screen.currentResolution.width && res.y == Screen.currentResolution.height)
                {
                    actualCurrentInd = i;
                    break;
                }
            }

            _resDropdown.value = actualCurrentInd;
        }
    }


    private void SetRefreshRateDropdownValueToSaved()
    {
        int savedInd = CurrentSettings._refreshRateIND;

        if (savedInd >= 0 && savedInd < _refreshRateOptions.Count)
        {
            _refreshRateDropdown.value = savedInd;
        }
        else
        {
            int actualCurrentInd = 0;
            for (int i = 0; i < _avalibleRefreshRates.Count; i++)
            {
                RefreshRate refreshRate = _avalibleRefreshRates[i];

                if (refreshRate.numerator == Screen.currentResolution.refreshRateRatio.numerator &&
                    refreshRate.denominator == Screen.currentResolution.refreshRateRatio.denominator)
                {
                    actualCurrentInd = i;
                    break;
                }
            }

            _refreshRateDropdown.value = actualCurrentInd;
        }
    }

}
