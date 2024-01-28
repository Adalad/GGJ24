using UnityEngine;

public class PlayerAnimationBehaviour : MonoBehaviour
{
    [Header("Component References")]
    public Animator playerAnimator;

    //Animation String IDs
    private int playerMovementAnimationID;
    private int playerAttackAnimationID;
    private int playerHitAnimationID;

    public void SetupBehaviour()
    {
        SetupAnimationIDs();
    }

    void SetupAnimationIDs()
    {
        playerMovementAnimationID = Animator.StringToHash("Movement");
        playerAttackAnimationID = Animator.StringToHash("Attack");
        playerHitAnimationID = Animator.StringToHash("Hit");
    }

    public void UpdateMovementAnimation(float movementBlendValue)
    {
        playerAnimator.SetFloat("Movement", movementBlendValue);
    }

    public void PlayAttackAnimation()
    {
        playerAnimator.SetTrigger(playerAttackAnimationID);
    }

    public void PlayHitAnimation()
    {
        playerAnimator.SetTrigger(playerHitAnimationID);
    }

    public void PlayWinAnimation()
    {
        playerAnimator.SetTrigger("Win");
    }

    public void PlayLoseAnimation()
    {
        playerAnimator.SetTrigger("Lose");
    }
}
