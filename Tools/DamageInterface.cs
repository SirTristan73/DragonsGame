using UnityEngine;

public interface DamageInterface
{
    void TakeDamage(DamageSource damageSource);
}


public enum DamageSource
{
    PlayerBullet,
    Enemy,
}
