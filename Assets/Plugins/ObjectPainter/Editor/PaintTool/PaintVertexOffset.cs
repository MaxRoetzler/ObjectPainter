using UnityEditor;
using UnityEngine;

namespace FantasticYes.Tools
{
	public class PaintVertexOffset : PaintTool, IPaintTool
	{
		private Vector3 [] m_normals;
		private Vector3 [] m_vertices;

		#region IPaintTool Implementation
		public GUIContent Label
		{
			get
			{
				return new GUIContent ("Vertex Offset", "Paint vertex offset.");
			}
		}

		public void OnGUI ()
		{
			GUILayout.Label ("Painting like crazy");
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

				m_normals = m_mesh.normals;
				m_vertices = m_mesh.vertices;

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
						float value = (distance / 0.99f);
						m_vertices [i] += m_normals[i] * value;
					}
				}

				m_mesh.vertices = m_vertices;
			}
		}
		#endregion
	}
}