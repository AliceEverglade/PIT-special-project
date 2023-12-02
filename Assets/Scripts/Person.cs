using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person
{
    [HideInInspector] public string Name => characterName;
    [SerializeField] private string characterName;
    public string CharacterName
    {
        get
        {
            return characterName;
        }
        private set
        {
            characterName = value;
        }
    }

    public enum Emotion
    {
        Neutral,
        Happy,
        Sad,
        Frustrated,
        Angry,
        Shocked
    }
}
