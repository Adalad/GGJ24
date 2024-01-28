using UnityEngine;

public class PlayerVisualBehaviour : MonoBehaviour
{
    [Header("Component References")]
    public Renderer PlayerMesh;

    public Material[] TypeMaterials;

    public void SetupBehaviour(int type)
    {
        PlayerMesh.material = TypeMaterials[type]; 
    }
}
