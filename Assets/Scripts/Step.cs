using UnityEngine;
using System.Collections;
using Stairs.Utils;
using System;
using Random = UnityEngine.Random;

namespace Stairs
{
    public class Step : MonoBehaviour, ITouchControllable
    {
        /// <summary>
        /// How many points should be awarded per step placed.
        /// </summary>
        [SerializeField, Range(1, 99)] private int PointsPerSwipe = 10;

        /// <summary>
        /// How far should the stairs be offset.
        /// </summary>
        [SerializeField, Range(1f, 30f)] private float OffsetDistance = 3f;

        /// <summary>
        /// How close to center until stair autosnaps
        /// </summary>
        [SerializeField, Range(0.1f, 5f)] private float SnapTolerance = 2.99f;

        /// <summary>
        /// Color for stationary step shader.
        /// </summary>
        [SerializeField] private Color Stationary = Color.black;

        /// <summary>
        /// Color for active interactable step shader.
        /// </summary>
        [SerializeField] private Color Active = Color.yellow;

        private GameObject _go = null;
        private Rigidbody _rb = null;
        private Renderer _renderer;
        private Vector3 _snapPosition;

        private AudioSource _audio;

        /// <summary>
        /// If the stair is interactable.
        /// </summary>
        public bool Interactable
        {
            get { return _interactable; }

            set
            {
                _interactable = value;
                _renderer.material.color = value ? Active : Stationary;
            }
        }
        private bool _interactable;

        /// <summary>
        /// Initialization at start.
        /// </summary>
        private void Awake()
        {
            InitializeStep();
        }

        /// <summary>
        /// Automatically snap into place.
        /// </summary>
        public void AutoSnap()
        {
            transform.position = _snapPosition;
            CheckForSnap();
        }

        /// <summary>
        /// Initialization.
        /// </summary>
        private void InitializeStep()
        {
            _audio = GetComponent<AudioSource>();
            Pool.Instance.GoToStepDictionary.Add(gameObject, this);
            _go = _go ?? gameObject;
            _rb = _rb ?? GetComponent<Rigidbody>();
            _renderer = _renderer ?? GetComponent<Renderer>();
        }

        /// <summary>
        /// Activates this step into a scene.
        /// </summary>
        /// <param name="alwaysSafe">Should the offset chance be ignored.</param>
        /// <param name="chanceToOffset">How often out of 1.0f is this step offset.</param>
        /// <returns></returns>
        public bool ActivateInScene(bool alwaysSafe, float chanceToOffset = 0.15f)
        {
            _snapPosition = transform.position;

            if (Random.Range(0f, 1f) < chanceToOffset && !alwaysSafe)
            {
                Interactable = true;
                transform.position += Vector3.right * OffsetDistance * (Random.value < 0.5f ? 1 : -1);
                return true;
            }
            else
            {
                Interactable = false;
                return false;
            }
        }

        /// <summary>
        /// Changes color to shader param.
        /// </summary>
        /// <param name="col"></param>
        private void ChangeColor(Color col)
        {
            _renderer.material.color = col;
        }

        /// <summary>
        /// Mark this object as stepped and return to pool.
        /// </summary>
        public void SteppedOn()
        {
            Pool.Instance.StairController.SetStep();
            Pool.Instance.ReturnObject(ref _go);
        }

        /// <summary>
        /// Not used.
        /// </summary>
        /// <param name="touch"></param>
        public void OnTouchCancel(Touch touch)
        {
            
        }

        /// <summary>
        /// Not used.
        /// </summary>
        /// <param name="touch"></param>
        public void OnTouchEnd(Touch touch)
        {
            
        }

        /// <summary>
        /// Handles dragging event on this step.
        /// </summary>
        /// <param name="touch">Information on the touch dragging on this stair.</param>
        public void OnTouchMove(Touch touch)
        {
            if (!Interactable || Pool.Instance.SceneControl.SaveDialogOpen) return;

            var howMuch = touch.deltaPosition.x;
            if (Mathf.Abs(howMuch) > OffsetDistance)
            {
                howMuch = (howMuch/Mathf.Abs(howMuch)) * OffsetDistance;
            }

            var vec = new Vector3(howMuch, 0, 0);
            _go.transform.Translate(vec * touch.deltaTime * CalculatePerspectiveToMovement());

            CheckForSnap();
        }

        /// <summary>
        /// Not used.
        /// </summary>
        /// <param name="touch"></param>
        public void OnTouchStay(Touch touch)
        {

        }

        /// <summary>
        /// Not used.
        /// </summary>
        /// <param name="touch"></param>
        public void OnTouchBegin(Touch touch)
        {
            
        }

        /// <summary>
        /// Calculates perspective's effect into dragging distance.
        /// </summary>
        /// <returns></returns>
        private float CalculatePerspectiveToMovement()
        {
            var vec = _go.transform.position;
            vec.x = Camera.main.transform.position.x;
            var dist = Vector3.Magnitude(Camera.main.transform.position - vec);
            return (dist * Mathf.PI / 2) / -Mathf.Tan(Camera.main.fieldOfView / 2);
        }

        /// <summary>
        /// Check for snap conditions and snap if required.
        /// </summary>
        private void CheckForSnap()
        {
            var dist = Vector3.Magnitude(transform.position - _snapPosition);
            if (dist < SnapTolerance)
            {
                PlaySlideInSound();
                Interactable = false;
                transform.position = _snapPosition;
                Pool.Instance.SceneControl.IncreasePlayerScore(10);
            }
        }

        /// <summary>
        /// Plays the sound associated with stair sliding in place.
        /// </summary>
        private void PlaySlideInSound()
        {
            var dir = transform.position.x - _snapPosition.x;
            _audio.panStereo = dir <= 0 ? 0.75f : -0.75f; 
            _audio.PlayOneShot(_audio.clip);
        }
    }
}