using UnityEngine;

public class Pickable : MonoBehaviour
{
    public int Mood;

    public Material[] BoxMoodMaterials; 

    private void Start()
    {
        int randomNumberMood = Random.Range(0, 6); 
        gameObject.GetComponent<Renderer>().material = BoxMoodMaterials[randomNumberMood];
        Mood= randomNumberMood;
    }
}
