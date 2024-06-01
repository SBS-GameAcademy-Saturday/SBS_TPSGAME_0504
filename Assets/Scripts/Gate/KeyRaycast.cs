using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyRaycast : MonoBehaviour
{
	[Header("RayCast Radius and Layer")]
	[SerializeField] private int rayRadius = 6;
	[SerializeField] private LayerMask LayerMaskCollective;
	[SerializeField] private KeyCode getObjectButton = KeyCode.F;
	[SerializeField] private PlayerStates playerStates;

	private KeyObjectRegulator keyObject = null;
	private KeyGateRegulator keyGate = null;

	private bool Founded = false;
	private void LateUpdate()
	{
		RaycastHit hitInfo;

		// Vector.forward를 현 Transform의 월드 좌표계를 기준으로 설정합니다.
		Vector3 forwardDirection = transform.TransformDirection(Vector3.forward);

		// Vector.forward를 현 Transform의 로컬 좌표계를 기준으로 설정합니다.
		//Vector3 localForwardDirection = transform.InverseTransformDirection(Vector3.forward);

		if (Physics.Raycast(transform.position,forwardDirection,
			out hitInfo,rayRadius, LayerMaskCollective))
		{
			if (!Founded)
			{
				keyObject = hitInfo.collider.GetComponent<KeyObjectRegulator>();
				keyGate = hitInfo.collider.GetComponentInParent<KeyGateRegulator>();
				Founded = true;
				playerStates.ChangeCrosshair(true);
			}
			// 키 오브젝트를 찾았습니다.
			if (keyObject && Input.GetKeyDown(getObjectButton))
			{
				keyObject.FoundObject();
			}

			// 게이트 오브젝트를 찾았습니다.
			if(keyGate && Input.GetKeyDown(getObjectButton))
			{
				keyGate.ToggleGate();
			}
		}
		else
		{
			Founded = false;
			keyObject = null;
			playerStates.ChangeCrosshair(false);
		}
	}

}
