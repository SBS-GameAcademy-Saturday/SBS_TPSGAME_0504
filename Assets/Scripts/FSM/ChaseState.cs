using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : MonoBehaviour, IState
{
	private Transform _target;
	private NavMeshAgent _navMeshAgent;
	public void EnterState()
	{
		if(!_target) _target = GameObject.FindGameObjectWithTag("Player").transform;
		if(!_navMeshAgent) _navMeshAgent = GetComponent<NavMeshAgent>();
		Debug.Log("Chase State : Enter");
	}

	public void UpdateState()
	{
		_navMeshAgent.SetDestination(_target.position);
		Debug.Log("Chase State : Update");
	}

	public void ExitState()
	{
		Debug.Log("Chase State : Exit");
	}


}
