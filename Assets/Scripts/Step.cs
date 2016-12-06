using UnityEngine;
using System.Collections;
using Stairs.Utils;
using System;
using Random = UnityEngine.Random;

namespace Stairs
{
    public class Step : MonoBehaviour, ITouchControllable
    {
        [SerializeField] private float DeactivationDelay = 10.1f;
        [SerializeField, Range(1f, 30f)] private float OffsetDistance = 3f;
        [SerializeField, Range(0.1f, 5f)] private float SnapTolerance = 2f;

        [SerializeField] private Color Stationary = Color.black;
        [SerializeField] private Color Active = Color.yellow;

        private bool _deactivating = false;

        private GameObject _go = null;
        private Rigidbody _rb = null;
        private Renderer _renderer;
        private Vector3 _snapPosition;

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

        private void Awake()
        {
            InitializeStep();
        }

        private void InitializeStep()
        {
            Pool.Instance.GoToStepDictionary.Add(gameObject, this);
            _go = _go ?? gameObject;
            _rb = _rb ?? GetComponent<Rigidbody>();
            _renderer = _renderer ?? GetComponent<Renderer>();
        }

        public void ActivateInScene(bool alwaysSafe, float chanceToOffset = 0.15f)
        {
            _snapPosition = transform.position;

            if (Random.Range(0f, 1f) < chanceToOffset && !alwaysSafe)
            {
                Interactable = true;
                transform.position += Vector3.right * OffsetDistance * (Random.value < 0.5f ? 1 : -1);
            }
            else
            {
                Interactable = false;
            }
        }

        private void ChangeColor(Color col)
        {
            _renderer.material.color = col;
        }

        public void SteppedOn()
        {
            if (_deactivating) return;
            _deactivating = true;
            Pool.Instance.StairController.SetStep();

            StartCoroutine(ReturnToPool(DeactivationDelay));
        }

        private IEnumerator ReturnToPool(float duration)
        {
            yield return new WaitForSeconds(duration);

            _deactivating = false;
            Pool.Instance.ReturnObject(ref _go);
        }

        public void OnTouchCancel(Touch touch)
        {
            
        }

        public void OnTouchEnd(Touch touch)
        {
            
        }

        public void OnTouchMove(Touch touch)
        {
            if (!Interactable) return;

            var vec = new Vector3(touch.deltaPosition.x, 0, 0);
            _go.transform.Translate(vec * touch.deltaTime * CalculatePerspectiveToMovement());

            CheckForSnap();
        }

        public void OnTouchStay(Touch touch)
        {

        }

        public void OnTouchBegin(Touch touch)
        {
            
        }

        private float CalculatePerspectiveToMovement()
        {
            var vec = _go.transform.position;
            vec.x = Camera.main.transform.position.x;
            var dist = Vector3.Magnitude(Camera.main.transform.position - vec);
            return (dist * Mathf.PI / 2) / -Mathf.Tan(Camera.main.fieldOfView / 2);
        }

        private void CheckForSnap()
        {
            var dist = Vector3.Magnitude(transform.position - _snapPosition);
            if (dist < SnapTolerance)
            {
                Interactable = false;
                transform.position = _snapPosition;
            }
        }
    }
}