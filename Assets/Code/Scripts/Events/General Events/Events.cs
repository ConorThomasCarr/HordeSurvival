using UnityEngine;

namespace Event
{
    public class Events : MonoBehaviour
    {
        public static DisplayMessageEvent DisplayMessageEvent = new DisplayMessageEvent();
    }

}

public class DisplayMessageEvent : GameEvent
{
    public string Message;
    public float DelayBeforeDisplay;
}

