using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class CharacterSelector : Singleton<CharacterSelector>
{
    public Sprite readySprite;
    public float speed;

    [Header("Controla las 4 cajas de jugadores")]
    //Controla la caja de selección de personaje de cada uno de los jugadores
    public TMP_Text[] CharacterSelectBoxText;
    public Image[] CharacterSelectBoxCharacterSprites;
    public Image[] CharacterReadyBox;
    public Image[] CharacterJoinBox;
    public RectTransform[] PlayerCursors;

    [Header("Controla los elementos de los 6 personajes")]
    //Controla QUE elementos pone en las cajas anteriores
    public string[] CharacterSelectFinalText;
    public Sprite[] CharacterSelectFinalCharacterSprites;

    private int[] m_SelectedCharacters;

    private float[] PlayersSpeeds;

    private void Start()
    {
        PlayersSpeeds = new float[] { 500, 500, 500, 500 };
        m_SelectedCharacters = new int[4];
        for (int i = 0; i < m_SelectedCharacters.Length; ++i)
        {
            m_SelectedCharacters[i] = -1;
        }

        for (int i = 0; i < CharacterReadyBox.Length; i++)
        {
            CharacterReadyBox[i].gameObject.SetActive(false);
        }
    }

    public void EnabledPlayers(int number)
    {
        for (int i = 0; i < number; i++)
        {
            CharacterJoinBox[i].gameObject.SetActive(false);
        }

        for (int i = number; i < 4; i++)
        {
            CharacterJoinBox[i].gameObject.SetActive(true);
        }
    }

    public void PlayerCursorMovement(int playerID, Vector2 movement)
    {
        Vector3 newPos = PlayerCursors[playerID].transform.position + (Vector3)movement * Time.deltaTime * PlayersSpeeds[playerID];
        PlayerCursors[playerID].transform.position = newPos;
    }

    public void PlayerSubmit(int playerID)
    {
        if (m_SelectedCharacters[playerID] == -1)
        {
            return;
        }

        if (!CharacterReadyBox[playerID].gameObject.activeSelf)
        {
            CharacterReadyBox[playerID].gameObject.SetActive(true);
            PlayerTypes.playerAsignedCharacter[playerID] = m_SelectedCharacters[playerID];
            PlayersSpeeds[playerID] = 0;
            PlayersReady();
        }
        else
        {
            CharacterReadyBox[playerID].gameObject.SetActive(false);
            PlayersSpeeds[playerID] = 500;
        }
    }

    //Cuando el jugador en cuestión tiene su puntero encima del botón del personaje. Cambia el texto y el sprite de personaje
    public void PlayerOverCharacter(int playerID, int characterSelected)
    {
        if (!CharacterReadyBox[playerID].gameObject.activeSelf)
        {
            if (characterSelected == -1)
            {
                CharacterSelectBoxText[playerID].text = CharacterSelectFinalText[CharacterSelectFinalText.Length - 1];
                CharacterSelectBoxCharacterSprites[playerID].sprite = CharacterSelectFinalCharacterSprites[CharacterSelectFinalText.Length - 1];
            }
            else
            {
                CharacterSelectBoxText[playerID].text = CharacterSelectFinalText[characterSelected];
                CharacterSelectBoxCharacterSprites[playerID].sprite = CharacterSelectFinalCharacterSprites[characterSelected];
            }

            //Añadir un int playerLoquesea y sustituirlo en el los siguientes arrays que pone 0 para hacer que cada jugador cambie su caja
            m_SelectedCharacters[playerID] = characterSelected;
        }
    }

    void PlayersReady()
    {
        if (CharacterReadyBox[0].gameObject.activeSelf && CharacterReadyBox[1].gameObject.activeSelf && CharacterReadyBox[2].gameObject.activeSelf && CharacterReadyBox[3].gameObject.activeSelf)
        {
            StartCoroutine(TimerBeforeChange());
        }
    }

    IEnumerator TimerBeforeChange()
    {
        yield return new WaitForSeconds(1);
        SceneChangeManager.Instance.ChangeSceneTo("PickScene");
    }


}
