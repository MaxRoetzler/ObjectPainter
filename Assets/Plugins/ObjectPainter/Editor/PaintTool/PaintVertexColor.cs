using UnityEditor;
using UnityEngine;

namespace FantasticYes.Tools
{
	public class PaintVertexColor : PaintTool, IPaintTool
	{
		private Color m_color;
		private Color [] m_colors;

		#region IPaintTool Implementation
		public GUIContent Label
		{
			get
			{
				return new GUIContent ("Vertex Painter", "Paint vertex colors.");
			}
		}

		public void OnGUI ()
		{
			m_color = EditorGUILayout.ColorField (new GUIContent ("Color", "Vertex color to paint with"), m_color);
		}

		public void OnSceneGUI (SceneView sceneView)
		{
			Event e = Event.current;
			int id = GUIUtility.GetControlID (FocusType.Passive);

			if (e.type == EventType.MouseDown && !e.alt)
			{
				GUIUtility.hotControl = id;
			}

			if (GUIUtility.hotControl == id && e.type == EventType.MouseDrag)
			{
				if (PickGameObject (e.mousePosition))
				{
					Paint (e.mousePosition);
				}

				e.Use ();
			}

			if (GUIUtility.hotControl == id && e.type == EventType.MouseUp)
			{
				e.Use ();
				GUIUtility.hotControl = 0;
			}

			/*if (e.type == EventType.MouseDown && !e.alt)
			{
				if (PickGameObject (e.mousePosition))
				{
					GUIUtility.hotControl = id;
					Paint (e.mousePosition);
				}
			}

			if (GUIUtility.hotControl == id)
			{
				if (e.type == EventType.MouseDrag)
				{
					Paint (e.mousePosition);
				}
			}*/
		}
		#endregion

		#region PaintTool Implementation
		protected override void SetGameObject (GameObject gameObject)
		{
			MeshFilter meshFilter = gameObject.GetComponent<MeshFilter> ();

			if (meshFilter != null && meshFilter.sharedMesh != null)
			{
				m_gameObject = gameObject;
				m_mesh = meshFilter.sharedMesh;

				if (m_mesh.colors.Length == 0)
				{
					m_mesh.colors = new Color [m_mesh.vertexCount];
				}
				m_colors = m_mesh.colors;
				m_matrix = gameObject.transform.localToWorldMatrix;
			}
		}

		protected void Paint (Vector2 position)
		{
			if (Raycast (position))
			{
				for (int i = 0; i < m_mesh.vertexCount; i++)
				{
					Vector3 vertex = m_gameObject.transform.TransformPoint (m_mesh.vertices [i]);

					float distance = (vertex - m_raycastHit.point).magnitude;

					if (distance > 0.1f)
					{
						float value = 1.0f - (distance / 0.5f);
						m_colors [i] = Color.Lerp (m_colors [i], m_color, value);
					}
				}

				m_mesh.colors = m_colors;
			}
		}
		#endregion
	}
}