using System;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerControls.PointAndClickControls
{
    public class PlayerPointAndClickController : MonoBehaviour
    {
        public UnityAction<Vector2> moveAction;

        private void Awake()
        {
            moveAction += OnMoveAction;
        }

        private static void OnMoveAction(Vector2 position)
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeCameraMain
            var ray = Camera.main!.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                var evtOnMoveCharacter = AIEvents.OnMoveCharacter;
                evtOnMoveCharacter.Position = hit.point;
                EventManager.Broadcast(evtOnMoveCharacter);
            }
        }
    }
}


