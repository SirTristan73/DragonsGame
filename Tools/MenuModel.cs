using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MenuModel : MonoBehaviour
{
    public static MenuModel Instance;
    GameObject _targetModel;
    private Vector3 _spawnScale = new Vector3(3.2f, 3.2f, 3.2f);


    void Start()
    {
        SwitchTargetModel();
        Instance = this;
    }

    public void SwitchTargetModel()
    {
        if (_targetModel != null)
        {
            Destroy(_targetModel);
        }

        CreatureStats pulledModel = PlayerChoise.Instance._avalibleSkins[PlayerChoise.Instance._currentChoise];
        _targetModel = Instantiate(pulledModel._model, this.transform);
        _targetModel.transform.localScale = _spawnScale;

        UIButtons.Instance.StatsText(
            pulledModel.BaseStats._health,
            pulledModel.BaseStats._speed);

    }


}
