using System;

[Serializable]
public class IntReference
{
	public bool UseStandard = true;
	public int StandardValue;
	public IntVariable Reference;

	public IntReference() { }

	public IntReference(int value) {
		UseStandard = true;
		StandardValue = value;
	}

	public int Value {
		get { return UseStandard ? StandardValue : Reference.value; }
	}

	public static implicit operator int(IntReference reference) {
		return reference.Value;
	}
	public static implicit operator string(IntReference reference) {
		return reference.Value.ToString();
	}
}
