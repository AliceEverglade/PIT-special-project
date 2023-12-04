using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLibrary : ScriptableObject
{
    public CharacterData MainCharacter;
    public List<CharacterData> Characters;

    public CharacterData GetCharacter(string id)
    {
        if(MainCharacter.ID == id)
        {
            return MainCharacter;
        }
        else
        {
            foreach(CharacterData character in Characters)
            {
                if(character.ID == id)
                {
                    return character;
                }
            }
        }
        return null;
    }
}
