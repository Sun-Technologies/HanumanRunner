using UnityEditor;
using UnityEngine;
using Pokkt;
using System.Collections;

public class PokktConfigEditor : EditorWindow
{
	/**
	 * menu-items for configuring Ad Screen
	 **/
	[MenuItem("Pokkt/IGA/Add Hoarding")]
	static void AddIGAHoarding()
	{
		if (Selection.activeGameObject != null)
		{
			Debug.Log("adding IGA Hoarding to selected gameObject: " + Selection.activeGameObject.name);
			PokktIGAContainer.AddIGAHoarding(Selection.activeGameObject);
		}
		else
		{
			Debug.Log("Select a gameobject to add container to it!");
		}
	}

	[MenuItem("Pokkt/IGA/Add Banner")]
	static void AddIGABanner()
	{
		if (Selection.activeGameObject != null)
		{
			Debug.Log("adding IGA Banner to selected gameObject: " + Selection.activeGameObject.name);
			PokktIGAContainer.AddIGABanner(Selection.activeGameObject);
		}
		else
		{
			Debug.Log("Select a gameobject to add container to it!");
		}
	}

	[MenuItem("Pokkt/IGA/Add Floating Unit")]
	static void AddIGAFloatingUnit()
	{
		if (Selection.activeGameObject != null)
		{
			Debug.Log("adding IGA Floating Unit to selected gameObject: " + Selection.activeGameObject.name);
			PokktIGAContainer.AddIGAFloatingButton(Selection.activeGameObject);
		}
		else
		{
			Debug.Log("Select a gameobject to add container to it!");
		}
	}
}
