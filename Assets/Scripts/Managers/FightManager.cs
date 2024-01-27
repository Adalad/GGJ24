using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    void Start()
    {
        FightUIManager.Instance.ClearPlayerPanels();
        m_IsPaused = false;

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
            m_ActivePlayerControllers[i].SetupPlayer(i);
        }
    }

    private IEnumerator StartCoolDownRoutine()
    {
        TogglePauseState(null);
        yield return new WaitForSecondsRealtime(CoolDownTime);
        TogglePauseState(null);
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
        int firstTeam = Random.Range(0, 1);
        int teamAPlayer = Random.Range(0, 1);
        int teamBPlayer = Random.Range(0, 1);
    }

    private void EndRound()
    {
        m_Round++;
    }
}
