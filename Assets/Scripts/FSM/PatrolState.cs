using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : MonoBehaviour, IState
{
	[SerializeField] Transform[] wayPoints;
	[SerializeField] float pointRadius = 1f;

	private NavMeshAgent _navMeshAgent;

	private int _currentWayPoint = 0;
	public void EnterState()
	{
		if(!_navMeshAgent) _navMeshAgent = GetComponent<NavMeshAgent>();

		Debug.Log("Patrol State : Enter");
	}
	public void UpdateState()
	{
		if (Vector3.Distance(wayPoints[_currentWayPoint].position,transform.position) 
			<= pointRadius)
		{
			_currentWayPoint = UnityEngine.Random.Range(0, wayPoints.Length);
		}

		_navMeshAgent.SetDestination(wayPoints[_currentWayPoint].position);
		Debug.Log("Patrol State : Update");
	}

	public void ExitState()
	{
		Debug.Log("Patrol State : Exit");
	}
}