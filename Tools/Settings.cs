using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Linq;

public class Settings : PersistentSingleton<Settings>
{
    public UnityEvent OnSettingsLoaded = new UnityEvent();
    public UnityEvent OnSettingsChanged = new UnityEvent();

    public GameSettings CurrentSettings { get; private set; }

    public Dropdown _resDropdown;
    private List<Resolution> _avalibleRes;
    private List<string> _resOptions;

    public AudioMixer _mainMixer;

    private const string _MasterVolumePar = "Master";

    public Slider _masterVolumeSL;



    protected override void Awake()
    {
        base.Awake();

        LoadSettingsFromSaveFile();

        PopulateResolutionsDropdown();

        OnSettingsLoaded.AddListener(SetCurrentResolutionInDropdown);

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

        ApplyAllSetting();

    }


    public void ApplyAllSetting()
    {
        if (CurrentSettings == null)
        {
            return;
        }

        if (CurrentSettings._resolutionIND >= 0 && CurrentSettings._resolutionIND < Screen.resolutions.Length)
        {
            Resolution selectedResolution = Screen.resolutions[CurrentSettings._resolutionIND];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, CurrentSettings._fullscreenIS);
        }

        float volumeToSet = CurrentSettings._masterVolume;
        if (volumeToSet < 0.0001f)
        {
            volumeToSet = 0.0001f;
        }

        _mainMixer.SetFloat(_MasterVolumePar, Mathf.Log10(volumeToSet) * 20);

        OnSettingsLoaded.Invoke();

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


    public void SetResolution(int res)
    {
        CurrentSettings._resolutionIND = res;
        ApplyAllSetting();
        OnSettingsChanged.Invoke();
        SaveCurrentSettings();
    }


    public void SetFullscreen(bool fullscreen)
    {
        CurrentSettings._fullscreenIS = fullscreen;
        ApplyAllSetting();
        OnSettingsChanged.Invoke();
        Debug.Log(fullscreen);
        SaveCurrentSettings();
    }


    public void PopulateResolutionsDropdown()
    {
        _resDropdown.ClearOptions();

        _avalibleRes = Screen.resolutions.ToList();

        // _avalibleRes = _avalibleRes
        //         .GroupBy(res => new { res.width, res.height })
        //         .Select(group => group.OrderByDescending(res => res.refreshRateRatio.numerator / (double)res.refreshRateRatio.denominator).First())
        //         .OrderBy(res => res.width)
        //         .ThenBy(res => res.height)
        //         .ToList();
        
        _resOptions = new List<string>();

        int currentResolutionIndex = 0;
        
        for (int i = 0; i < _avalibleRes.Count; i++)
        {
            Resolution res = _avalibleRes[i];
            string option = $"{res.width}x{res.height}";
            _resOptions.Add(option);

            if (res.width == Screen.currentResolution.width &&
                res.height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        _resDropdown.AddOptions(_resOptions);
        _resDropdown.value = currentResolutionIndex;
        _resDropdown.RefreshShownValue();      

    }


    public void SetCurrentResolutionInDropdown()
    {
        int savedInd = CurrentSettings._resolutionIND;
        if (savedInd >= 0 && savedInd < _resOptions.Count)
        {
            _resDropdown.onValueChanged.RemoveListener(SetResolution);
            _resDropdown.value = savedInd;
            _resDropdown.onValueChanged.AddListener(SetResolution);
        }

        else
        {
            int actualCurrentInd = 0;
            for (int i = 0; i < _avalibleRes.Count; i++)
            {
                Resolution res = _avalibleRes[i];
                if (res.width == Screen.currentResolution.width &&
                    res.height == Screen.currentResolution.height)
                {
                    actualCurrentInd = i;
                    break;
                }
            }
            _resDropdown.value = actualCurrentInd;
            SetResolution(actualCurrentInd);
        }
    }

}
