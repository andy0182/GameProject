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
            var bytes = Pathfinding.Serialization.AstarSerializer.LoadFromFile(@"C:\Users\Andy\Documents\Visual Studio 2015\Projects\GameServer\New Unity Project\graph.bytes");
            DeserializeGraphs(bytes);
            Update();
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
        public void OnUpdate()
        {
            script.Update();
        }
        async void Update()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(100);
                    OnUpdate();
                }
            });
        }
    }

}
