using UnityEngine;

public static class AITaskEvents
{
    public static readonly OnContinueTask OnContinueTask = new OnContinueTask();
    public static readonly OnDiscontinueTask OnDiscontinueTask = new OnDiscontinueTask();
    
    public static readonly OnChangeTask OnChangeTask = new OnChangeTask();
    
    public static readonly OnGrsdGenerateArrayRowAndColum OnGrsdGenerateArrayRowAndColum = new OnGrsdGenerateArrayRowAndColum(); 
    public static readonly OnGrsdClearArrayRowAndColum OnGrsdClearArrayRowAndColum = new OnGrsdClearArrayRowAndColum();
}

public class OnContinueTask : GameEvent
{
    public string Sender;
}

public class OnGrsdGenerateArrayRowAndColum : GameEvent
{
    public string Sender;
}

public class OnGrsdClearArrayRowAndColum : GameEvent
{
    public string Sender;
}

public class OnDiscontinueTask : GameEvent
{
    public string Sender;
}

public class OnChangeTask : GameEvent
{
    public string ActionSender;
    public string StateSender;
    
    public FsmActionPhase action;
    public FsmStatePhase state;
}



