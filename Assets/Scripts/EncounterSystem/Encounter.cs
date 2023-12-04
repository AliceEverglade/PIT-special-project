using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Encounter
{
    public string Name => encounterID;
    public string encounterID;
    public List<EncounterState> EncounterStates;

    public int GetStateIndexById(string id)
    {
        for (int i = 0; i < EncounterStates.Count; i++)
        {
            if (EncounterStates[i].stateID == id)
            {
                return i;
            }
        }
        Debug.LogError("State index could not be found with id: " + id);
        return -1;
    }

    public bool HasState(string id)
    {
        return EncounterStates.Contains(GetStateById(id));
    }

    public bool HasState(EncounterState state)
    {
        return EncounterStates.Contains(state);
    }

    public EncounterState GetStateById(string id)
    {
        foreach(EncounterState encounterState in EncounterStates)
        {
            if(encounterState.stateID == id) return encounterState;
        }
        Debug.LogError("State could not be found with id: " + id);
        return null;
    }
}

[System.Serializable]
public class EncounterState
{
    public string Name => stateID;
    public string stateID = "";
    //might get more variables in the future
}
