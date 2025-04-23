using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyObjectPool : MonoBehaviour
{
    public static EnemyObjectPool Instance;

    public List<AIMaster> _zombies = new List<AIMaster>();
    
    [SerializeField]
    private int amountToPoolZombies = 20;
    
    [SerializeField]
    private AIMaster zombiesPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        for (int i = 0; i < amountToPoolZombies; i++)
        {
            var zombiePrefab = Instantiate(zombiesPrefab);
            zombiePrefab.name = "Zombie" + " " + "(" + i + ")";
            _zombies.Add(zombiePrefab);
        }

    }

    public AIMaster GetZombiesPooledObject()
    {
        for (var i = 0; i < _zombies.Count; i++)
        {
            if (!_zombies[i].enabled)
            {
                return _zombies[i];
            }

        }

        return null;
    }
    

    
    public void RemoveZombiesPooledObject(AIMaster zombie)
    {
        if (zombie != null)
        { 
            _zombies.Remove(zombie);
        }
    }
    

    public void AddZombiePooledObject(AIMaster zombie)
    {
        if (zombie != null)
        {
            _zombies.Add(zombie);
        }
    }
}
