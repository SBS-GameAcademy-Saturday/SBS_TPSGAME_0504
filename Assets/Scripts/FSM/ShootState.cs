using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootState : MonoBehaviour, IState
{
	[Header("Enemy Shoot")]
	[SerializeField]
	private Camera shootingArea;
	[SerializeField]
	private float shootingRange = 50f;
	[SerializeField]
	private float normalRange = 0.03f;
	[SerializeField]
	private float TimeShoot = 1f;

	[Header("Enemy Damage")]
	[SerializeField]
	private float Damage = 5f;

	[Header("Enemy FX")]
	[SerializeField] private ParticleSystem _muzzleSpark;
	[SerializeField] private ParticleSystem _muzzleFlame;
	[SerializeField] private AudioClip _shootSFX;
	[SerializeField] private AudioSource _audioSource;

	private Transform _target;
	private NavMeshAgent _navMeshAgent;
	private bool previousShoot = false;

	public void EnterState()
	{
		if (!_target) _target = GameObject.FindGameObjectWithTag("Player").transform;
		if (!_navMeshAgent) _navMeshAgent = GetComponent<NavMeshAgent>();

		_navMeshAgent.isStopped = true;
		Debug.Log("Shoot State : Enter");
	}

	public void UpdateState()
	{
		transform.LookAt(_target.position);

		if (!previousShoot)
		{
			_muzzleSpark.Play();
			if (_muzzleFlame != null && !_muzzleFlame.isPlaying) _muzzleFlame.Play();
			_audioSource.PlayOneShot(_shootSFX);

			RaycastHit hit;
			if (Physics.Raycast(shootingArea.transform.position, GetRandomForward(), out hit, shootingRange))
			{
				Debug.Log("Shooting " + hit.transform.name);

				Damageable damageable = hit.transform.GetComponent<Damageable>();
				if (damageable)
				{
					damageable.HitDamage(Damage);
				}

			}
			previousShoot = true;
			Invoke(nameof(ActiveShooting), TimeShoot);
		}
		Debug.Log("Shoot State : Update");
	}

	private void ActiveShooting()
	{
		previousShoot = false;
	}


	// 총기 반동으로 인한 Spread 효과 구현
	private Vector3 GetRandomForward()
	{
		Vector3 forward = shootingArea.transform.forward;
		forward.x = forward.x + UnityEngine.Random.Range(-normalRange, normalRange);
		forward.y = forward.y + UnityEngine.Random.Range(-normalRange, normalRange);
		return forward.normalized;
	}

	public void ExitState()
	{
		_navMeshAgent.isStopped = false;
		Debug.Log("Shoot State : Exit");
	}


}
