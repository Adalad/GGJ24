using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndManager : Singleton<EndManager>
{
    //Local Multiplayer
    public GameObject PlayerPrefab;
    public Transform[] Spawns;

    //Spawned Players
    private List<PlayerControllerEnd> m_ActivePlayerControllers;
    private PlayerControllerEnd m_FocusedPlayerController;

    void Start()
    {
        SetupLocalMultiplayer();
    }

    void SetupLocalMultiplayer()
    {
        SpawnPlayers();
        SetupActivePlayers();
    }

    void SpawnPlayers()
    {
        m_ActivePlayerControllers = new List<PlayerControllerEnd>();
        for (int i = 0; i < 4; i++)
        {
            GameObject spawnedPlayer = GameObject.Instantiate(PlayerPrefab, Spawns[i].position, Spawns[i].rotation);
            AddPlayerToActivePlayerList(spawnedPlayer.GetComponent<PlayerControllerEnd>());
        }
    }

    void AddPlayerToActivePlayerList(PlayerControllerEnd newPlayer)
    {
        m_ActivePlayerControllers.Add(newPlayer);
    }

    void SetupActivePlayers()
    {
        int result = 0;
        if (PlayerTypes.TeamScores[0] > PlayerTypes.TeamScores[1])
        {
            result = 1;
        }
        else if (PlayerTypes.TeamScores[0] > PlayerTypes.TeamScores[1])
        {
            result = 2;
        }

        for (int i = 0; i < m_ActivePlayerControllers.Count; i++)
        {
            m_ActivePlayerControllers[i].SetupPlayer(i, PlayerTypes.playerAsignedCharacter[i]);
            if ((i < 2) && (result == 1))
            {
                m_ActivePlayerControllers[i].EndReaction(true);
            }
            else if ((i >= 2) && (result == 2))
            {
                m_ActivePlayerControllers[i].EndReaction(true);
            }
            else
            {
                m_ActivePlayerControllers[i].EndReaction(false);
            }
        }
    }

    //Get Data ----
    public List<PlayerControllerEnd> GetActivePlayerControllers()
    {
        return m_ActivePlayerControllers;
    }

    public PlayerControllerEnd GetFocusedPlayerController()
    {
        return m_FocusedPlayerController;
    }

    public int NumberOfConnectedDevices()
    {
        return InputSystem.devices.Count;
    }
}
