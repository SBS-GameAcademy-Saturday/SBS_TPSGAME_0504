using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Damageable))]
public class DroneEnemyController : MonoBehaviour
{
	[Header("Conditions")]
	[SerializeField] private LayerMask _playerLayer;
	[SerializeField] private float _visionRadius = 11f;
	[SerializeField] private float _onHitVisionRadius = 30f;
	[SerializeField] private float _shootingRadius = 5f;
	[SerializeField] private bool _playerInVisionRadius = false;
	[SerializeField] private bool _playerInShootingRadius = false;

	[Header("States")]
	[SerializeField] private PatrolState _patrolState;
	[SerializeField] private ChaseState _chaseState;
	[SerializeField] private ShootState _shootState;

	[Header("VFX")]
	[SerializeField] private ParticleSystem _destoryEffect;

	private NavMeshAgent _navMeshAgent;
	private CharacterController _controller;
	private StateContext _stateContext;
	private Animator _animator;
	private Damageable _damageable;

	private EState _currentState = EState.PATROL;

	private float _initialVisionRadius;

	private void Start()
	{
		_navMeshAgent = GetComponent<NavMeshAgent>();
		_controller = GetComponent<CharacterController>();
		_animator = GetComponent<Animator>();
		_damageable = GetComponent<Damageable>();

		_damageable.OnDeathEvent.AddListener(OnDeath);
		_damageable.OnDamageEvent.AddListener(OnDamage);

		_stateContext = new StateContext();

		UpdateState(_currentState);

		_initialVisionRadius = _visionRadius;
	}

	private void Update()
	{
		if (_currentState == EState.DEATH) return;

		// �÷��̾ �߰��� �� �ִ� ���� �ȿ� ���Դ� �� Ȯ��
		_playerInVisionRadius = Physics.CheckSphere(transform.position, _visionRadius, _playerLayer);
		// �÷��̾ ������ �� �ִ� ���� �ȿ� ���Դ� �� Ȯ��
		_playerInShootingRadius = Physics.CheckSphere(transform.position, _shootingRadius, _playerLayer);

		// �÷��̾ �߰� �� �� �ִ� �����ȿ� �ȵ�� �԰� ���� �� �� �ִ� �������� ������ PATROL ���·� ��ȯ
		if (!_playerInVisionRadius && !_playerInShootingRadius) UpdateState(EState.PATROL);
		// �÷��̾ �߰� �� �� �ִ� �����ȿ� ��� �԰� ���� �� �� �ִ� �������� ������ CHASE ���·� ��ȯ
		if (_playerInVisionRadius && !_playerInShootingRadius) UpdateState(EState.CHASE);
		// �÷��̾ �߰� �� �� �ִ� �����ȿ� ��� �԰� ���� �� �� �ִ� �������� ��� ������ SHOOT ���·� ��ȯ
		if (_playerInVisionRadius && _playerInShootingRadius) UpdateState(EState.SHOOT);

		_stateContext.CurrentState.UpdateState();
		UpdateAniamtion();
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

	private void UpdateAniamtion()
	{
		_animator.SetBool(AnimationStrings.Walk, _currentState == EState.PATROL);
		_animator.SetBool(AnimationStrings.AimRun, _currentState == EState.CHASE);
		_animator.SetBool(AnimationStrings.Shoot, _currentState == EState.SHOOT);
		_animator.SetBool(AnimationStrings.Die, _currentState == EState.DEATH);
	}

	private void OnDamage()
	{
		if(_visionRadius < _onHitVisionRadius)
		{
			_visionRadius = _onHitVisionRadius;
			Invoke(nameof(ResetVisionRadius), 10);
		}
	}

	private void ResetVisionRadius()
	{
		_visionRadius = _initialVisionRadius;
	}

	private void OnDeath()
	{
		_navMeshAgent.isStopped = true;
		_currentState = EState.DEATH;
		_controller.enabled = false;
		_shootingRadius = 0;
		_visionRadius = 0;
		_playerInShootingRadius = false;
		_playerInVisionRadius = false;
		_destoryEffect.Play();
		UpdateAniamtion();
		Destroy(this.gameObject, 5.0f);
	}

	private void OnDisable()
	{
		_damageable.OnDeathEvent.RemoveAllListeners();
		_damageable.OnDamageEvent.RemoveAllListeners();
	}
}
