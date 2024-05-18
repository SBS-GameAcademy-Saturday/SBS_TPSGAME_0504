using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
	public UnityEvent OnDeathEvent;
	public UnityEvent OnDamageEvent;
	public float Health = 100;

	public void HitDamage(float amount)
	{
		Health -= amount;

		OnDamageEvent?.Invoke();

		if (Health <= 0)
		{
			Die();
		}
	}

	void Die()
	{
		//if (OnDeathEvent != null)
		//{
		//	OnDeathEvent.Invoke();
		//}
		// �� ó�� Null üũ�� �ϰ� Null�� �ƴϸ� �����ϴ� ��� => ?
		OnDeathEvent?.Invoke();
		//Destroy(gameObject);
	}
}
