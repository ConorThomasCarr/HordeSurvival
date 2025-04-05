using System;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerControls.CharacterControls 
{
    public class PlayerCharacterController : MonoBehaviour
    {
        public UnityAction<Vector2> rotateAction;

        private Transform _characterBody;

        private void Awake()
        {
            rotateAction += OnRotateAction;
            
            _characterBody = transform.GetChild(0).transform;
        }


        private void OnRotateAction(Vector2 position)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (groundPlane.Raycast(ray, out var rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                LookAt(point);
            }
        }
        
        private void LookAt(Vector3 lookAtPoint)
        {
            Vector3 heightCorrectedPoint = new Vector3(lookAtPoint.x, _characterBody.position.y, lookAtPoint.z);
            _characterBody.LookAt(heightCorrectedPoint);
        }
    }
}

