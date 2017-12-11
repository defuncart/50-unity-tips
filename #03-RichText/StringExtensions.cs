/*
 * 	Written by James Leahy (c) 2017 DeFunc Art.
 *	https://github.com/defuncart/
 */
using System;
using System.Text;
using UnityEngine;

/// <summary>A class of extention methods for string.</summary>
public static class StringExtensions
{
	/// <summary>Sets the color of each character of the string.</summary>
	/// <param name="value">The string.</param>
	/// <param name="color">The RichTextColor color.</param>
	public static string SetColor(this string value, RichTextColor color)
	{
		return value.SetColor(color.ToString());
	}

	/// <summary>Sets the color of each character of the string.</summary>
	/// <param name="value">The string.</param>
	/// <param name="color">The color as a HEX string.</param>
	public static string SetColor(this string value, string color)
	{
		return string.Format("<color={0}>{1}</color>", color, value);
	}

	/// <summary>Sets the color of a variable number of words.</summary>
	/// <param name="value">The string.</param>
	/// <param name="color">The RichTextColor color.</param>
	/// <param name="highlightedIndices">The variable number of word indicies.</param>
	public static string SetColorForWords(this string value, RichTextColor color, params int[] highlightedIndices)
	{
		return value.SetColorForWords(color.ToString(), highlightedIndices);
	}

	/// <summary>Sets the color of a variable number of words.</summary>
	/// <param name="value">The string.</param>
	/// <param name="color">The RichTextColor color.</param>
	/// <param name="highlightedIndices">The variable number of word indicies.</param>
	public static string SetColorForWords(this string value, string color, params int[] highlightedIndices)
	{
		string[] words = value.Split(' '); //split the string into an array of words
		StringBuilder sb = new StringBuilder();
		for(int i=0; i < words.Length; i++) //and recombine the words into a string with tags for each highlightedIndex
		{
			if(Array.IndexOf(highlightedIndices, i) > -1) //highlightedIndices contains i
			{
				sb.Append(words[i].SetColor(color));
			}
			else { sb.Append(words[i]); }
			sb.Append(' ');
		}
		return sb.ToString();
	}

	/// <summary>Sets the size of each character of the string in pixels.</summary>
	/// <param name="value">The string.</param>
	/// <param name="size">The size in pixels.</param>
	public static string SetSize(this string value, int size)
	{
		return string.Format("<size={0}>{1}</size>", size, value);
	}

	/// <summary>Sets the size in pixels of a variable number of words.</summary>
	/// <param name="value">The string.</param>
	/// <param name="size">The size in pixels.</param>
	/// <param name="highlightedIndices">The variable number of word indicies.</param>
	public static string SetSizeForWords(this string value, int size, params int[] highlightedIndices)
	{
		string[] words = value.Split(' '); //split the string into an array of words
		StringBuilder sb = new StringBuilder();
		for(int i=0; i < words.Length; i++) //and recombine the words into a string with tags for each highlightedIndex
		{
			if(Array.IndexOf(highlightedIndices, i) > -1) //highlightedIndices contains i
			{
				sb.Append(words[i].SetSize(size));
			}
			else { sb.Append(words[i]); }
			sb.Append(' ');
		}
		return sb.ToString();
	}

	/// <summary>Set the string to be boldface.</summary>
	/// <param name="value">The string.</param>
	public static string SetBold(this string value)
	{
		return string.Format("<b>{0}</b>", value);
	}

	/// <summary>Sets a variable number of words to be boldface.</summary>
	/// <param name="value">The string.</param>
	/// <param name="highlightedIndices">The variable number of word indicies.</param>
	public static string SetBoldForWords(this string value, params int[] highlightedIndices)
	{
		string[] words = value.Split(' '); //split the string into an array of words
		StringBuilder sb = new StringBuilder();
		for(int i=0; i < words.Length; i++) //and recombine the words into a string with tags for each highlightedIndex
		{
			if(Array.IndexOf(highlightedIndices, i) > -1) //highlightedIndices contains i
			{
				sb.Append(words[i].SetBold());
			}
			else { sb.Append(words[i]); }
			sb.Append(' ');
		}
		return sb.ToString();
	}

	/// <summary>Set the string to be italics.</summary>
	/// <param name="value">The string.</param>
	public static string SetItalics(this string value)
	{
		return string.Format("<i>{0}</i>", value);
	}

	/// <summary>Sets a variable number of words to be italics.</summary>
	/// <param name="value">The string.</param>
	/// <param name="highlightedIndices">The variable number of word indicies.</param>
	public static string SetItalicsForWords(this string value, params int[] highlightedIndices)
	{
		string[] words = value.Split(' '); //split the string into an array of words
		StringBuilder sb = new StringBuilder();
		for(int i=0; i < words.Length; i++) //and recombine the words into a string with tags for each highlightedIndex
		{
			if(Array.IndexOf(highlightedIndices, i) > -1) //highlightedIndices contains i
			{
				sb.Append(words[i].SetItalics());
			}
			else { sb.Append(words[i]); }
			sb.Append(' ');
		}
		return sb.ToString();
	}
}

/// <summary>A public enum of Colors for Rich Text tags built into Unity.</summary>
public enum RichTextColor
{
	/// <summary>aqua (same as cyan) #00ffffff</summary>
	aqua,
	/// <summary>black #000000ff</summary>
	black,
	/// <summary>blue #0000ffff</summary>
	blue,
	/// <summary>brown #a52a2aff</summary>
	brown,
	/// <summary>cyan (same as aqua) #00ffffff</summary>
	cyan,
	/// <summary>darkblue #0000a0ff</summary>
	darkblue,
	/// <summary>fuchsia (same as magenta) #ff00ffff</summary>
	fuchsia,
	/// <summary>green #008000ff</summary>
	green,
	/// <summary>grey #808080ff</summary>
	grey,
	/// <summary>lightblue #add8e6ff</summary>
	lightblue,
	/// <summary>lime #00ff00ff</summary>
	lime,
	/// <summary>magenta (same as fuchsia) #ff00ffff</summary>
	magenta,
	/// <summary>maroon #800000ff</summary>
	maroon,
	/// <summary>navy #000080ff</summary>
	navy,
	/// <summary>olive #808000ff</summary>
	olive,
	/// <summary>orange #ffa500ff</summary>
	orange,
	/// <summary>purple #800080ff</summary>
	purple,
	/// <summary>red #ff0000ff</summary>
	red,
	/// <summary>silver #c0c0c0ff</summary>
	silver,
	/// <summary>teal #008080ff</summary>
	teal,
	/// <summary>white #ffffffff</summary>
	white,
	/// <summary>yellow #ffff00ff</summary>
	yellow
}
