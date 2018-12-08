using System.IO;
using UnityEngine;
using UnityEditor;


public struct Paths {
	public const string PathToLogFiles = "Assets/Log Files";
}

public static class TextFile {		

	[MenuItem("Tools/Open log file folder")]
	static void OpenLogFileFolder() {
		EditorUtility.RevealInFinder(Paths.PathToLogFiles);
	}

	public static void WriteString(string message, string path, bool append = true, bool reload = true) {

		StreamWriter writer = new StreamWriter(path, append);
		writer.WriteLine(message);
		writer.Close();

		//Re-import the file to update the reference in the editor
		AssetDatabase.ImportAsset(path);
		Resources.Load(path);
	}

	public static string ReadString(string path) {
		StreamReader reader = new StreamReader(path);
		string text = reader.ReadToEnd();
		reader.Close();

		return text;
	}
}


