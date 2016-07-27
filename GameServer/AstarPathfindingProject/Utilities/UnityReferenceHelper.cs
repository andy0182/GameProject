using UnityEngine;
using Pathfinding;

namespace Pathfinding {
	[ExecuteInEditMode]
	/** Helper class to keep track of references to GameObjects.
	 * Does nothing more than to hold a GUID value.
	 */
	public class UnityReferenceHelper : System.Runtime.Remoting.CoroutineManager
    {
		
		[HideInInspector]
		[SerializeField]
		private string guid;
		
		public string GetGUID () {
			return guid;
		}
		
		public void Awake () {
			Reset ();
		}
		
		public void Reset () {
			if (string.IsNullOrEmpty (guid)) {
				guid = Pathfinding.Util.Guid.NewGuid ().ToString ();
				Debug.Log ("Created new GUID - "+guid);
			} else {
			}
		}
	}
}