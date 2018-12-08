public enum LinkLevel {
	None = 0,			//0000
	Info = 1,			//0001
	Debug = 2,			//0010
	Warning = 4,		//0100
	Error = 8,			//1000
	All = 15			//1111
}

/// <summary>
/// Interface for the Chain of Responsibility Pattern.
/// </summary>
public abstract class ILink {

	#region Properties
	protected LinkLevel mask;
	public LinkLevel Mask {
		get { return mask; }
		set { mask = value; }
	}

	protected ILink next;
	public ILink Next {
		get { return next; }
		set { next = value; }
	}
	#endregion

	#region Constructors
	public ILink(LinkLevel mask) {
		this.mask = mask;
	}

	public ILink(LinkLevel mask, ILink next) {
		this.mask = mask;
		this.next = next;
	}
	#endregion

	public ILink SetNext(ILink next) {
		this.next = next;
		return next;
	}

	#region Chain of Responsibility -> Main Function
	/// <summary>
	/// Handle the request when severity fits with the mask and continue the chain, if another part is there.
	/// </summary>
	/// <param name="message">String that should be written somewhere.</param>
	/// <param name="severity"></param>
	public void Request(string message, LinkLevel severity) {
		if((severity & mask) != 0) {
			HandleRequest(message);
		}
		if(next != null) {
			next.Request(message, severity);
		}
	}

	/// <summary>
	/// Handle the request. This should be overwritten when writing your own Link class.
	/// </summary>
	/// <param name="message">String that should be written somewhere.</param>
	abstract protected void HandleRequest(string message);
	#endregion
}
