using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorButton : MonoBehaviour
{
	[Header("GeneratorButton On/Off")]
	[SerializeField] private GameObject greenLight;
	[SerializeField] private GameObject redLight;

	[Header("Generator")]
	[SerializeField] private Animator generatorAnimator;
	[SerializeField] private AudioSource generatorAudioSource;

	[Header("Condition")]
	[SerializeField] private PlayerController player;
	[SerializeField] private float radius = 2f;

	[SerializeField] private KeyCode InteractionKey = KeyCode.F;

	private bool isOff = false; // 현재 꺼진 상태인지에 따른 변수

    void Update()
    {
		// 이미 꺼진 상태면 종료
		if (isOff) return;

		// 플레이어 캐릭터와의 거리값을 판단
		if (Vector3.Distance(transform.position, player.transform.position) > radius)
			return;

		// 플레이어 캐릭터가 버튼을 눌렀는지 판단
		if (Input.GetKeyDown(InteractionKey))
		{
			// 컴퓨터를 끄는 로직 처리
			greenLight.SetActive(false);
			redLight.SetActive(true);
			generatorAnimator.enabled = false;
			generatorAudioSource.Stop();
			isOff = true;
			MissionDisplay.Instance.CompleteMission(3);
		}

	}
}
