using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using UnityEngine;

namespace System.Runtime.Remoting
{
    public class ServerObject
    {
        public static void StartListening(int Port)
        {
            BinaryServerFormatterSinkProvider serverFormatter = new BinaryServerFormatterSinkProvider() { TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full };
            Hashtable ht = new Hashtable() { { "name", "ServerChannel" }, { "port", Port } };
            channel = new TcpChannel(ht, null, serverFormatter);
            ChannelServices.RegisterChannel(channel, false);
        }
        public static void AddObject<T>(string TypeName, WellKnownObjectMode mode = WellKnownObjectMode.SingleCall)
        {
            RemotingConfiguration.RegisterWellKnownServiceType(new WellKnownServiceTypeEntry(typeof(T), TypeName, mode));
        }
        static TcpChannel channel;
        public static void InitConnect(int port=9003)
        {
            channel = new TcpChannel(port);
            ChannelServices.RegisterChannel(channel, false);
        }
        public static void Close()
        {
            channel.StopListening(null);
        }
        public static T GetObject<T>(string url)
        {
            return (T)Activator.GetObject(typeof(T), url);
        }
    }
    [AttachAttribute(typeof(HookTransction))]
    public abstract class RemotingObject : ContextBoundObject
    {
        public RemotingObject(){}
        public RemotingObject(CoroutineManager mCoroutine)
        {
            mCoroutine.StartCoroutine(Start());
        }
        public QueueEX<Action> Queues = new QueueEX<Action>();

        public virtual void Update()
        {
            lock (Queues)
            {
                if (Queues.Count > 0) Queues.Dequeue()();
            }
        }
        IEnumerator Start()
        {
            return OnUpdate();
        }
        IEnumerator OnUpdate()
        {
            while (true)
            {
                Update();
                yield return null;
            }
        }
    }


    public delegate void Action();
    public class QueueEX<T> : Queue<T>
    {
        GameObject obj;
        public void AddObject(GameObject obj)
        {
            this.obj = obj;
        }
    }
}
