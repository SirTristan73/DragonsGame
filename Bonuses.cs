using UnityEngine;

public class Bonuses : MonoBehaviour, DamageInterface
{
    private float _bonusSpeed;
    private float _enemyHP;
    private bool _isDead = false;
    Rigidbody _rb;

    
    void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        _rb.linearVelocity = Vector3.back * _bonusSpeed * Time.deltaTime;
    }


    public void TakeDamage(DamageSource source)
    {
        if (_isDead)
        {
            return;
        }
        
        if (source == DamageSource.PlayerBullet)
        {
            _enemyHP--;

            if (_enemyHP <= 0)
            {
                _isDead = true;
                Destroy(this.gameObject);
            }
        }
    }


    public void SetEnemyStats(float health, float speed)
    {
        _enemyHP = health;
        _bonusSpeed = speed;

    }


    public void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }

}
