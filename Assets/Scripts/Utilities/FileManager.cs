using UnityEngine;
using System.Collections;
using System.IO;

/// <summary>
/// Manages file / resource loading and saving.
/// </summary>
public class FileManager{
	public const string TOWER_PREFIX = "TowerData/";
	public const string LEVEL_PREFIX = "LevelData/";

	/// <summary>
	/// Loads the given text asset resource into a stream. If the resource cannot be
	/// found, throws a <c>FileNotFoundException</c>.
	/// </summary>
	/// <returns>A stream loaded from the given resource.</returns>
	/// <param name="name">Name of the resource to load.</param>
	public static Stream loadTextStream(string name){
		TextAsset textAsset = Resources.Load(name) as TextAsset;
		if (textAsset == null){
			throw new FileNotFoundException("Error: Could not load resource: " + name);
		}
		MemoryStream stream = new MemoryStream();
		StreamWriter writer = new StreamWriter(stream);
		writer.Write(textAsset.text);
		writer.Flush();
		stream.Position = 0;
		return stream;
	}
}