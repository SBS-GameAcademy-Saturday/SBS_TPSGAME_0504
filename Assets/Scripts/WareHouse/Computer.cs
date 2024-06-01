using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
	[Header("Computer On/Off")]
	[SerializeField] private GameObject lampOn; // 컴퓨터가 켜졌을 때 나올 램프
	[SerializeField] private GameObject lampOff;// 컴퓨터가 꺼졌을 때 나올 램프

	[Header("Condition")]
	[SerializeField] private PlayerController player; // 플레이어 
	[SerializeField] private float radius = 2.5f; // 플레이어와의 최소 거리값

	[SerializeField] private KeyCode InteractionKey = KeyCode.F;

	private bool isOff = false; // 현재 꺼진 상태인지에 따른 변수

    void Update()
    {
		// 이미 꺼진 상태이면 종료
		if (isOff) return;

		// 플레이어 캐릭터와의 거리값을 판단
		if (Vector3.Distance(transform.position, player.transform.position) > radius)
			return;

		// 플레이어 캐릭터가 버튼을 눌렀는지 판단
		if (Input.GetKeyDown(InteractionKey))
		{
			// 컴퓨터를 끄는 로직 처리
			lampOn.SetActive(false);
			lampOff.SetActive(true);
			isOff = true;
			MissionDisplay.Instance.CompleteMission(2);
		}
	}
}
