using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace Stairs
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        private enum WaypointType { Step = 0, Walk = 1 }

        private struct Waypoint
        {
            public Vector3 position;
            public WaypointType type;
            public Step step;

            public Waypoint(Vector3 position, Step step = null, WaypointType type = WaypointType.Step)
            {
                this.position = position;
                this.type = type;
                this.step = step;
            }
        }

        [SerializeField, Range(0.01f, 5.0f)] private float TimePerStep = 4.20f;
        
        private readonly Queue<Waypoint> _playerPath = new Queue<Waypoint>();
        private bool _takingStep = false;

        private void Awake()
        {
           
        }

        private void Update()
        {
            if (!_takingStep && _playerPath.Count > 0)
            {
                StartCoroutine(WalkToWaypoint(_playerPath.Dequeue(), TimePerStep));
            }
        }

        public void AddStep(Vector3 destination, Step step = null)
        {
            _playerPath.Enqueue(new Waypoint(destination, step));
        }

        private IEnumerator WalkToWaypoint(Waypoint wayPoint, float duration)
        {
            _takingStep = true;
            var start = transform.position;
            var timer = 0f;

            while (timer <= duration)
            {
                timer += Time.deltaTime;
                var ratio = Mathf.Min(1f, timer/duration);

                transform.position = Vector3.Lerp(start, wayPoint.position, ratio);
                yield return new WaitForEndOfFrame();
            }

            _takingStep = false;
            if (wayPoint.step != null) wayPoint.step.SteppedOn();
        }

    }
}
