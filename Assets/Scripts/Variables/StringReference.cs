using System;

[Serializable]
public class StringReference
{
	public bool UseStandard = false;
	public string StandardText;
	public StringVariable Reference;

	public StringReference() { }

	public StringReference(string text) {
		UseStandard = true;
		StandardText = text;
	}

	public string Text {
		get { return UseStandard ? StandardText : Reference.text; }
	}
	public static implicit operator string(StringReference reference) {
		return reference.Text;
	}
	public static implicit operator char[](StringReference reference) {
		return reference.Text.ToCharArray();
	}
}
