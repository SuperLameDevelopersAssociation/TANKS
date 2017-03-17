using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ArcReactor_PoolManager))]
public class ArcReactor_PoolManagerEditor : Editor {

	public override void OnInspectorGUI()
	{
		EditorGUILayout.HelpBox("Pool manager will automatically handle instantiation of Arcs by Launchers",MessageType.Info);
	}
}
