using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableSingleton<T> : ScriptableObject where T : ScriptableSingleton<T>
{
    public static T Instance { get; protected set; }

    public void SetInstance()
    {
        Instance = (T)this;
        if(Instance == null)
        {
            Debug.LogError("assignment of instance faled in " + this.name);
        }
    }
}
