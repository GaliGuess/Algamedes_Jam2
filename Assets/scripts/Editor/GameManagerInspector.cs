using UnityEditor;
using UnityEngine;
using Game;

[CustomEditor(typeof(GameManager))]
public class GameManagerInspector : Editor {

	SerializedProperty gameSceneName;
	SerializedProperty secondsToNewRound;
	SerializedProperty BPM;


	private static int beat_num = 0;

	void OnEnable()
	{
		gameSceneName = serializedObject.FindProperty("gameSceneName");
		secondsToNewRound = serializedObject.FindProperty("secondsToNewRound");
		BPM = serializedObject.FindProperty("BPM");

	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		GameManager myTarget = (GameManager)target;

		EditorGUILayout.PropertyField(gameSceneName);
		EditorGUILayout.PropertyField(secondsToNewRound);
		EditorGUILayout.PropertyField(BPM);

		EditorGUI.BeginChangeCheck();
		beat_num = Mathf.Max(EditorGUILayout.IntField("beat number:",beat_num),0);
		if (EditorGUI.EndChangeCheck () ) {
			myTarget.MockPlatformsAtBeat(beat_num);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
