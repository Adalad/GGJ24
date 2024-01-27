using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectorPlayerManager : Singleton<SelectorPlayerManager>
{
    int ActualPlayerNumbers; 
    public void OnPlayerEnter(PlayerInput player)
    {
        ActualPlayerNumbers++;
        CharacterSelector.Instance.EnabledPlayers(ActualPlayerNumbers); 
    }

    public void OnPlayerLeft(PlayerInput player)
    {
        ActualPlayerNumbers--;
        CharacterSelector.Instance.EnabledPlayers(ActualPlayerNumbers);
    }

}
