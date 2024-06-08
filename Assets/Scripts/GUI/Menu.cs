using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	[Header("Menus")]
	[SerializeField] private MissionDisplay missionDisplay;
	[SerializeField] private GameObject PauseDisplay;
	[SerializeField] private GameObject EndGameDisplay;

	[Header("Buttons")]
	[SerializeField] private Button _resumeButton;
	[SerializeField] private Button _menuButton;
	[SerializeField] private Button _quitButton;
	[SerializeField] private Button _restartButton;
	[SerializeField] private Button _exitButton;

	private bool _pauseDisplayToggle = false;
	private bool _missionDisplaytoggle = false;
	private bool _isEndGame = false; 

	private void Start()
	{
		_resumeButton.onClick.AddListener(Resume);
		_menuButton.onClick.AddListener(OnMenuButton);
		_quitButton.onClick.AddListener(OnQuitButton);
		_restartButton.onClick.AddListener(OnRestartButton);
		_exitButton.onClick.AddListener(OnExitButton);
	}

	//1, 미션 창이 열린 상태에서 ESC 키를 누르면 
	// => 미션창이 닫히고, 일시정지 화면이 보인다.
	//	 일시 정지 창이 열린 상태에서 M 키를 누르면 
	// => 일시 정지 창이 닫히고, 미션 화면이 보인다.
	private void Update()
	{
		if (_isEndGame)
			return;
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (_pauseDisplayToggle)
			{
				Resume();
			}
			else
			{
				if (_missionDisplaytoggle == true)
				{
					HideMissionDisplay();
				}
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
				if (_pauseDisplayToggle)
				{
					Resume();
				}
				ShowMissionDisplay();
			}
		}
	}

	//2, 미션 창이 열린 상태에서 ESC 키를 누르면 
	// => 미션창이 닫힙니다.
	//	 일시 정지 창이 열린 상태에서 M 키를 누르면 
	// => 일시 정지 창이 닫힙니다.
	//private bool _gameIsStopped = false;
	//private void Update()
	//{
	//	if (Input.GetKeyDown(KeyCode.Escape) && !_gameIsStopped)
	//	{
	//		Pause();
	//		_gameIsStopped = true;
	//	}
	//	else if (Input.GetKeyDown(KeyCode.M) && !_gameIsStopped)
	//	{
	//		ShowMissionDisplay();
	//		_gameIsStopped = true;
	//	}
	//	else if((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.M)) && _gameIsStopped)
	//	{
	//		Resume();
	//		HideMissionDisplay();
	//		_gameIsStopped = false;
	//	}
	//}

	private void Pause()
	{
		Time.timeScale = 0;
		PauseDisplay.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
		_pauseDisplayToggle = true;
	}

	private void Resume()
	{
		Time.timeScale = 1;
		PauseDisplay.SetActive(false);
		Cursor.lockState = CursorLockMode.Locked;
		_pauseDisplayToggle = false;
	}

	private void ShowMissionDisplay()
	{
		Time.timeScale = 0;
		missionDisplay.gameObject.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
		_missionDisplaytoggle = true;
	}

	private void HideMissionDisplay()
	{
		Time.timeScale = 1;
		missionDisplay.gameObject.SetActive(false);
		Cursor.lockState = CursorLockMode.Locked;
		_missionDisplaytoggle = false;

	}

	private void OnMenuButton()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene("Title");
	}

	private void OnQuitButton()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	private void OnRestartButton()
	{
		Time.timeScale = 1;
		Scene currentScene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(currentScene.name);
	}

	private void OnExitButton()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene("EndGame");
	}

	public void EndGame()
	{
		EndGameDisplay.gameObject.SetActive(true);
		_isEndGame = true;
		Cursor.lockState = CursorLockMode.None;
		Time.timeScale = 0;
	}
}
