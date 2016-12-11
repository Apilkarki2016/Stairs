using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stairs
{
    /// <summary>
    /// Interface for objects controllable with finger.
    /// </summary>
    public interface ITouchControllable
    {
        void OnTouchCancel(Touch touch);

        void OnTouchEnd(Touch touch);

        void OnTouchMove(Touch touch);

        void OnTouchStay(Touch touch);

        void OnTouchBegin(Touch touch);
    }
}