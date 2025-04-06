using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletObjectPool : MonoBehaviour
{
    public static BulletObjectPool Instance;
    
    [FormerlySerializedAs("noiseEmitters")] [SerializeField]
    private List<BaseProjectile> projectiles = new List<BaseProjectile>();
    
    [SerializeField]
    private int amountToPoolProjectiles = 20;
    
    [SerializeField]
    private BaseProjectile projectilesPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        for (int i = 0; i < amountToPoolProjectiles; i++)
        {  
            var projectilePrefab = Instantiate(projectilesPrefab);
              projectilePrefab.name = "projectile" + " " + i;
              projectilePrefab.enabled = false;
              projectilePrefab.gameObject.SetActive(false);
            projectiles.Add(projectilePrefab);
        }
    }

    public BaseProjectile GetProjectilesPooledObject()
    {
        for (int i = 0; i < projectiles.Count; i++)
        {
            if (!projectiles[i].enabled)
            {
                return projectiles[i];
            }
        }
        return null;
    }
    
    public void RemoveProjectilesPooledObject(BaseProjectile projectile)
    {
        if (projectile != null)
        {
            this.projectiles.Remove(projectile);
        }
    }

    public void AddProjectilesPooledObject(BaseProjectile projectile)
    {
        if (projectile != null)
        {
            projectile.enabled = false;
            projectile.gameObject.SetActive(false);
            this.projectiles.Add(projectile);
        }
    }
}

