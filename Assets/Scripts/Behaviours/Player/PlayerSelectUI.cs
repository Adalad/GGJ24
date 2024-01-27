using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerSelectUI : MonoBehaviour
{
    private PlayerInput m_PlayerInputComponent;
    private Vector2 m_RawInput;

    private void Start()
    {
        m_PlayerInputComponent = GetComponent<PlayerInput>();
    }

    public void OnNavigate(InputAction.CallbackContext value)
    {
        m_RawInput = value.ReadValue<Vector2>();
    }

    public void OnSubmit(InputAction.CallbackContext value)
    {
        if (m_PlayerInputComponent == null) return;

        if (value.started)
        {
            CharacterSelector.Instance.PlayerSubmit(m_PlayerInputComponent.playerIndex);
        }
    }

    private void Update()
    {
        CharacterSelector.Instance.PlayerCursorMovement(m_PlayerInputComponent.playerIndex, m_RawInput);
    }
}
