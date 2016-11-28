using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Stairs
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Vector3 speed;

        private Rigidbody _rb = null;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            transform.Translate(speed * Time.deltaTime);
        }
    }
}
