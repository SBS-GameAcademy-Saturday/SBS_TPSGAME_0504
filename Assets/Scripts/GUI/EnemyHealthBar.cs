using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
	[SerializeField] private Image fillAmount;

	private void Awake()
	{
		Damageable damageable = GetComponentInParent<Damageable>();
		damageable.OnHealthChangeEvent.AddListener(UpdateHealthbar);
	}

	private void UpdateHealthbar(float current,float max)
	{
		float value = current / max;
		fillAmount.fillAmount = value;
	}

}
