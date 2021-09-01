﻿using System.Collections.Generic;
using Plugins.GameService.Tools.NaughtyAttributes.Scripts.Core.DrawerAttributes_SpecialCase;
using Plugins.GameService.Tools.NaughtyAttributes.Scripts.Editor.Utility;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Plugins.GameService.Tools.NaughtyAttributes.Scripts.Editor.PropertyDrawers_SpecialCase
{
	public class ReorderableListPropertyDrawer : SpecialCasePropertyDrawerBase
	{
		public static readonly ReorderableListPropertyDrawer Instance = new ReorderableListPropertyDrawer();

		private readonly Dictionary<string, ReorderableList> _reorderableListsByPropertyName = new Dictionary<string, ReorderableList>();

		private string GetPropertyKeyName(SerializedProperty property)
		{
			return property.serializedObject.targetObject.GetInstanceID() + "/" + property.name;
		}

		protected override void OnGUI_Internal(SerializedProperty property, GUIContent label)
		{
			if (property.isArray)
			{
				string key = GetPropertyKeyName(property);

				if (!_reorderableListsByPropertyName.ContainsKey(key))
				{
					ReorderableList reorderableList = new ReorderableList(property.serializedObject, property, true, true, true, true)
					{
						drawHeaderCallback = (Rect rect) =>
						{
							EditorGUI.LabelField(rect, string.Format("{0}: {1}", label.text, property.arraySize), EditorStyles.boldLabel);
						},

						drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
						{
							SerializedProperty element = property.GetArrayElementAtIndex(index);
							rect.y += 1.0f;
							rect.x += 10.0f;
							rect.width -= 10.0f;

							EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, 0.0f), element, true);
						},

						elementHeightCallback = (int index) =>
						{
							return EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(index)) + 4.0f;
						}
					};

					_reorderableListsByPropertyName[key] = reorderableList;
				}

				_reorderableListsByPropertyName[key].DoLayoutList();
			}
			else
			{
				string message = typeof(ReorderableListAttribute).Name + " can be used only on arrays or lists";
				NaughtyEditorGUI.HelpBox_Layout(message, MessageType.Warning, context: property.serializedObject.targetObject);
				EditorGUILayout.PropertyField(property, true);
			}
		}

		public void ClearCache()
		{
			_reorderableListsByPropertyName.Clear();
		}
	}
}
