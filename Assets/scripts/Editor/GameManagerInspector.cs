using UnityEditor;
using UnityEngine;
using Game;

[CustomEditor(typeof(GameManager))]
public class GameManagerInspector : Editor {

	SerializedProperty gameSceneName;
	SerializedProperty secondsToNewRound;
	SerializedProperty BPM;

	SerializedProperty countDown;
	SerializedProperty countDownEveryRound;


	private static int beat_num = 0;

	void OnEnable()
	{
		gameSceneName = serializedObject.FindProperty("gameSceneName");
		secondsToNewRound = serializedObject.FindProperty("secondsToNewRound");
		BPM = serializedObject.FindProperty("BPM");

		countDown = serializedObject.FindProperty("countDown");
		countDownEveryRound = serializedObject.FindProperty("countDownEveryRound");

	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		GameManager myTarget = (GameManager)target;

		EditorGUILayout.PropertyField(gameSceneName);
		EditorGUILayout.PropertyField(secondsToNewRound);
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("BPM options", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(BPM);

		EditorGUI.BeginChangeCheck();
		beat_num = EditorGUILayout.IntSlider("beat number", beat_num, 1,16);
//		beat_num = Mathf.Max(EditorGUILayout.IntField("beat number:",beat_num),0);
		if (EditorGUI.EndChangeCheck () ) {
			myTarget.MockPlatformsAtBeat(beat_num-1);
		}

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Countdown options", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(countDown);
		EditorGUILayout.PropertyField(countDownEveryRound);
		
		serializedObject.ApplyModifiedProperties();
	}
}
