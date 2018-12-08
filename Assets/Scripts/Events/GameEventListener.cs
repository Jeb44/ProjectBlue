using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
	[Tooltip("Event to register with.")]
	public GameEvent gameEvent;

	[Tooltip("Response to invoke when Event is raised.")]
	public UnityEvent response;

	public virtual void OnEventRaised() {
		response.Invoke();
	}

	private void OnEnable() {
		gameEvent.Register(this);
	}

	private void OnDisable() {
		gameEvent.Unregister(this);
	}
}
