using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    //Local Multiplayer
    public GameObject playerPrefab;
    public float CoolDownTime = 1f;
    public float RoundTime = 60f;

    public Transform[] Spawns;
    public float SpawnRadius = 1f;
    public GameObject BoxSpawnPrefab;
    public Rect BoxSpawnArea = new Rect();
    public float BoxSpawnHeight = 5f;
    public float BoxSpawnMinTime = 1f;
    public float BoxSpawnMaxTime = 3f;

    public TMP_Text ChronoText;

    //Spawned Players
    private List<PlayerController> m_ActivePlayerControllers;
    private bool m_IsPaused;
    private PlayerController m_FocusedPlayerController;

    public DropZone DropZoneTeamA;
    public DropZone DropZoneTeamB;

    void Start()
    {
        DropZoneTeamA = DropZoneTeamA.GetComponent<DropZone>();
        DropZoneTeamB = DropZoneTeamB.GetComponent<DropZone>();
        m_IsPaused = false;

        SetupLocalMultiplayer();
    }

    void SetupLocalMultiplayer()
    {
        SpawnPlayers();
        SetupActivePlayers();
        StartCoroutine(StartCoolDownRoutine());
        StartCoroutine(SpawnBoxes());
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

                GameObject spawnedPlayer = GameObject.Instantiate(playerPrefab, spawnPosition, spawnRotation);
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

    private IEnumerator StartCoolDownRoutine()
    {
        TogglePauseState(null);
        yield return new WaitForSecondsRealtime(CoolDownTime);
        TogglePauseState(null);
        StartCoroutine(ClockRoutine());
    }

    private IEnumerator ClockRoutine()
    {
        float acumTime = 0f;
        while (acumTime < RoundTime)
        {
            ChronoText.text = (RoundTime - acumTime).ToString();
            acumTime += Time.deltaTime;
            yield return null;
        }

        // TODO count results and change phase
        for (int i = 0; i < DropZoneTeamA.InsidePickables.Count; i++)
        {
           PlayerTypes.TeamAChoices[DropZoneTeamA.InsidePickables[i].gameObject.GetComponent<Pickable>().Mood]++; 
        }

        for (int i = 0; i < DropZoneTeamB.InsidePickables.Count; i++)
        {
            PlayerTypes.TeamAChoices[DropZoneTeamB.InsidePickables[i].gameObject.GetComponent<Pickable>().Mood]++;
        }

        StopAllCoroutines();

        SceneChangeManager.Instance.ChangeSceneTo("FightScene"); 
    }

    private IEnumerator SpawnBoxes()
    {
        float waitTime;
        while (true)
        {
            waitTime = Random.Range(BoxSpawnMinTime, BoxSpawnMaxTime);
            yield return new WaitForSeconds(waitTime);

            // TODO Spawn prefab
            Vector3 newPos = new Vector3(Random.Range(BoxSpawnArea.xMin, BoxSpawnArea.xMax), BoxSpawnHeight, Random.Range(BoxSpawnArea.yMin, BoxSpawnArea.yMax));
            GameObject.Instantiate(BoxSpawnPrefab, newPos, Random.rotation);
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
                m_FocusedPlayerController?.EnablePauseMenuControls();
                break;

            case false:
                m_FocusedPlayerController?.EnableGameplayControls();
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
        float x = Mathf.Cos(angle) * SpawnRadius;
        float z = Mathf.Sin(angle) * SpawnRadius;
        return Spawns[teamID].position + new Vector3(x, 0, z);
    }

    Quaternion CalculateRotation()
    {
        return Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
    }

}
