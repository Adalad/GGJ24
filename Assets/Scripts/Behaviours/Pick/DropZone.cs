using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    private List<GameObject> InsidePickables;

    private void Start()
    {
        InsidePickables = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            InsidePickables.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            InsidePickables.Remove(other.gameObject);
        }
    }
}
