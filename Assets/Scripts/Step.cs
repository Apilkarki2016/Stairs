using UnityEngine;
using System.Collections;
using Stairs.Utils;

namespace Stairs
{
    public class Step : MonoBehaviour
    {
        [SerializeField] private float DeactivationDelay = 2.1f;
        private bool _deactivating = false;

        private GameObject _go = null;
        private Rigidbody _rb = null;

        private void Start()
        {
            Debug.Log("Step::Awake()");

            Pool.Instance.GoToStepDictionary.Add(gameObject, this);
            _go = gameObject;
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _rb.isKinematic = true;
        }

        public void SteppedOn()
        {
            if (_deactivating) return;
            _deactivating = true;

            StartCoroutine(ReturnToPool(DeactivationDelay));
        }

        private IEnumerator ReturnToPool(float duration)
        {
            _rb.isKinematic = false;

            yield return new WaitForSeconds(duration);

            _deactivating = false;
            Pool.Instance.ReturnObject(ref _go);
        }
    }
}
