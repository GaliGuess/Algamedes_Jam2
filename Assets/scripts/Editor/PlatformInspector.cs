using UnityEditor;
using UnityEngine;
using Game;

[CustomEditor(typeof(PlatformManager))]
public class PlatformInspector : Editor {

	SerializedProperty cycle_period;
	SerializedProperty points;

	void OnEnable()
	{
		cycle_period = serializedObject.FindProperty("cycle_period");
		points = serializedObject.FindProperty("points");
	}


	public override void OnInspectorGUI()
	{
		PlatformManager myTarget = (PlatformManager)target;

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