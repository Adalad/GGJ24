using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FightPlayerController : MonoBehaviour
{

    [Header("Sub Behaviours")]
    public PlayerAnimationBehaviour playerAnimationBehaviour;

    [Header("Input Settings")]
    public PlayerInput playerInput;

    private string m_ActionMapPlayerControls = "Player Controls";
    private string m_ActionMapMenuControls = "Menu Controls";

    private int m_PlayerID;
    private GameObject m_PickedObject;
    private bool m_MovementDisabled;

    //This is called from the GameManager; when the game is being setup.
    public void SetupPlayer(int newPlayerID)
    {
        m_PlayerID = newPlayerID;

        playerAnimationBehaviour.SetupBehaviour();
    }

    //This is called from PlayerInput, when a button has been pushed, that corresponds with the 'Attack' action
    public void OnAttack(InputAction.CallbackContext value)
    {
    }

    //This is called from Player Input, when a button has been pushed, that correspons with the 'TogglePause' action
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
                playerInput.DeactivateInput();
                break;

            case false:
                playerInput.ActivateInput();
                break;
        }
    }

    void RemoveAllBindingOverrides()
    {
        InputActionRebindingExtensions.RemoveAllBindingOverrides(playerInput.currentActionMap);
    }

    //Switching Action Maps ----
    public void EnableGameplayControls()
    {
        playerInput.SwitchCurrentActionMap(m_ActionMapPlayerControls);
    }

    public void EnablePauseMenuControls()
    {
        playerInput.SwitchCurrentActionMap(m_ActionMapMenuControls);
    }

    //Get Data ----
    public int GetPlayerID()
    {
        return m_PlayerID;
    }

    public InputActionAsset GetActionAsset()
    {
        return playerInput.actions;
    }

    public PlayerInput GetPlayerInput()
    {
        return playerInput;
    }
}
