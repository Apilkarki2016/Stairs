﻿using System.Collections.Generic;
using Stairs.Utils;
using UnityEngine;
using System.Collections;

namespace Stairs
{
    public class Stairs : MonoBehaviour
    {
        [SerializeField] private GameObject StairPrefab;
        [SerializeField] private float StepOffsetChange = 0.22f;
        [SerializeField] private int NumberOfSteps = 50;
        [SerializeField] private int SafeStepsAtStart = 4;

        private readonly Queue<GameObject> _stairs = new Queue<GameObject>();

        private Vector3 _stepSize = Vector3.zero;
        private Vector3 _nextStep = Vector3.zero;

        private PlayerController _player;
        private int _stepsPlaced = 0;

        private void Awake()
        {
            Pool.Instance.AddToPool(StairPrefab, NumberOfSteps, transform);
            _stepSize = StairPrefab.GetComponent<MeshRenderer>().bounds.size;
            _player = FindObjectOfType<PlayerController>();

            for (var i = 0; i < NumberOfSteps; i++)
            {
                SetStep();
            }     
        }

        public void SetStep()
        {
            var go = Pool.Instance.GetObject(StairPrefab);
            go.transform.position = _nextStep;
            go.transform.rotation = Quaternion.identity;

            _stairs.Enqueue(go);

            _nextStep += new Vector3(0, _stepSize.y, _stepSize.z);
        }

        private void Update()
        {
            while (_stairs.Count > 0)
            {
                var go = _stairs.Dequeue();
                var step = Pool.Instance.GoToStepDictionary[go] as Step;

                if (step == null) continue;
                _player.AddStep(go.transform.position, step);
                step.ActivateInScene(SafeStepsAtStart >= ++_stepsPlaced, StepOffsetChange);
            }
        }
    }
}