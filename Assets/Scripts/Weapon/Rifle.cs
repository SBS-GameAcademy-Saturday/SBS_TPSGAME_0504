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
	[SerializeField] private int maxAmmo = 30; // �ִ� ���� �Ѿ� ����
	[SerializeField] private int currentAmmo = 0; // ���� �Ѿ� ����
	[SerializeField] private int remainAmmo = 150; // ���� �Ѿ� ����
	[SerializeField] private float reloadingTime = 1.3f; // ������ �ð�
	[SerializeField] private bool setReloading = false; // ������ ������ �Ǵ��ϴ� ����

	[Header("Rifle VFX")]
	[SerializeField] private ParticleSystem muzzleSpark;
	[SerializeField] private GameObject impactEffect;
	[SerializeField] private GameObject goreEffect;
	[SerializeField] private GameObject droneEffect;

	[Header("Rifle SFX")]
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip shootingSound;

	// �Ѿ� ������ �ٲ� ������ ȣ�� �� �̺�Ʈ
	// �Ű������� ���� �Ѿ�, ���� �Ѿ� ������ �Ѱ��ݴϴ�.
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
		// ������ ���̸��� Update���� ������ ���Ѵ�.
		if (setReloading) return;

		// Time.time = ������Ʈ ��� ���� �� ����� �ð� �� ������ ��ȯ�մϴ�.
		// Time.deltaTime = ������ �������� �Ϸ�� �� ����� �ð��� �� ������ ��ȯ�մϴ�.
		if (Input.GetKeyDown(KeyCode.R))
		{
			StartCoroutine(Reload());
			return;
		}

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
		// ���� �Ѿ��� 0������ �����ִ� �Ѿ��� ������ ������
		if(currentAmmo <= 0 && remainAmmo > 0)
		{
			Debug.Log("���� �Ѿ��� �����ϴ�.");
			StartCoroutine(Reload());
			return;
		}

		// ���� �Ѿ��� ����, �����ִ� �Ѿ˵� ������ ����
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

			// ���� �������� Gameobject�� Damageable Component�� �����´�.
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

	// �ڷ�ƾ : 
	// �񵿱� ���α׷��� ��� �� �ϳ���, �ߴ������� ������ ���߰�
	// ���߿� �ߴ������� �ٽ� ������ ������ �� �ִ� �Լ��Դϴ�.
	// �ַ� �񵿱� �۾��� ���� ó���ϱ� ���ؼ� ���˴ϴ�.
	// IEnumerator,yield

	// ���� : �۾��� ���������� ó���ϸ�, �ϳ��� �۾��� �Ϸ�� ������ ���� �۾��� �������� �ʴ� ��
	// �񵿱� : �۾��� �Ϸ�� ������ ��ٸ��� �ʰ� ���� �۾��� ���ÿ� ó���մϴ�.I/O
	IEnumerator Reload()
	{
		playerController.SetCanWalk(false);
		setReloading = true;
		Debug.Log("Reloading");
		_animator.SetBool(AnimationStrings.Reload, true);
		// yield return null; // ���� �����ӱ��� ���;
		// ������ reloadingTime �ʱ��� �ڵ� ������ �ߴ��Ѵ�.
		yield return new WaitForSeconds(reloadingTime);
		// ������ reloadingTime�ʱ��� ��ٷ����� ���� �ڵ带 �����Ѵ�.
		_animator.SetBool(AnimationStrings.Reload, false);

		// �Ѿ� ���
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
