using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FantasticYes.Tools
{
	public class Painter : EditorWindow
	{
		private int m_index;
		private GUIContent [] m_labels;
		private IPaintTool m_activeTool;
		private List<IPaintTool> m_tools;

		[MenuItem ("Window/Painter")]
		private static void ShowWindow ()
		{
			GetWindow<Painter> ().Show ();
		}

		/// <summary>
		/// Populate the paint tools and labels for the UI.
		/// </summary>
		private void PopulateTools ()
		{
			m_tools = new List<IPaintTool> ();

			foreach (Type type in Assembly.GetExecutingAssembly ().GetTypes ())
			{
				if (type.IsClass && (typeof (IPaintTool).IsAssignableFrom (type)))
				{
					m_tools.Add ((IPaintTool) Activator.CreateInstance (type, false));
				}
			}

			m_labels = m_tools.Select (x => x.Label).ToArray ();
		}

		/// <summary>
		/// Register the specified tool to the scene view update.
		/// </summary>
		/// <param name="tool"></param>
		private void RegisterTool (IPaintTool tool)
		{
			m_activeTool = tool;

			UnityEditor.Tools.current = Tool.None;
			SceneView.onSceneGUIDelegate += m_activeTool.OnSceneGUI;
		}

		/// <summary>
		/// Unregister the active tool from the scene view update.
		/// </summary>
		private void UnregisterTool ()
		{
			if (m_activeTool != null)
			{
				UnityEditor.Tools.current = Tool.Move;
				SceneView.onSceneGUIDelegate -= m_activeTool.OnSceneGUI;

				m_activeTool = null;
			}
		}

		/// <summary>
		/// Handle the GUI drawing.
		/// </summary>
		protected void OnGUI ()
		{
			EditorGUI.BeginChangeCheck ();
			m_index = GUIControls.ToggleGroup (m_index, m_labels);

			if (EditorGUI.EndChangeCheck ())
			{
				if (m_index > -1)
				{
					UnregisterTool ();
					RegisterTool (m_tools [m_index]);
				}
				else
				{
					UnregisterTool ();
				}
			}

			if (m_activeTool != null)
			{
				m_activeTool.OnGUI ();
			}
		}

		/// <summary>
		/// Handle the OnEnable event.
		/// </summary>
		protected void OnEnable ()
		{
			m_index = -1;
			PopulateTools ();
		}

		/// <summary>
		/// Handle the OnDisable event.
		/// </summary>
		protected void OnDisable ()
		{
			UnregisterTool ();
		}
	}

}