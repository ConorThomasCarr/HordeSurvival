using System.Collections.Generic;
using ISensor;
using ISensor.ISensorMemory;
using UnityEngine;

public class ZombieSoundSensorMemoryModule : ISensorMemoryModule
{
    public List<AIMemory> memories {get; set;}
        
    public GameObject[] characters {get; set;}

    public ZombieSoundSensorMemoryModule(int maxPlayers)
    {
        memories = new List<AIMemory>();
        characters = new GameObject[maxPlayers];
    }
    
    public void UpdateSenses(ISensorModule sensorModule)
    {
        int targets = sensorModule.Filter(characters, "Noise Emitter");

        for (int i = 0; i < targets; i++)
        {
            GameObject target = characters[i];
            RefreshMemory(sensorModule.SensorObject, target);
        }
    }

    public void RefreshMemory(GameObject agent, GameObject target)
    {
        AIMemory memory = FetchMemory(target);
        memory.gameObject = target;
        memory.position = target.transform.position;
        memory.direction = target.transform.position - agent.transform.position;
        memory.range = memory.direction.magnitude;
        memory.angle = Vector3.Angle(agent.transform.forward, memory.direction);
        memory.lastSeen = Time.time;
    }

    public AIMemory FetchMemory(GameObject gameObject)
    {
        AIMemory memory = memories.Find(x => x.gameObject == gameObject);

        if (memory == null)
        {
            memory = new AIMemory();
            memories.Add(memory);
        }
        
        return memory;
        
    }

    public void ForgetMemory(float olderThen)
    { 
        memories.RemoveAll(x => x.age > olderThen);
    }
}
