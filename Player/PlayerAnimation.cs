using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // Elements
    public Animator _animator;

    // Animation Names
    private const string _moveAnim = "Move";
    private const string _fireAnim = "Fire";

    public void GetAnimator(Animator i)
    
    {
        _animator = i;
    }


    public void DefaultFlight()

    {
        _animator.SetInteger(_moveAnim, 0);
    }

    public void MoveLeft()

    {
        _animator.SetInteger(_moveAnim, 1);
    }

    public void MoveRight()

    {
        _animator.SetInteger(_moveAnim, -1);
    }

    public void FireAnimation(bool isFire)

    {
        _animator.SetBool(_fireAnim, isFire);
    }

}
