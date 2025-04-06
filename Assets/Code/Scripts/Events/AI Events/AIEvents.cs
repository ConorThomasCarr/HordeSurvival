using UnityEngine;
using UnityEngine.AI;

public static class AIEvents
{
    public static readonly OnMoveCharacter OnMoveCharacter = new OnMoveCharacter();
    public static readonly OnDestination OnDestination = new OnDestination();
    public static readonly OnTarget OnTarget = new OnTarget();
    public static readonly OnChangeDestination OnChangeDestination = new OnChangeDestination();
    public static readonly OnCommenceCombat OnCommenceCombat = new OnCommenceCombat();
    public static readonly OnCommenceAttack OnCommenceAttack = new OnCommenceAttack();
}

public class OnMoveCharacter : GameEvent
{
    public Vector3 Position;
}

public class OnDestination : GameEvent
{   
    public string Sender;
    public Vector3 Destination;
}

public class OnTarget : GameEvent
{   
    public string Sender;
    public GameObject Target;
}

public class OnChangeDestination : GameEvent
{   
    public string Sender;
    public Vector3 Destination;
}
public class OnCommenceCombat : GameEvent
{   
    public string Sender;
    public bool CommenceCombat;

}

public class OnCommenceAttack : GameEvent
{   
    public string Sender;
}