using System;
using System.Collections.Generic;

// USING NAME SPACE GOES HERE !!

// USING UNITY GOES HERE !!
using UnityEngine;

namespace ISensor.ISensorMemory
{
    public class AIMemory
    {
        public float age => Time.time - lastSeen;
        public GameObject gameObject;
        public Renderer rendererCharacter;
        public Renderer rendererGun;
        public Canvas canvasRenderer;
        public Renderer rangeMeshRender;
        public Vector3 position;
        public Vector3 direction;
        public float range;
        public float angle;
        public float lastSeen;
        public float score;
    }

    
    public interface ISensorMemoryModule
    {
        public List<AIMemory> memories {get; set;}
        
        public GameObject[] characters {get; set;}
        
        public void UpdateSenses(ISensorModule sensorModule);
        
        public void RefreshMemory(GameObject agent, GameObject target);

        public AIMemory FetchMemory(GameObject gameObject);

        public void ForgetMemory(float olderThen);
    }
    
}


