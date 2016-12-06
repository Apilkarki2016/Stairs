using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Stairs
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private UnityEvent PlayerDeathEvent;
        [SerializeField] private UnityEvent PlayerStartsRunningEvent;
        [SerializeField] private UnityEvent PlayerTakesStep;
        [SerializeField] private GameObject TouchControlBackdrop;

        public const string StepTag = "Step";

        /// <summary>
        /// If the character is currently running his path.
        /// </summary>
        /// <remarks>
        /// In c# bool is non-nullable type and has default value of false.
        /// We must trust this because of several reasons that can't be helped:
        /// 1. Unity forces us to use c# 4, autoproperty initializers are c# 6 feature.
        /// 2. Unity gameObjects must inherit from MonoBehavior and custom constructor is
        /// not supported.
        /// 3. private void Awake() is called only at the first frame after this object is
        /// loaded into a scene. We really should not trust it.
        /// 4. tldr: Because Unity is shit.
        /// </remarks>
        public bool IsRunning { private set; get; }

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
        private Rigidbody _rigidBody;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        public void AddStep(Vector3 destination, Step step = null)
        {
            _playerPath.Enqueue(new Waypoint(destination, step));
        }

        private IEnumerator WalkToWaypoint(Waypoint wayPoint, float duration)
        {
            var start = transform.position;
            var timer = 0f;

            while (timer <= duration)
            {
                timer += Time.deltaTime;
                var ratio = Mathf.Min(1f, timer/duration);

                transform.position = Vector3.Lerp(start, wayPoint.position, ratio);
                yield return new WaitForEndOfFrame();
            }

            if (wayPoint.step != null)
            {
                wayPoint.step.SteppedOn();
                PlayerTakesStep.Invoke();
                if (wayPoint.step.Interactable)
                {
                    OnMissedStep();
                    yield break;
                }
            }

            LookForNextStep();
        }

        private void OnMissedStep()
        {
            Debug.Log("Player missed a step and lost.");

            // We must release the backdrop, as it's non-convex mesh and thus can't be
            // made non-kinematic!
            TouchControlBackdrop.transform.parent = null;

            _rigidBody.isKinematic = false;
            StopAllCoroutines();

            PlayerDeathEvent.Invoke();
        }

        private void LookForNextStep()
        {
            if (_playerPath.Count > 0) StartCoroutine(WalkToWaypoint(_playerPath.Dequeue(), TimePerStep));
        }

        public void StartRunning()
        {
            if (IsRunning) return;

            PlayerStartsRunningEvent.Invoke();
            IsRunning = true;
            LookForNextStep();
        }
    }
}
