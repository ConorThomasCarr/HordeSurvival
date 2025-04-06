using System;
using UnityEngine;


public class Projectile : BaseProjectile
{
    public float Radius = 0.01f;
   
    public Transform Root;
    
    public float MaxLifeTime = 5f;
    
    public float Speed = 20f;
    
    public float TrajectoryCorrectionDistance = -1;
    
    public bool InheritWeaponVelocity = false;

    BaseProjectile m_ProjectileBase;
   
    Vector3 m_LastRootPosition;
   
    Vector3 m_Velocity;

    private bool m_HasTrajectoryOverride = true;
  
    float m_ShootTime;
   
    Vector3 m_TrajectoryCorrectionVector;
  
    Vector3 m_ConsumedTrajectoryCorrectionVector;

    private Vector3 start;
    

    void OnEnable()
    {
        m_ProjectileBase = GetComponent<BaseProjectile>();

        m_ProjectileBase.OnShoot += OnShoot;

        MaxLifeTime = 5;
       
    }

    new void OnShoot()
    {
        m_ShootTime = Time.time;
        m_LastRootPosition = Root.position;
        m_Velocity = transform.forward * Speed;
        start = Root.position;
    }

    void Update()
    {
        // Move
        transform.position += m_Velocity * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, start.y, transform.position.z);
        
        transform.forward = m_Velocity.normalized;
        
        m_LastRootPosition = Root.position;

        MaxLifeTime -= 1 * Time.deltaTime;
        
        if (MaxLifeTime < 0)
        {
            BulletObjectPool.Instance.AddProjectilesPooledObject(this);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(start, transform.position);
    }

    
    
}