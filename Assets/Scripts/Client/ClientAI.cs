using System;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

namespace Client
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class ClientAI : MonoBehaviour
	{
		[SerializeField] private float radius = 10f;
		[SerializeField] private float changeDestinationTime = 3f;
		
		private NavMeshAgent agent;
		private float timer;

		private void Start()
		{
			if (!TryGetComponent(out agent))
				Debug.LogError("No navmesh on client");
			
			timer = changeDestinationTime;
			SetNewRandomDestination();
		}

		private void Update()
		{
			timer -= Time.deltaTime;
			if (timer <= 0 || agent.remainingDistance <= 0.5f)
			{
				timer = changeDestinationTime;
				SetNewRandomDestination();
			}
		}

		private void SetNewRandomDestination()
		{
			Vector3 rndDir = UnityEngine.Random.insideUnitSphere * radius;
			rndDir += transform.position;

			NavMeshHit hit;
			bool valid = NavMesh.SamplePosition(rndDir, out hit, radius, NavMesh.AllAreas);
			if(valid)
				agent.SetDestination(hit.position);
		}
	}
}