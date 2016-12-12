using System.Collections;
using System.Collections.Generic;
using Stairs.Utils;
using UnityEngine;

namespace Stairs
{
    /// <summary>
    /// Controls touch input on 3d objects.
    /// </summary>
    public class TouchController : MonoBehaviour
    {
        private Dictionary<int, ITouchControllable> fingerIdToObject = new Dictionary<int, ITouchControllable>();

        /// <summary>
        /// Iterates active touches on the scene and passes their references onwards.
        /// </summary>
        private void Update()
        {
            for (var i = 0; i < Input.touchCount; i++)
            {
                TouchToGameObject(i);
            }
        }

        /// <summary>
        /// Handles a touch event.
        /// </summary>
        /// <param name="touchIndex">Index of touch to handle.</param>
        private void TouchToGameObject(int touchIndex)
        {
            var ray = Camera.main.ScreenPointToRay(Input.GetTouch(touchIndex).position);
            var touch = Input.touches[touchIndex];
            RaycastHit hit;
            
            if (!Physics.Raycast(ray, out hit)) return;
            var step = Pool.Instance.GoToStepDictionary[hit.collider.gameObject] as ITouchControllable;
            if (step == null) return;

            if (touch.phase == TouchPhase.Began && fingerIdToObject.ContainsKey(touch.fingerId))
            {
                fingerIdToObject.Remove(touch.fingerId);
            }

            if (!fingerIdToObject.ContainsKey(touch.fingerId)) fingerIdToObject.Add(touch.fingerId, step);

            step = fingerIdToObject[touch.fingerId];

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    step.OnTouchBegin(touch);
                    break;
                case TouchPhase.Canceled:
                    step.OnTouchCancel(touch);
                    break;
                case TouchPhase.Ended:
                    step.OnTouchEnd(touch);
                    break;
                case TouchPhase.Moved:
                    step.OnTouchMove(touch);
                    break;
                case TouchPhase.Stationary:
                    step.OnTouchStay(touch);
                    break;
                default:
                    break;
            }
        }
    }
}
