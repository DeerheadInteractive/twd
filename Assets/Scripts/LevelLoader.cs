using UnityEngine;
using System.Collections;
using System.IO;

/// <summary>
/// Loads a level via a level text file in Assets/LevelData.
/// Uses StreamReader, though TextAsset is recommended.
/// </summary>
public class LevelLoader : MonoBehaviour {
	private static string levelPrefix = "Assets/LevelData/";
	private static string levelSuffix = ".txt";
	public string levelName;
	public GameObject wallObj;
	public GameObject groundObj;
	public float groundHeight;

	private const int _WALL = 1;

	void Start () {
		if (Load(levelPrefix + levelName + levelSuffix))
			print ("Loaded " + levelName + " successfully.");
		else
			print ("Error loading level " + levelName + ".");
	}

	private bool Load(string fileName){
		try
		{
			Tokenizer tokenizer = new Tokenizer("");
			StreamReader reader = new StreamReader(fileName);
			using (reader)
			{
				tokenizer.ResetWithString(reader.ReadLine());
				int width = int.Parse(tokenizer.nextToken());
				int height = int.Parse(tokenizer.nextToken());

				int min = 0;
				int[,] grid = new int[width, height];
				for (int x = 0; x < width; ++x){
					tokenizer.ResetWithString(reader.ReadLine());
					for (int y = 0; y < height; ++y){
						grid[x, y] = int.Parse(tokenizer.nextToken());
						if (grid[x, y] < min){
							min = grid[x, y];
						}
					}
				}

				// ------ Creating objects -------------------------

				// Create floor
				GameObject floor = Instantiate(groundObj, new Vector3(width / 2, groundHeight, height / 2), Quaternion.identity) as GameObject;
				floor.transform.localScale = new Vector3(width, 1, height);

				Vector3[] vec3waypoints = new Vector3[-min];

				// Go through grid and create other GameObjects (like walls)
				for (int x = 0; x < width; ++x){
					for (int y = 0; y < height; ++y){
						int val = grid[x, y];
						if (val < 0){
							vec3waypoints[-(val + 1)] = new Vector3(x, 0, y);
						} else{
							switch (val){
							case _WALL:
								GameObject wall = Instantiate(wallObj, new Vector3(x, 0, y), Quaternion.identity) as GameObject;
								break;
							}
						}
					}
				}

				// Create waypoints queue from array.
				Queue waypoints = new Queue();
				for (int i = 0; i < -min; ++i){
					waypoints.Enqueue(vec3waypoints[i]);
				}

				reader.Close();
				return true;
			}
		} catch (IOException e){
			print (e.Message);
			return false;
		}
	}

	/// <summary>
	/// Class used to tokenize strings, assuming whitespace delimiter.
	/// </summary>
	public class Tokenizer{
		public string str;
		private int curIndex;
		private int len;
		public Tokenizer(string str){
			this.str = str;
			len = str.Length;
			curIndex = 0;
		}
		public void ResetWithString(string str){
			this.str = str;
			len = str.Length;
			curIndex = 0;
		}
		public string nextToken(){
			// Advance to next non-whitespace char
			while (curIndex < len && char.IsWhiteSpace(str[curIndex])){
				curIndex++;
			}

			// Advance to next whitespace char
			int i = curIndex;
			int first = curIndex;
			while (i < len && !(char.IsWhiteSpace(str[i]))){
				i++;
			}
			curIndex = i; // Move curIndex to last searched char.
			return str.Substring(first, (i - first));
		}
	}

	/**
	 * Level files should look like this:
	 * n m		// Where n and m are integers, and (n, m) is the size of the level grid.
	 * ... i ...
	 * .
	 * .
	 * j		// The level grid, where each integer represents something (wall, path node, etc.)
	 * .		// 0 = empty, 1 = wall, (we can distinguish 2 = wall that can be turned into tower?)
	 * .		// Negative numbers will indicate path node order. (eg. monsters will start at -1
	 * .		// and move to -2, -3, etc. wherever they are).
	 * 
	 * 
	 */
}
