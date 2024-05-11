using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float speed = 3f;
	[SerializeField] private float sprint = 5.0f;

	[Header("Player Gravity")]
	[SerializeField] private float _gravity = -9.81f;
	[SerializeField] private float _fallingSpeed = 0.5f;

	[Header("Player Ground And Jump")]
	[SerializeField] private Transform _surfacePosition; // 땅바닥을 체크할 위치
	[SerializeField] private float surfaceDistance = 0.4f; // 땅바닥을 체크할 범위
	[SerializeField] private LayerMask surfaceMask; // 땅바닥만 체크하도록 구분하는 Layer
	[SerializeField] private float _jumpRange = 1f;

	private Transform playerCamera;
	private CharacterController _controller;
	private Animator _animator;

	private float turnCalmVelocity;
	private float turnCalmTime = 0.1f;

	private Vector3 velocity;
	private bool OnSurface;


	void Start()
    {
		playerCamera = Camera.main.transform;
		_controller = GetComponent<CharacterController>();
		_animator = GetComponent<Animator>();
	}

    void Update()
    {
		OnSurface = Physics.CheckSphere(_surfacePosition.position, surfaceDistance, surfaceMask);
		// 그라운드 위에 있으면 -2로 초기화 합니다.
		if(OnSurface && velocity.y < 0)
		{
			velocity.y = -2f;
		}
		velocity.y += _gravity * Time.deltaTime;
		_controller.Move(velocity * Time.deltaTime * _fallingSpeed);


		Move();

		Jump();
	}

	/// <summary>
	/// 캐릭터 컨트롤러를 통한 3인칭 캐릭터 움직임 구현
	/// </summary>
	private void Move()
	{
		//Input.GetAxisRaw() => -1,0,1 이 셋중 하나만
		//Input.GetAxis() => -1~ 1 사이의 소주점까지 다 반환을 한다.
		float horizontal_Axis = Input.GetAxisRaw("Horizontal");
		float vertical_Axis = Input.GetAxisRaw("Vertical");

		//         값      =     조건                       ? 조건이 참 : 조건이 거짓 => 삼항연산자
		float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprint : speed;

		Vector3 direction = new Vector3(horizontal_Axis, 0, vertical_Axis).normalized;

		if (direction.magnitude < 0.1f) currentSpeed = 0;

		float targetAngle =
			Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg
			+ playerCamera.eulerAngles.y;

		//float angle = Mathf.SmoothDampAngle(
		//	transform.eulerAngles.y
		//	, targetAngle,
		//	ref turnCalmVelocity,
		//	turnCalmTime);

		//transform.rotation = Quaternion.Euler(0, angle, 0);

		Vector3 moveDirection =
			Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

		_controller.Move(moveDirection * Time.deltaTime * currentSpeed);

		_animator.SetFloat("Speed", currentSpeed);

		//if (currentSpeed <= 0)
		//{
		//	_animator.SetBool("Walk", false);
		//	_animator.SetBool("Run", false);
		//	_animator.SetBool("Idle", true);
		//}
		//else if(currentSpeed > 0 || currentSpeed < sprint)
		//{
		//	_animator.SetBool("Walk", true);
		//	_animator.SetBool("Run", false);
		//	_animator.SetBool("Idle", false);
		//}
		//else if(currentSpeed >= sprint)
		//{
		//	_animator.SetBool("Walk", false);
		//	_animator.SetBool("Run", true);
		//	_animator.SetBool("Idle", false);
		//}
		Debug.Log(currentSpeed);
	}

	/// <summary>
	/// 캐릭터 컨트롤러를 통한 캐릭터 점프 구현
	/// </summary>
	private void Jump()
	{
		if(Input.GetButtonDown("Jump") && OnSurface)
		{
			velocity.y = Mathf.Sqrt(_jumpRange * -2 * _gravity);
		}
	}
}
