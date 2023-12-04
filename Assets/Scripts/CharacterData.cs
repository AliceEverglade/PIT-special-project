using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string ID;
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

    public string GetPortrait(Emotion emotion)
    {
        foreach (EmotionProfile profile in emotionProfiles)
        {
            if(profile.emotion == emotion)
            {
                return profile.portrait;
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
                return profile.audioProfile;
            }
        }
        return null;
    }

}

[System.Serializable]
public class EmotionProfile
{
    public CharacterData.Emotion emotion;
    public string portrait;
    public AudioProfile audioProfile;
}

[System.Serializable]
public class AudioProfile
{
    public List<AudioClip> clips;
}


