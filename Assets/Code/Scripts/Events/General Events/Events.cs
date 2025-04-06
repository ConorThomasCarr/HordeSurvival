using UnityEngine;

namespace Event
{
    public class Events : MonoBehaviour
    {
        public static DisplayMessageEvent DisplayMessageEvent = new DisplayMessageEvent();
    }

}

public static class DetectionEvent
{
    public static readonly AddNoiseMemory AddNoiseMemory = new AddNoiseMemory();
    public static readonly ShowMesh ShowMesh = new ShowMesh();
    public static readonly MeshIsDetected MeshIsDetected = new MeshIsDetected();
    public static readonly HideMesh HideMesh = new HideMesh();
    
}

public static class SearchAreaObjective
{
    public static readonly OnCreatePoint OnCreatePoint = new  OnCreatePoint();
    public static readonly OnUpdateSearchPositionArray OnUpdateSearchPositionArray = new  OnUpdateSearchPositionArray();

}

public class AddNoiseMemory: GameEvent
{   
    public Vector3 NoiseLocation;
    public float NoiseIntensity;
}

public class ShowMesh: GameEvent
{
    public string ReceiverName;
    public float DetectionAge;
}

public class MeshIsDetected: GameEvent
{
    public string ReceiverName;
    public GameObject ReceiverObject;
}

public class HideMesh: GameEvent
{   
    public string ReceiverName;
}

public class OnCreatePoint: GameEvent
{
    public int ObjectiveIndex;
    public int SearchPositionArrayRow;
    public int SearchPositionArrayColumn;

}

public class OnUpdateSearchPositionArray: GameEvent
{
    public Vector3[,] PositionArray;
}

public class DisplayMessageEvent : GameEvent
{
    public string Message;
    public float DelayBeforeDisplay;
}

