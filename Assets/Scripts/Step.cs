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

        private void Awake()
        {
            Pool.Instance.GoToStepDictionary.Add(gameObject, this);
            _go = gameObject;
            _rb = GetComponent<Rigidbody>();
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
    }
}
