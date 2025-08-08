using UnityEngine;


[System.Serializable]
public class SaveFile
{
    public int _enemyKilled;
    public float _timePlayed;
    public int _currentPlayerModel;
    public int _currentMainCurrency;
    public int _currentModelProgress;
    public int _currentUpgradePrice;

    public GameSettings GameSettings = new GameSettings();


    public SaveFile()
    {
        _enemyKilled = 0;
        _timePlayed = 0;
        _currentPlayerModel = 0;
        _currentMainCurrency = 0;
        _currentModelProgress = 0;

    }
}



[System.Serializable]
public class GameSettings
{
    public float _masterVolume;
    public int _resolutionIND;
    public bool _fullscreenIS;

    


    public GameSettings()
    {
        _masterVolume = 1f;
        _fullscreenIS = true;
        _resolutionIND = 1;
        
    }

}