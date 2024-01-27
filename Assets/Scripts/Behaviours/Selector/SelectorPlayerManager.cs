using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorPlayerManager : Singleton<SelectorPlayerManager>
{
    int ActualPlayerNumbers; 
    public void OnPlayerEnter()
    {
        ActualPlayerNumbers++;
        CharacterSelector.Instance.EnabledPlayers(ActualPlayerNumbers); 
    }

    public void OnPlayerLeft()
    {
        ActualPlayerNumbers--;
        CharacterSelector.Instance.EnabledPlayers(ActualPlayerNumbers);
    }

}
