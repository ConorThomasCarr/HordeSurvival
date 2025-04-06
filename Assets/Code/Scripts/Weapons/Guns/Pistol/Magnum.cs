using System;
using UnityEngine;
using UnityEngine.Events;

namespace Weapon.BaseGun.BaseMagnum.Magnum
{
    public class Magnum : BaseMagnum
    {
        private float _nextFire = 0.0f;

        private bool _isAiming;

        private int _ammoUsed = 0;

        public override void InitializeEvents()
        {
            base.InitializeEvents();
        }

        public override void Shoot()
        {
            if (CanShoot() && _ammoUsed < weaponConfig.CoreConfig.magazineCapacity)
            {
                var projectilePrefabOne = BulletObjectPool.Instance.GetProjectilesPooledObject();
                BulletObjectPool.Instance.RemoveProjectilesPooledObject(projectilePrefabOne);

                if (projectilePrefabOne != null)
                {
                    projectilePrefabOne.enabled = true;
                    projectilePrefabOne.gameObject.SetActive(true);

                    projectilePrefabOne.transform.position = weaponConfig.GeneralConfig.MuzzleOne.position;

                    projectilePrefabOne.transform.rotation =
                        Quaternion.LookRotation(GetShotDirectionWithinSpread(weaponConfig.GeneralConfig.MuzzleOne));
                }

                projectilePrefabOne.Shoot(this, weaponConfig.GeneralConfig.Parent);

                _nextFire = Time.time + 0.5f;
                _ammoUsed++;

            }
        }

        public override void Reload()
        {
            _ammoUsed = 0;
        }

        public override void HasAim()
        {
            WeaponSpread -= 5 * Time.deltaTime;
            AimAngle -= 10 * Time.deltaTime;

            AimAngle = Mathf.Clamp(AimAngle, weaponConfig.CoreConfig.WeaponRangeMinAngle,
                weaponConfig.CoreConfig.WeaponRangeMaxAngle);
            WeaponSpread = Mathf.Clamp(WeaponSpread, weaponConfig.CoreConfig.WeaponSpreadMinAngle,
                weaponConfig.CoreConfig.WeaponSpreadMaxAngle);

            _isAiming = true;
        }


        public override void HasNoAim()
        {
            WeaponSpread += 5 * Time.deltaTime;
            AimAngle += 10 * Time.deltaTime;

            AimAngle = Mathf.Clamp(AimAngle, weaponConfig.CoreConfig.WeaponRangeMinAngle,
                weaponConfig.CoreConfig.WeaponRangeMaxAngle);
            WeaponSpread = Mathf.Clamp(WeaponSpread, weaponConfig.CoreConfig.WeaponSpreadMinAngle,
                weaponConfig.CoreConfig.WeaponSpreadMaxAngle);

            _isAiming = false;
        }

        public override bool CanAim()
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return WeaponSpread == weaponConfig.CoreConfig.WeaponSpreadMaxAngle && _isAiming == false;
        }

        public override bool CanShoot()
        {
            return (Time.time > _nextFire && _isAiming);
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
            var mesh = new Mesh();

            var numTriangles = (32 * 10) + 2 + 2;
            var numVertices = numTriangles * 3;

            var vertices = new Vector3[numVertices];
            var triangles = new int[numVertices];

            vertices[0] = Vector3.zero;

            var currentAngle = -((AimAngle / 2) * Mathf.Deg2Rad);
            var deltaAngle = (((AimAngle / 2) * Mathf.Deg2Rad) * 2) / 32;

            for (int i = 0; i < 32; i++)
            {
                var sine = Mathf.Sin(currentAngle);
                var cosine = Mathf.Cos(currentAngle);

                var directionality = (transform.forward * cosine) + (transform.right * sine);
                var forward = (Vector3.forward * cosine) + (Vector3.right * sine);

                if (Physics.Raycast(new Vector3(transform.position.x, 2, transform.position.z), directionality,
                        out var hit, 500, LayerMask.GetMask("Default")))
                {
                    vertices[i + 1] = forward * hit.distance;
                }
                else
                {
                    vertices[i + 1] = forward * 500;
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

            return mesh;
        }
    }
}



