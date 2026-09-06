using System;
using _Project.Logic.Extensions;
using _Project.Logic.Services;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _Project.Logic.Entities.AI_Movement
{
    public class AnimalNavMeshMovement : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;

        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                var currentPos = transform.position;
                if (NavMesh.SamplePosition(RandomNearPosition(currentPos), out NavMeshHit hit,
                        maxDistance: 2f, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
            }
        }

        private static Vector3 RandomNearPosition(Vector3 currentPos)
        {
            return new Vector3(currentPos.x + Random.Range(-5, 5), currentPos.y, currentPos.z + Random.Range(-5, 5));
        }
    }
}