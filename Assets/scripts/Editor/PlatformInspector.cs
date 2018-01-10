using UnityEditor;
using UnityEngine;
using Game;

[CustomEditor(typeof(PlatformManager))]
public class PlatformInspector : Editor {

	SerializedProperty beats_per_cycle;
	SerializedProperty cycle_period;
	SerializedProperty segment_period;
	SerializedProperty points;
	SerializedProperty init_platform_framework;

	static float cycle_percentage = 0.0f;


	void OnEnable()
	{
		beats_per_cycle = serializedObject.FindProperty("beats_per_cycle");
		cycle_period = serializedObject.FindProperty("cycle_period");
		segment_period = serializedObject.FindProperty("segment_period");
		points = serializedObject.FindProperty("points");
		init_platform_framework = serializedObject.FindProperty("init_platform_framework");
	}


	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		PlatformManager myTarget = (PlatformManager)target;

		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(init_platform_framework);
		if (EditorGUI.EndChangeCheck () ) {
			Undo.RecordObject(target, "Changed platform framework");
//			myTarget.SetFramework((Game.Framework)init_platform_framework.enumValueIndex);
		}

		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(beats_per_cycle);
		if (EditorGUI.EndChangeCheck () ) {
			Undo.RecordObject(target, "Changed beats per cycle");
			serializedObject.ApplyModifiedProperties();
			myTarget.UpdateSegmentPeriod();
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

		EditorGUI.BeginChangeCheck();
		cycle_percentage = EditorGUILayout.Slider(cycle_percentage, 0, 1);
		if (EditorGUI.EndChangeCheck () ) {
			myTarget.SetPosition(cycle_percentage);
		}

		serializedObject.ApplyModifiedProperties();


		if (GUI.changed) { //TODO: remove this and allow to update list of points from editor + support undo 
			EditorUtility.SetDirty(myTarget);
		}
			
	}
		
}