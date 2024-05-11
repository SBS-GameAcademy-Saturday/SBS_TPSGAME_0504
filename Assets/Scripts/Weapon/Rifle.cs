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
		// Time.time = ������Ʈ ��� ���� �� ����� �ð� �� ������ ��ȯ�մϴ�.
		// Time.deltaTime = ������ �������� �Ϸ�� �� ����� �ð��� �� ������ ��ȯ�մϴ�.

		// ���콺 ��Ŭ�� ���� && Ÿ�̸� ����(���� ��� �ð��� ���� �߻� �ð����� �̻��� ���)
		if (Input.GetButton(InputStrings.Fire1) && Time.time >= nextTimeToShoot)
		{
			_animator.SetBool(AnimationStrings.Shoot, true);
			_animator.SetBool(AnimationStrings.Idle, false);
			nextTimeToShoot = Time.time + 1 / fireCharge; // ���� �ð�(�߻�) + 0.07
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

			// ���� �������� Gameobject�� Damageable Component�� �����´�.
			Damageable damageable = hitInfo.collider.GetComponent<Damageable>();
			if(damageable != null)
			{
				damageable.HitDamage(Damage);
			}
		}
	}

	// �ѱ� �ݵ����� ���� Spread ȿ�� ����
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
