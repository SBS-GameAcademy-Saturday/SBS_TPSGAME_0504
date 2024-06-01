using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
	[Header("Menus")]
	[SerializeField] private MissionDisplay missionDisplay;
	[SerializeField] private GameObject PauseDisplay;

	private bool _pauseDisplayToggle = false;
	private bool _missionDisplaytoggle = false;
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (_pauseDisplayToggle)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}
		if (Input.GetKeyDown(KeyCode.M))
		{
			if (_missionDisplaytoggle)
			{
				HideMissionDisplay();
			}
			else
			{
				ShowMissionDisplay();
			}
		}
	}

	private void Pause()
	{
		PauseDisplay.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
		_pauseDisplayToggle = true;
	}

	private void Resume()
	{
		PauseDisplay.SetActive(false);
		Cursor.lockState = CursorLockMode.Locked;
		_pauseDisplayToggle = false;
	}

	private void ShowMissionDisplay()
	{
		missionDisplay.gameObject.SetActive(true);
		_missionDisplaytoggle = true;
	}

	private void HideMissionDisplay()
	{
		missionDisplay.gameObject.SetActive(false);
		_missionDisplaytoggle = false;
	}
}
