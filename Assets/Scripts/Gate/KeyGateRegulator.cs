using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGateRegulator : MonoBehaviour
{
	[SerializeField] private KeyList keyList;
	[SerializeField] private float interactionTimer = 4f;
	private Animator animator;

	private bool pauseInteraction = false;
	private void Start()
	{
		animator = GetComponentInChildren<Animator>();
	}

	public void ToggleGate()
	{
		if (!keyList.hasKey)
		{
			return;
		}

		if (pauseInteraction) 
			return;

		if (animator.GetBool(AnimationStrings.Open))
			animator.SetBool(AnimationStrings.Open, false);
		else
			animator.SetBool(AnimationStrings.Open, true);
		pauseInteraction = true;
		Invoke(nameof(ResumeInteraction), interactionTimer);
	}

	private void ResumeInteraction()
	{
		pauseInteraction = false;
	}
}
