using System.Collections;
using System.Collections.Generic;
using Stairs.Utils;
using UnityEngine;

namespace Stairs
{
    public class TouchBackground : MonoBehaviour, ITouchControllable
    {
        private void Awake()
        {
            Pool.Instance.GoToStepDictionary.Add(gameObject, this);
        }

        public void OnTouchCancel(Touch touch)
        {
            
        }

        public void OnTouchEnd(Touch touch)
        {
            
        }

        public void OnTouchMove(Touch touch)
        {
            
        }

        public void OnTouchStay(Touch touch)
        {
            
        }

        public void OnTouchBegin(Touch touch)
        {
            
        }
    }
}
