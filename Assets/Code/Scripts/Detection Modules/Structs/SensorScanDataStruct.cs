using System;
using UnityEngine;

namespace ISensor.SensorScanData
{
    [Serializable]
    public struct SensorScanDataStruct
    {
        public float ScanInterval
        {
            get => 1 / scanFrequency;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
                ScanInterval = 1 / value;
            }
        }
        
        public float scanFrequency;
        
        public LayerMask detectionMask;
        
        public LayerMask obstructionMask;
    }

}

