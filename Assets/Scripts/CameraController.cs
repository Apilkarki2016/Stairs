using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stairs
{
    /// <summary>
    /// Controller for player follow camera.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        /// <summary>
        /// Object the camera is following.
        /// </summary>
        [SerializeField] private GameObject PlayerObject;

        /// <summary>
        /// How long should the death fall camera chase take.
        /// </summary>
        [SerializeField, Range(0.1f, 3f)] private float AngleTransitionTime = 0.64f;

        /// <summary>
        /// Interpolation curve for the angle transition.
        /// </summary>
        [SerializeField] private AnimationCurve AngleInterpolation;

        private float _zoomDistance;

        /// <summary>
        /// Initializes the on death camera transition.
        /// </summary>
        public void TransitionToDeathFollowMode()
        {
            var location = PlayerObject.transform.position;
            location.y = transform.position.y;
            transform.parent = null;

            _zoomDistance = Vector3.Magnitude(transform.position - PlayerObject.transform.position);
            StartCoroutine(DeathTransition(location));
        }

        /// <summary>
        /// Performs the on death camera transition.
        /// </summary>
        /// <param name="targetLocation">Target position of the camera in the transition aniamtion.</param>
        /// <returns>Not used.</returns>
        private IEnumerator DeathTransition(Vector3 targetLocation)
        {
            var startLocation = transform.position;
            var timer = 0f;
            while (timer < AngleTransitionTime)
            {
                timer += Time.deltaTime;
                transform.LookAt(PlayerObject.transform);
                transform.position = Vector3.down*timer*_zoomDistance + Vector3.Lerp(startLocation, targetLocation, AngleInterpolation.Evaluate(timer/AngleTransitionTime));

                
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
