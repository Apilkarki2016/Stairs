using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stairs
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private GameObject PlayerObject;
        [SerializeField, Range(0.1f, 3f)] private float AngleTransitionTime = 0.64f;
        [SerializeField] private AnimationCurve AngleInterpolation;
        private float ZoomDistance;


        public void TransitionToDeathFollowMode()
        {
            var location = PlayerObject.transform.position;
            location.y = transform.position.y;
            transform.parent = null;

            ZoomDistance = Vector3.Magnitude(transform.position - PlayerObject.transform.position);
            StartCoroutine(DeathTransition(location));
        }

        private IEnumerator DeathTransition(Vector3 targetLocation)
        {
            var startLocation = transform.position;
            var timer = 0f;
            while (timer < AngleTransitionTime)
            {
                timer += Time.deltaTime;
                transform.LookAt(PlayerObject.transform);
                transform.position = Vector3.down*timer*ZoomDistance + Vector3.Lerp(startLocation, targetLocation, AngleInterpolation.Evaluate(timer/AngleTransitionTime));

                
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
