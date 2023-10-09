using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.Android;
using Unity.VisualScripting.YamlDotNet.Serialization.TypeInspectors;

[CustomEditor(typeof(ThemeStyle), true)]
public class ThemeStyleInspector : Editor
{
	private int _selectedPropertyIndex = -1;

	private Object[] GetSubAssets()
	{
		string assetPath = AssetDatabase.GetAssetPath(target);
		return AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
	}

	private SerializedProperty GetPropertiesListProperty()
	{
		return serializedObject.FindProperty("_properties");
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		DrawPropertiesList();
		DrawAddPropertyGui();

		serializedObject.ApplyModifiedProperties();
	}

	private void OnEnable()
	{
		RefreshProperties();
	}

	private void DrawPropertiesList()
	{
		EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);
		var propertiesList = GetPropertiesListProperty();

		EditorGUI.indentLevel++;
		for (int i = 0; i < propertiesList.arraySize; i++)
		{
			var element = propertiesList.GetArrayElementAtIndex(i);
			var asset = element.objectReferenceValue;

			if (asset == null)
			{
				propertiesList.DeleteArrayElementAtIndex(i);
				i--;
				continue;
			}

			EditorGUILayout.BeginHorizontal();
			element.isExpanded = EditorGUILayout.Foldout(element.isExpanded, asset.name);

			if (GUILayout.Button("—", GUILayout.ExpandWidth(false)))
			{
				// TODO: delete asset
			}
			EditorGUILayout.EndHorizontal();
			if (!element.isExpanded)
			{
				continue;
			}

			EditorGUI.indentLevel++;

			var editor = CreateEditor(asset);
			editor.OnInspectorGUI();

			EditorGUI.indentLevel--;
		}

		EditorGUI.indentLevel--;
	}

	//private void DrawPropertiesList()
	//{
	//	static bool ArrayPropertyContains(SerializedProperty property, Object obectReference)
	//	{
	//		for (int i = 0; i < property.arraySize; i++)
	//		{
	//			var element = property.GetArrayElementAtIndex(i);
	//			if (element.objectReferenceValue == obectReference)
	//			{
	//				return true;
	//			}
	//		}

	//		return false;
	//	}

	//	var propertiesList = GetPropertiesListProperty();

	//	EditorGUI.BeginChangeCheck();
	//	EditorGUILayout.PropertyField(propertiesList);
	//	if (!EditorGUI.EndChangeCheck())
	//	{
	//		return;
	//	}

	//	var propertyAssets = GetSubAssets();

	//	foreach (var asset in propertyAssets)
	//	{
	//		if (ArrayPropertyContains(propertiesList, asset))
	//		{
	//			continue;
	//		}

	//		Undo.DestroyObjectImmediate(asset);
	//	}

	//	AssetDatabase.SaveAssets();
	//	AssetDatabase.Refresh();

	//	RefreshProperties();

	//	Repaint();
	//}

	private void DrawAddPropertyGui()
	{
		if (target is not ThemeStyle themeStyle)
		{
			Debug.LogWarning($"{nameof(target)} is not {nameof(ThemeStyle)}", target);
			return;
		}

		var propertyNames = themeStyle.PropertyDefinitions.Keys
			.Where(x => !themeStyle.PropertyNames.Contains(x))
			.ToArray();

		if (propertyNames.Length == 0)
		{
			return;
		}

		_selectedPropertyIndex = Mathf.Clamp(_selectedPropertyIndex, 0, propertyNames.Length - 1);

		EditorGUILayout.LabelField("Add Property", EditorStyles.boldLabel);
		EditorGUILayout.BeginHorizontal();
		{
			_selectedPropertyIndex = EditorGUILayout.Popup(_selectedPropertyIndex, propertyNames);
			if (GUILayout.Button("Add", GUILayout.ExpandWidth(false)))
			{
				string propertyName = propertyNames[_selectedPropertyIndex];
				AddProperty(propertyName);
			}
		}
		EditorGUILayout.EndHorizontal();
	}

	private void AddProperty(string propertyName)
	{
		if (string.IsNullOrEmpty(propertyName))
		{
			Debug.LogError($"{nameof(propertyName)} is null or empty", target);
			return;
		}

		if (target is not ThemeStyle themeStyle)
		{
			Debug.LogError($"{nameof(target)} is not {nameof(ThemeStyle)}", target);
			return;
		}

		if (!themeStyle.PropertyDefinitions.TryGetValue(propertyName, out var propertyType))
		{
			Debug.LogError($"!{nameof(ThemeStyle.PropertyDefinitions)}.TryGetValue '{propertyName}'", target);
			return;
		}

		if (!typeof(StyleProperty).IsAssignableFrom(propertyType))
		{
			Debug.LogError($"{propertyType} is not subclass of {nameof(StyleProperty)}");
			return;
		}

		var property = CreateInstance(propertyType) as StyleProperty;
		property.name = propertyName;

		Undo.RegisterCreatedObjectUndo(property, "Add StyleProperty");
		AssetDatabase.AddObjectToAsset(property, target);

		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		RefreshProperties();

		Repaint();
	}

	private void RefreshProperties()
	{
		var properties = GetSubAssets()
			.Select(x => x as StyleProperty)
			.ToArray();

		var propertiesList = GetPropertiesListProperty();
		propertiesList.arraySize = properties.Length;

		for (int i = 0; i < properties.Length; i++)
		{
			var propertyListElement = propertiesList.GetArrayElementAtIndex(i);
			propertyListElement.objectReferenceValue = properties[i];
		}

		serializedObject.ApplyModifiedProperties();
	}
}