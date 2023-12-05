using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateManagers : MonoBehaviour
{
    [SerializeField] private ScriptableSingleton<EncounterLibrary> encounterLibrary;
    [SerializeField] private ScriptableSingleton<CharacterLibrary> characterLibrary;

    private void Awake()
    {
        encounterLibrary.SetInstance();
        characterLibrary.SetInstance();
    }
}
