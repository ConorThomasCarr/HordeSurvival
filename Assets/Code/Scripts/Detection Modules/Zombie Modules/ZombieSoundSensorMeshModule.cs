using System;
using System.Collections.Generic;

// USING NAME SPACE GOES HERE !!
using ISensor.ISensorMesh;
using ISensor.MeshData;

// USING UNITY GOES HERE !!
using UnityEngine;

public class ZombieSoundSensorMeshModule : MonoBehaviour, ISensorMeshModule
{
    public Mesh mesh { get; set; }
    public MeshDataStruct MeshData { get; set; }

    public Mesh CreateWedgeMesh()
    {
        var sensorMesh = new Mesh();

        var numTriangles = (MeshData.meshSegments * 10) + 2 + 2;
        var numVertices = numTriangles * 3;

        var vertices = new Vector3[numVertices];
        var triangles = new int[numVertices];

        var buttonCenter = Vector3.zero;
        var buttonLeft = Quaternion.Euler(0, -MeshData.meshAngle, 0) * Vector3.forward * MeshData.meshDistance;
        var buttonRight = Quaternion.Euler(0, MeshData.meshAngle, 0) * Vector3.forward * MeshData.meshDistance;

        var topCenter = buttonCenter + transform.up * MeshData.meshHeight;
        var topLeft = buttonLeft + transform.up * MeshData.meshHeight;
        var topRight = buttonRight + transform.up * MeshData.meshHeight;

        var middleCenter = buttonCenter + transform.up * (MeshData.meshHeight / 2);
        var middleLeft = buttonLeft + transform.up * (MeshData.meshHeight / 2);
        var middleRight = buttonRight + transform.up * (MeshData.meshHeight / 2);

        var vert = 0;

        // Left
        vertices[vert++] = buttonCenter;
        vertices[vert++] = buttonLeft;
        vertices[vert++] = middleLeft;

        vertices[vert++] = middleLeft;
        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;

        vertices[vert++] = topCenter;
        vertices[vert++] = middleCenter;
        vertices[vert++] = middleLeft;

        vertices[vert++] = middleLeft;
        vertices[vert++] = middleCenter;
        vertices[vert++] = buttonCenter;

        // Right
        vertices[vert++] = buttonCenter;
        vertices[vert++] = middleCenter;
        vertices[vert++] = middleRight;

        vertices[vert++] = middleRight;
        vertices[vert++] = middleCenter;
        vertices[vert++] = topCenter;

        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;
        vertices[vert++] = middleRight;

        vertices[vert++] = middleRight;
        vertices[vert++] = buttonRight;
        vertices[vert++] = buttonCenter;

        var currentAngle = -MeshData.meshAngle;
        var deltaAngle = (MeshData.meshAngle * 2) / MeshData.meshSegments;


        for (var i = 0; i < MeshData.meshSegments; ++i)
        {
            buttonLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * MeshData.meshDistance;
            buttonRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * MeshData.meshDistance;

            topLeft = buttonLeft + transform.up * MeshData.meshHeight;
            topRight = buttonRight + transform.up * MeshData.meshHeight;

            middleLeft = buttonLeft + transform.up * (MeshData.meshHeight / 2);
            middleRight = buttonRight + transform.up * (MeshData.meshHeight / 2);

            // Far Side
            vertices[vert++] = buttonLeft;
            vertices[vert++] = buttonRight;
            vertices[vert++] = middleRight;

            vertices[vert++] = middleRight;
            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;

            vertices[vert++] = topLeft;
            vertices[vert++] = middleLeft;
            vertices[vert++] = middleRight;

            vertices[vert++] = middleRight;
            vertices[vert++] = middleLeft;
            vertices[vert++] = buttonLeft;


            //Top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            // Button
            vertices[vert++] = buttonCenter;
            vertices[vert++] = buttonRight;
            vertices[vert++] = buttonLeft;


            currentAngle += deltaAngle;

        }

        for (var i = 0; i < numVertices; ++i)
        {
            triangles[i] = i;
        }

        sensorMesh.vertices = vertices;
        sensorMesh.triangles = triangles;
        sensorMesh.RecalculateNormals();

        return sensorMesh;
    }

    public void InitializeMeshModule(MeshDataStruct meshData)
    {
        MeshData = meshData;
        mesh = CreateWedgeMesh();
    }
    
    public void Enable()
    {
        //Debug.Log("Sound Sensor Mesh Module Enable: " + gameObject.name);
        
        enabled = true;
    }

    public void Disable()
    {
        //Debug.Log("Sound Sensor Mesh Module Disable: " + gameObject.name);
        
        enabled = false;
    }
}
