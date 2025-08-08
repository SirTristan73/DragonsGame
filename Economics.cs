using UnityEngine;

public class Economics : PersistentSingleton<Economics>
{
    public int _coins;
    public int _currentPlayerUpgrade;


    public void AddCoinsProcess(int coins)
    {
        _coins += coins;
    }


    public void PayingProcess()
    {

        int price = PlayerUpgradePrice();

        if (CheckIfNextUpgradeAvalible())
        {
            _coins -= price;
            _currentPlayerUpgrade++;

            UIButtons.Instance.IsUpgradeAvalible();

            PlayerChoise.Instance._currentChoise = _currentPlayerUpgrade;
            PlayerChoise.Instance.ChooseAction();

            GameManager.Instance.SaveGameData();
        }

    }


    public int PlayerUpgradePrice()
    {
        int price;

        if (_currentPlayerUpgrade + 1 < PlayerChoise.Instance._avalibleSkins.Count - 1)
        {
            price = PlayerChoise.Instance._avalibleSkins[_currentPlayerUpgrade + 1].BaseStats._points;
        }

        else
        {
            price = 0;
        }

        return price;

    }


    public bool CheckIfNextUpgradeAvalible()
    {
        if (_coins >= PlayerUpgradePrice() && _currentPlayerUpgrade < PlayerChoise.Instance._avalibleSkins.Count - 1)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
