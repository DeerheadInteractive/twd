﻿using UnityEngine;
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
	public GameObject invisWallObj;
	public GameObject bridgeObj;
	public GameObject groundObj;
	public float groundHeight;
	public float bridgeHeight;
	public float playerHeight;

	public GameObject portalStart;
	public GameObject portalEnd;

	private const int _WALL = 1;
	private const int _BRIDGE = 2;

	void Start () {
		if (Load(levelPrefix + levelName + levelSuffix)){
			print ("Loaded " + levelName + " successfully.");
			GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
			gc.updateMonsterCount(0);
		} else{
			print ("Error loading level " + levelName + ".");
		}
	}

	private bool Load(string fileName){
		try
		{
			Tokenizer tokenizer = new Tokenizer("");
			StreamReader reader = new StreamReader(fileName);
			using (reader)
			{
				tokenizer.ResetWithString(reader.ReadLine());

				// Read dimensions of level
				int width = tokenizer.nextInt();
				int height = tokenizer.nextInt();

				// Read level data (waypoints, walls, towers, maybe bridges?)
				int min = 0;
				int[,] grid = new int[width, height];
				for (int x = 0; x < width; ++x){
					tokenizer.ResetWithString(reader.ReadLine());
					for (int y = 0; y < height; ++y){
						grid[x, y] = tokenizer.nextInt();
						if (grid[x, y] < min){
							min = grid[x, y];
						}
					}
				}

				// Read in wave data.
				tokenizer.ResetWithString(reader.ReadLine());
				Queue waveInfo = new Queue();
				do {
					tokenizer.ResetWithString(reader.ReadLine());
					int m = tokenizer.nextInt();
					if (m == -1)
						break;
					Queue curWave = new Queue();
					int n = tokenizer.nextInt();
					float t = tokenizer.nextFloat();
					Vector3 entry = new Vector3(m, n, t);
					curWave.Enqueue(entry);
					do {
						tokenizer.ResetWithString(reader.ReadLine());
						m = tokenizer.nextInt();
						if (m == -1)
							break;
						n = tokenizer.nextInt();
						t = tokenizer.nextFloat();
						entry = new Vector3(m, n, t);
						curWave.Enqueue(entry);
					} while (true);
					waveInfo.Enqueue(curWave);
				} while (true);


				// ------ Creating objects -------------------------
				
				// Create floor
				GameObject floor = Instantiate(groundObj, new Vector3(width / 2 - 0.5f, groundHeight, height / 2 - 0.5f), Quaternion.identity) as GameObject;
				floor.transform.localScale = new Vector3(width, 1, height);
				
				Vector3[] waypoints = new Vector3[-min];
				
				// Go through grid and create other GameObjects (like walls)
				for (int x = 0; x < width; ++x){
					for (int y = 0; y < height; ++y){
						int val = grid[x, y];
						if (val < 0){
							waypoints[-(val + 1)] = new Vector3(x, 0, y);
						}
						switch (val){
						case _WALL:
							Instantiate(wallObj, new Vector3(x, 0, y), Quaternion.identity);
							break;
						case _BRIDGE:
							GameObject bridgeInstance = Instantiate(bridgeObj, new Vector3(x, bridgeHeight, y), new Quaternion(0,0,0,0)) as GameObject;
							bridgeInstance.transform.Rotate(90, 0, 0);
							break;
						default:
							Instantiate(invisWallObj, new Vector3(x, playerHeight, y), Quaternion.identity);
							break;
						}
					}
				}
				// Create invisible wall border
				for (int x = -1; x < width + 1; ++x){
					Instantiate(invisWallObj, new Vector3(x, playerHeight, -1), Quaternion.identity);
					Instantiate(invisWallObj, new Vector3(x, playerHeight, height), Quaternion.identity);
				}
				for (int y = 0; y < height; ++y){
					Instantiate(invisWallObj, new Vector3(-1, playerHeight, y), Quaternion.identity);
					Instantiate(invisWallObj, new Vector3(width, playerHeight, y), Quaternion.identity);
				}
				
				// Create portals
				Instantiate(portalStart, waypoints[0], Quaternion.identity);
				Instantiate(portalEnd, waypoints[-(min + 1)], Quaternion.identity);
				
				// Give waypoint and waveinfo data to game controller.
				GameObject obj = GameObject.FindGameObjectWithTag("GameController");
				if (obj == null){
					print ("No game controller found during level loading.");
				} else{
					GameController gc = obj.GetComponent<GameController>();
					gc.waypoints = waypoints;
					gc.waveInfo = waveInfo;
					Camera cam = Camera.main;
					cam.transform.position = new Vector3(width / 2, GameController.CAMERA_HEIGHT, height / 2);
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
		public int nextInt(){
			return int.Parse(nextToken());
		}
		public float nextFloat(){
			return float.Parse(nextToken());
		}
	}

	/**
	 * Apparently you may place empty lines wherever you want.
	 * Dashed lines (---) are not actually present in the level file; they
	 * are just here for readability. Don't put those in, because that WILL break the level loader.
	 * 
	 * NO NON-NUMERIC SYMBOLS (except '-' and '.') ALLOWED! Those are characters, not faces.
	 * You may write things after each line's expected input, however.
	 * Eg.
	 * 3 3
	 * 0 0 0  Comments here
	 * 0 0 0  are perfectly
	 * 0 0 0  okay. But don't get carried away!
	 * (etc.)
	 * 
	 * Level files should look like this:
	 * n m       // Where n and m are integers, and (n, m) is the size of the level grid.
	 * ... i ...
	 * .........
	 * ......... // The level grid, where each integer represents something (wall, path node, etc.)
	 * j.......j // 0 = empty, 1 = wall, (we can distinguish 2 = wall that can be turned into tower?)
	 * ......... // Negative numbers will indicate path node order. (eg. monsters will start at -1
	 * ......... // and move to -2, -3, etc. wherever they are).
	 * ... i ...
	 * ---------------------------------- // Wave descriptions:
	 * m n t     // m = Monster index, n = how many of said monsters, t = time delay between monsters of this batch.
	 * -1        // End wave
	 * -1        // End wave description
	 * 
	 * Choose your monster indices based on where the monster appears in the GameController's list of monsters.
	 * Please avoid rearranging monsters in said list, because all levels depend on the same ordering of monsters.
	 */
}
