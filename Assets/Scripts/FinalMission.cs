using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMission : MonoBehaviour
{
	[Header("Condition")]
	[SerializeField] private KeyCode _interactionKey = KeyCode.F;

	[SerializeField] private PlayerController _player;
	[SerializeField] private float _distance = 3;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(_interactionKey) && 
			Vector3.Distance(transform.position, _player.transform.position) < _distance && 
			MissionDisplay.Instance.IsFinalMission())
		{
			MissionDisplay.Instance.CompleteMission(4);
			Menu menu = FindObjectOfType<Menu>();
			if(menu != null) 
			{
				menu.EndGame();
			}
		}
    }



}
