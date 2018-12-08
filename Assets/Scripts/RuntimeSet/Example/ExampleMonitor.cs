using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleMonitor : MonoBehaviour
{
	public ExampleSet set;

	private int prevCount = -1;
	private string text;

	private void OnEnable() {
		//UpdateText();
	}

	private void Update() {
		if(prevCount != set.items.Count) {
			UpdateText();
			prevCount = set.items.Count;
		}
	}

	public void UpdateText() {
		Debug.Log("Items Count: " + set.items.Count);
	}

	public void SelectRandom() {
		int index = Random.Range(0, set.items.Count);
		Debug.Log(set.items[index].ToString());
	}

	public void SelectAll() {
		for(int i = set.items.Count -1; i >= 0; i--) {
			Debug.Log(set.items[i].ToString());
		}
	}
}
