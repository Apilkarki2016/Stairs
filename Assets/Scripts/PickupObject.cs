using System.Collections;
using System.Collections.Generic;
using Stairs.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Stairs
{
    /// <summary>
    /// Enumaration of different types of pickup objects.
    /// </summary>
    public enum PickupType { Coin, Gem }

    /// <summary>
    /// Class for pickup objects.
    /// </summary>
    public sealed class PickupObject : MonoBehaviour
    {
        /// <summary>
        /// How high should this object be hovering.
        /// </summary>
        [SerializeField] private float SpawnHeigh;

        /// <summary>
        /// How fast in each direction should this pickup be rotating.
        /// </summary>
        [SerializeField] private Vector3 RotationSpeed;

        /// <summary>
        /// Actors of this tag can pick this object.
        /// </summary>
        [SerializeField] private string LimitPickupByTag = "Player";

        /// <summary>
        /// Type of this pickup.
        /// </summary>
        [SerializeField] private PickupType MyPickupType = PickupType.Coin;

        /// <summary>
        /// Calls initialization at start.
        /// </summary>
        private void Awake()
        {
            InitializePickup();
        }

        /// <summary>
        /// Rotates this object around.
        /// </summary>
        private void Update()
        {
            transform.Rotate(RotationSpeed * Time.deltaTime);    
        }

        /// <summary>
        /// Initializes this pickup.
        /// </summary>
        private void InitializePickup()
        {
            Pool.Instance.GoToPickupDictionary.Add(gameObject, this);
        }

        /// <summary>
        /// Reacts to collisions. Gets picked up by actors mathcing LimitPickupByTag tag.
        /// </summary>
        /// <param name="other">Collision data from Unity.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(LimitPickupByTag))
            {
                Pool.Instance.SceneControl.CollectCoin();
                ReturnToPool();
            }
        }

        /// <summary>
        /// Positions the pickup in scene.
        /// </summary>
        /// <param name="whereTo">Location of the ground on which to spawn this.</param>
        public void PositionInScene(Vector3 whereTo)
        {
            transform.position = whereTo + Vector3.up * SpawnHeigh;
        }

        /// <summary>
        /// Returns GO of this to pool.
        /// </summary>
        private void ReturnToPool()
        {
            var go = gameObject;
            Pool.Instance.ReturnObject(ref go);
        }
    }
}
