using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelUI : MonoBehaviour
{
    public TMP_Text PlayerText;
    public Image[] PlayerOptions;
    public Sprite[] MoodSprites;

    public void SetUpPanel(int playerId, int[] playerMoods)
    {
        PlayerText.text = "P" + playerId;
        for (int i = 0; i < playerMoods.Length; ++i)
        {
            PlayerOptions[i].sprite = MoodSprites[i];
            PlayerOptions[i].gameObject.SetActive(true);
        }
        for (int i = playerMoods.Length; i < PlayerOptions.Length; ++i)
        {
            PlayerOptions[i].gameObject.SetActive(false);
        }
    }
}
