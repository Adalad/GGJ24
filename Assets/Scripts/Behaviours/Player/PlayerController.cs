using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform CarryTransform;
    public float ThrowForce;
    [Header("Sub Behaviours")]
    public PlayerMovementBehaviour playerMovementBehaviour;
    public PlayerAnimationBehaviour playerAnimationBehaviour;

    [Header("Input Settings")]
    public PlayerInput playerInput;
    public float movementSmoothingSpeed = 1f;

    private Vector3 m_RawInputMovement;
    private Vector3 m_SmoothInputMovement;

    private string m_ActionMapPlayerControls = "Player Controls";
    private string m_ActionMapMenuControls = "Menu Controls";

    private int m_PlayerID;
    private GameObject m_PickedObject;

    //This is called from the GameManager; when the game is being setup.
    public void SetupPlayer(int newPlayerID)
    {
        m_PlayerID = newPlayerID;

        playerAnimationBehaviour.SetupBehaviour();
    }


    //INPUT SYSTEM ACTION METHODS --------------

    //This is called from PlayerInput; when a joystick or arrow keys has been pushed.
    //It stores the input Vector as a Vector3 to then be used by the smoothing function.


    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        m_RawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);
    }

    //This is called from PlayerInput, when a button has been pushed, that corresponds with the 'Attack' action
    public void OnAttack(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            if ((m_PickedObject == null) && TryPickUp())
            {
                playerAnimationBehaviour.PlayAttackAnimation();
            }
            else if (m_PickedObject != null)
            {
                ThrowPickUp();
            }
        }
    }

    //This is called from Player Input, when a button has been pushed, that correspons with the 'TogglePause' action
    public void OnTogglePause(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            GameManager.Instance.TogglePauseState(this);
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

    //Update Loop - Used for calculating frame-based data
    void Update()
    {
        CalculateMovementInputSmoothing();
        UpdatePlayerMovement();
        UpdatePlayerAnimationMovement();
    }

    //Input's Axes values are raw
    void CalculateMovementInputSmoothing()
    {

        m_SmoothInputMovement = Vector3.Lerp(m_SmoothInputMovement, m_RawInputMovement, Time.deltaTime * movementSmoothingSpeed);

    }

    void UpdatePlayerMovement()
    {
        playerMovementBehaviour.UpdateMovementData(m_SmoothInputMovement);
    }

    void UpdatePlayerAnimationMovement()
    {
        playerAnimationBehaviour.UpdateMovementAnimation(m_SmoothInputMovement.magnitude);
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

    private bool TryPickUp()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + 0.5f * Vector3.up, transform.forward, out hitInfo, 1.5f))
        {
            if (hitInfo.collider.CompareTag("Pickable"))
            {
                m_PickedObject = hitInfo.collider.gameObject;
                m_PickedObject.GetComponent<Collider>().enabled = false;
                m_PickedObject.GetComponent<Rigidbody>().isKinematic = true;
                m_PickedObject.transform.parent = CarryTransform;
                m_PickedObject.transform.position = CarryTransform.position;
                m_PickedObject.transform.rotation = CarryTransform.rotation;
                return true;
            }

            return false;
        }

        return false;
    }

    private void ThrowPickUp()
    {
        if (m_PickedObject == null)
        {
            return;
        }

        m_PickedObject.transform.parent = null;
        m_PickedObject.GetComponent<Rigidbody>().isKinematic = false;
        m_PickedObject.GetComponent<Collider>().enabled = true;
        m_PickedObject.GetComponent<Rigidbody>().AddForce(ThrowForce * transform.forward);
        m_PickedObject = null;
    }
}
