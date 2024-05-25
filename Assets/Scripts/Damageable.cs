using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
	public UnityEvent OnDeathEvent;
	public UnityEvent OnDamageEvent;
	public UnityEvent<float,float> OnHealthChangeEvent;

	public float Health = 100;
	public float MaxHealth = 100;

	private void Awake()
	{
		Health = MaxHealth;
	}

	public void HitDamage(float amount)
	{
		Health -= amount;

		OnDamageEvent?.Invoke();

		OnHealthChangeEvent?.Invoke(Health, MaxHealth);

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
