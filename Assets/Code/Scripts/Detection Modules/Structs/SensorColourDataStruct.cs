using System;
using UnityEngine;

namespace ISensor.SensorColourData
{   
    [Serializable]
    public struct SensorColourDataStruct
    {
        public Color sensorMeshColor;
        
        public Color inRangeColor;
        
        public Color sensorDetectedColor;
        
        public Color inMemoryColor;
        
        public Color bestMemoryColor;
    }
}


