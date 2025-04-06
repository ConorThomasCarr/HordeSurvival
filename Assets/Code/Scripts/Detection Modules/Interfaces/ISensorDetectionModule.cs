using UnityEngine;
using UnityEngine.Events;

public interface ISensorDetectionModule
{
    public UnityAction<Vector3> SoundAlert {get; set;}
   
    public UnityAction<Vector3> VisionAlert {get; set;}
   
    public UnityAction<GameObject> CombatAlert {get; set;}
    
    public void Enable();

    public void Disable();
}
