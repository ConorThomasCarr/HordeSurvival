using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectiveZone : MonoBehaviour
{
 
    [SerializeField]
    private List<ObjectiveSlot> _objectiveSlot;
    
    private void Awake()
    {
        EventManager.AddListener<OnCreatePoint>(AllowSearchPosition);
        
        var evtExit = SearchAreaObjective.OnUpdateSearchPositionArray;
        evtExit.PositionArray = new Vector3 [100, 100];
        EventManager.Broadcast(evtExit);
    }

    private void Start()
    {
        
    }

    public void AddRoomData(string slotName, float minX, float maxX, float minZ, float maxZ)
    {
        var objSlot = new ObjectiveSlot();
        var objDate = new ObjectiveData();
        
        objSlot.objectiveName = slotName;
        objDate.roomMinWidthX = minX;
        objDate.roomMaxWidthX = maxX;
        objDate.roomMinHeightZ = minZ;
        objDate.roomMaxHeightZ = maxZ;
        
        objSlot.objectiveData = objDate;
        _objectiveSlot.Add(objSlot);
    }

    private void FixedUpdate()
    {
        
    }
    
    private void CreateRandomValue(int objectiveIndex, int searchPositionArrayRow, int searchPositionArrayColumn)
    {
        var randomSearchPosition = new Vector3
        {
            x = Random.Range(_objectiveSlot[objectiveIndex].objectiveData.roomMinWidthX, _objectiveSlot[objectiveIndex].objectiveData.roomMaxWidthX),
            y = 1,
            z = Random.Range(_objectiveSlot[objectiveIndex].objectiveData.roomMinHeightZ, _objectiveSlot[objectiveIndex].objectiveData.roomMaxHeightZ)
        };

        CheckRandomPoint(objectiveIndex, randomSearchPosition, searchPositionArrayRow, searchPositionArrayColumn);
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
}


[System.Serializable]
internal class ObjectiveSlot
{
    public string objectiveName;
    public ObjectiveData objectiveData;
}



[System.Serializable]
internal struct ObjectiveData
{
    [Header("Room Random Point Variables")] [Space(10)] [Space(5)]
    public float roomMinWidthX;

    [Space(5)] public float roomMinHeightZ;

    [Space(5)] public float roomMaxWidthX;

    [Space(5)] public float roomMaxHeightZ;
}



