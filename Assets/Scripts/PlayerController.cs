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
	[SerializeField] private Transform _surfacePosition; // ���ٴ��� üũ�� ��ġ
	[SerializeField] private float surfaceDistance = 0.4f; // ���ٴ��� üũ�� ����
	[SerializeField] private LayerMask surfaceMask; // ���ٴڸ� üũ�ϵ��� �����ϴ� Layer
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
		// �׶��� ���� ������ -2�� �ʱ�ȭ �մϴ�.
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
	/// ĳ���� ��Ʈ�ѷ��� ���� 3��Ī ĳ���� ������ ����
	/// </summary>
	private void Move()
	{
		//Input.GetAxisRaw() => -1,0,1 �� ���� �ϳ���
		//Input.GetAxis() => -1~ 1 ������ ���������� �� ��ȯ�� �Ѵ�.
		float horizontal_Axis = Input.GetAxisRaw("Horizontal");
		float vertical_Axis = Input.GetAxisRaw("Vertical");

		//         ��      =     ����                       ? ������ �� : ������ ���� => ���׿�����
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
	/// ĳ���� ��Ʈ�ѷ��� ���� ĳ���� ���� ����
	/// </summary>
	private void Jump()
	{
		if(Input.GetButtonDown("Jump") && OnSurface)
		{
			velocity.y = Mathf.Sqrt(_jumpRange * -2 * _gravity);
		}
	}
}
