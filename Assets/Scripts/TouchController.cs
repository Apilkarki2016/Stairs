using System.Collections;
using System.Collections.Generic;
using Stairs.Utils;
using UnityEngine;

namespace Stairs
{
    public class TouchController : MonoBehaviour
    {
        void Update()
        {
            for (var i = 0; i < Input.touchCount; i++)
            {
                TouchToGameObject(i);
            }
        }

        private void TouchToGameObject(int touchIndex)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(touchIndex).position);
            var touch = Input.touches[touchIndex];
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit))
            {
                var step = Pool.Instance.GoToStepDictionary[hit.collider.gameObject];
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        step.OnTouchBegin();
                        break;
                    case TouchPhase.Canceled:
                        step.OnTouchCancel();
                        break;
                    case TouchPhase.Ended:
                        step.OnTouchEnd();
                        break;
                    case TouchPhase.Moved:
                        step.OnTouchMove(touch.deltaPosition, touch.deltaTime);
                        break;
                    case TouchPhase.Stationary:
                        step.OnTouchStay();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
