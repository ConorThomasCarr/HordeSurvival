using System;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
public class ShowObjectObstacleBounds : MonoBehaviour
{
    void OnDrawGizmos()
    {
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        
        if (obstacle != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 obstacleSize = new Vector3(obstacle.size.x * transform.localScale.x, obstacle.size.y * transform.localScale.y, obstacle.size.z * transform.localScale.z);
            Gizmos.DrawCube(transform.GetComponent<Renderer>().bounds.center, obstacleSize);
        }
    }
}
