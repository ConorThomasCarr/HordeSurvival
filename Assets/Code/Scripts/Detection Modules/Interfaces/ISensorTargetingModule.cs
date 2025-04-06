using ISensor;
using ISensor.ISensorMemory;

namespace ISensor.ISensorTargeting
{
    public interface ISensorTargetingModule
    {
        public ISensorMemoryModule Memory { get; set; }
        public AIMemory BestMemory { get; set; }
        public ISensorModule Sensor { get; set; }

        public float RangeWeight { get; set; }
        public float AngleWeight { get; set; }
        public float AgeWeight { get; set; }

        public void Awake();
        
        public void Enable();

        public void Disable();
        
        public void InitializeTargetingModule(ISensorModule sensorModule);

        public void UpdateMemory();

        public float CalculateScore(AIMemory aiMemory);

        public void EvaluateScore();

        public float Normalize(float value, float maxValue);

    }
}


