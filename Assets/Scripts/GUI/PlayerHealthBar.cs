using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
	[SerializeField] private Image fillAmount;
	[SerializeField] private Damageable damageable;

	private void Start()
	{
		damageable.OnHealthChangeEvent.AddListener(UpdateHealthBar);
	}

	private void UpdateHealthBar(float currentHealth,float maxHealth)
	{
		float value = currentHealth / maxHealth;
		fillAmount.fillAmount = value;
	}
}
