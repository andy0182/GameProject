using System.Collections;
using System.Collections.Generic;

namespace System.Runtime.Remoting
{
    public interface YieldInstruction
    {
        bool IsDone { get; }
    }
    public class Coroutine : YieldInstruction
    {
        public IEnumerator enumerator;
        public Coroutine(IEnumerator menumerator)
        {
            enumerator = menumerator;
        }

        public bool IsDone
        {
            get
            {
                return !enumerator.MoveNext();
            }
        }
    }
    public class CoroutineManager
    {
        List<IEnumerator> AddEnumeratos = new List<IEnumerator>();
        List<IEnumerator> m_enumerators = new List<IEnumerator>();
        List<IEnumerator> RemoveEnumeratos = new List<IEnumerator>();
        public virtual void Update()
        {
            lock(AddEnumeratos)
            {
                AddEnumeratos.ForEach(a => m_enumerators.Add(a));
                AddEnumeratos.Clear();
            }
            for (int i = 0; i < m_enumerators.Count; i++)
            {

                YieldInstruction yieldInstruction = m_enumerators[i].Current as Coroutine;
                if (yieldInstruction == null || yieldInstruction.IsDone)
                {
                    yieldInstruction = m_enumerators[i].Current as YieldInstruction;
                    if ((yieldInstruction == null || yieldInstruction.IsDone) && !m_enumerators[i].MoveNext())
                    {
                        RemoveEnumeratos.Add(m_enumerators[i]);
                    }
                }
            }
            lock (RemoveEnumeratos)
            {
                RemoveEnumeratos.ForEach(a => m_enumerators.Remove(a));
                RemoveEnumeratos.Clear();
            }
        }
        public Coroutine StartCoroutine(IEnumerator Start)
        {
            Coroutine tmp = null;
            lock (AddEnumeratos)
            {
                AddEnumeratos.Add(Start);
                tmp = new Coroutine(Start);
            }
            return tmp;
        }
        public void StopCoroutine(Coroutine corountine)
        {
            lock(RemoveEnumeratos)
            {
                RemoveEnumeratos.Add(corountine.enumerator);
            }
        }
    }
}
