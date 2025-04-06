using UnityEngine;

public static class FsmActionEvents
{
    public static readonly OnEnterAction OnEnterAction = new OnEnterAction();
    public static readonly OnExitAction OnExitAction = new OnExitAction();
    
    public static readonly OnEnterTaskActionCondition OnEnterTaskActionCondition = new OnEnterTaskActionCondition();
    public static readonly OnExitTaskActionCondition OnExitTaskActionCondition = new OnExitTaskActionCondition();
    
    public static readonly OnContinueAction OnContinueAction = new OnContinueAction();
    public static readonly OnDiscontinueAction OnDiscontinueAction = new OnDiscontinueAction();
    
    public static readonly OnDiscontinueActionToSearch OnDiscontinueActionToSearch = new OnDiscontinueActionToSearch();
    public static readonly OnDiscontinueActionToChase OnDiscontinueActionToChase = new OnDiscontinueActionToChase();
    public static readonly OnDiscontinueActionToCombat OnDiscontinueActionToCombat = new OnDiscontinueActionToCombat();
}

public class OnEnterAction : GameEvent
{
    public string Sender;
    public bool Enter;
}

public class OnExitAction : GameEvent
{   
    public string Sender;
    public bool Exited;
}

public class OnEnterTaskActionCondition  : GameEvent
{
    public string Sender;
    public bool Condition;
}

public class OnExitTaskActionCondition   : GameEvent
{
    public string Sender;
    public bool Condition;
}

public class OnContinueAction : GameEvent
{
    public string Sender;
}

public class OnDiscontinueAction : GameEvent
{
    public string Sender;
}

public class OnDiscontinueActionToSearch : GameEvent
{
    public string Sender;
}
public class OnDiscontinueActionToChase : GameEvent
{
    public string Sender;
}
public class OnDiscontinueActionToCombat : GameEvent
{
    public string Sender;
}