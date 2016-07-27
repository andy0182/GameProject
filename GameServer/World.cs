using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UtilityHelper;

namespace GameServer
{
    class World : TransportInterface
    {
        public World()
        {
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            //Entitys.Add(new WorldObject() { ID = 1 });
            //Entitys.Add(new WorldObject() { ID = 1 });
            //Entitys.Add(new WorldObject() { ID = 1 });
            Update();
        }
        public List<WorldEntity> Entitys = new List<WorldEntity>();
        public List<ServerEntity> PlayerEntitys = new List<ServerEntity>();
        public override void Loging(Player mPlayer)
        {
            lock (Entitys)
            {
                var Player=ServerPlayer.GetServerPlayer(mPlayer);
                Entitys.ForEach(a => a.OnAddPlayer(Player));
                var mavatar=Player.CreateAvater();
                List<ServerPlayer> entitys = new List<ServerPlayer>();
                ServerPlayer.Players.ForEach((a, b) => entitys.Add(b));
                entitys.RemoveAll(a => a == Player);
                ///把avatar加入到其他客户端
                entitys.ForEach(a => mavatar.AddAvatar(a.Create(100)));
                ///其他客户端加入到avatar
                entitys.ForEach(a => a.mAvatar.AddAvatar(Player.Create(101)));
            }
        }

        public void RemovePlayer(Player mPlayer)
        {
            lock(Entitys)
            {
                Entitys.ForEach(a => a.OnRemovePlayer(mPlayer));
            }
        }
        async void Update()
        {
            await Task.Run(() => { while (true) OnUpdate(); });
        }
        void OnUpdate()
        {
            lock(Entitys)
            {
                DateTime TimerStart = DateTime.Now;
                Entitys.ForEach(a => a.OnUpdate());
                Thread.Sleep(100);
                Time.deltaTime = (float)((DateTime.Now - TimerStart).TotalMilliseconds * 0.001f);
            }
        }
    }
    public class ServerEntity
    {
        Queue<System.Action> Queues = new Queue<System.Action>();
        public ServerPlayer Parasitifer;
        Entity entity;
        public ServerEntity()
        {
        }
        public void SetParasitifer(ServerPlayer player)
        {
            Parasitifer = player;
        }
        public static implicit operator ServerEntity(Entity mentity)
        {
            return new ServerEntity() { entity = mentity };
        }
        public static implicit operator Entity(ServerEntity entity)
        {
            return entity.entity;
        }
        List<System.Action> Actions = new List<System.Action>();
        public void setPosition(Vector3 position)
        {
            lock(Actions)
            {
                Actions.Add(() => entity.SetPosition(position.x, position.y, position.z));
            }
        }
        public void SetTarget(Vector3 Target)
        {
            lock(Actions)
            {
                Actions.Add(() => entity.SetTarget(Target.x, Target.y, Target.z));
            }
        }
        public void Update()
        {
            lock (Actions)
            {
                Actions.ForEach(a => Queues.Enqueue(a));
                Actions.Clear();
            }
            if (Queues.Count > 0) Queues.Dequeue()();
        }
    }

    public class ServerPlayer
    {
        public static DictionaryEx<int, ServerPlayer> Players = new DictionaryEx<int, ServerPlayer>();

        public static ServerPlayer GetServerPlayer(Player player)
        {
            ServerPlayer tmp= Players[player.GetHashCode()];
            tmp.mPlayer = player;
            return tmp;
        }
        Player mPlayer;
        List<ServerEntity> mEntitys = new List<ServerEntity>();
        List<ServerEntity> Adds = new List<ServerEntity>();
        public Avatar mAvatar=new Avatar();
        public ServerPlayer()
        {
            Update();
        }
        async void Update()
        {
            await Task.Run(() => { while (true) OnUpdate(); });
        }
        void OnUpdate()
        {
            lock (Adds)
            {
                Adds.ForEach(a => mEntitys.Add(a));
                Adds.Clear();
            }
            mEntitys.ForEach(a => a.Update());
        }
        public ServerEntity Create(int ID)
        {
            ServerEntity mEntity = mPlayer.Create(ID);
            lock(Adds)
            {
                Adds.Add(mEntity);
            }
            return mEntity;
        }
        public Avatar CreateAvater()
        {
            mAvatar.AddAvatar(mPlayer.CreateAvatar(mAvatar));
            return mAvatar;
        }
    }
    public class WorldEntity
    {
        public int ID;
        public virtual void OnAddPlayer(ServerPlayer mPlayer)
        {

        }
        public virtual void OnRemovePlayer(Player mPlayer)
        {

        }
        public virtual void OnUpdate()
        {
        }
    }
    public static class Time
    {
        public static float deltaTime;
    }
    public class WorldObject : WorldEntity
    {
        List<ServerEntity> entitys = new List<ServerEntity>();
        CoroutineManager mCorountine = new CoroutineManager();
        Vector3 position = new Vector3(0, 0, 5);
        Vector3 Target;
        public WorldObject()
        {
            Test();
        }
        public override void OnAddPlayer(ServerPlayer mPlayer)
        {
            ServerEntity entity = mPlayer.Create(ID);
            entitys.Add(entity);
            entitys.ForEach(a => a.SetTarget(Target));
            entitys.ForEach(a => a.setPosition(position));
        }
        public ABPath GetNewPath(Vector3 start, Vector3 end)
        {
            // Construct a path with start and end points
            ABPath p = ABPath.Construct(start, end, null);

            return p;
        }
        void Test()
        {
            System.Random r = new System.Random();
            Target = new Vector3(r.Next(80) - 40, 0, r.Next(80) - 40);
            Target = (Vector3)AstarPath.active.GetNearest(Target).node.position;
            Path path = GetNewPath(position, Target);
            path.callback = a => mCorountine.StartCoroutine(RunPath(a));
            AstarPath.StartPath(path);
            entitys.ForEach(a => a.SetTarget(Target));
            entitys.ForEach(a => a.setPosition(position));
        }
        public Vector3 MoveTo(Vector3 from, Vector3 to, float t)
        {
            Vector3 distance = (to - from);
            Vector3 direct = distance.normalized;
            Vector3 target = direct * t;
            if (target.sqrMagnitude > distance.sqrMagnitude)
                target = distance;
            return target;
        }
        IEnumerator RunPath(Path p)
        {
            currentWaypointIndex = 0;
            targetReached = false;
            bool isfinish = true;
            while (isfinish)
            {
                position += CalculateVelocity(p, position,()=> isfinish=false) * Time.deltaTime*3;
                yield return null;
            }
            Test();
        }
        int currentWaypointIndex = 0;
        protected float XZSqrMagnitude(Vector3 a, Vector3 b)
        {
            float dx = b.x - a.x;
            float dz = b.z - a.z;
            return dx * dx + dz * dz;
        }
        bool targetReached = false;
        protected Vector3 CalculateVelocity(Path path, Vector3 currentPosition,System.Action OnTargetReached)
        {
            if (path == null || path.vectorPath == null || path.vectorPath.Count == 0) return Vector3.zero;

            List<Vector3> vPath = path.vectorPath;

            if (vPath.Count == 1)
            {
                vPath.Insert(0, currentPosition);
            }

            if (currentWaypointIndex >= vPath.Count) { currentWaypointIndex = vPath.Count - 1; }

            if (currentWaypointIndex <= 1) currentWaypointIndex = 1;

            while (true)
            {
                if (currentWaypointIndex < vPath.Count - 1)
                {
                    float dist = XZSqrMagnitude(vPath[currentWaypointIndex], currentPosition);
                    if (dist < 4)
                    {
                        currentWaypointIndex++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            Vector3 dir;
            Vector3 targetPosition = CalculateTargetPoint(currentPosition, vPath[currentWaypointIndex - 1], vPath[currentWaypointIndex]);
            dir = targetPosition - currentPosition;
            dir.y = 0;
            float targetDist = dir.magnitude;
            if (currentWaypointIndex == vPath.Count - 1 && targetDist <= 0.2f)
            {
                if (!targetReached) { targetReached = true; OnTargetReached(); }
            }
            return dir.normalized;
        }
        protected Vector3 CalculateTargetPoint(Vector3 p, Vector3 a, Vector3 b)
        {
            a.y = p.y;
            b.y = p.y;

            float magn = (a - b).magnitude;
            if (magn == 0) return a;

            float closest = AstarMath.Clamp01(AstarMath.NearestPointFactor(a, b, p));
            Vector3 point = (b - a) * closest + a;
            float distance = (point - p).magnitude;

            float lookAhead = Mathf.Clamp(1 - distance, 0.0F, 1);

            float offset = lookAhead / magn;
            offset = Mathf.Clamp(offset + closest, 0.0F, 1.0F);
            return (b - a) * offset + a;
        }

        public override void OnRemovePlayer(Player mPlayer)
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            mCorountine.Update();
        }
    }
    public class Avatar : TransportEntity
    {
        List<ServerEntity> entitys = new List<ServerEntity>();
        public void AddAvatar(ServerEntity mEntity)
        {
            entitys.Add(mEntity);
        }
        public override void SetTarget(float x, float y, float z)
        {
            Vector3 Target = new Vector3(x, y, z);
            entitys.ForEach(a => a.SetTarget(Target));
        }
    }
}
