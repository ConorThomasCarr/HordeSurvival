using UnityEngine;
using UnityEngine.Events;
using Weapon.BaseGun;

public abstract class BaseProjectile : MonoBehaviour
{
    public GameObject Owner { get; private set; }
    public Vector3 InitialPosition { get; private set; }
    public Vector3 InitialDirection { get; private set; }
    public UnityAction OnShoot { get; set; }
    
    public void Shoot(IGun gun, GameObject parent)
    {
        Owner = parent;
        InitialPosition = transform.position;
        InitialDirection = transform.forward;

        OnShoot?.Invoke();
    }
}
