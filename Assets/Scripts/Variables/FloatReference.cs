using System;

[Serializable]
public class FloatReference
{
	public bool UseStandard = true;
	public float StandardValue;
	public FloatVariable Reference;

	public FloatReference() { }

	public FloatReference(float value) {
		UseStandard = true;
		StandardValue = value;
	}

	public float Value {
		get { return UseStandard ? StandardValue : Reference.value; }
	}

	

	public static implicit operator float(FloatReference reference) {
		return reference.Value;
	}
	public static implicit operator string(FloatReference reference) {
		return reference.Value.ToString();
	}
}
