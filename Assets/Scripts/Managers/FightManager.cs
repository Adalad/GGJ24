using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class FightManager : Singleton<FightManager>
{
    //Local Multiplayer
    public GameObject PlayerPrefab;
    public float CoolDownTime = 1f;

    public Transform[] Spawns;

    //Spawned Players
    private List<FightPlayerController> m_ActivePlayerControllers;
    private bool m_IsPaused;
    private FightPlayerController m_FocusedPlayerController;

    private int m_Round;
    private int[] m_Scores;
    private int[] m_PublicChoices;
    private int m_TeamAPlayer;
    private int[] m_TeamAPlayerOptions;
    private int m_TeamAOption;
    private int m_TeamBPlayer;
    private int[] m_TeamBPlayerOptions;
    private int m_TeamBOption;

    public GameObject[] lightPointReference;

    void Start()
    {
        FightUIManager.Instance.ClearPlayerPanels();
        FightUIManager.Instance.ClearPublicPanel();
        m_IsPaused = false;
        m_Scores = new int[2];
        m_PublicChoices = new int[3];

        SetupLocalMultiplayer();
    }

    void SetupLocalMultiplayer()
    {
        SpawnPlayers();
        SetupActivePlayers();
        StartCoroutine(StartCoolDownRoutine());
    }

    void SpawnPlayers()
    {
        m_ActivePlayerControllers = new List<FightPlayerController>();
        for (int i = 0; i < 4; i++)
        {
            GameObject spawnedPlayer = GameObject.Instantiate(PlayerPrefab, Spawns[i].position, Spawns[i].rotation);
            AddPlayerToActivePlayerList(spawnedPlayer.GetComponent<FightPlayerController>());
        }
    }

    void AddPlayerToActivePlayerList(FightPlayerController newPlayer)
    {
        m_ActivePlayerControllers.Add(newPlayer);
    }

    void SetupActivePlayers()
    {
        for (int i = 0; i < m_ActivePlayerControllers.Count; i++)
        {
            // Player types
            m_ActivePlayerControllers[i].SetupPlayer(i, PlayerTypes.playerAsignedCharacter[i]);
        }
    }

    private IEnumerator StartCoolDownRoutine()
    {
        TogglePauseState(null);
        yield return new WaitForSecondsRealtime(CoolDownTime);
        TogglePauseState(null);
        FightUIManager.Instance.GameReady();
        StartRound();
    }

    public void TogglePauseState(FightPlayerController newFocusedPlayerController)
    {
        m_FocusedPlayerController = newFocusedPlayerController;

        m_IsPaused = !m_IsPaused;

        ToggleTimeScale();

        UpdateActivePlayerInputs();

        SwitchFocusedPlayerControlScheme();
    }

    void UpdateActivePlayerInputs()
    {
        for (int i = 0; i < m_ActivePlayerControllers.Count; i++)
        {
            if (m_ActivePlayerControllers[i] != m_FocusedPlayerController)
            {
                m_ActivePlayerControllers[i].SetInputActiveState(m_IsPaused);
            }

        }
    }

    void SwitchFocusedPlayerControlScheme()
    {
        switch (m_IsPaused)
        {
            case true:
                m_FocusedPlayerController?.EnablePauseMenuControls();
                break;

            case false:
                m_FocusedPlayerController?.EnableGameplayControls();
                break;
        }
    }

    //Get Data ----
    public List<FightPlayerController> GetActivePlayerControllers()
    {
        return m_ActivePlayerControllers;
    }

    public FightPlayerController GetFocusedPlayerController()
    {
        return m_FocusedPlayerController;
    }

    public int NumberOfConnectedDevices()
    {
        return InputSystem.devices.Count;
    }

    //Pause Utilities ----
    void ToggleTimeScale()
    {
        float newTimeScale = 0f;

        switch (m_IsPaused)
        {
            case true:
                newTimeScale = 0f;
                break;

            case false:
                newTimeScale = 1f;
                break;
        }

        Time.timeScale = newTimeScale;
    }

    private void StartRound()
    {
        FightUIManager.Instance.GameReady();
        GeneratePublicChoices();
        int firstTeam = Random.Range(0, 2);
        m_TeamAPlayer = Random.Range(0, 2);
        m_TeamAOption = -1;
        m_TeamBPlayer = Random.Range(0, 2);
        m_TeamBOption = -1;
        m_TeamAPlayerOptions = new int[4];
        int maxOptions = 0;
        for (int i = 0; i < PlayerTypes.TeamAChoices.Length; ++i)
        {
            if (PlayerTypes.TeamAChoices[i] != 0)
            {
                maxOptions++;
            }
        }
        m_TeamAPlayerOptions[0] = PlayerTypes.playerAsignedCharacter[m_TeamAPlayer];
        if (maxOptions == 0)
        {
            maxOptions = 1;
            m_TeamAPlayerOptions[1] = Random.Range(0, 6);
        }
        else
        {
            maxOptions = maxOptions > 3 ? 3 : maxOptions;
            for (int i = 1; i < maxOptions + 1; ++i)
            {
                while (true)
                {
                    int nextTry = Random.Range(0, PlayerTypes.TeamAChoices.Length);
                    if (PlayerTypes.TeamAChoices[nextTry] != 0)
                    {
                        m_TeamAPlayerOptions[i] = nextTry;
                        break;
                    }
                }
            }
        }
        for (int i = maxOptions + 1; i < m_TeamAPlayerOptions.Length; ++i)
        {
            m_TeamAPlayerOptions[i] = -1;
        }

        FightUIManager.Instance.SetPlayerPanel(m_TeamAPlayer, m_TeamAPlayerOptions);

        m_TeamBPlayerOptions = new int[4];
        maxOptions = 0;
        for (int i = 0; i < PlayerTypes.TeamAChoices.Length; ++i)
        {
            if (PlayerTypes.TeamAChoices[i] != 0)
            {
                maxOptions++;
            }
        }
        m_TeamBPlayerOptions[0] = PlayerTypes.playerAsignedCharacter[m_TeamBPlayer];
        if (maxOptions == 0)
        {
            maxOptions = 1;
            m_TeamBPlayerOptions[1] = Random.Range(0, 6);
        }
        else
        {
            maxOptions = maxOptions > 3 ? 3 : maxOptions;
            for (int i = 1; i < maxOptions + 1; ++i)
            {
                while (true)
                {
                    int nextTry = Random.Range(0, PlayerTypes.TeamBChoices.Length);
                    if (PlayerTypes.TeamBChoices[nextTry] != 0)
                    {
                        m_TeamBPlayerOptions[i] = nextTry;
                        break;
                    }
                }
            }
        }
        for (int i = maxOptions + 1; i < m_TeamBPlayerOptions.Length; ++i)
        {
            m_TeamBPlayerOptions[i] = -1;
        }

        FightUIManager.Instance.SetPlayerPanel(2 + m_TeamBPlayer, m_TeamBPlayerOptions);
    }

    public void ReceivePlayerOption(int playerId, int option)
    {
        if ((m_TeamAPlayer == playerId) && (m_TeamAOption == -1))
        {
            m_TeamAOption = m_TeamAPlayerOptions[option];
            FightUIManager.Instance.ClearPlayerPanel(playerId);
        }
        else if ((m_TeamBPlayer + 2 == playerId) && (m_TeamBOption == -1))
        {
            m_TeamBOption = m_TeamBPlayerOptions[option];
            FightUIManager.Instance.ClearPlayerPanel(playerId);
        }

        if ((m_TeamAOption != -1) && (m_TeamBOption != -1))
        {
            EndRound();
        }
    }

    private void GeneratePublicChoices()
    {
        bool retry;
        for (int i = 0; i < 3; ++i)
        {
            int nextTry = -1;
            do
            {
                retry = false;
                nextTry = Random.Range(0, 6);
                for (int j = 0; j < 3; ++j)
                {
                    if (m_PublicChoices[j] == nextTry)
                    {
                        retry = true;
                    }
                }
            }
            while (retry);

            m_PublicChoices[i] = nextTry;
        }

        FightUIManager.Instance.SetPublicPanel(m_PublicChoices);
    }

    private void EndRound()
    {
        FightUIManager.Instance.ClearPublicPanel();
        m_Round++;

        int currentA = 0;
        int currentB = 0;
        // TEAM A Score
        if (m_PublicChoices[0] == m_TeamAOption)
        {
            currentA = 2;
        }
        else if (m_PublicChoices[1] == m_TeamAOption)
        {
            currentA = 1;
        }
        else if (m_PublicChoices[2] == m_TeamAOption)
        {
            currentA = 0;
        }
        // TEAM B Score
        if (m_PublicChoices[0] == m_TeamBOption)
        {
            currentB = 2;
        }
        else if (m_PublicChoices[1] == m_TeamBOption)
        {
            currentB = 1;
        }
        else if (m_PublicChoices[2] == m_TeamBOption)
        {
            currentB = 0;
        }

        if (currentA > currentB)
        {
            m_Scores[0]++;
            StartCoroutine(InterRoundRoutine(0));
        }
        else if (currentA < currentB)
        {
            m_Scores[1]++;
            StartCoroutine(InterRoundRoutine(1));
        }
        else
        {
            StartCoroutine(InterRoundRoutine(2));
        }
    }

    private IEnumerator InterRoundRoutine(int result)
    {
        if (m_Round < 3)
        {
            FightUIManager.Instance.GameInterval();
        }

        yield return StartCoroutine(ReactionsRoutine(result));
        if (m_Round == 3)
        {
            SceneChangeManager.Instance.ChangeSceneTo("EndScene");
        }
        else
        {
            StartRound();
        }
    }

    private IEnumerator ReactionsRoutine(int result)
    {
        // TODO Round result lights
        // Players reactions
        if (result == 0)
        {
            m_ActivePlayerControllers[0].RoundReaction(true);
            m_ActivePlayerControllers[1].RoundReaction(true);
            m_ActivePlayerControllers[2].RoundReaction(false);
            m_ActivePlayerControllers[3].RoundReaction(false);
        }
        else if (result == 1)
        {
            m_ActivePlayerControllers[0].RoundReaction(false);
            m_ActivePlayerControllers[1].RoundReaction(false);
            m_ActivePlayerControllers[2].RoundReaction(true);
            m_ActivePlayerControllers[3].RoundReaction(true);
        }
        if (result == 2)
        {
            m_ActivePlayerControllers[0].RoundReaction(false);
            m_ActivePlayerControllers[1].RoundReaction(false);
            m_ActivePlayerControllers[2].RoundReaction(false);
            m_ActivePlayerControllers[3].RoundReaction(false);
        }
        // Delay
        yield return new WaitForSeconds(5f);
    }
}
