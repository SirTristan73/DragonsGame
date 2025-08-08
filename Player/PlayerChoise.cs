using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerChoise : PersistentSingleton<PlayerChoise>
{
    public List<CreatureStats> _avalibleSkins;

    public int _currentChoise;

    public bool _nextAvalible;
    public bool _previousAvalible;




    public void ChooseAction()
    {
        MenuModel.Instance.SwitchTargetModel();
    }


    public void NextSkin()
    {
        _currentChoise++;

        CheckSkinAvalible();

        ChooseAction();
    }


    public void PreviousSkin()
    {
        _currentChoise--;

        CheckSkinAvalible();

        ChooseAction();
    }


    public void CheckSkinAvalible()
    {
        _previousAvalible = _currentChoise > 0;
        Debug.Log("<<<"+_previousAvalible);

        _nextAvalible = (_currentChoise < _avalibleSkins.Count - 1 && _currentChoise < Economics.Instance._currentPlayerUpgrade);
        Debug.Log(">>>"+_nextAvalible);
    }


}
