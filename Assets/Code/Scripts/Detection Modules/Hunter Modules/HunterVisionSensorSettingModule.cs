using System;
using System.Collections.Generic;

// USING NAME SPACE GOES HERE !!
using ISensor;
using ISensor.AISSensorScanModule;
using ISensor.ISensorMesh;
using ISensor.MeshData;
using ISensor.SensorData;
using ISensor.SensorColourData;
using ISensor.SensorScanData;
using Unity.Mathematics;
using Unity.VisualScripting;

// USING UNITY GOES HERE !!
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;

public class HunterVisionSensorSettingModule : MonoBehaviour, ISensorSettingModule
{
    public ISensorModule SensorModule { get; set; }
        
    public ISensorScanModule SensorScanModule { get; set; }
    
    public ISensorMeshModule SensorMeshModule{ get; set; }

    public SensorDataStruct SensorData { get; set; }
        
    public SensorScanDataStruct SensorScanData { get; set; }
        
    public SensorColourDataStruct SensorColourData { get; set; }

    [SerializeField]
    private SensorDataStruct _sensorData;

    [SerializeField] 
    private SensorColourDataStruct _sensorColourData;

    [SerializeField]
    private SensorScanDataStruct _sensorScanData;

    public void Awake()
    {
        SensorData = new SensorDataStruct()
        {
            sensorAngle = _sensorData.sensorAngle,
            sensorRange = _sensorData.sensorRange,
            sensorHeight = _sensorData.sensorHeight,
            sensorSegments = _sensorData.sensorSegments,
        };

        SensorScanData = new SensorScanDataStruct()
        {
            scanFrequency = _sensorScanData.scanFrequency,
            detectionMask = _sensorScanData.detectionMask,
            obstructionMask = _sensorScanData.obstructionMask,
        };

        SensorColourData = new SensorColourDataStruct()
        {
            sensorMeshColor = _sensorColourData.sensorMeshColor,
            inRangeColor = _sensorColourData.inRangeColor,
            sensorDetectedColor = _sensorColourData.sensorDetectedColor,
            inMemoryColor = _sensorColourData.inMemoryColor,
            bestMemoryColor = _sensorColourData.bestMemoryColor,
        };

        var meshData = new MeshDataStruct()
        {
            meshAngle = SensorData.sensorAngle,
            meshDistance = SensorData.sensorRange,
            meshHeight = SensorData.sensorHeight,
            meshSegments = SensorData.sensorSegments,
        };


        SensorMeshModule = GetComponent<ISensorMeshModule>();
        SensorModule = GetComponent<ISensorModule>();
        SensorScanModule = GetComponent<ISensorScanModule>();

        SensorMeshModule.InitializeMeshModule(meshData);
        SensorModule.InitializeSensorModule(SensorData, SensorScanData, SensorColourData);
        SensorScanModule.InitializeScanSensorModule(SensorData, SensorScanData, SensorColourData);
    }
    
    public void Enable()
    {
        //Debug.Log("Vision Sensor Setting Module Enable: " + gameObject.name);

        SensorMeshModule.Enable();
        SensorModule.Enable();
        SensorScanModule.Enable();
        
        enabled = true;
    }

    public void Disable()
    {
        //Debug.Log("Vision Sensor Setting Module Disable: " + gameObject.name);
        
        SensorMeshModule.Disable();
        SensorModule.Disable();
        SensorScanModule.Disable();
        
        enabled = false;
    }

    public void OnDrawGizmos()
    {
        if (SensorMeshModule != null)
        {
            if ( SensorMeshModule.mesh)
            {
                Gizmos.color = SensorColourData.sensorMeshColor;
                Gizmos.DrawMesh(SensorMeshModule.mesh, transform.position, transform.rotation);
            }
        }
    }
}