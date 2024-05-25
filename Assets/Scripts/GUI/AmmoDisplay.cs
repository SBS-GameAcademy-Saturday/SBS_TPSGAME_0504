using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI currentAmmo;
	[SerializeField] private TextMeshProUGUI remainAmmo;

	private void Awake()
	{
		Rifle rifle = FindObjectOfType<Rifle>();
		rifle.OnAmmoChanged.AddListener(UpdateAmmo);
	}

	private void UpdateAmmo(int current,int remain)
	{
		currentAmmo.text = $"Current : {current}";
		remainAmmo.text = $"Remain : {remain}";
	}
}
