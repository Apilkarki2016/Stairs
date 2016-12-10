using System.Collections;
using System.Collections.Generic;
using Stairs.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Stairs
{
    public enum PickupType { Coin, Gem }

    public class PickupObject : MonoBehaviour
    {
        [SerializeField] private float SpawnHeigh;
        [SerializeField] private Vector3 RotationSpeed;
        [SerializeField] private string LimitPickupByTag = "Player";
        [SerializeField] private PickupType MyPickupType = PickupType.Coin;

        private void Awake()
        {
            InitializePickup();
        }

        private void Update()
        {
            transform.Rotate(RotationSpeed * Time.deltaTime);    
        }

        private void InitializePickup()
        {
            Pool.Instance.GoToPickupDictionary.Add(gameObject, this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(LimitPickupByTag))
            {
                Pool.Instance.SceneControl.CollectCoin();
                ReturnToPool();
            }
        }

        public void PositionInScene(Vector3 whereTo)
        {
            transform.position = whereTo + Vector3.up * SpawnHeigh;
        }

        private void ReturnToPool()
        {
            var go = gameObject;
            Pool.Instance.ReturnObject(ref go);
        }
    }
}
