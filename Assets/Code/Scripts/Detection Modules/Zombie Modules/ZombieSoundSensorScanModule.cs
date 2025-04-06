using System.Collections.Generic;

using ISensor.AISSensorScanModule;
using ISensor.SensorColourData;
using ISensor.SensorData;
using ISensor.SensorScanData;

using UnityEngine;

public class ZombieSoundSensorScanModule : MonoBehaviour, ISensorScanModule
{
    public SensorDataStruct SensorData { get; set; }
    public SensorScanDataStruct SensorScanData { get; set; }
    public SensorColourDataStruct SensorColourData { get; set; }
        
    public int Count { get; set; }
        
    public Collider[] Collider {get; set;} 
        
    public List<GameObject> InRangeObjects {get; set;}

    public void Awake()
    {
        InRangeObjects = new List<GameObject>();
    }
    
    public void Enable()
    {
        //Debug.Log("SoundS ensor Scan Module Enable: " + gameObject.name);

        enabled = true;
    }

    public void Disable()
    {
        //Debug.Log("Sound Sensor Scan Module Disable: " + gameObject.name);
        
        enabled = false;
    }

    public void InitializeScanSensorModule(SensorDataStruct sensorData, SensorScanDataStruct sensorScanData, SensorColourDataStruct sensorColourData)
    {
        SensorData = sensorData;
        SensorScanData = sensorScanData;
        SensorColourData = sensorColourData;
    }

    public void Scan()
    {
        Collider = new Collider[50];
        
        Count = Physics.OverlapSphereNonAlloc(transform.position, SensorData.sensorRange, Collider,
            SensorScanData.detectionMask,
            QueryTriggerInteraction.Collide);
        
        InRangeObjects.Clear();

        for (var i = 0; i < Count; ++i)
        {
            var obj = Collider[i].gameObject;
         
            if (IsInSight(obj))
            {
                InRangeObjects.Add(obj);
                
            }
        }
    }

    public bool IsInSight(GameObject obj)
    {
        var origin = transform.position;
        var dest = obj.transform.position;
        var direction = dest - origin;

        if (direction.y < 0 || direction.y > SensorData.sensorHeight)
        {
            return false;
        }

        direction.y = 0f;

        var deltaAngle = Vector3.Angle(direction, transform.forward);

        if (deltaAngle > SensorData.sensorAngle)
        {
            return false;
        }

        origin.y += SensorData.sensorHeight / 2;
        dest.y = origin.y;

        if (Physics.Linecast(origin, dest, SensorScanData.obstructionMask))
        {
            return false;
        }

        return true;
    }

    public void OnDrawGizmos()
    {
        if (Collider != null)
        {
            Gizmos.color = SensorColourData.sensorDetectedColor;
            for (var i = 0; i < Count; ++i)
            {
                Gizmos.DrawWireSphere(Collider[i].transform.position, 1f);
            }
        }
            
        if (InRangeObjects != null)
        {
            Gizmos.color = SensorColourData.inRangeColor;
            foreach (var obj in InRangeObjects)
            {
                Gizmos.DrawWireSphere(obj.transform.position, 1f);
            }
        }
    }

}
