using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.Voxels;

namespace Pathfinding {
	/** Explicit mesh object for recast graphs.
	 * Adding this component to an object will make sure it is included in any recast graphs.
	 * It will be included even though the Rasterize Meshes toggle is set to false.
	 * 
	 * Using RecastMeshObjs instead of relying on the Rasterize Meshes option is good for several reasons.
	 * - Rasterize Meshes is slow. If you are using a tiled graph and you are updaing it, every time something is recalculated
	 * the graph will have to search all meshes in your scene for ones to rasterize, in contrast, RecastMeshObjs are stored
	 * in a tree for extreamly fast lookup (O(log n + k) compared to O(n) where \a n is the number of meshes in your scene and \a k is the number of meshes
	 * which should be rasterized, if you know Big-O notation).
	 * - The RecastMeshObj exposes some options which can not be accessed using the Rasterize Meshes toggle. See member documentation for more info.
	 * 		This can for example be used to include meshes in the recast graph rasterization, but make sure that the character cannot walk on them.
	 *
	 * Since the objects are stored in a tree, and trees are slow to update, there is an enforcement that objects are not allowed to move
	 * unless the #dynamic option is enabled. When the dynamic option is enabled, the object will be stored in an array instead of in the tree.
	 * This will reduce the performance improvement over 'Rasterize Meshes' but is still faster.
	 */
	[AddComponentMenu("Pathfinding/Navmesh/RecastMeshObj")]
	public class RecastMeshObj : System.Runtime.Remoting.CoroutineManager
    {
		
		/** Static objects are stored in a tree for fast bounds lookups */
		protected static RecastBBTree tree = new RecastBBTree();
		
		/** Dynamic objects are stored in a list since it is costly to update the tree every time they move */
		protected static List<RecastMeshObj> dynamicMeshObjs = new List<RecastMeshObj>();
		
		/** Fills the buffer with all RecastMeshObjs which intersect the specified bounds */
		public static void GetAllInBounds (List<RecastMeshObj> buffer, Bounds bounds) {
		}
		
		[HideInInspector]
		public Bounds bounds;
		
		/** Check if the object will move.
		 * Recalculation of bounding box trees is expensive so if this is true, the object
		 * will simply be stored in an array. Easier to move, but slower lookup, so use wisely.
		 * If you for some reason want to move it, but don't want it dynamic (maybe you will only move it veery seldom and have lots of similar
		 * objects so they would add overhead by being dynamic). You can enable and disable the component every time you move it.
		 * Disabling it will remove it from the bounding box tree and enabling it will add it to the bounding box tree again.
		 * 
		 * The object should never move unless being dynamic or disabling/enabling it as described above.
		 */
		public bool dynamic = false;
		
		/** Voxel area for mesh.
		 * This area (not to be confused with pathfinding areas, this is only used when rasterizing meshes for the recast graph) field
		 * can be used to explicitly insert edges in the navmesh geometry or to make some parts of the mesh unwalkable.
		 * If the area is set to -1, it will be removed from the resulting navmesh. This is useful if you have some object that you want to be included in the rasterization,
		 * but you don't want to let the character walk on it.
		 * 
		 * When rasterizing the world and two objects with different area values are adjacent to each other, a split in the navmesh geometry
		 * will be added between them, characters will still be able to walk between them, but this can be useful when working with navmesh updates.
		 * 
		 * Navmesh updates which recalculate a whole tile (updatePhysics=True) are very slow So if there are special places
		 * which you know are going to be updated quite often, for example at a door opening (opened/closed door) you
		 * can use areas to create splits on the navmesh for easier updating using normal graph updates (updatePhysics=False).
		 * See the below video for more information.
		 * 
		 * \htmlonly
		 * <iframe width="600" height="400" src="http://www.youtube.com/embed/CS6UypuEMwM" frameborder="0" allowfullscreen></iframe>
		 * \endhtmlonly
		 * 
		 * 
		 */
		public int area = 0;
		
		bool _dynamic = false;
		bool registered = false;
		
		void OnEnable () {
			Register();
		}

		void Register () {
			if (registered) return;
			
			registered = true;
			
			//Clamp area, upper limit isn't really a hard limit, but if it gets much higher it will start to interfere with other stuff
			area = Mathf.Clamp (area,-1,1 << 25);
			
			
			//Default to renderer
			
			_dynamic = dynamic;
			if (_dynamic) {
				dynamicMeshObjs.Add (this);
			} else {
				tree.Insert (this);
			}
		}
		
		/** Recalculates the internally stored bounds of the object */
		private void RecalculateBounds ()
        {
		}
		
		/** Bounds completely enclosing the mesh for this object */
		public Bounds GetBounds () {
			if (_dynamic) {
				RecalculateBounds ();
			}
			return bounds;
		}
		
		
		void OnDisable () {
			
			registered = false;
			
			if (_dynamic) {
				dynamicMeshObjs.Remove (this);
			} else {
				if (!tree.Remove (this)) {
					throw new System.Exception ("Could not remove RecastMeshObj from tree even though it should exist in it. Has the object moved without being marked as dynamic?");
				}
			}
			_dynamic = dynamic;
		}
	}
}