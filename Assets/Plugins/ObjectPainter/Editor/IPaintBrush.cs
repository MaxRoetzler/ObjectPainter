using UnityEngine;

namespace FantasticYes.Tools
{
	public interface IPaintBrush
	{
		float Evaluate (float delta);

		void OnGUI ();
		void OnDrawGizmo (RaycastHit raycastHit);
	}
}