using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStates : MonoBehaviour
{
	[SerializeField] CinemachineVirtualCamera thirdPersonCamera;
	[SerializeField] CinemachineVirtualCamera aimingCamera;
	[SerializeField] GameObject thirdPersonCanvas;
	[SerializeField] GameObject AimingCanvas;

	[SerializeField] Image thirdPersionCrosshair;
	[SerializeField] Image aimingCrosshair;

	private Animator _animator;
	private int activePriority = 15;
	private int deactivePriority = 10;

	void Start()
    {
		_animator = GetComponent<Animator>();        
    }

    void Update()
    {
		// 마우스 우클릭 감지
		if (Input.GetButton(InputStrings.Fire2))
		{
			// 현재 상태 변수인 Aiming을 true로 전환합니다.
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

	public void ChangeCrosshair(bool change)
	{
		//if (change)
		//{
		//	thirdPersionCrosshair.color = Color.blue;
		//	aimingCrosshair.color = Color.blue;
		//}
		//else
		//{
		//	thirdPersionCrosshair.color = Color.white;
		//	aimingCrosshair.color = Color.white;
		//}

		// 삼항 연산자              =  조건 ? 참일 때 처리  : 거짓일 때 처리
		thirdPersionCrosshair.color = change ? Color.blue : Color.white;
		aimingCrosshair.color = change ? Color.blue : Color.white;
	}
}
