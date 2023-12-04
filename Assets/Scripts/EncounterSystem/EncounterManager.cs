using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public static event Action<string/*encounterID*/, string/*encounterStateID*/> OnEncounterActivated;
    public static EncounterManager Instance;
    [SerializeField] EncounterLibrary library;
    private void Awake()
    {
        Instance = this;
        library.SetInstance();
    }


    public void ActivateEncounter(string encounterID, string encounterStateID)
    {
        OnEncounterActivated?.Invoke(encounterID, encounterStateID);
    }
}
