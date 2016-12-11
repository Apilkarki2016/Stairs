using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Stairs.Utils;
using UnityEngine.Events;

namespace Stairs
{
    /// <summary>
    /// Player game object.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// Fires on player death.
        /// </summary>
        [SerializeField] private UnityEvent PlayerDeathEvent;

        /// <summary>
        /// Fires when player starts running.
        /// </summary>
        [SerializeField] private UnityEvent PlayerStartsRunningEvent;

        /// <summary>
        /// Fires each time player steps on a step.
        /// </summary>
        [SerializeField] private UnityEvent PlayerTakesStep;

        /// <summary>
        /// Fires when player is about to die.
        /// </summary>
        [SerializeField] private UnityEvent PlayerNeedsSave;

        /// <summary>
        /// Fires if player is saved by spending coins or watching ad.
        /// </summary>
        [SerializeField] private UnityEvent PlayerIsSaved;

        /// <summary>
        /// Reference to backdrop that catches touches.
        /// </summary>
        [SerializeField] private GameObject TouchControlBackdrop;

        /// <summary>
        /// Tag to be expected from steps.
        /// </summary>
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

        /// <summary>
        /// Enumeration of waypoint types.
        /// </summary>
        private enum WaypointType { Step = 0, Walk = 1 }

        /// <summary>
        /// Waypoints that guide the character.
        /// </summary>
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

        /// <summary>
        /// Velocity of the player.
        /// </summary>
        [SerializeField, Range(0.1f, 100.0f)] private float StepsPerSecond = 1.50f;

        /// <summary>
        /// Pace of acceleration.
        /// </summary>
        [SerializeField, Range(1, 100)] private int AccelerateAfterHowManySteps = 8;

        /// <summary>
        /// Amounth of acceleration.
        /// </summary>
        [SerializeField, Range(0f, 500f)] private float Acceleration = 120f;

        private readonly Queue<Waypoint> _playerPath = new Queue<Waypoint>();
        private readonly Queue<Waypoint> _playerTrail = new Queue<Waypoint>();
        private Rigidbody _rigidBody;
        private int _stepsTaken = 0;
        private AudioSource _stepSource;

        /// <summary>
        /// If the player should perish on missed step.
        /// </summary>
        public static bool DieOnMiss = true;

        /// <summary>
        /// Initialization at start.
        /// </summary>
        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _stepSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Adds a waypoint
        /// </summary>
        /// <param name="destination">Where should the character walk.</param>
        /// <param name="step">If this waypoint is a step, reference to it.</param>
        public void AddStep(Vector3 destination, Step step = null)
        {
            _playerPath.Enqueue(new Waypoint(destination, step));
        }

        /// <summary>
        /// Walk to next waypoint.
        /// </summary>
        /// <param name="wayPoint">Where to.</param>
        /// <param name="duration">How long should it take.</param>
        /// <returns>Not used.</returns>
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
                _playerTrail.Enqueue(wayPoint);
                PlayerTakesStep.Invoke();

                if (wayPoint.step.Interactable)
                {
                    DieOnMiss = true;
                    Pool.Instance.SceneControl.SaveDialogOpen = true;
                    PlayerNeedsSave.Invoke();
                    yield return new WaitWhile(() => Pool.Instance.SceneControl.SaveDialogOpen);
                     
                    if (DieOnMiss)
                    {
                        OnMissedStep();
                        yield break;
                    }
                    else
                    {
                        wayPoint.step.AutoSnap();
                        PlayerIsSaved.Invoke();
                    }
                }

                _stepSource.PlayOneShot(_stepSource.clip);
            }

            LookForNextStep();
        }

        /// <summary>
        /// Handles player missing a step and not getting saved.
        /// </summary>
        private void OnMissedStep()
        {
            // We must release the backdrop, as it's non-convex mesh and thus can't be
            // made non-kinematic!
            TouchControlBackdrop.transform.parent = null;

            _rigidBody.isKinematic = false;
            StopAllCoroutines();

            PlayerDeathEvent.Invoke();
        }

        /// <summary>
        /// Find next waypoint and start walking towards it.
        /// </summary>
        private void LookForNextStep()
        {
            while (_playerTrail.Count > Pool.Instance.StairController.PlayerTrailLenght)
            {
                _playerTrail.Dequeue().step.SteppedOn();
            }

            if (++_stepsTaken%AccelerateAfterHowManySteps == 0) StepsPerSecond += Acceleration/1000f;
            if (_playerPath.Count > 0) StartCoroutine(WalkToWaypoint(_playerPath.Dequeue(), 1/StepsPerSecond));
        }

        /// <summary>
        /// Starts the player movement.
        /// </summary>
        public void StartRunning()
        {
            if (IsRunning) return;

            PlayerStartsRunningEvent.Invoke();
            IsRunning = true;
            LookForNextStep();
        }
    }
}
