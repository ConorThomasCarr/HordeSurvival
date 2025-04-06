using System;

namespace ISensor.SensorData
{    
    [Serializable]
    public struct SensorDataStruct 
    {
        public float sensorRange;
        
        public float sensorAngle;
        
        public float sensorHeight;
        
        public int sensorSegments;
    }

}

