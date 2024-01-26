using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    //Local Multiplayer
    public GameObject playerPrefab;

    public Transform[] Spawns;
    public float spawnRingRadius;

    //Spawned Players
    private List<PlayerController> m_ActivePlayerControllers;
    private bool m_IsPaused;
    private PlayerController m_FocusedPlayerController;

    void Start()
    {

        m_IsPaused = false;

        SetupLocalMultiplayer();
    }

    void SetupLocalMultiplayer()
    {
        SpawnPlayers();

        SetupActivePlayers();
    }

    void SpawnPlayers()
    {
        m_ActivePlayerControllers = new List<PlayerController>();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                Vector3 spawnPosition = CalculatePositionForTeam(i, j, 2);
                Quaternion spawnRotation = CalculateRotation();

                GameObject spawnedPlayer = Instantiate(playerPrefab, spawnPosition, spawnRotation) as GameObject;
                AddPlayerToActivePlayerList(spawnedPlayer.GetComponent<PlayerController>());
            }
        }
    }

    void AddPlayerToActivePlayerList(PlayerController newPlayer)
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

    public void TogglePauseState(PlayerController newFocusedPlayerController)
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
                m_FocusedPlayerController.EnablePauseMenuControls();
                break;

            case false:
                m_FocusedPlayerController.EnableGameplayControls();
                break;
        }
    }

    //Get Data ----

    public List<PlayerController> GetActivePlayerControllers()
    {
        return m_ActivePlayerControllers;
    }

    public PlayerController GetFocusedPlayerController()
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


    //Spawn Utilities

    Vector3 CalculatePositionForTeam(int teamID, int positionID, int numberOfPlayers)
    {
        float angle = (positionID) * Mathf.PI * 2 / numberOfPlayers;
        float x = Mathf.Cos(angle) * spawnRingRadius;
        float z = Mathf.Sin(angle) * spawnRingRadius;
        return Spawns[teamID].position + new Vector3(x, 0, z);
    }

    Quaternion CalculateRotation()
    {
        return Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
    }

}
