using UnityEngine;

[CreateAssetMenu]
public class StringVariable : ScriptableObject
{
#if UNITY_EDITOR
	[Multiline]
	public string DeveloperDescription = "";
#endif

	public string text = "";

	public void SetText(string text) {
		this.text = text;
	}
	public void SetText(StringVariable text) {
		this.text = text.text;
	}
}
