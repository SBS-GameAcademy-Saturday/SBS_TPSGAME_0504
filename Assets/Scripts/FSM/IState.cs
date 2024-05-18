using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public interface IState
{
	/// <summary>
	/// �� ���¿� ���� �� ȣ���ϴ� �Լ�
	/// </summary>
	public void EnterState();
	/// <summary>
	/// �� ���¸� �� �����Ӹ��� �����ϴ� �Լ�
	/// </summary>
	public void UpdateState();
	/// <summary>
	/// �� ���¸� �������� �� ȣ���ϴ� �Լ�
	/// </summary>
	public void ExitState();
}