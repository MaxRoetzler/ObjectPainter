using UnityEngine;
using UnityEditor;

namespace FantasticYes.Tools
{
	public interface IPaintTool
	{
		GUIContent Label
		{
			get;
		}

		void OnGUI ();
		void OnSceneGUI (SceneView sceneView);
	}
}