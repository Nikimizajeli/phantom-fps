using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private Spotter spotter;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float chasingRange = 5f;

    private bool _playerDetected;
    private float _distanceToTarget = Mathf.Infinity;
    private Transform _target;
    
    protected void Start()
    {
        
    }

    protected void OnEnable()
    {
        spotter.TargetFound += OnTargetFound;
    }

    protected void OnDisable()
    {
        spotter.TargetFound -= OnTargetFound;
    }

    protected void Update()
    {
        if (_target == null)
        {
            return;
        }
        _distanceToTarget = Vector3.Distance(_target.position, transform.position);
        if (_distanceToTarget < chasingRange)
        {
            navMeshAgent.SetDestination(_target.position);
        }
    }

    private void OnTargetFound(GameObject target)
    {
        _target = target.GetComponent<Transform>();
    }
}
