using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(menuName = "Data/CharacterLibrary")]
public class CharacterLibrary : ScriptableSingleton<CharacterLibrary>
{
    public Character MainCharacter;
    public List<Character> Characters;

    public CharacterData GetCharacter(string id)
    {
        if(MainCharacter.data.ID == id)
        {
            return MainCharacter.data;
        }
        else
        {
            foreach(Character character in Characters)
            {
                if(character.data.ID == id)
                {
                    return character.data;
                }
            }
        }
        return null;
    }

    public CharacterData.Emotion GetEmotion(string name)
    {
        CharacterData.Emotion emotion = CharacterData.Emotion.Neutral;
        switch (name.ToLower().Trim())
        {
            case "neutral": emotion = CharacterData.Emotion.Neutral; break;
            case "happy": emotion = CharacterData.Emotion.Happy; break;
            case "sad": emotion = CharacterData.Emotion.Sad; break;
            case "frustrated": emotion = CharacterData.Emotion.Frustrated; break;
            case "angry": emotion = CharacterData.Emotion.Angry; break;
            case "shocked": emotion = CharacterData.Emotion.Shocked; break;
            case "tired": emotion = CharacterData.Emotion.Tired; break;
            case "pouting": emotion = CharacterData.Emotion.Pouting; break;
        }
        return emotion;
    }
}

[System.Serializable]
public class Character
{
    public string Name => data.CharacterName;
    public CharacterData data;
}
