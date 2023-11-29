using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task
{
    public string Name => TaskName;
    public string TaskName;
    [TextArea(5,5)] 
    public string Description;
}
