using System.Collections.Generic;
using UnityEngine;

namespace LeftOut
{
    public abstract class UnitySerializedDictionary<TKey, TValue> 
        : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        List<TKey> m_Keys = new List<TKey>();
	
        [SerializeField, HideInInspector]
        List<TValue> m_Values = new List<TValue>();

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            this.Clear();
            for (int i = 0; i < this.m_Keys.Count && i < this.m_Values.Count; i++)
            {
                this[this.m_Keys[i]] = this.m_Values[i];
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            this.m_Keys.Clear();
            this.m_Values.Clear();

            foreach (var item in this)
            {
                this.m_Keys.Add(item.Key);
                this.m_Values.Add(item.Value);
            }
        }
    }
}
