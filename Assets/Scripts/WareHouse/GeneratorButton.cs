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

	private bool isOff = false; // ���� ���� ���������� ���� ����

    void Update()
    {
		// �̹� ���� ���¸� ����
		if (isOff) return;

		// �÷��̾� ĳ���Ϳ��� �Ÿ����� �Ǵ�
		if (Vector3.Distance(transform.position, player.transform.position) > radius)
			return;

		// �÷��̾� ĳ���Ͱ� ��ư�� �������� �Ǵ�
		if (Input.GetKeyDown(InteractionKey))
		{
			// ��ǻ�͸� ���� ���� ó��
			greenLight.SetActive(false);
			redLight.SetActive(true);
			generatorAnimator.enabled = false;
			generatorAudioSource.Stop();
			isOff = true;
			MissionDisplay.Instance.CompleteMission(3);
		}

	}
}
