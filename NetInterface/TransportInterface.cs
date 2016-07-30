namespace System.Runtime.Remoting
{
    public abstract class TransportInterface : ContextBoundObject
    {
        public override object InitializeLifetimeService()
        {
            return null;
        }
        public abstract void Loging(Player mPlayer);
    }
    public abstract class TransportEntity: ContextBoundObject
    {
        public abstract void SetTarget(float x, float y, float z);
        public abstract void Skill(int ID);
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
