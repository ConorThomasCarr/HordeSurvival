using System;
using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Weapon.BaseGun.BaseMagnum.Magnum
{
    public class Magnum : BaseMagnum
    {
        private float _nextFire = 0.0f;

        private float _nextReload = 0.0f;
        
        private bool _isAiming;   
        
        public override void InitializeEvents()
        {
            base.InitializeEvents();
        }

        public override void Shoot()
        {
            if (CanShoot())
            {
                var projectilePrefabOne = BulletObjectPool.Instance.GetProjectilesPooledObject();
                BulletObjectPool.Instance.RemoveProjectilesPooledObject(projectilePrefabOne);

                if (projectilePrefabOne != null)
                {
                    projectilePrefabOne.enabled = true;
                    projectilePrefabOne.gameObject.SetActive(true);

                    projectilePrefabOne.transform.position = WeaponConfig.GeneralConfig.MuzzleOne.position;

                    projectilePrefabOne.transform.rotation =
                        Quaternion.LookRotation(GetShotDirectionWithinSpread(WeaponConfig.GeneralConfig.MuzzleOne));
                }

                projectilePrefabOne.Shoot(this, WeaponConfig.GeneralConfig.Parent);

                _nextFire = Time.time + 0.5f;

                magazineSize -= 1;
            }
        }

        public override void Reload()
        {
            if (!CanReload() && isReloading)
            {
                int ammoRemaining = WeaponConfig.CoreConfig.magazineCapacity  - magazineSize ;
                
                maxAmmo -= ammoRemaining;
                magazineSize += ammoRemaining;
                _nextReload = Time.time + 0.5f;

                isReloading = false;
            }
            
            if (CanReload() && !isReloading)
            {
                isReloading = true;
            }

           
        }

        public override void HasAim()
        {
            WeaponSpread -= WeaponConfig.CoreConfig.spreadSpeed * Time.deltaTime;
            AimAngle -=  WeaponConfig.CoreConfig.aimSpeed * Time.deltaTime;

            AimAngle = Mathf.Clamp(AimAngle, WeaponConfig.CoreConfig.WeaponRangeMinAngle,
                WeaponConfig.CoreConfig.WeaponRangeMaxAngle);
            WeaponSpread = Mathf.Clamp(WeaponSpread, WeaponConfig.CoreConfig.WeaponSpreadMinAngle,
                WeaponConfig.CoreConfig.WeaponSpreadMaxAngle);

            _isAiming = true;
        }


        public override void HasNoAim()
        {
            WeaponSpread += WeaponConfig.CoreConfig.spreadSpeed * Time.deltaTime;
            AimAngle +=  WeaponConfig.CoreConfig.aimSpeed * Time.deltaTime;

            AimAngle = Mathf.Clamp(AimAngle, WeaponConfig.CoreConfig.WeaponRangeMinAngle,
                WeaponConfig.CoreConfig.WeaponRangeMaxAngle);
            WeaponSpread = Mathf.Clamp(WeaponSpread, WeaponConfig.CoreConfig.WeaponSpreadMinAngle,
                WeaponConfig.CoreConfig.WeaponSpreadMaxAngle);

            _isAiming = false;
        }

        public override bool CanAim()
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return WeaponSpread == WeaponConfig.CoreConfig.WeaponSpreadMaxAngle && _isAiming == false;
        }

        public override bool CanShoot()
        {
            return (Time.time > _nextFire && _isAiming && magazineSize > 0 && maxAmmo > 0);
        }

        
        public override bool CanReload()
        {
            return (Time.time > _nextReload && !isReloading && magazineSize <= WeaponConfig.CoreConfig.magazineCapacity && maxAmmo <= WeaponConfig.CoreConfig.maxAmmoCapacity && maxAmmo != 0);
        }
        
        private Vector3 GetShotDirectionWithinSpread(Transform shootTransform)
        {
            var deltaAngleRatio = WeaponSpread / 180f;

            var spreadWorldDirection = Vector3.Slerp
                (shootTransform.forward, UnityEngine.Random.insideUnitCircle, deltaAngleRatio);

            return spreadWorldDirection;
        }
        
        public override Mesh AimMesh(Transform transform)
        {
            var mesh = new Mesh
            {
                name = "Aim Mesh"
            };

            var numTriangles = (128 * 10) + 2 + 2;
            var numVertices = numTriangles * 3;

            var vertices = new Vector3[numVertices];
            var triangles = new int[numVertices];

            vertices[0] = Vector3.zero;

            var currentAngle = -((AimAngle / 2) * Mathf.Deg2Rad);
            var deltaAngle = (((AimAngle / 2) * Mathf.Deg2Rad) * 2) / 128;

            for (int i = 0; i < 128; i++)
            {
                var sine = Mathf.Sin(currentAngle);
                var cosine = Mathf.Cos(currentAngle);
                
                var directionality = (transform.forward * cosine) + (transform.right * sine);
                var forward = (Vector3.forward * cosine) + (Vector3.right * sine);

                if (Physics.Raycast(transform.position, directionality, out var hit, 50, LayerMask.GetMask("Wall")))
                {
                    vertices[i + 1] = forward * hit.distance;
                }
                else
                {
                    vertices[i + 1] = forward * 50;
                }

                currentAngle += deltaAngle;
            }

            for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
            {
                triangles[i] = 0;
                triangles[i + 1] = j + 1;
                triangles[i + 2] = j + 2;
            }
            
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}



