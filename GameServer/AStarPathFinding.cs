using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Pathfinding
{
    class AStarPathFinding
    {
        AstarPath script=new AstarPath();
        public void Initialization()
        {
            script.Awake();
            var bytes = Pathfinding.Serialization.AstarSerializer.LoadFromFile(@"C:\Users\Andy\Documents\New Unity Project\graph.bytes");
            DeserializeGraphs(bytes);
            Update();
            var Path = GetNewPath(new Vector3(-12.63644f, 0, 0), new Vector3(25.28727f, 0, -24.41525f));
            AstarPath.StartPath(Path);
        }
        void DeserializeGraphs(byte[] bytes)
        {
            AstarPath.active.AddWorkItem(new AstarPath.AstarWorkItem(force =>
            {
                var sr = new Pathfinding.Serialization.AstarSerializer(script.astarData);
                if (sr.OpenDeserialize(bytes))
                {
                    script.astarData.DeserializeGraphsPart(sr);
                    sr.CloseDeserialize();
                }
                return true;
            }));

            // Make sure the above work item is run directly
            AstarPath.active.FlushWorkItems();
        }
        Vector3[] path;
        int pathIndex;
        Vector3 Target = new Vector3(0, 1.13f, -26);
        Vector3 Position = new Vector3(-13, 1.13f, -24);
        IEnumerator RunPath(Path p)
        {
            var path = p.vectorPath;
            for(int i=0;i< path.Count;i++)
            {
                Vector3 normal = (path[i] - Position).normalized;
                while ((Position-path[i]).sqrMagnitude>0.5f)
                {
                    Position += normal;
                    yield return null;
                }
            }
            System.Random r = new System.Random();
            Target = new Vector3(r.Next(0, 48), 0, r.Next(0, 48));
            var mpath = GetNewPath(Position, Target);
            mpath.callback = a=> script.StartCoroutine(RunPath(a));
            AstarPath.StartPath(mpath);
        }
        public ABPath GetNewPath(Vector3 start, Vector3 end)
        {
            // Construct a path with start and end points
            ABPath p = ABPath.Construct(start, end, null);

            return p;
        }
        public void OnUpdate()
        {
            script.Update();
        }
        async void Update()
        {
            await Task.Run(() =>
            {
                while(true)
                {
                    OnUpdate();
                }
            });
        }
    }
}
