using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MenuModel : MonoBehaviour
{
    GameObject _targetModel;
    private Vector3 _spawnScale = new Vector3(3.2f, 3.2f, 3.2f);


    void Start()
    {
        SwitchTargetModel();
    }

    public void SwitchTargetModel()
    {
        if (_targetModel != null)
        {
            Destroy(_targetModel);
        }

        GameObject pulledModel = PlayerChoise.Instance._avalibleSkins[PlayerChoise.Instance._currentChoise]._model;
        _targetModel = Instantiate(pulledModel, this.transform);
        _targetModel.transform.localScale = _spawnScale;

    }


}
