using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ObjectiveZone : MonoBehaviour
{ 
    [SerializeField] private List<ObjectiveSlot> _objectiveData;
    
    public int ObjectiveCount => _objectiveData.Count;

    public static ObjectiveZone  Instance;
    
    private void Awake()
    {
        EventManager.AddListener<OnCreatePoint>(AllowSearchPosition);
        
        var evtExit = SearchAreaObjective.OnUpdateSearchPositionArray;
        evtExit.PositionArray = new Vector3 [100, 100];
        EventManager.Broadcast(evtExit);
    }

    private void Start()
    {
        AddWaveObjectiveData();
        
        if (Instance == null)
        {
            Instance = this;
        }
    }


    private void Update()
    {
      
    }


    private void AddWaveObjectiveData()
    {
        foreach (var t in _objectiveData)
        {
            var objectiveOffsetStartPoint = new Vector3(
                t.objectiveOriginData.objectiveStartPoint.position.x - t.objectiveOffsetData.offsetPointsByX,
                t.objectiveOriginData.objectiveStartPoint.position.y,
                t.objectiveOriginData.objectiveStartPoint.position.z - t.objectiveOffsetData.offsetPointsByZ);

            var objectiveOffsetEndPoint = new Vector3(
                t.objectiveOriginData.objectiveEndPoint.position.x + t.objectiveOffsetData.offsetPointsByX,
                t.objectiveOriginData.objectiveEndPoint.position.y,
                t.objectiveOriginData.objectiveEndPoint.position.z + t.objectiveOffsetData.offsetPointsByZ);

            var objectiveOffsetCentre = (objectiveOffsetStartPoint + objectiveOffsetEndPoint) / 2;
            objectiveOffsetCentre.y = objectiveOffsetStartPoint.y;

            t.objectiveOriginData.objectiveWidth = (objectiveOffsetStartPoint.x - objectiveOffsetEndPoint.x);
            t.objectiveOriginData.objectiveLength = (objectiveOffsetStartPoint.z - objectiveOffsetEndPoint.z);

            t.objectiveData.objectiveMinWidthX =
                objectiveOffsetCentre.x - (t.objectiveOriginData.objectiveWidth / 2);

            t.objectiveData.objectiveMaxWidthX =
                objectiveOffsetCentre.x + (t.objectiveOriginData.objectiveWidth / 2);

            t.objectiveData.objectiveMinLengthZ =
                objectiveOffsetCentre.z - (t.objectiveOriginData.objectiveLength / 2);

            t.objectiveData.objectiveMaxLengthZ =
                objectiveOffsetCentre.z + (t.objectiveOriginData.objectiveLength / 2);
        }
    }
    

    private void FixedUpdate()
    {
        
    }
    
    private void CreateRandomValue(int objectiveIndex, int searchPositionArrayRow, int searchPositionArrayColumn)
    {
        var randomSearchPosition = new Vector3
        {
            x = Random.Range(_objectiveData[objectiveIndex].objectiveData.objectiveMinWidthX, _objectiveData[objectiveIndex].objectiveData.objectiveMaxWidthX),
            y = _objectiveData[objectiveIndex].objectiveOriginData.objectiveStartPoint.position.y,
            z = Random.Range(_objectiveData[objectiveIndex].objectiveData.objectiveMinLengthZ, _objectiveData[objectiveIndex].objectiveData.objectiveMaxLengthZ)
        };
        
        if (RandomPoint(randomSearchPosition, 1, out var point))
        {
            randomSearchPosition.y = point.y;
        }
        
        CheckRandomPoint(objectiveIndex, randomSearchPosition, searchPositionArrayRow, searchPositionArrayColumn);
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
    
    private void CheckRandomPoint(int objectiveIndex , Vector3 searchPosition, int searchPositionArrayRow, int searchPositionArrayColumn)
    {
        if (Physics.Raycast( searchPosition, transform.TransformDirection(Vector3.up), out var hit, Mathf.Infinity))
        { 
            Debug.DrawRay(searchPosition, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow,1); 
            
            CreateRandomValue(objectiveIndex, searchPositionArrayRow, searchPositionArrayColumn);
        }
        else
        { 
            Debug.DrawRay(searchPosition, transform.TransformDirection(Vector3.up) * 1000, Color.white, 1); 
            
            var evtExit = SearchAreaObjective.OnUpdateSearchPositionArray;
            evtExit.PositionArray[searchPositionArrayRow, searchPositionArrayColumn] = searchPosition;
            EventManager.Broadcast(evtExit);
            
        }
    }

    private void AllowSearchPosition(OnCreatePoint onCreatePoint)
    {
        CreateRandomValue(onCreatePoint.ObjectiveIndex, onCreatePoint.SearchPositionArrayRow, onCreatePoint.SearchPositionArrayColumn);
        
    }
    
    private void OnDrawGizmos()
    {
        if (_objectiveData == null) return;
        foreach (var t in _objectiveData)
        {

            Gizmos.color = Color.magenta;

            Gizmos.DrawWireCube(
                new Vector3(t.objectiveData.objectiveMinWidthX,
                    t.objectiveOriginData.objectiveEndPoint.position.y, t.objectiveData.objectiveMinLengthZ),
                new Vector3(1, 1, 1));
            Gizmos.DrawWireCube(
                new Vector3(t.objectiveData.objectiveMaxWidthX,
                    t.objectiveOriginData.objectiveStartPoint.position.y, t.objectiveData.objectiveMaxLengthZ),
                new Vector3(1, 1, 1));


            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(new Vector3(
                    (t.objectiveData.objectiveMinWidthX + t.objectiveData.objectiveMaxWidthX) / 2,
                    t.objectiveOriginData.objectiveStartPoint.position.y,
                    (t.objectiveData.objectiveMinLengthZ + t.objectiveData.objectiveMaxLengthZ) / 2),

                new Vector3(t.objectiveOriginData.objectiveWidth, 1, t.objectiveOriginData.objectiveLength));
        }
    }

}


[Serializable]
internal class ObjectiveSlot
{
    [Space(5)]
    public string objectiveName; 
    [Space(5)]
    public ObjectiveOriginData objectiveOriginData; 
    [Space(5)]
    public ObjectiveOffsetData objectiveOffsetData; 
    [Space(5)]
    public ObjectiveData objectiveData;
}


[Serializable]
internal struct ObjectiveOriginData
{ 
    public Transform objectiveStartPoint;

    public Transform objectiveEndPoint;
    
    public float objectiveWidth; 
    
    public float objectiveLength;
    
}

[Serializable]
internal struct ObjectiveOffsetData
{ 
    public float offsetPointsByX;
    
    public float offsetPointsByZ;
    
}



[Serializable]
internal struct ObjectiveData
{
    public float objectiveMinWidthX;
    
    public float objectiveMaxWidthX;

    public float objectiveMinLengthZ; 
    
    public float objectiveMaxLengthZ;
}



