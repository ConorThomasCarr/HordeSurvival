using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


[ExecuteInEditMode]
public class WaveManager : MonoBehaviour
{

    [SerializeField] private List<WaveSpawnSlot> _waveSpawnData;

    [SerializeField] private List<WaveData> _waveData;

    [SerializeField] private bool spawn;

    [SerializeField] private int currentWave;

    private void Start()
    {
       
    }

    private void Update()
    {
        if (spawn)
        {
            for (int i = 0; i < _waveData[currentWave].waveSlot.waveNumber; i++)
            {
                SpawnEnemy();
            }

            spawn = false;

        }
        
        AddWaveSpawnData();

    }

    private void AddWaveSpawnData()
    {
        foreach (var t in _waveSpawnData)
        {
            var spawnOffsetStartPoint = new Vector3(
                t.spawnOriginData.spawnStartPoint.position.x - t.spawnOffsetData.offsetPointsByX,
                t.spawnOriginData.spawnStartPoint.position.y,
                t.spawnOriginData.spawnStartPoint.position.z - t.spawnOffsetData.offsetPointsByZ);

            var spawnOffsetEndPoint = new Vector3(
                t.spawnOriginData.spawnEndPoint.position.x + t.spawnOffsetData.offsetPointsByX,
                t.spawnOriginData.spawnEndPoint.position.y,
                t.spawnOriginData.spawnEndPoint.position.z + t.spawnOffsetData.offsetPointsByZ);

            var spawnOffsetCentre = (spawnOffsetStartPoint + spawnOffsetEndPoint) / 2;
            spawnOffsetCentre.y = spawnOffsetStartPoint.y;

            t.spawnOriginData.spawnWidth = (spawnOffsetStartPoint.x - spawnOffsetEndPoint.x);
            t.spawnOriginData.spawnLength = (spawnOffsetStartPoint.z - spawnOffsetEndPoint.z);

            t.spawnData.spawnMinWidthX =
                spawnOffsetCentre.x - (t.spawnOriginData.spawnWidth / 2);

            t.spawnData.spawnMaxWidthX =
                spawnOffsetCentre.x + (t.spawnOriginData.spawnWidth / 2);

            t.spawnData.spawnMinLengthZ =
                spawnOffsetCentre.z - (t.spawnOriginData.spawnLength / 2);

            t.spawnData.spawnMaxLengthZ =
                spawnOffsetCentre.z + (t.spawnOriginData.spawnLength / 2);
        }
    }

    private void SpawnEnemy()
    {
        var enemy = EnemyObjectPool.Instance.GetZombiesPooledObject();

        enemy.transform.position = CreateRandomSpawn();
        enemy.gameObject.SetActive(true);
        enemy.enabled = true;
       
        EnemyObjectPool.Instance.RemoveZombiesPooledObject(enemy);
    }

    private Vector3 CreateRandomSpawn()
    {
        var randomIndex = Random.Range(0, _waveSpawnData.Count);

        var spawnPosition = new Vector3
        {
            x = Random.Range(_waveSpawnData[randomIndex].spawnData.spawnMinWidthX,
                _waveSpawnData[randomIndex].spawnData.spawnMaxWidthX),
            y = _waveSpawnData[randomIndex].spawnOriginData.spawnStartPoint.position.y,
            z = Random.Range(_waveSpawnData[randomIndex].spawnData.spawnMinLengthZ,
                _waveSpawnData[randomIndex].spawnData.spawnMaxLengthZ)
        };

        if (RandomPoint(spawnPosition, 1, out var point))
        {
            spawnPosition.y = point.y;
        }

        return spawnPosition;
    }


    private static bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        if (NavMesh.SamplePosition(center, out var hit, range, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void OnDrawGizmos()
    {
        if (_waveSpawnData == null) return;
        foreach (var t in _waveSpawnData)
        {

            Gizmos.color = Color.blue;

            Gizmos.DrawWireCube(
                new Vector3(t.spawnData.spawnMinWidthX,
                    t.spawnOriginData.spawnEndPoint.position.y, t.spawnData.spawnMinLengthZ),
                new Vector3(1, 1, 1));
            Gizmos.DrawWireCube(
                new Vector3(t.spawnData.spawnMaxWidthX,
                    t.spawnOriginData.spawnStartPoint.position.y, t.spawnData.spawnMaxLengthZ),
                new Vector3(1, 1, 1));


            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector3(
                    (t.spawnData.spawnMinWidthX + t.spawnData.spawnMaxWidthX) / 2,
                    t.spawnOriginData.spawnStartPoint.position.y,
                    (t.spawnData.spawnMinLengthZ + t.spawnData.spawnMaxLengthZ) / 2),

                new Vector3(t.spawnOriginData.spawnWidth, 1, t.spawnOriginData.spawnLength));
        }
    }
}

[Serializable]
internal struct WaveData
{
    public WaveSlot waveSlot;
}

[Serializable]
internal struct WaveSlot
{
    public int waveNumber;
}


[Serializable]
internal class WaveSpawnSlot
{  
    [Space(5)]
    public string spawnName;
    [Space(5)]
    public SpawnOriginData spawnOriginData;
    [Space(5)]
    public SpawnOffsetData spawnOffsetData;
    
    [Space(5)]
    public SpawnData spawnData;
}

[Serializable]
internal struct SpawnOriginData
{ 
    public Transform spawnStartPoint;

    public Transform spawnEndPoint;
    
    public float spawnWidth; 
    
    public float spawnLength;
    
}

[Serializable]
internal struct SpawnOffsetData
{ 
    public float offsetPointsByX;
    
    public float offsetPointsByZ;
    
}

[Serializable]
internal struct SpawnData
{
    public float spawnMinWidthX;
    
    public float spawnMaxWidthX;

    public float spawnMinLengthZ; 
    
    public float spawnMaxLengthZ;
}

