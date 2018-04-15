using UnityEngine;
using UnityEditor;

namespace FantasticYes.Tools
{
	public abstract class PaintTool
	{
		protected Mesh m_mesh;
		protected Matrix4x4 m_matrix;
		protected GameObject m_gameObject;
		protected RaycastHit m_raycastHit;
		protected VertexStream m_vertexStream;

		protected abstract void SetGameObject (GameObject gameObject);

		protected bool Raycast (Vector2 position)
		{
			Ray ray = HandleUtility.GUIPointToWorldRay (position);

			return SceneUtility.IntersectRayMesh (ray, m_mesh, m_matrix, out m_raycastHit);
		}

		protected bool PickGameObject (Vector2 position)
		{
			GameObject pickedObject = SceneUtility.PickGameObject (position);

			if (pickedObject != null)
			{
				if (pickedObject != m_gameObject)
				{
					SetGameObject (pickedObject);
				}

				return true;
			}
			else
			{
				m_gameObject = null;
				return false;
			}
		}
	}
}