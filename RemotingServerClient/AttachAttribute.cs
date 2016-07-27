using System;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace System.Runtime.Remoting
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AttachAttribute : ContextAttribute, IContributeObjectSink
    {
        Type HandlerType;
        public AttachAttribute(Type Type) : base("Interceptor")
        {
            HandlerType = Type;
        }

        public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink)
        {
            return Activator.CreateInstance(HandlerType, new object[] { obj, nextSink }) as IMessageSink;
        }
    }
    [AttributeUsage(AttributeTargets.Method)]
    public class ExecuteInMaintheadAttribute : Attribute
    {
    }
    [Serializable]
    public class HookTransction : IMessageSink
    {
        public QueueEX<Action> Queues = new QueueEX<Action>();
        protected object Target;
        private IMessageSink nextSink;
        EventWaitHandle eHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        Type type;
        public IMessageSink NextSink
        {
            get
            {
                return nextSink;
            }
        }
        public HookTransction(object Target, IMessageSink nextSink)
        {
            this.nextSink = nextSink;
            this.Target = Target;
            Queues=(Target as RemotingObject).Queues;
            type = Target.GetType();
        }
        public IMessage SyncProcessMessage(IMessage msg)
        {
            IMessage retMsg = null;
            IMethodCallMessage call = msg as IMethodCallMessage;
            if (call == null)
            {
                retMsg = nextSink.SyncProcessMessage(msg);
            }
            else
            {
                MethodInfo mInfo = type.GetMethod(call.MethodBase.Name);
                if(!Attribute.IsDefined(mInfo, typeof(ExecuteInMaintheadAttribute)))
                    retMsg = nextSink.SyncProcessMessage(msg);
                else
                {
                    lock (Queues)
                    {
                        Queues.Enqueue(() => { retMsg = nextSink.SyncProcessMessage(msg); eHandle.Set(); });
                    }
                    eHandle.WaitOne();
                    //retMsg=new ReturnMessage(null, call);
                }
            }
            return retMsg;
        }

        IMessageCtrl IMessageSink.AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            throw new NotImplementedException();
        }
    }
}