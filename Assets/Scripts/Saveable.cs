using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveableObject
{
    public Dictionary<string, string> Save();

    public void Load(Dictionary<string, string> data);
}

public interface ISaveablePref
{
    public Dictionary<string, string> Save();

    public void Load(Dictionary<string, string> data);
}