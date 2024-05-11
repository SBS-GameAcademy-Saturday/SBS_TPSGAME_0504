using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
	[SerializeField] private float fireCharge = 15.0f;
	[SerializeField] private float ShootRange = 100.0f;
	[SerializeField] private float Damage = 10.0f;

	[SerializeField] private float normalRange = 0.03f;
	[SerializeField] private float aimingRange = 0.01f;

	[Header("Rifle VFX")]
	[SerializeField] private ParticleSystem muzzleSpark;
	[SerializeField] private GameObject impactEffect;

	[Header("Rifle SFX")]
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip shootingSound;


	private Transform cam;
	private Animator _animator;
	private float nextTimeToShoot = 0;
    void Start()
    {
        _animator = GetComponentInParent<Animator>();
		cam = Camera.main.transform;
	}

    // Update is called once per frame
    void Update()
    {
		// Time.time = 프로젝트 재생 시작 후 경과한 시간 초 단위로 반환합니다.
		// Time.deltaTime = 마지막 프레임이 완료된 후 경과한 시간을 초 단위로 반환합니다.

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
		muzzleSpark.Play();
		audioSource.PlayOneShot(shootingSound);

		if (Physics.Raycast(cam.position, GetRandomForward()
			, out RaycastHit hitInfo, ShootRange))
		{
			Debug.Log(hitInfo.collider.gameObject.name);

			GameObject impacktGO = Instantiate(impactEffect, hitInfo.point, 
				Quaternion.LookRotation(hitInfo.normal));
			Destroy(impacktGO,1);

			// 맞은 정보에서 Gameobject의 Damageable Component를 가져온다.
			Damageable damageable = hitInfo.collider.GetComponent<Damageable>();
			if(damageable != null)
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

	//private void OnDrawGizmos()
	//{
	//	Gizmos.color = Color.red;
	//	Gizmos.DrawRay(cam.position, cam.forward);
	//}
}
