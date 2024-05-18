using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EState
{
	NONE,
	PATROL,
	CHASE,
	SHOOT,
	DEATH,
}

public class StateContext
{
	/// <summary>
	/// 현재 상태를 나타내는 프로퍼티
	/// </summary>
	public IState CurrentState { get; set; }

	/// <summary>
	/// 현재 상태를 새로운 상태로 전환하는 함수
	/// </summary>
	/// <param name="state"> 새로운 상태의 인터페이스</param>
	public void Transition(IState state) 
	{
		if (CurrentState != null) CurrentState.ExitState();
		CurrentState = state;
		CurrentState.EnterState();
	}
}
