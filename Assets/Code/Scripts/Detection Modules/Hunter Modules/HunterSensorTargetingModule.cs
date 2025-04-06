using System;
using ISensor;
using ISensor.ISensorMemory;
using ISensor.ISensorTargeting;
using UnityEngine;

public class HunterSensorTargetingModule : MonoBehaviour,  ISensorTargetingModule
{
    public ISensorMemoryModule Memory { get; set; }
    public AIMemory BestMemory { get; set; }
    public ISensorModule Sensor { get; set; }

    public float RangeWeight { get; set; }
    public float AngleWeight { get; set; }
    public float AgeWeight { get; set; }

    public void Awake()
    {
        Memory = new HunterSensorMemoryModule(100);
        
        RangeWeight = 1.0f;
        AngleWeight = 1.0f;
        AgeWeight = 1.0f;
    }
    
    public void Enable()
    {
        //Debug.Log("Vision Sensor Targeting Module Enable: " + gameObject.name);

        enabled = true;
    }

    public void Disable()
    {
        //Debug.Log("Vision Sensor Targeting Module Disable: " + gameObject.name);
        
        enabled = false;
    }

    public void InitializeTargetingModule(ISensorModule sensorModule)
    {
        Sensor = sensorModule;
    }

    public void UpdateMemory()
    {
        Memory.UpdateSenses(Sensor);
        Memory.ForgetMemory(2);
        EvaluateScore();
        
    }

    public float CalculateScore(AIMemory aiMemory)
    {
        var ageScore = Normalize(aiMemory.age, 10) * AgeWeight;
        return ageScore;
    }

    public void EvaluateScore()
    {
        foreach (var aiMemory in Memory.memories)
        {
            aiMemory.score = CalculateScore(aiMemory);

            if (BestMemory == null || aiMemory.score > BestMemory.score)
            {
                BestMemory = aiMemory;
                
            }
        }
    }

    public float Normalize(float value, float maxValue)
    {
        return 1.0f - (value / maxValue);
    }

}
