using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityHelper
{
    public class DictionaryEx<T1, T2> : Dictionary<T1, T2> where T2 : new()
    {
        public new T2 this[T1 key]
        {
            get
            {
                if (!ContainsKey(key))
                {
                    Add(key, new T2());
                }
                return base[key];
            }
            set
            {
                base[key] = value;
            }
        }
        public void ForEach(Action<T1, T2> Callback)
        {
            var keys = new List<T1>(Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                Callback(keys[i], this[keys[i]]);
            }
        }
    }
}
