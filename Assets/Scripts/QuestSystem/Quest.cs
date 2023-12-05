using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Quest
{
    public string Name => QuestName;
    public string QuestName;
    public List<Task> tasks;
}

