using UnityEditor;
using UnityEngine;
using Game;

[CustomEditor(typeof(PlatformManager))]
public class PlatformInspector : Editor {

	SerializedProperty beats_per_cycle;
	SerializedProperty cycle_period;
	SerializedProperty segment_period;
	SerializedProperty points;
	SerializedProperty platform_framework;

	void OnEnable()
	{
		beats_per_cycle = serializedObject.FindProperty("beats_per_cycle");
		cycle_period = serializedObject.FindProperty("cycle_period");
		segment_period = serializedObject.FindProperty("segment_period");
		points = serializedObject.FindProperty("points");
		platform_framework = serializedObject.FindProperty("platform_framework");
	}


	public override void OnInspectorGUI()
	{
		PlatformManager myTarget = (PlatformManager)target;

		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(platform_framework);
		if (EditorGUI.EndChangeCheck () ) {
			Undo.RecordObject(target, "Changed platform framework");
			serializedObject.ApplyModifiedProperties();
			myTarget.SetFramework(myTarget.platform_framework);
		}

		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(beats_per_cycle);
		if (EditorGUI.EndChangeCheck () ) {
			Undo.RecordObject(target, "Changed beats per cycle");
			serializedObject.ApplyModifiedProperties();
			myTarget.UpdateSegmentPeriod();
			serializedObject.Update();
		}

		EditorGUILayout.LabelField("cycle period:", cycle_period.floatValue.ToString());

		EditorGUILayout.LabelField("segment period:", segment_period.floatValue.ToString());

		EditorGUILayout.PropertyField(points, true);

		if(GUILayout.Button("Add point"))
		{
			GameObject point = new GameObject();
			Undo.RegisterCreatedObjectUndo (point, "Created point");
			myTarget.AddPoint(point);
		}

		serializedObject.ApplyModifiedProperties();
		serializedObject.Update();

		if (GUI.changed) {
			EditorUtility.SetDirty(myTarget);
		}
			
	}
		
}