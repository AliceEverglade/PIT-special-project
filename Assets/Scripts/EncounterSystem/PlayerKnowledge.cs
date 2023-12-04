using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnowledge : MonoBehaviour
{
    [SerializeField] private List<MemoryState> PlayerEncounterMemory;

    #region enable disable
    private void OnEnable()
    {
        EncounterManager.OnEncounterActivated += PrepareMemory;
    }

    private void OnDisable()
    {
        EncounterManager.OnEncounterActivated -= PrepareMemory;
    }
    #endregion

    #region CheckIfKnown
    //check if player has knowledge of certain events
    public bool CheckIfKnown(MemoryState state)
    {
        bool known = false;
        foreach (MemoryState memoryState in PlayerEncounterMemory)
        {
            //if the required encounter was found, continue
            if(memoryState.EncounterID == state.EncounterID)
            {
                //if input state's index is lower than the memoryState.state's index, set known to true;
                if (EncounterLibrary.Instance.GetEncounterByID(state.EncounterID).GetStateIndexById(state.State.stateID) <
                    EncounterLibrary.Instance.GetEncounterByID(state.EncounterID).GetStateIndexById(memoryState.State.stateID))
                {
                    return true;
                }
            }
            
        }
        return known;
    }
    public bool CheckIfKnown(List<MemoryState> states)
    {
        bool known = false;
        foreach (MemoryState state in states)
        {
            known = CheckIfKnown(state);
        }
        return known;
    }
    public bool CheckIfKnown(string encounterId, string stateId)
    {
        return CheckIfKnown(new MemoryState(encounterId, EncounterLibrary.Instance.GetEncounterByID(encounterId).GetStateById(stateId)));
    }
    public bool CheckIfKnown(List<string> encounterIds, List<string> stateIds)
    {
        bool known = false;
        for(int i = 0; i < encounterIds.Count; i++)
        {
            known = CheckIfKnown(encounterIds[i], stateIds[i]);
        }
        return known;
    }
    #endregion
    
    private void PrepareMemory(string encounterId, string stateId)
    {
        EncounterState state = EncounterLibrary.Instance.GetEncounterByID(encounterId).GetStateById(stateId);
        MemoryState memoryState = new MemoryState(encounterId, state);
        UpdateMemory(memoryState);
    }
    public void UpdateMemory(MemoryState state)
    {
        bool stateExists = false;
        foreach(MemoryState memoryState in PlayerEncounterMemory)
        {
            //if no memory state with the state machine ID is present in Memory, add it
            if (memoryState.EncounterID == state.EncounterID)
            {
                Debug.Log("Matching encounter found in memory, checking to update");
                stateExists = true;
                //if new state is higher than old state, set new state
                if (EncounterLibrary.Instance.CheckIfStateUpdates(
                    state.EncounterID, 
                    state.State.stateID,
                    memoryState.State.stateID))
                {
                    memoryState.UpdateState(state.State);
                }
                else
                {
                    Debug.Log("didn't update state because the new state is not higher than the previous state");
                }
            }

        }
        //if no state with the same EncounterID exists, add state to the memory
        if (!stateExists)
        {
            PlayerEncounterMemory.Add(state);
        }
    }
}

[System.Serializable]
public class MemoryState
{
    public static event Action <MemoryState> MemoryStateChanged;
    public string EncounterID;
    public EncounterState State;
    public MemoryState(string id, EncounterState state)
    {
        this.EncounterID = id;
        UpdateState(state);
    }

    public void UpdateState(EncounterState state)
    {
        this.State = state;
        MemoryStateChanged?.Invoke(this);
    }
}
