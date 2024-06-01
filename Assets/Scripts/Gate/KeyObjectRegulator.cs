using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObjectRegulator : MonoBehaviour
{
	[SerializeField] private KeyList keyList;

	public void FoundObject()
	{
		keyList.hasKey = true;
		gameObject.SetActive(false);
		MissionDisplay.Instance.CompleteMission(1);
	}
}
