using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 제네릭(Generic)
// 데이터 형식에 의존하지 않고, 하나의 값이 여러 다른 데이터 타입들을 가질 수 있는 프로그래밍

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	// TODO : 제네릭(Generic) 설명
	public static T Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = FindObjectOfType<T>(true);
				if(_instance == null)
				{
					GameObject obj = new GameObject();
					obj.name = typeof(T).Name;
					_instance = obj.AddComponent<T>();
				}
			}
			return _instance;
		}
	}

	private void Awake()
	{
		// TODO : as is 키워드 설명
		_instance = this as T;
	}
}
