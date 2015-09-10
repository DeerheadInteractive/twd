/// <summary>
/// Class used to tokenize strings. Can be given a string of delimiting characters.
/// All whitespace characters are assumed to be delimiters.
/// </summary>
using System;
using UnityEngine;


public class Tokenizer{
	private string str;
	private string delimiters;
	private int curIndex;
	private int len;

	public Tokenizer(){
		setup("", "");
	}

	public Tokenizer(string str){
		setup(str, "");
	}

	public Tokenizer(string str, string delimiters){
		setup(str, delimiters);
	}

	/// <summary>
	/// Initializes this tokenizer with the given string and delimiters.
	/// </summary>
	/// <param name="str">String.</param>
	/// <param name="delimiters">Delimiters.</param>
	private void setup(string str, string delimiters){
		this.delimiters = delimiters;
		this.str = str;
        len = 0;
        if (str != null){
		    len = str.Length;
        }
		curIndex = 0;
	}

	public void resetWithString(string str){
		this.str = str;
		len = str.Length;
		curIndex = 0;
	}

	/// <summary>
	/// Nexts the token.
	/// </summary>
	/// <returns>The next token, or an empty string if no tokens remain.</returns>
	public string nextToken(){
		// Advance to next non-whitespace char
		while (curIndex < len && isDelimiter(str[curIndex])){
			curIndex++;
		}
		
		// Advance to next whitespace char
		int i = curIndex;
		int first = curIndex;
		while (i < len && !(isDelimiter(str[i]))){
			i++;
		}
		curIndex = i; // Move curIndex to last searched char.
		return str.Substring(first, (i - first));
	}

    /// <summary>
    /// Returns the next token without advancing past it.
    /// </summary>
    public string peek(){
        int temp = curIndex;
        string toReturn = nextToken();
        curIndex = temp;
        return toReturn;
    }

	/// <summary>
	/// Returns true if this tokenizer has another token.
	/// </summary>
	/// <returns><c>true</c>, if tokenizer has another token, <c>false</c> otherwise.</returns>
	public bool hasNext(){
		int temp = curIndex;
		while (temp < len && isDelimiter(str[temp])){
			temp++;
		}
		return temp != len;
	}

	public int nextInt(){
        string str = nextToken();
        int toReturn = 0;
        try {
            toReturn = int.Parse(str);
        } catch (FormatException e){
            Debug.Log("Could not parse this token as an int: " + str);
        }
        return toReturn;
		//return int.Parse(nextToken());
	}

	public float nextFloat(){
		return float.Parse(nextToken());
	}

	/// <summary>
	/// Returns the rest of the buffer as one string.
	/// </summary>
	/// <returns>The rest of the buffer.</returns>
	public string nextTokenToEnd(){
		return str.Substring(curIndex, len - curIndex);
	}

	/// <summary>
	/// Whether or not the given character is part of the delimiting set of characters.
	/// </summary>
	/// <returns><c>true</c>, if the given character is a delimiter, <c>false</c> otherwise.</returns>
	/// <param name="c">C.</param>
	private bool isDelimiter(char c){
		if (char.IsWhiteSpace(c)){
			return true;
		}
		for (int i = 0; i < delimiters.Length; ++i){
			if (delimiters[i] == c){
				return true;
			}
		}
		return false;
	}
}