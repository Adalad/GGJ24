using UnityEngine;

public class Pickable : MonoBehaviour
{
    public int Mood;

    public Material[] BoxMoodMaterials; 

    private void Start()
    {
        gameObject.GetComponent<Renderer>().material = BoxMoodMaterials[Random.Range(0, 6)];
    }
}
