namespace System.Runtime.Remoting
{
    public abstract class TransportInterface : ContextBoundObject
    {
        public abstract void Loging(Player mPlayer);
    }
    public abstract class TransportEntity: ContextBoundObject
    {
        public abstract void SetTarget(float x, float y, float z);
    }
}
