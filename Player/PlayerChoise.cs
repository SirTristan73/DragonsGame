using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerChoise : PersistentSingleton<PlayerChoise>
{
    public List<CreatureStats> _avalibleSkins;

    public int _currentChoise;

    public bool _nextAvalible;
    public bool _previousAvalible;


    protected override void Awake()
    {
        base.Awake();
        CheckSkinAvalible();
    }


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
        _previousAvalible = (_currentChoise > 0);

        _nextAvalible = (_currentChoise < _avalibleSkins.Count - 1);
    }


}
