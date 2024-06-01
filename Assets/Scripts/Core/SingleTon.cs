using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���׸�(Generic)
// ������ ���Ŀ� �������� �ʰ�, �ϳ��� ���� ���� �ٸ� ������ Ÿ�Ե��� ���� �� �ִ� ���α׷���

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	// TODO : ���׸�(Generic) ����
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
		// TODO : as is Ű���� ����
		_instance = this as T;
	}
}
