using System;
using System.Collections.Generic;
using UnityEngine;


public class HouseObjectiveZone : MonoBehaviour
{ 
    [SerializeField]
    private ObjectiveZone _objectiveZone;

    [SerializeField]
     private List<RoomData> _roomDatas;
    

    private void Awake()
    {
        _objectiveZone = GetComponentInParent<ObjectiveZone>();
        AddRoomData();
    }

    private void Update()
    {
      
    }

    private void AddRoomData()
    {
        for (int i = 0; i < _roomDatas.Count; i++)
        {
            _roomDatas[i].roomOffsetStartPoint = new Vector3(
                _roomDatas[i].roomStartPoint.position.x - _roomDatas[i].offsetPointsByX,
                _roomDatas[i].roomStartPoint.position.y,
                _roomDatas[i].roomStartPoint.position.z - _roomDatas[i].offsetPointsByZ);

            _roomDatas[i].roomOffsetEndPoint = new Vector3(
                _roomDatas[i].roomEndPoint.position.x + _roomDatas[i].offsetPointsByX,
                _roomDatas[i].roomEndPoint.position.y,
                _roomDatas[i].roomEndPoint.position.z + _roomDatas[i].offsetPointsByZ);

            _roomDatas[i].roomOffsetCentre =
                (_roomDatas[i].roomOffsetStartPoint + _roomDatas[i].roomOffsetEndPoint) / 2;
            _roomDatas[i].roomOffsetCentre.y = _roomDatas[i].roomOffsetStartPoint.y;

            _roomDatas[i].roomWidth = (_roomDatas[i].roomOffsetStartPoint.x - _roomDatas[i].roomOffsetEndPoint.x);
            _roomDatas[i].roomHeight = (_roomDatas[i].roomOffsetStartPoint.z - _roomDatas[i].roomOffsetEndPoint.z);

            
            _objectiveZone.AddRoomData(_roomDatas[i].roomName,
                _roomDatas[i].roomOffsetCentre.x - (_roomDatas[i].roomWidth / 2),
                _roomDatas[i].roomOffsetCentre.x + (_roomDatas[i].roomWidth / 2),
                _roomDatas[i].roomOffsetCentre.z - (_roomDatas[i].roomHeight / 2),
                _roomDatas[i].roomOffsetCentre.z + (_roomDatas[i].roomHeight / 2));
        }
    }


  private void OnDrawGizmos()
  {
      if (_roomDatas == null) return;
      foreach (var t in _roomDatas)
      {
          Gizmos.color = Color.green;
          Gizmos.DrawWireCube(t.roomOffsetStartPoint, new Vector3(1, 1, 1));
          Gizmos.DrawWireCube(t.roomOffsetEndPoint, new Vector3(1, 1, 1));

          Gizmos.color = Color.blue;
          Gizmos.DrawWireCube(t.roomOffsetCentre,
              new Vector3(t.roomWidth, 1, t.roomHeight));
      }
  }
}


[Serializable]
internal class RoomData
{
    public string roomName;
    
    public Transform roomStartPoint;

    public Transform roomEndPoint;
    
    public Vector3 roomOffsetStartPoint;
   
    public Vector3 roomOffsetEndPoint;
    
    public Vector3 roomOffsetCentre;
    
    public float roomWidth;
   
    public float roomHeight;
    
    public float offsetPointsByX;
    
    public float offsetPointsByZ;
    
}

