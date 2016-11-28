using System.Collections.Generic;
using Stairs.Utils;
using UnityEngine;
using System.Collections;

namespace Stairs
{
    public class Stairs : MonoBehaviour
    {
        [SerializeField] private GameObject StairPrefab;
        [SerializeField] private int NumberOfSteps = 50;

        private Vector3 _stepSize = Vector3.zero;
        private Vector3 _nextStep = Vector3.zero;

        private PlayerController _player;

        private void Awake()
        {
            Pool.Instance.AddToPool(StairPrefab, 25, transform);
            _stepSize = StairPrefab.GetComponent<MeshRenderer>().bounds.size;
            _player = FindObjectOfType<PlayerController>();

            for (int i = 0; i < NumberOfSteps; i++)
            {
                SetStep(i);
            }            
        }

        private void SetStep(int number)
        {
            var go = Pool.Instance.GetObject(StairPrefab);
            go.transform.position = _nextStep;

            StartCoroutine(AddToPlayer(go));

            _nextStep += new Vector3(0, _stepSize.y, _stepSize.z);
        }

        /// <summary>
        /// Must wait until end of a frame to have Step::OnEnable run.
        /// </summary>
        /// <returns>Yields until next frame.</returns>
        private IEnumerator AddToPlayer(GameObject go)
        {
            yield return new WaitForEndOfFrame();
            if (_player != null) _player.AddStep(_nextStep + new Vector3(0, _stepSize.y, 0), Pool.Instance.GoToStepDictionary[go]);
        }
    }
}
