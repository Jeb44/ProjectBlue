using UnityEngine;

[CreateAssetMenu]
public class IntVariable : ScriptableObject 
{
#if UNITY_EDITOR
	[Multiline]
	public string DeveloperDescription = "";
#endif

	public int value;

	public void SetValue(int value) {
		this.value = value;
	}
	public void SetValue(IntVariable value) {
		this.value = value.value;
	}

	public void AddValue(int amount) {
		this.value += amount;
	}
	public void AddValue(IntVariable amount) {
		this.value += amount.value;
	}
}
