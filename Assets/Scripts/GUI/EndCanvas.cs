using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndCanvas : MonoBehaviour
{
	[SerializeField] private Button _replayButton;
	[SerializeField] private Button _quitButton;

	void Start()
	{
		_replayButton.onClick.AddListener(OnReplayButton);
		_quitButton.onClick.AddListener(OnQuitButton);
	}

	private void OnReplayButton()
	{
		SceneManager.LoadScene("Main");
	}

	private void OnQuitButton()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
