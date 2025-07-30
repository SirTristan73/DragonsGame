using Unity.VisualScripting;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{

    private float _currentSpeed;


    private DamageSource _playerBullet = DamageSource.PlayerBullet;

    //placeholder for damage value (maybe)

    private void OnEnable()
    {
        _currentSpeed = WeaponContoller.SharedInstance._currentSpeed;

    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * _currentSpeed * Time.deltaTime);
        if (this.transform.position.z >= 50f)
        {
            this.gameObject.SetActive(false);
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        DamageInterface damageObject = collision.gameObject.GetComponent<DamageInterface>();
        if (damageObject != null)
        {
            damageObject.TakeDamage(_playerBullet);
        }
        gameObject.SetActive(false);
    }
}
