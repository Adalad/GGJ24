using UnityEngine;

public class FightUIManager : Singleton<FightUIManager>
{
    public GameObject ReadyText;
    public PlayerPanelUI[] PlayerPanels;

    public void GameReady()
    {
        ReadyText.SetActive(false);
    }

    public void SetPlayerPanel(int playerID, int[] options)
    {
        if ((playerID < 0) || (PlayerPanels.Length <= playerID))
        {
            return;
        }

        PlayerPanels[playerID].SetUpPanel(playerID, options);
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
}
