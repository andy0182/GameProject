namespace System.Runtime.Remoting
{
    public abstract class Entity : RemotingObject
    {
        public abstract void SetTarget(float x, float y, float z);
        public abstract void SetPosition(float x, float y, float z);
        public abstract void OnDestroy();
        public abstract void Skill(int id);
    }
    public abstract class Player : RemotingObject
    {
        public abstract Entity Create(int ID);
        public abstract Entity CreateAvatar(TransportEntity mEntity);
    }
}
