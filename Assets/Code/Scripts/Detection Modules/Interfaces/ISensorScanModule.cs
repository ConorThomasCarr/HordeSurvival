using System.Collections.Generic;
using UnityEngine;

using ISensor.SensorColourData;
using ISensor.SensorData;
using ISensor.SensorScanData;

namespace ISensor.AISSensorScanModule
{
    public interface ISensorScanModule
    {
        public SensorDataStruct SensorData { get; set; }
        public SensorScanDataStruct SensorScanData { get; set; }
        public SensorColourDataStruct SensorColourData { get; set; }
        
        public int Count { get; set; }
        
        public Collider[] Collider {get; set;} 
        
        public List<GameObject> InRangeObjects {get; set;}

        public void InitializeScanSensorModule(SensorDataStruct sensorData, SensorScanDataStruct sensorScanData, SensorColourDataStruct sensorColourData);
        
        public void Enable();

        public void Disable();
        
        public void Scan();

        public bool IsInSight(GameObject obj);

        public void OnDrawGizmos();

    }
}