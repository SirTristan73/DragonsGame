using UnityEngine;

public class EnemyBehavior : MonoBehaviour, DamageInterface
{

    private float _enemySpeed;
    private float _enemyHP;
    private DamageSource _enemyDamage = DamageSource.Enemy;
    Animator _animator;
    private const string _hitAnim = "Hit";
    public AudioSource _audio;
    Rigidbody _rb;


    void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        _rb.linearVelocity = Vector3.back * _enemySpeed * Time.deltaTime;
    }


    public void TakeDamage(DamageSource source)
    {
        if (source == DamageSource.PlayerBullet)
        {
            _enemyHP--;

            if (_audio != null)
            {
                _audio.Play();
            }

            _animator.SetTrigger(_hitAnim);

            if (_enemyHP <= 0)
            {
                EnemyController.SharedInstance.EnemyKilled();
                Destroy(this.gameObject);
            }
        }
    }


    public void SetEnemyStats(float health, float speed)
    {
        _enemyHP = health;
        _enemySpeed = speed;
    }


    public void OnTriggerEnter(Collider other)
    {
        DamageInterface killObj = other.gameObject.GetComponent<DamageInterface>();
        if (killObj != null)
        {
            killObj.TakeDamage(DamageSource.Enemy);
        }
    }

}
