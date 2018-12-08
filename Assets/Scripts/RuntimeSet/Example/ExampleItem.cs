using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleItem : MonoBehaviour
{
	public ExampleSet set;

	private void OnEnable() {
		set.Add(this);
	}

	private void OnDisable() {
		set.Remove(this);
	}
}
