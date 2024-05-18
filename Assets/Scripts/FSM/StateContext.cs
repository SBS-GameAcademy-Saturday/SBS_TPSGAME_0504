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
	/// ���� ���¸� ��Ÿ���� ������Ƽ
	/// </summary>
	public IState CurrentState { get; set; }

	/// <summary>
	/// ���� ���¸� ���ο� ���·� ��ȯ�ϴ� �Լ�
	/// </summary>
	/// <param name="state"> ���ο� ������ �������̽�</param>
	public void Transition(IState state) 
	{
		if (CurrentState != null) CurrentState.ExitState();
		CurrentState = state;
		CurrentState.EnterState();
	}
}
