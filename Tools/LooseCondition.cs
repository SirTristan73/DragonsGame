using UnityEngine;

public class LooseCondition : MonoBehaviour, DamageInterface
{
    public void TakeDamage(DamageSource damageSource)
    {
        if (damageSource == DamageSource.Enemy)
        {
            GameManager.Instance.ChangeGameState(GameState.Lost);
        }
    }

    
}
