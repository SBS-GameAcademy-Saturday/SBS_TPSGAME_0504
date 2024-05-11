using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootSteps : MonoBehaviour
{
	private AudioSource audioSource;

	[Header("FootStep Sounds")]
	[SerializeField] private AudioClip[] footstepSounds;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void Step()
	{
		audioSource.PlayOneShot(GetRandomFootStep());
	}

	// Aiming 상태 애니메이션애서 나올 SFX
	public void StepVolum()
	{
		audioSource.PlayOneShot(GetRandomFootStep(),0.2f);
	}

	private AudioClip GetRandomFootStep()
	{
		return footstepSounds[UnityEngine.Random.Range(0, footstepSounds.Length)];
	}
}
