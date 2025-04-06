using UnityEngine;

public static class SensorEvents
{
    public static readonly OnTargetHeard OnTargetHeard = new OnTargetHeard();
    public static readonly OnTargetSighted OnTargetSighted = new OnTargetSighted();
    public static readonly OnTargetIsInRange OnTargetIsInRange = new OnTargetIsInRange();
}

public class OnTargetHeard: GameEvent
{   
    public string Sender;
    public bool Heard;    
    public Vector3 SoundLocation;
}

public class OnTargetSighted: GameEvent
{   
    public string Sender;
    public bool Sighted;
    public Vector3 SightedLocation;
}

public class OnTargetIsInRange: GameEvent
{   
    public string Sender;
    public bool Combat;
    public GameObject Target;
}


