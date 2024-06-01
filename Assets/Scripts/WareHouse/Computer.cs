using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
	[Header("Computer On/Off")]
	[SerializeField] private GameObject lampOn; // ��ǻ�Ͱ� ������ �� ���� ����
	[SerializeField] private GameObject lampOff;// ��ǻ�Ͱ� ������ �� ���� ����

	[Header("Condition")]
	[SerializeField] private PlayerController player; // �÷��̾� 
	[SerializeField] private float radius = 2.5f; // �÷��̾���� �ּ� �Ÿ���

	[SerializeField] private KeyCode InteractionKey = KeyCode.F;

	private bool isOff = false; // ���� ���� ���������� ���� ����

    void Update()
    {
		// �̹� ���� �����̸� ����
		if (isOff) return;

		// �÷��̾� ĳ���Ϳ��� �Ÿ����� �Ǵ�
		if (Vector3.Distance(transform.position, player.transform.position) > radius)
			return;

		// �÷��̾� ĳ���Ͱ� ��ư�� �������� �Ǵ�
		if (Input.GetKeyDown(InteractionKey))
		{
			// ��ǻ�͸� ���� ���� ó��
			lampOn.SetActive(false);
			lampOff.SetActive(true);
			isOff = true;
			MissionDisplay.Instance.CompleteMission(2);
		}
	}
}
