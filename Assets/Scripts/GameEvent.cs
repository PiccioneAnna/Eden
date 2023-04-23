using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent
{
    public string EventDescription;
}

public class BuildingGameEvent : GameEvent
{
    public string BuildingName;

    public BuildingGameEvent(string name)
    {
        BuildingName = name;
    }
}

public class HoeGameEvent : GameEvent
{
    public HoeGameEvent() { }
}

public class ChopTreeGameEvent : GameEvent
{
    public ChopTreeGameEvent() { }
}
