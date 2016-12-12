using System.Collections;
using System.Collections.Generic;
using Stairs.Utils;
using UnityEngine;

namespace Stairs
{
    /// <summary>
    /// Touch backdrop to catch touches that exit the steps momentarily.
    /// </summary>
    public class TouchBackground : MonoBehaviour, ITouchControllable
    {
        /// <summary>
        /// Initialize at start.
        /// </summary>
        private void Awake()
        {
            Pool.Instance.GoToStepDictionary.Add(gameObject, this);
        }

        /// <summary>
        /// Not used.
        /// </summary>
        /// <param name="touch"></param>
        public void OnTouchCancel(Touch touch)
        {
            
        }

        // <summary>
        /// Not used.
        /// </summary>
        /// <param name="touch"></param>
        public void OnTouchEnd(Touch touch)
        {
            
        }

        // <summary>
        /// Not used.
        /// </summary>
        /// <param name="touch"></param>
        public void OnTouchMove(Touch touch)
        {
            
        }

        // <summary>
        /// Not used.
        /// </summary>
        /// <param name="touch"></param>
        public void OnTouchStay(Touch touch)
        {
            
        }

        // <summary>
        /// Not used.
        /// </summary>
        /// <param name="touch"></param>
        public void OnTouchBegin(Touch touch)
        {
            
        }
    }
}
