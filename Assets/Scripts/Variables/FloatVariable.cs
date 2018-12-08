using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject
{
#if UNITY_EDITOR
	[Multiline]
	public string DeveloperDescription = "";
#endif

	public float value;
	
	public void SetValue(float value) {
		this.value = value;
	}
	public void SetValue(FloatVariable value) {
		this.value = value.value;
	}

	public void AddValue(float amount) {
		this.value += amount;
	}
	public void AddValue(FloatVariable amount) {
		this.value += amount.value;
	}
}
