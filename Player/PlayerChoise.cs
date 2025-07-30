using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerChoise : PersistentSingleton<PlayerChoise>
{
    public List<CreatureStats> _avalibleSkins;

    public int _currentChoise = 0;

    public MenuModel _modelInMenu;

    public void ChooseAction()
    {

        Debug.Log(_currentChoise);
        _modelInMenu.SwitchTargetModel();
        // PlayerController.Instance.SetPlayerModel(_avalibleSkins[_currentChoise]);

    }


    public void FirstUpgrade()
    {
        _currentChoise = 1;
        ChooseAction();
        WeaponContoller.SharedInstance.ProjectileSwitching(1);
    }


    public void DefaultUpgrade()
    {
        _currentChoise = 0;
        ChooseAction();
        WeaponContoller.SharedInstance.ProjectileSwitching(0);

    }
}
