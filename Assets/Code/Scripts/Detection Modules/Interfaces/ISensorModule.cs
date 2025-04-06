using ISensor.AISSensorScanModule;
using ISensor.ISensorTargeting;
using ISensor.SensorColourData;
using ISensor.SensorData;
using ISensor.SensorScanData;

using UnityEngine;

namespace ISensor
{
        public interface ISensorModule
        {
                public GameObject SensorObject { get; set; }

                public SensorDataStruct SensorData { get; set; }

                public SensorScanDataStruct SensorScanData { get; set; }

                public SensorColourDataStruct SensorColourData { get; set; }

                public ISensorScanModule ScanModule { get; }

                public ISensorTargetingModule SensorTargetingModule { get; set; }

                public void Awake();

                public void Enable();

                public void Disable();

                public void Start();

                public void Update();

                public void InitializeSensorModule(SensorDataStruct sensorData, SensorScanDataStruct sensorScanData,
                        SensorColourDataStruct sensorColourData);

                public int Filter(GameObject[] buffer, string layerName);

                public void OnDrawGizmos();

        }
}

