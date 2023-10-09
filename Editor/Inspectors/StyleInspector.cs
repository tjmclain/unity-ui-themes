using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(Style), true)]
public class StyleInspector : Editor
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
			bool delete = GUILayout.Button("—", GUILayout.ExpandWidth(false));

			EditorGUILayout.EndHorizontal();
			if (!element.isExpanded)
			{
				continue;
			}

			EditorGUILayout.BeginVertical(EditorStyles.textArea);

			EditorGUI.indentLevel++;

			var editor = CreateEditor(asset);
			editor.OnInspectorGUI();

			EditorGUILayout.EndVertical();

			EditorGUI.indentLevel--;

			if (delete)
			{
				Undo.DestroyObjectImmediate(asset);

				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

				RefreshProperties();

				Repaint();

				break;
			}
		}

		EditorGUI.indentLevel--;

		// Add Property button
		var themeStyle = target as Style;
		var propertyNames = themeStyle.PropertyDefinitions.Keys
			.Where(x => !themeStyle.Properties.Exists(y => y.name == x))
			.ToArray();

		if (propertyNames.Length == 0)
		{
			return;
		}

		_selectedPropertyIndex = Mathf.Clamp(_selectedPropertyIndex, 0, propertyNames.Length - 1);

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

		if (target is not Style themeStyle)
		{
			Debug.LogError($"{nameof(target)} is not {nameof(Style)}", target);
			return;
		}

		if (!themeStyle.PropertyDefinitions.TryGetValue(propertyName, out var propertyType))
		{
			Debug.LogError($"!{nameof(Style.PropertyDefinitions)}.TryGetValue '{propertyName}'", target);
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