using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
	[SerializeField] CinemachineVirtualCamera thirdPersonCamera;
	[SerializeField] CinemachineVirtualCamera aimingCamera;
	[SerializeField] GameObject thirdPersonCanvas;
	[SerializeField] GameObject AimingCanvas;

	private Animator _animator;
	private int activePriority = 15;
	private int deactivePriority = 10;

	void Start()
    {
		_animator = GetComponent<Animator>();        
    }

    void Update()
    {
		// ���콺 ��Ŭ�� ����
		if (Input.GetButton(InputStrings.Fire2))
		{
			// ���� ���� ������ Aiming�� true�� ��ȯ�մϴ�.
			_animator.SetBool(AnimationStrings.Aiming, true);

			aimingCamera.Priority = activePriority;
			thirdPersonCamera.Priority = deactivePriority;
			thirdPersonCanvas.SetActive(false);
			AimingCanvas.SetActive(true);
		}
		else
		{
			_animator.SetBool(AnimationStrings.Aiming, false);

			aimingCamera.Priority = deactivePriority;
			thirdPersonCamera.Priority = activePriority;
			thirdPersonCanvas.SetActive(true);
			AimingCanvas.SetActive(false);
		}
    }
}
