#pragma warning disable

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// Creates a popup window that lets you replace all selected objects with a new object or prefab.
/// </summary>
public class ReplaceSelection : ScriptableWizard
{
	static GameObject replacement = null;
	static bool keep = false;
	
	public GameObject ReplacementObject = null;
	public bool KeepOriginals = false;
	
	[MenuItem("Tools/PHL/Replace Selection")]
	public static void CreateWizard()
	{
		DisplayWizard("Replace Selection", typeof(ReplaceSelection), "Replace");
	}
	
	public ReplaceSelection()
	{
		ReplacementObject = replacement;
		KeepOriginals = keep;
	}
	
	void OnWizardUpdate()
	{
		replacement = ReplacementObject;
		keep = KeepOriginals;
	}
	
	void OnWizardCreate()
	{
        if (replacement == null)
        {
            return;
        }

		Undo.RegisterSceneUndo("Replace Selection");
		
		Transform[] selectedTransforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.Editable);

		List<Scene> affectedScenes = new List<Scene>();

		foreach (Transform selectedTransform in selectedTransforms)
		{
            GameObject newObject = null;
			PrefabType prefabType = EditorUtility.GetPrefabType(replacement);
			
			if (prefabType == PrefabType.Prefab || prefabType == PrefabType.ModelPrefab)
			{
                if(selectedTransform.parent == null)
                {
                    newObject = (GameObject)PrefabUtility.InstantiatePrefab(replacement, selectedTransform.gameObject.scene);
                }
                else
                {
                    newObject = (GameObject)PrefabUtility.InstantiatePrefab(replacement, selectedTransform.parent);
                }
			}
			else
			{
                if (selectedTransform.transform.parent == null)
                {
                    newObject = (GameObject)Editor.Instantiate(replacement);
                    EditorSceneManager.MoveGameObjectToScene(newObject, selectedTransform.gameObject.scene);
                }
                else
                {
                    newObject = (GameObject)Editor.Instantiate(replacement, selectedTransform.parent);
                }
			}
			
			Transform newTransform = newObject.transform;
			newTransform.parent = selectedTransform.parent;
			newObject.name = replacement.name;
			newTransform.localPosition = selectedTransform.localPosition;
			newTransform.localScale = selectedTransform.localScale;
			newTransform.localRotation = selectedTransform.localRotation;

			if(!affectedScenes.Contains(newObject.scene))
            {
				affectedScenes.Add(newObject.scene);
			}
		}

		if (!keep)
		{
			foreach (GameObject g in Selection.gameObjects)
			{
				GameObject.DestroyImmediate(g);
			}
		}

		foreach(Scene scene in affectedScenes)
		{
			EditorSceneManager.MarkSceneDirty(scene);
		}
	}
}