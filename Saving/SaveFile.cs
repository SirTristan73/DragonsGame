using UnityEngine;


[System.Serializable]
public class SaveFile
{
    public int _enemyKilled;
    public float _timePlayed;

    public GameSettings GameSettings = new GameSettings();


    public SaveFile()
    {
        _enemyKilled = 0;
        _timePlayed = 0;

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