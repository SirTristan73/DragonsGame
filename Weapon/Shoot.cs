using UnityEngine;

public class Shoot : MonoBehaviour
{

    private float _nextShotTime = 0.3f;

    private bool _isFireActive;

    public PlayerController _playerInput;



    public void Update() // В апдейте проверяем доступна ли стрельба и запускаем процесс стрельбы
    {
        if(_isFireActive)
        {
            ShootingProcess();
        }
    }



    //Стрельба
    private void ShootingProcess()
    {
        GameObject _projectileToShoot = WeaponContoller.SharedInstance.GetPooledProj();

        float fireRate = WeaponContoller.SharedInstance._currentFireRate;

        float time = Time.time;

        if (time >= _nextShotTime)
        {

            _nextShotTime = time + 1 / fireRate;

            if (_projectileToShoot != null)
            {
                _projectileToShoot.transform.position = this.transform.position;
                _projectileToShoot.transform.rotation = this.transform.rotation;
                _projectileToShoot.SetActive(true);
            }
        }
    }

    //Получить файринпут от плеер контроллера, можно добавить кондишн когда шутинг нот эвейлибл
    public void IsFireActive(bool fire)
    {
        _isFireActive = fire;
    }


}
