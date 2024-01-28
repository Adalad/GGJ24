using UnityEngine;

public class PlayerControllerEnd : MonoBehaviour
{
    [Header("Sub Behaviours")]
    public PlayerAnimationBehaviour PlayerAnimationBehaviour;
    public PlayerVisualBehaviour PlayerVisualBehaviour;

    //This is called from the GameManager; when the game is being setup.
    public void SetupPlayer(int newPlayerID, int playerType)
    {
        PlayerAnimationBehaviour.SetupBehaviour();
        PlayerVisualBehaviour.SetupBehaviour(playerType);
    }

    public void EndReaction(bool win)
    {
        if (win)
        {
            PlayerAnimationBehaviour.PlayWinAnimation();
        }
        else
        {
            PlayerAnimationBehaviour.PlayLoseAnimation();
        }
    }
}
