// USING SYSTEM AND OTHER CRITICAL STUFF UNITY GOES HERE !!
using System;
using System.Collections.Generic;

// USING NAME SPACE GOES HERE !!

// USING UNITY GOES HERE !!
using UnityEngine;
using UnityEngine.Events;

public class DetectionModule : MonoBehaviour, ISensorDetectionModule
{
    public UnityAction<Vector3> SoundAlert {get; set;}
   
    public UnityAction<Vector3> VisionAlert {get; set;}
   
    public UnityAction<GameObject> CombatAlert {get; set;}

    public void Enable()
    {
        SoundAlert += RiseSoundAlert;
        VisionAlert += RiseVisionAlert;
        CombatAlert += RiseCombatAlert;
        
        enabled = true;
    }

    public void Disable()
    {
        SoundAlert -= RiseSoundAlert;
        VisionAlert -= RiseVisionAlert;
        CombatAlert -= RiseCombatAlert;
        
        enabled = false;
    }

    private void RiseSoundAlert(Vector3 noiseLocation)
    {
        var evtOnHeard = SensorEvents.OnTargetHeard;
        evtOnHeard.Heard = true;
        evtOnHeard.SoundLocation = noiseLocation;
        evtOnHeard.Sender = transform.parent.name;
            
        EventManager.Broadcast(evtOnHeard);
    }
        
    private void RiseVisionAlert(Vector3 visionLocation)
    {
        var evtOnSighted = SensorEvents.OnTargetSighted;
        evtOnSighted.Sighted = true;
        evtOnSighted.SightedLocation = visionLocation;
        evtOnSighted.Sender = transform.parent.name;
        EventManager.Broadcast(evtOnSighted);
    }
     
    private void RiseCombatAlert(GameObject target)
    {
        var evtOnCombat = SensorEvents.OnTargetIsInRange;
        evtOnCombat.Combat = true;
        evtOnCombat.Target = target;
        evtOnCombat.Sender = transform.parent.name;
        EventManager.Broadcast(evtOnCombat);
    }
}

    
