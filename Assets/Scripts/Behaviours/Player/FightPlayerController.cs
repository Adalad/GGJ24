using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FightPlayerController : MonoBehaviour
{

    [Header("Sub Behaviours")]
    public PlayerAnimationBehaviour PlayerAnimationBehaviour;
    public PlayerVisualBehaviour PlayerVisualBehaviour;

    [Header("Input Settings")]
    public PlayerInput PlayerInput;

    private string m_ActionMapPlayerControls = "Player Controls";
    private string m_ActionMapMenuControls = "Menu Controls";

    private int m_PlayerID;
    private GameObject m_PickedObject;
    private bool m_MovementDisabled;

    //This is called from the GameManager; when the game is being setup.
    public void SetupPlayer(int newPlayerID, int playerType)
    {
        m_PlayerID = newPlayerID;

        PlayerAnimationBehaviour.SetupBehaviour();
        PlayerVisualBehaviour.SetupBehaviour(playerType);
    }

    public void OnOption1(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            FightManager.Instance.ReceivePlayerOption(m_PlayerID, 0);
        }
    }

    public void OnOption2(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            FightManager.Instance.ReceivePlayerOption(m_PlayerID, 1);
        }
    }

    public void OnOption3(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            FightManager.Instance.ReceivePlayerOption(m_PlayerID, 2);
        }
    }

    public void OnOption4(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            FightManager.Instance.ReceivePlayerOption(m_PlayerID, 3);
        }
    }

    public void OnTogglePause(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            FightManager.Instance.TogglePauseState(this);
        }
    }

    //This is automatically called from PlayerInput, when the input device has been disconnected and can not be identified
    //IE: Device unplugged or has run out of batteries
    public void OnDeviceLost()
    {
    }


    public void OnDeviceRegained()
    {
        StartCoroutine(WaitForDeviceToBeRegained());
    }

    IEnumerator WaitForDeviceToBeRegained()
    {
        yield return new WaitForSeconds(0.1f);
    }

    public void SetInputActiveState(bool gameIsPaused)
    {
        switch (gameIsPaused)
        {
            case true:
                PlayerInput.DeactivateInput();
                break;

            case false:
                PlayerInput.ActivateInput();
                break;
        }
    }

    void RemoveAllBindingOverrides()
    {
        InputActionRebindingExtensions.RemoveAllBindingOverrides(PlayerInput.currentActionMap);
    }

    //Switching Action Maps ----
    public void EnableGameplayControls()
    {
        PlayerInput.SwitchCurrentActionMap(m_ActionMapPlayerControls);
    }

    public void EnablePauseMenuControls()
    {
        PlayerInput.SwitchCurrentActionMap(m_ActionMapMenuControls);
    }

    //Get Data ----
    public int GetPlayerID()
    {
        return m_PlayerID;
    }

    public InputActionAsset GetActionAsset()
    {
        return PlayerInput.actions;
    }

    public PlayerInput GetPlayerInput()
    {
        return PlayerInput;
    }

    public void RoundReaction(bool win)
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
