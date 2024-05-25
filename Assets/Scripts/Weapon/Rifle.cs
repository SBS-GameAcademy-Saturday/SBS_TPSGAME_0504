using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Rifle : MonoBehaviour
{
	[SerializeField] private float fireCharge = 15.0f;
	[SerializeField] private float ShootRange = 100.0f;
	[SerializeField] private float Damage = 10.0f;

	[SerializeField] private float normalRange = 0.03f;
	[SerializeField] private float aimingRange = 0.01f;

	[Header("Rifle Ammunition And Shooting")]
	[SerializeField] private int maxAmmo = 30; // 최대 장전 총알 갯수
	[SerializeField] private int currentAmmo = 0; // 현재 총알 갯수
	[SerializeField] private int remainAmmo = 150; // 남은 총알 갯수
	[SerializeField] private float reloadingTime = 1.3f; // 재장전 시간
	[SerializeField] private bool setReloading = false; // 재장전 중인지 판단하는 변수

	[Header("Rifle VFX")]
	[SerializeField] private ParticleSystem muzzleSpark;
	[SerializeField] private GameObject impactEffect;
	[SerializeField] private GameObject goreEffect;
	[SerializeField] private GameObject droneEffect;

	[Header("Rifle SFX")]
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip shootingSound;

	// 총알 갯수가 바뀔 때마다 호출 할 이벤트
	// 매개변수로 현재 총알, 남은 총알 갯수를 넘겨줍니다.
	public UnityEvent<int,int> OnAmmoChanged;

	private PlayerController playerController;
	private Transform cam;
	private Animator _animator;
	private float nextTimeToShoot = 0;
    void Start()
    {
		playerController = GetComponentInParent<PlayerController>();
		_animator = GetComponentInParent<Animator>();
		cam = Camera.main.transform;
		currentAmmo = maxAmmo;

		OnAmmoChanged?.Invoke(currentAmmo, remainAmmo);
	}

    // Update is called once per frame
    void Update()
    {
		// 재장전 중이면은 Update문을 실행을 안한다.
		if (setReloading) return;

		// Time.time = 프로젝트 재생 시작 후 경과한 시간 초 단위로 반환합니다.
		// Time.deltaTime = 마지막 프레임이 완료된 후 경과한 시간을 초 단위로 반환합니다.
		if (Input.GetKeyDown(KeyCode.R))
		{
			StartCoroutine(Reload());
			return;
		}

		// 마우스 좌클릭 감지 && 타이머 설정(현재 경과 시간이 다음 발사 시간보다 이상일 경우)
		if (Input.GetButton(InputStrings.Fire1) && Time.time >= nextTimeToShoot)
		{
			_animator.SetBool(AnimationStrings.Shoot, true);
			_animator.SetBool(AnimationStrings.Idle, false);
			nextTimeToShoot = Time.time + 1 / fireCharge; // 현재 시간(발사) + 0.07
			Shoot();
		}
		else if(Input.GetButton(InputStrings.Fire1) == false)
		{ 
			_animator.SetBool(AnimationStrings.Shoot, false);
			_animator.SetBool(AnimationStrings.Idle, true);
		}
    }

	private void Shoot()
	{
		// 현재 총알이 0이지만 남아있는 총알이 있으면 재장전
		if(currentAmmo <= 0 && remainAmmo > 0)
		{
			Debug.Log("현재 총알이 없습니다.");
			StartCoroutine(Reload());
			return;
		}

		// 현재 총알이 없고, 남아있는 총알도 없으면 종료
		if (currentAmmo <= 0 && remainAmmo <= 0)
			return;

		currentAmmo--;

		OnAmmoChanged?.Invoke(currentAmmo, remainAmmo);

		muzzleSpark.Play();
		audioSource.PlayOneShot(shootingSound);

		if (Physics.Raycast(cam.position, GetRandomForward()
			, out RaycastHit hitInfo, ShootRange))
		{
			Debug.Log(hitInfo.collider.gameObject.name);

			// 맞은 정보에서 Gameobject의 Damageable Component를 가져온다.
			Damageable damageable = hitInfo.collider.GetComponent<Damageable>();
			SoldierEnemyController soldierEnemyController = hitInfo.collider.GetComponent<SoldierEnemyController>();
			DroneEnemyController droneEnemyController = hitInfo.collider.GetComponent<DroneEnemyController>();
			if (soldierEnemyController != null)
			{
				GameObject impacktGO = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
				Destroy(impacktGO, 1);
			}
			else if (droneEnemyController != null)
			{
				GameObject impacktGO = Instantiate(droneEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
				Destroy(impacktGO, 1);
			}
			else
			{
				GameObject impacktGO = Instantiate(impactEffect, hitInfo.point,Quaternion.LookRotation(hitInfo.normal));
				Destroy(impacktGO, 1);
			}
			if (damageable != null)
			{
				damageable.HitDamage(Damage);
			}

		}
	}

	// 총기 반동으로 인한 Spread 효과 구현
	private Vector3 GetRandomForward()
	{
		Vector3 forward = cam.forward;
		if (_animator.GetBool(AnimationStrings.Aiming))
		{
			forward.x = forward.x + UnityEngine.Random.Range(-aimingRange, aimingRange);
			forward.y = forward.y + UnityEngine.Random.Range(-aimingRange, aimingRange);
		}
		else
		{
			forward.x = forward.x + UnityEngine.Random.Range(-normalRange, normalRange);
			forward.y = forward.y + UnityEngine.Random.Range(-normalRange, normalRange);
		}
		return forward.normalized;
	}

	// 코루틴 : 
	// 비동기 프로그래밍 기법 중 하나로, 중단점에서 실행을 멈추고
	// 나중에 중단점부터 다시 실행을 시작할 수 있는 함수입니다.
	// 주로 비동기 작업을 쉽게 처리하기 위해서 사용됩니다.
	// IEnumerator,yield

	// 동기 : 작업을 순차적으로 처리하며, 하나의 작업이 완료될 때까지 다음 작업을 실행하지 않는 것
	// 비동기 : 작업이 완료될 때까지 기다리지 않고 다음 작업을 동시에 처리합니다.I/O
	IEnumerator Reload()
	{
		playerController.SetCanWalk(false);
		setReloading = true;
		Debug.Log("Reloading");
		_animator.SetBool(AnimationStrings.Reload, true);
		// yield return null; // 다음 프레임까지 대기;
		// 지정된 reloadingTime 초까지 코드 실행을 중단한다.
		yield return new WaitForSeconds(reloadingTime);
		// 지정된 reloadingTime초까지 기다렸으면 다음 코드를 실행한다.
		_animator.SetBool(AnimationStrings.Reload, false);

		// 총알 계산
		int sumAmmo = currentAmmo + remainAmmo;
		int result = sumAmmo - maxAmmo;
		if (result > 0)
		{
			currentAmmo = maxAmmo;
			remainAmmo = result;
		}
		else
		{
			currentAmmo = sumAmmo;
			remainAmmo = 0;
		}
		OnAmmoChanged?.Invoke(currentAmmo, remainAmmo);
		playerController.SetCanWalk(true);
		setReloading = false;
	}


	//private void OnDrawGizmos()
	//{
	//	Gizmos.color = Color.red;
	//	Gizmos.DrawRay(cam.position, cam.forward);
	//}
}
