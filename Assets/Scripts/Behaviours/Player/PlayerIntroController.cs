using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerIntroController : MonoBehaviour
{
    public float TransitionTime = 1f;
    public void OnSubmit(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            StartCoroutine(TransitionRoutine());
        }
    }

    private IEnumerator TransitionRoutine()
    {
        yield return new WaitForSeconds(TransitionTime);
        SceneManager.LoadScene(1);
    }
}
