using System.Collections;
using Stairs.Utils;
using UnityEngine;

namespace Stairs
{
    public class Stairs : MonoBehaviour
    {
        [SerializeField] private GameObject StairPrefab;

        private Vector3 _stepSize = Vector3.zero;
        private Vector3 _nextStep = Vector3.zero;

        private void Awake()
        {
            Pool.Instance.AddToPool(StairPrefab, 25, transform);
            _stepSize = StairPrefab.GetComponent<MeshRenderer>().bounds.size;


            for (int i = 0; i < 50; i++)
            {
                SetStep(i);
            }
        }

        private void SetStep(int number)
        {
            var go = Pool.Instance.GetObject(StairPrefab);
            go.transform.position = _nextStep;
            _nextStep += new Vector3(0, _stepSize.y, _stepSize.z);
        }
    }
}
