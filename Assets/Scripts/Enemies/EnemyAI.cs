using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    public float maxWaitTime = 4.0f;
    public float minWaitTime = 0.5f;
    public float maxWalkDistance = 3.0f;
    public float minWalkDistance = 0.5f;
    public LayerMask whatIsGround;
    public bool isPatrolling = true;

    private bool _walkPointSet;
    private bool _isMoving;
    private NavMeshAgent _agent;
    private float _cooldown;
    private Vector3 _walkPoint;
    private float _timer;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        GetNextDestination();
        GenerateCooldown();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPatrolling) return;
        Patrolling();
    }

    void Patrolling()
    {
        if (!_walkPointSet)
        {
            //wait for a certain time and then get next destination
            if (_timer >= _cooldown)
            {
                _timer = 0.0f;
                GetNextDestination();
            }
            _timer += Time.deltaTime;
            return;
        }

        if(_walkPointSet)
            _agent.SetDestination(_walkPoint);

        Vector3 distanceToWalkPoint = transform.position - _walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            _walkPointSet = false;
            GenerateCooldown();
        }
    }

    void GetNextDestination()
    {
        float randomDistance = Random.Range(minWalkDistance, maxWalkDistance);
        float randomZ = Random.Range(-randomDistance, randomDistance);
        float randomX = Random.Range(-randomDistance, randomDistance);

        _walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(_walkPoint, -transform.up, 2f, whatIsGround))
            _walkPointSet = true;

    }

    void GenerateCooldown()
    {
        _cooldown = Random.Range(minWaitTime, maxWaitTime);
    }
    
}
