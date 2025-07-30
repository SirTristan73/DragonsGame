using System.Collections.Generic;
using UnityEngine;

public class WeaponContoller : MonoBehaviour
{
    public static WeaponContoller SharedInstance;

    //Лист созданных прожектайлов
    public List<GameObject> _pooledProjectiles;

    //Скок пулить
    private int _poolAmmount = 50;

    //Лист оружий и их статы
    public List<PlayerWeapon> _projectile;
    public int _startingProjectile = 0;
    public float _currentSpeed;
    public float _currentFireRate;
    

    void Awake()
    {
        if (SharedInstance != null && SharedInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            SharedInstance = this;
        }
    }

    void Start() // В старте создаём пул прожектайлов в зависимости от выбора в меню
    {
        CreateProjectiles(_startingProjectile);
    }

    // Процесс создания оружия, используемый на старте, когда оружия ещё нет
    public void CreateProjectiles(int current)
    {
        GameObject proj;

        for (int i = 0; i < _poolAmmount; i++)
        {
            proj = Instantiate(_projectile[current]._projectile);
            proj.SetActive(false);
            _pooledProjectiles.Add(proj);

        }        

        _currentSpeed = _projectile[current].BaseWeaponStats._projSpeed;
        _currentFireRate = _projectile[current].BaseWeaponStats._fireRate;

    } 

    //Запуленный прожектайл с листа
    public GameObject GetPooledProj()
    {
        for (int i = 0; i < _poolAmmount; i++)
        {
            if (!_pooledProjectiles[i].activeInHierarchy)
            {
                return _pooledProjectiles[i];
            }
        }
        return null;
    }

    
    //Тут уже процесс смены оружия, с учётом того что надо удалить старый пул и почистить лист запуленных
    public void ProjectileSwitching(int projectileChosen)
    {

        if (_pooledProjectiles.Count != 0)
        {
            {
                for (int i = 0; i < _poolAmmount; i++)
                {
                    Destroy(_pooledProjectiles[i]);
                }
            }
        }

        _pooledProjectiles.Clear();

        CreateProjectiles(projectileChosen);

    }
}
