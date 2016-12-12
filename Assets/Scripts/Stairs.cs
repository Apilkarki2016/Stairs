using System;
using System.Collections.Generic;
using Stairs.Utils;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

namespace Stairs
{
    public class Stairs : MonoBehaviour
    {
        /// <summary>
        /// Prefab for a step.
        /// </summary>
        [SerializeField] private GameObject StairPrefab;

        /// <summary>
        /// How often should a step be offset.
        /// </summary>
        [SerializeField] private float StepOffsetChange = 0.22f;

        /// <summary>
        /// How many steps to initialize initially.
        /// </summary>
        [SerializeField] private int NumberOfSteps = 50;

        /// <summary>
        /// How many steps are safe for sure at the start.
        /// </summary>
        [SerializeField] private int SafeStepsAtStart = 4;

        /// <summary>
        /// How many steps worth of trail should the player leave.
        /// </summary>
        [SerializeField] public int PlayerTrailLenght = 6;

        /// <summary>
        /// After hos many consecutive safe steps should rng manipulation kick in.
        /// </summary>
        [SerializeField] public int SafeStepsBeforeRngManipulation = 5;

        /// <summary>
        /// Effect of rng manipulation.
        /// </summary>
        [SerializeField, Range(1f, 5f)] public float IncreasePerStepAfterLimit = 1.25f;

        /// <summary>
        /// Coin pickup prefab.
        /// </summary>
        [SerializeField] private GameObject CoinPrefab;

        /// <summary>
        /// Ratio of pickups spawning per step.
        /// </summary>
        [SerializeField, Range(0f, 1f)] private float PickupRarity = 0.10f;

        private readonly Queue<GameObject> _stairs = new Queue<GameObject>();

        private Vector3 _stepSize = Vector3.zero;
        private Vector3 _nextStep = Vector3.zero;

        private PlayerController _player;
        private int _stepsPlaced = 0;
        private int _consecutiveSafeSteps = 0;

        /// <summary>
        /// Initialization at the start.
        /// </summary>
        private void Awake()
        {
            Pool.Instance.AddToPool(StairPrefab, NumberOfSteps, transform);
            Pool.Instance.AddToPool(CoinPrefab, (int) (NumberOfSteps*PickupRarity), transform);
            _stepSize = StairPrefab.GetComponent<MeshRenderer>().bounds.size;
            _player = FindObjectOfType<PlayerController>();

            for (var i = 0; i < NumberOfSteps; i++)
            {
                SetStep();
            }     
        }

        /// <summary>
        /// Gets a step from the pool and sets its location and rotation to the top of the stairs.
        /// </summary>
        public void SetStep()
        {
            var go = Pool.Instance.GetObject(StairPrefab);
            go.transform.position = _nextStep;
            go.transform.rotation = Quaternion.identity;
            _stairs.Enqueue(go);

            _nextStep += new Vector3(0, _stepSize.y, _stepSize.z);
        }

        /// <summary>
        /// If there are stairs yet to give to player as waypoints, give them.
        /// </summary>
        private void Update()
        {
            while (_stairs.Count > 0)
            {
                var go = _stairs.Dequeue();
                var step = Pool.Instance.GoToStepDictionary[go] as Step;

                if (step == null) continue;
                _player.AddStep(go.transform.position, step);

                if (SafeStepsAtStart < _stepsPlaced && Random.Range(0f, 1f) < PickupRarity)
                {
                    var pu = Pool.Instance.GetObject(CoinPrefab);
                    (Pool.Instance.GoToPickupDictionary[pu]).PositionInScene(go.transform.position);
                }

                if (step.ActivateInScene(SafeStepsAtStart >= ++_stepsPlaced, StepOffsetChange*EnsureConstantAction()))
                {
                    _consecutiveSafeSteps = 0;
                }
                else
                {
                    _consecutiveSafeSteps++;
                }
            }
        }

        /// <summary>
        /// Stops coroutines on all steps. Might come handy.
        /// </summary>
        public void StopCoroutinesOnAllStairs()
        {
            var dictionary = Pool.Instance.GoToStepDictionary;
            foreach (var kvp in dictionary)
            {
                var step = kvp.Value as Step;
                if (step != null) step.StopAllCoroutines();
            }
        }

        /// <summary>
        /// Increases chances of stair spawning for consecutive safe steps.
        /// </summary>
        /// <returns>Increase to chance to spawn the stair.</returns>
        private float EnsureConstantAction()
        {
            return _consecutiveSafeSteps <= SafeStepsBeforeRngManipulation ? 1f : Mathf.Pow(IncreasePerStepAfterLimit, _consecutiveSafeSteps - SafeStepsBeforeRngManipulation);
        }
    }
}