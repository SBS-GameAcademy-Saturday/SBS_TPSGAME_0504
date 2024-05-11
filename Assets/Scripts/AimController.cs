using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
	public Cinemachine.AxisState xAxis, yAxis;
	[SerializeField] private Transform camFollow;

	private void Update()
	{
		xAxis.Update(Time.deltaTime);
		yAxis.Update(Time.deltaTime);
	}

	private void LateUpdate()
	{
		camFollow.localEulerAngles =
			new Vector3(
			-yAxis.Value,
			camFollow.localEulerAngles.y, 
			camFollow.localEulerAngles.z
			);
		transform.eulerAngles =
			new Vector3(
			transform.eulerAngles.x, 
			xAxis.Value, 
			transform.eulerAngles.z
			);
	}

}
