using System;
using UnityEngine;


/// <summary>
/// Request messages to Unity console.
/// </summary>
public class UnityConsoleLogger : ILink {

	public UnityConsoleLogger(LinkLevel mask)
		: base(mask) {	}

	protected override void HandleRequest(string message) {
		Debug.Log(message);
	}
}


/// <summary>
/// Request messages into files.
/// </summary>
public class FileLogger : ILink {

	const string dateTimeFormat = "yyyy-MM-dd_HH-mm-ss";

	private string path;
	/// <summary>
	/// Where the file should be saved. Note: The current date+time is added on the path name. Please get the path name again, when you want to reuse the file.
	/// </summary>
	public string Path {
		get { return path; }
		set {
			//Put the current date & time between path and file
			int index = value.LastIndexOf('/');
			if(index == -1) {
				throw new ArgumentException("FileLogger: Path (Setter) must have a '/' inside it's string.");
			}

			string newStart = value.Substring(0, index + 1);
			string newEnd = value.Substring(index + 1);

			path = newStart + "[" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "] " + newEnd;
		}
	}

	public FileLogger(LinkLevel mask, string path)
		: base(mask) {
		Path = path;
	}

	protected override void HandleRequest(string message) {
		TextFile.WriteString(message, Path);
	}
}

/// <summary>
/// Request messages to Ingame console.
/// </summary>
public class IngameConsoleLogger : ILink {

	public IngameConsoleLogger(LinkLevel mask)
		: base(mask) {
		//add stuff
	}

	protected override void HandleRequest(string message) {
		//TODO: add text to the ingame console
	}
}
