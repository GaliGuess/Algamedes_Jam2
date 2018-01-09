using UnityEditor;
using UnityEngine;
using Game;

[CustomEditor(typeof(PlatformManager))]
public class PlatformInspector : Editor {

	SerializedProperty cycle_period;
	SerializedProperty points;
	SerializedProperty platform_framework;

	void OnEnable()
	{
		cycle_period = serializedObject.FindProperty("cycle_period");
		points = serializedObject.FindProperty("points");
		platform_framework = serializedObject.FindProperty("platform_framework");
	}


	public override void OnInspectorGUI()
	{
		PlatformManager myTarget = (PlatformManager)target;
		
		EditorGUILayout.PropertyField(platform_framework);

		EditorGUILayout.PropertyField(cycle_period);


		EditorGUILayout.PropertyField(points, true);
		
		


//		ArrayGUI(points);

		if(GUILayout.Button("Add point"))
		{
			myTarget.AddPoint();
		}

		serializedObject.ApplyModifiedProperties();

		if (GUI.changed) EditorUtility.SetDirty(myTarget);
	}

	void ArrayGUI (SerializedProperty property) {
		
		EditorGUILayout.PropertyField(property);
		EditorGUI.indentLevel ++;
		SerializedProperty arraySizeProp = property.FindPropertyRelative("Array.size");
		EditorGUILayout.PropertyField(arraySizeProp);
		for (int i = 0; i < arraySizeProp.intValue; i++) {
			EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i));
		}

		EditorGUI.indentLevel --;
	}

}