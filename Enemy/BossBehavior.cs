using UnityEngine;

public class BossBehavior : MonoBehaviour, DamageInterface
{

    private float _enemySpeed;
    private float _enemyHP;
    private bool _isDead = false;
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
        if (_isDead)
        {
            return;
        }
        
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
                _isDead = true;
                EnemyController.SharedInstance.BossKilled();
                Destroy(this.gameObject);
            }
        }
    }


    public void SetBossStats(float health, float speed)
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
