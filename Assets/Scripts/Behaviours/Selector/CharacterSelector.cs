using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterSelector : Singleton<CharacterSelector>
{
    public Sprite readySprite;

    [Header("Controla las 4 cajas de jugadores")]
    //Controla la caja de selección de personaje de cada uno de los jugadores
    public TMP_Text[] CharacterSelectBoxText;
    public Image[] CharacterSelectBoxCharacterSprites;
    public Image[] CharacterReadyBox;
    public Image[] CharacterJoinBox;

    [Header("Controla los elementos de los 6 personajes")]
    //Controla QUE elementos pone en las cajas anteriores
    public string[] CharacterSelectFinalText;
    public Sprite[] CharacterSelectFinalCharacterSprites;

    private void Start()
    {
        for (int i = 0; i < CharacterReadyBox.Length; i++)
        {
            CharacterReadyBox[i].gameObject.SetActive(false);
            Debug.Log("Desactivo");
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

    //Cuando el jugador en cuestión tiene su puntero encima del botón del personaje. Cambia el texto y el sprite de personaje
    public void PlayerOverCharacter(int characterSelected)
    {
        if (CharacterReadyBox[0].gameObject.active == false)
        {
            //Añadir un int playerLoquesea y sustituirlo en el los siguientes arrays que pone 0 para hacer que cada jugador cambie su caja
            CharacterSelectBoxText[0].text = CharacterSelectFinalText[characterSelected];
            CharacterSelectBoxCharacterSprites[0].sprite = CharacterSelectFinalCharacterSprites[characterSelected];
        }

    }

    //Cuando el jugador pulsa el botón
    public void PlayerReadyCharacter(int characterSelected)
    {
        if (CharacterReadyBox[0].gameObject.active == false)
        {
            CharacterReadyBox[0].gameObject.SetActive(true);
            PlayerTypes.playerAsignedCharacter[0] = characterSelected;
        }

        else
        {
            CharacterReadyBox[0].gameObject.SetActive(false);
        }
    }

    public void PlayersReady()
    {

    }
}
