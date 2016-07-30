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
    public class World : TransportInterface
    {
        public World()
        {
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Entitys.Add(new WorldObject() { ID = 1 });
            Update();
        }
        public List<WorldEntity> Entitys = new List<WorldEntity>();
        public List<ServerEntity> PlayerEntitys = new List<ServerEntity>();
        public override void Loging(Player mPlayer)
        {
            lock (Entitys)
            {
                var Player = ServerPlayer.GetServerPlayer(mPlayer);
                Player.Parasitifer = this;
                Entitys.ForEach(a => a.OnAddPlayer(Player));
                var mavatar = Player.CreateAvater();
                List<ServerPlayer> entitys = new List<ServerPlayer>();
                ServerPlayer.Players.ForEach((a, b) => entitys.Add(b));
                entitys.RemoveAll(a => a == Player);
                ///把avatar加入到其他客户端
                entitys.ForEach(a => mavatar.AddAvatar(a.Create(0)));
                ///其他客户端加入到avatar
                entitys.ForEach(a => a.mAvatar.AddAvatar(Player.Create(0)));
            }
        }

        public void RemovePlayer(ServerPlayer mPlayer)
        {
            lock (Entitys)
            {
                Entitys.ForEach(a => a.OnRemovePlayer(mPlayer));
                mPlayer.OnExit();
            }
        }
        async void Update()
        {
            await Task.Run(() => { while (true) OnUpdate(); });
        }
        void OnUpdate()
        {
            lock (Entitys)
            {
                DateTime TimerStart = DateTime.Now;
                Entitys.ForEach(a => a.OnUpdate());
                Thread.Sleep(25);
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
            lock (Actions)
            {
                Actions.Add(() => entity.SetPosition(position.x, position.y, position.z));
            }
        }
        public void Skill(int ID)
        {
            lock (Actions)
            {
                Actions.Add(() => entity.Skill(ID));
            }
        }
        public void OnExit()
        {
            lock (Actions)
            {
                Actions.Add(() => entity.OnDestroy());
            }
        }
        public void SetTarget(Vector3 Target)
        {
            lock (Actions)
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
        public World Parasitifer;
        public static ServerPlayer GetServerPlayer(Player player)
        {
            ServerPlayer tmp = Players[player.GetHashCode()];
            tmp.mPlayer = player;
            return tmp;
        }
        Player mPlayer;
        List<ServerEntity> mEntitys = new List<ServerEntity>();
        List<ServerEntity> Adds = new List<ServerEntity>();
        public Avatar mAvatar = new Avatar();
        public ServerPlayer()
        {
            Update();
        }
        bool Abort = false;
        public void OnExit()
        {
            Abort = true;
            Players.Remove(mPlayer.GetHashCode());
            Players.ForEach((a, b) => b.mAvatar.RemoveAvatar(this));
            mAvatar.OnDestroy();
        }
        async void Update()
        {
            await Task.Run(() => { while (!Abort) OnUpdate(); });
        }
        void OnUpdate()
        {
            try
            {
                lock (Adds)
                {
                    Adds.ForEach(a => mEntitys.Add(a));
                    Adds.Clear();
                }
                mEntitys.ForEach(a => a.Update());
            }
            catch (Exception e)
            {
                Parasitifer.RemovePlayer(this);
            }
        }
        public ServerEntity Create(int ID)
        {
            ServerEntity mEntity = mPlayer.Create(ID);
            mEntity.Parasitifer = this;
            lock (Adds)
            {
                Adds.Add(mEntity);
            }
            return mEntity;
        }
        public Avatar CreateAvater()
        {
            ServerEntity mEntity = mPlayer.CreateAvatar(mAvatar);
            mAvatar.AddAvatar(mEntity);
            mAvatar.Parasitifer = this;
            mEntity.Parasitifer = this;
            lock (Adds)
            {
                Adds.Add(mEntity);
            }
            return mAvatar;
        }
    }
    public class WorldEntity
    {
        public int ID;
        public virtual void OnAddPlayer(ServerPlayer mPlayer)
        {

        }
        public virtual void OnRemovePlayer(ServerPlayer mPlayer)
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
            //mCorountine.StartCoroutine(SetPoint());
        }
        IEnumerator SetPoint()
        {
            while(true)
            {
                yield return new WaitformSecond(3);
                entitys.ForEach(a => a.setPosition(position));
            }
        }
        public override void OnAddPlayer(ServerPlayer mPlayer)
        {
            ServerEntity entity = mPlayer.Create(ID);
            entitys.Add(entity);
            entitys.ForEach(a => a.SetTarget(Target));
            entitys.ForEach(a => a.setPosition(position));
        }
        public override void OnRemovePlayer(ServerPlayer mPlayer)
        {
            entitys.RemoveAll(a => a.Parasitifer == mPlayer);
        }
        public ABPath GetNewPath(Vector3 start, Vector3 end)
        {
            // Construct a path with start and end points
            ABPath p = ABPath.Construct(start, end, null);

            return p;
        }
        static System.Random r = new System.Random();

        void Test()
        {
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
            foreach (var path in p.vectorPath)
            {
                while(XZSqrMagnitude(path, position)>0.4f)
                {
                    position += MoveTo(position,path, Time.deltaTime * 3.1f);
                    yield return null;
                }
            }
            Test();
        }
        protected float XZSqrMagnitude(Vector3 a, Vector3 b)
        {
            float dx = b.x - a.x;
            float dz = b.z - a.z;
            return dx * dx + dz * dz;
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            mCorountine.Update();
        }
    }
    public class Avatar : TransportEntity
    {
        public List<ServerEntity> entitys = new List<ServerEntity>();
        public ServerPlayer Parasitifer;
        public void AddAvatar(ServerEntity mEntity)
        {
            entitys.Add(mEntity);
        }
        public void OnDestroy()
        {
            for (int i = 1; i < entitys.Count; i++)
            {
                entitys[i].OnExit();
            }
        }
        public void RemoveAvatar(ServerPlayer mPlayer)
        {
            entitys.RemoveAll(a => a.Parasitifer == mPlayer);
        }
        public override void SetTarget(float x, float y, float z)
        {
            Vector3 Target = new Vector3(x, y, z);
            entitys.ForEach(a => a.SetTarget(Target));
        }
        public override void Skill(int ID)
        {
            entitys.ForEach(a => a.Skill(ID));
        }
    }
}
