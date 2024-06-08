using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleCanvas : MonoBehaviour
{
	[SerializeField] private Button _playButton;
	[SerializeField] private Button _quitButton;
    void Start()
    {
		_playButton.onClick.AddListener(OnPlayButton);
		_quitButton.onClick.AddListener(OnQuitButton);
    }

	private void OnPlayButton()
	{
		SceneManager.LoadScene("Main");
	}

	private void OnQuitButton()
	{
		Debug.Log("On Quit Button Clicked");
		// ���� ����Ƽ ������ ���� Ȯ��
#if UNITY_EDITOR
		// ����Ƽ �����Ϳ����� ����
		UnityEditor.EditorApplication.isPlaying = false;
#else
		// APK,exe ���� ���Ͽ��� ������ ����
		Application.Quit();
#endif
	}
}
