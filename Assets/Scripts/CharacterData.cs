using EasyButtons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string ID;
    public string Name => characterName;
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
    [SerializeField] private List<EmotionProfile> emotionProfiles;
    public enum Emotion
    {
        Neutral,
        Happy,
        Sad,
        Frustrated,
        Angry,
        Shocked,
        Tired,
        Pouting
    }

    [Button]
    private void SetEmotionNames()
    {
        foreach (EmotionProfile emotion in emotionProfiles)
        {
            emotion.Name = emotion.emotion.ToString();
        }
    }
    public string GetPortrait(Emotion emotion)
    {
        foreach (EmotionProfile profile in emotionProfiles)
        {
            if(profile.emotion == emotion)
            {
                return profile.Portrait;
            }
        }
        return null;
    }

    public AudioProfile GetAudioProfile(Emotion emotion)
    {
        foreach (EmotionProfile profile in emotionProfiles)
        {
            if (profile.emotion == emotion)
            {
                return profile.AudioProfile;
            }
        }
        return null;
    }
}

[System.Serializable]
public class EmotionProfile
{
    [HideInInspector] public string Name;
    public CharacterData.Emotion emotion;
    public string Portrait;
    public AudioProfile AudioProfile;
}

[System.Serializable]
public class AudioProfile
{
    public List<AudioClip> clips;
}


