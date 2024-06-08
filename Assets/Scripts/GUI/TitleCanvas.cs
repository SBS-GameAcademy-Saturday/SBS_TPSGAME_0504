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
		// 현재 유니티 에디터 인지 확인
#if UNITY_EDITOR
		// 유니티 에디터에서만 동작
		UnityEditor.EditorApplication.isPlaying = false;
#else
		// APK,exe 빌드 파일에서 게임을 종료
		Application.Quit();
#endif
	}
}
