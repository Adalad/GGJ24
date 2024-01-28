using UnityEngine;
using UnityEngine.UI;

public class FightUIManager : Singleton<FightUIManager>
{
    public GameObject ReadyText;
    public PlayerPanelUI[] PlayerPanels;

    public GameObject PublicPanel;
    public Image[] PublicChoices;
    public Sprite[] MoodSprites;

    public void GameReady()
    {
        ReadyText.SetActive(false);
    }

    public void GameInterval()
    {
        ReadyText.SetActive(true);
    }

    public void SetPlayerPanel(int playerID, int[] options)
    {
        if ((playerID < 0) || (PlayerPanels.Length <= playerID))
        {
            return;
        }

        PlayerPanels[playerID].SetUpPanel(playerID, options);
    }

    public void SetPublicPanel(int[] choices)
    {
        for (int t = 0; t < choices.Length; t++)
        {
            int tmp = choices[t];
            int r = Random.Range(t, choices.Length);
            choices[t] = choices[r];
            choices[r] = tmp;
        }

        for (int i = 0; i < PublicChoices.Length; ++i)
        {
            PublicChoices[i].sprite = MoodSprites[choices[i]];
        }

        PublicPanel.SetActive(true);
    }

    public void ClearPlayerPanel(int playerID)
    {
        PlayerPanels[playerID].gameObject.SetActive(false);
    }

    public void ClearPlayerPanels()
    {
        for (int i = 0; i < PlayerPanels.Length; ++i)
        {
            PlayerPanels[i].gameObject.SetActive(false);
        }
    }

    public void ClearPublicPanel()
    {
        PublicPanel.SetActive(false);
    }
}
