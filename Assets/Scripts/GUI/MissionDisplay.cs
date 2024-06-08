using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MissionDisplay : SingleTon<MissionDisplay>
{
	[Header("Missions")]
	[SerializeField] private TextMeshProUGUI mission1;
	[SerializeField] private TextMeshProUGUI mission2;
	[SerializeField] private TextMeshProUGUI mission3;
	[SerializeField] private TextMeshProUGUI mission4;

	private int mission3_Stack = 0;

	private bool mission1_Cleared = false;
	private bool mission2_Cleared = false;
	private bool mission3_Cleared = false;
	private bool mission4_Cleared = false;

	//// MissionDisplay Class의 객체를 정적 변수로 선언합니다.
	//public static MissionDisplay Instance;

	//// 해당 게임 오브젝트가 켜진 상태에서
	//// 씬에서 MissionDisplay를 사용할려고
	//// MissionDisplay 코드를 Compile하는 시점
	//private void Awake()
	//{
	//	// Instance를 Awake에서 자기 자신으로 할당을 시켜줍니다.
	//	Instance = this;
	//}

	public void CompleteMission(int missionIndex)
	{
		switch (missionIndex)
		{
			case 1:
				mission1.text = "1, Key Picked up";
				mission1.color = Color.green;
				mission1_Cleared = true;
				break;
			case 2:
				mission2.text = "2, Computer is Off";
				mission2.color = Color.green;
				mission2_Cleared = true;
				break;
			case 3:
				mission3_Stack += 1;
				if(mission3_Stack >= 2)
				{
					mission3.text = "3, Generators is Off";
					mission3.color = Color.green;
					mission3_Cleared = true;
				}
				break;
			case 4:
				mission4.text = "4, Mission Completed";
				mission4.color = Color.green;
				mission4_Cleared = true;
				break;
		}
	}

	public bool IsFinalMission()
	{
		return mission1_Cleared && mission2_Cleared && mission3_Cleared;
	}

}
