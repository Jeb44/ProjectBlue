using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleReferences : MonoBehaviour
{
	public FloatReference floatValue;
	public IntReference intValue;
	public StringReference stringValue;

	private void Start()
    {
		Debug.Log("String -> " + stringValue);
		Debug.Log("Int -> " + intValue);
		Debug.Log("Float-> " + floatValue);
	}
}
