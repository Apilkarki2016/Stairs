using System.Collections.Generic;
using Stairs.Utils;
using UnityEngine;

namespace Stairs
{
    public class Stairs : MonoBehaviour
    {
        [SerializeField] private GameObject StairPrefab;

        private Vector3 _stepSize = Vector3.zero;
        private Vector3 _nextStep = Vector3.zero;

        private PlayerController _player;

        private void Awake()
        {
            Pool.Instance.AddToPool(StairPrefab, 25, transform);
            _stepSize = StairPrefab.GetComponent<MeshRenderer>().bounds.size;
            _player = FindObjectOfType<PlayerController>();

            for (int i = 0; i < 50; i++)
            {
                SetStep(i);
            }
        }

        private void SetStep(int number)
        {
            var go = Pool.Instance.GetObject(StairPrefab);
            go.transform.position = _nextStep;
            if (_player != null) _player.AddStep(_nextStep + new Vector3(0, _stepSize.y, 0));
            _nextStep += new Vector3(0, _stepSize.y, _stepSize.z);
        }
    }
}
