using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Damageable))]
public class SoldierEnemyController : MonoBehaviour
{
	[Header("Conditions")]
	[SerializeField] private LayerMask playerLayer;
	[SerializeField] private float visionRadius = 11f;
	[SerializeField] private float onHitVisionRadius = 30f;
	[SerializeField] private float shootingRadius = 5f;
	[SerializeField] private bool playerInVisionRaidus = false;
	[SerializeField] private bool playerInShootingRadius = false;

	[Header("States")]
	[SerializeField] private PatrolState _patrolState;
	[SerializeField] private ChaseState _chaseState;
	[SerializeField] private ShootState _shootState;

	private NavMeshAgent _navMeshAgent;
	private CharacterController _controller;
	private StateContext _stateContext;
	private Animator _animator;
	private Damageable _damageable;

	private EState _currentState = EState.PATROL;

	private float _initialVisionRadius;

	// Start is called before the first frame update
	void Start()
    {
		_navMeshAgent = GetComponent<NavMeshAgent>();
		_animator = GetComponent<Animator>();
		_damageable = GetComponent<Damageable>();
		_controller = GetComponent<CharacterController>();

		_damageable.OnDeathEvent.AddListener(OnDeath);
		_damageable.OnDamageEvent.AddListener(OnHitEvent);

		_stateContext = new StateContext();
		
		UpdateState(_currentState);

		_initialVisionRadius = visionRadius;
	}

	// Update is called once per frame
	void Update()
	{
		if (_currentState == EState.DEATH) return;

		// �÷��̾ �߰��� �� �ִ� ���� �ȿ� ���Դ� �� Ȯ��
		playerInVisionRaidus = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
		// �÷��̾ ������ �� �ִ� ���� �ȿ� ���Դ� �� Ȯ��
		playerInShootingRadius = Physics.CheckSphere(transform.position, shootingRadius, playerLayer);

		// �÷��̾ �߰� �� �� �ִ� �����ȿ� �ȵ�� �԰� ���� �� �� �ִ� �������� ������ PATROL ���·� ��ȯ
		if (!playerInVisionRaidus && !playerInShootingRadius) UpdateState(EState.PATROL);
		// �÷��̾ �߰� �� �� �ִ� �����ȿ� ��� �԰� ���� �� �� �ִ� �������� ������ CHASE ���·� ��ȯ
		if (playerInVisionRaidus && !playerInShootingRadius) UpdateState(EState.CHASE);
		// �÷��̾ �߰� �� �� �ִ� �����ȿ� ��� �԰� ���� �� �� �ִ� �������� ��� ������ SHOOT ���·� ��ȯ
		if (playerInVisionRaidus && playerInShootingRadius) UpdateState(EState.SHOOT);

		_stateContext.CurrentState.UpdateState();
		UpdateAnimation();
	}

	private void UpdateState(EState state)
	{
		switch (state)
		{
			case EState.PATROL:
				_stateContext.Transition(_patrolState);
				break;
			case EState.CHASE:
				_stateContext.Transition(_chaseState);
				break;
			case EState.SHOOT:
				_stateContext.Transition(_shootState);
				break;
		}
		_currentState = state;
	}

	private void UpdateAnimation()
	{
		_animator.SetBool(AnimationStrings.Walk, _currentState == EState.PATROL);
		_animator.SetBool(AnimationStrings.AimRun, _currentState == EState.CHASE);
		_animator.SetBool(AnimationStrings.Shoot, _currentState == EState.SHOOT);
		_animator.SetBool(AnimationStrings.Die, _currentState == EState.DEATH);
	}

	private void OnHitEvent()
	{
		if(visionRadius < onHitVisionRadius)
		{
			visionRadius = onHitVisionRadius;
			Invoke(nameof(ResetVisionRadius), 10);
		}
	}

	private void ResetVisionRadius()
	{
		visionRadius = _initialVisionRadius;
	}

	private void OnDeath()
	{
		_navMeshAgent.isStopped = true;
		_currentState = EState.DEATH;
		_controller.enabled = false;
		shootingRadius = 0;
		visionRadius = 0;
		playerInShootingRadius = false;
		playerInVisionRaidus = false;
		UpdateAnimation();
		Destroy(gameObject, 5.0f);
	}

	private void OnDisable()
	{
		_damageable.OnDeathEvent.RemoveAllListeners();
	}
}
