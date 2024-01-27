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
        PlayerText.text = "P" + (playerId + 1);
        for (int i = 0; i < playerMoods.Length; ++i)
        {
            if (playerMoods[i] >= 0)
            {
                PlayerOptions[i].sprite = MoodSprites[playerMoods[i]];
                PlayerOptions[i].gameObject.SetActive(true);
            }
            else
            {
                PlayerOptions[i].gameObject.SetActive(false);
            }
        }

        gameObject.SetActive(true);
    }
}
