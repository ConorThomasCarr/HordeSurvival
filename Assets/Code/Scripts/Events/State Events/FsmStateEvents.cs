using UnityEngine;

public static class FsmStateEvents
{
    public static readonly OnEnterState OnEnterState = new OnEnterState();
    public static readonly OnExitState OnExitState = new OnExitState();
    
    public static readonly OnEnterTaskStateCondition OnEnterTaskStateCondition = new OnEnterTaskStateCondition();
    public static readonly OnExitTaskStateCondition OnExitTaskStateCondition = new OnExitTaskStateCondition();
    
    public static readonly OnContinueState OnContinueState = new OnContinueState();
    public static readonly OnDiscontinueState OnDiscontinueState = new OnDiscontinueState();
    
    public static readonly OnDiscontinueStateToSearch OnDiscontinueStateToSearch = new OnDiscontinueStateToSearch();
    public static readonly OnDiscontinueStateToChase OnDiscontinueStateToChase = new OnDiscontinueStateToChase();
    public static readonly OnDiscontinueStateToCombat OnDiscontinueStateToCombat = new OnDiscontinueStateToCombat();
   
}


public class OnEnterState : GameEvent
{
    public string Sender;
    public bool Enter;
}

public class OnExitState : GameEvent
{   
    public string Sender;
    public bool Exited;
}

public class OnEnterTaskStateCondition   : GameEvent
{
    public string Sender;
    public bool Condition;
}


public class OnExitTaskStateCondition  : GameEvent
{
    public string Sender;
    public bool Condition;
}

public class OnContinueState : GameEvent
{
    public string Sender;
}

public class OnDiscontinueState : GameEvent
{
    public string Sender;
}

public class OnDiscontinueStateToSearch : GameEvent
{
    public string Sender;
}
public class OnDiscontinueStateToChase : GameEvent
{
    public string Sender;
}
public class OnDiscontinueStateToCombat: GameEvent
{
    public string Sender;
}