using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

[CreateAssetMenu(menuName ="Data/EncounterLibrary")]
public class EncounterLibrary : ScriptableSingleton<EncounterLibrary>
{
    public List<Encounter> Encounters;

    public Encounter GetEncounterByID(string id)
    {
        foreach (Encounter encounter in Encounters)
        {
            if(encounter.encounterID == id) return encounter;
        }
        Debug.LogError("Encounter not found with id: " + id);
        return null;
    }

    public bool CheckIfStateUpdates(string encounterId, string newStateId, string oldStateId)
    {
        Encounter encounter = GetEncounterByID(encounterId);
        if(encounter.GetStateIndexById(newStateId) > encounter.GetStateIndexById(oldStateId))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
