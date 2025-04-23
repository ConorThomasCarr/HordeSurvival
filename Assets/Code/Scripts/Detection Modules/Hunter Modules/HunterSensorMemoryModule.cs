using System.Collections.Generic;
using ISensor;
using ISensor.ISensorMemory;
using UnityEngine;

public class HunterSensorMemoryModule : ISensorMemoryModule
{
    public List<AIMemory> memories { get; set; }

    public GameObject[] characters { get; set; }

    public HunterSensorMemoryModule(int maxPlayers)
    {
        memories = new List<AIMemory>();
        characters = new GameObject[maxPlayers];

        EventManager.AddListener<MeshIsDetected>(CheckMeshIsDetected);
    }

    public void UpdateSenses(ISensorModule sensorModule)
    {
        int targets = sensorModule.Filter(characters, "Enemies");

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

        var evtShowMesh = DetectionEvent.ShowMesh;
        evtShowMesh.ReceiverName = memory.gameObject.transform.parent.name;
        evtShowMesh.DetectionAge = 2;
        
        EventManager.Broadcast(evtShowMesh);
        
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
        memories.RemoveAll(x => x.gameObject == null);
        memories.RemoveAll(x => x.age > olderThen);
    }

    void CheckMeshIsDetected(MeshIsDetected meshIsDetected)
    {
        AIMemory memory = memories.Find(x => x.gameObject == meshIsDetected.ReceiverObject);

        if (memory == null)
        {
            var evtHideMesh = DetectionEvent.HideMesh; 
            
            evtHideMesh.ReceiverName = meshIsDetected.ReceiverName;
            
            EventManager.Broadcast(evtHideMesh);
        }
    }
}
