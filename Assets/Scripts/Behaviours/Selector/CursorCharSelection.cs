using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorCharSelection : MonoBehaviour
{

    public GameObject[] CharacterSelectorPick;
    public int playerIDReference;
    public int selectedCharacterNumber;

    private void Start()
    {
        selectedCharacterNumber = CharacterSelectorPick.Length;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            for (int i = 0; i < CharacterSelectorPick.Length; i++)
            {
                if (CharacterSelectorPick[i].gameObject == hit.collider.gameObject)
                {
                    selectedCharacterNumber = i;
                    CharacterSelector.Instance.PlayerOverCharacter(playerIDReference, selectedCharacterNumber);
                }
            }

        }
        else
        {
            selectedCharacterNumber = CharacterSelectorPick.Length;
            CharacterSelector.Instance.PlayerOverCharacter(playerIDReference, -1);
        }

    }
}
