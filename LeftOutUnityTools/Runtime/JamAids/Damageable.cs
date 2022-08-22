using UnityEngine;

namespace LeftOut
{
    public class Damageable : MonoBehaviour
    {
        public class DamageEventArgs : System.EventArgs
        {
            public GameObject Source { get; internal set; }
            public int Amount { get; internal set; }
        }

        int m_LastFrameDamaged = int.MinValue;

        // We don't make this a UnityEvent because it's just raw data passing - downstream handlers can decide
        // when to raise UnityEvents based on whether or not the damage resolves, etc.
        public event System.EventHandler<DamageEventArgs> OnDamageReceived;

        public void ReceiveDamage(GameObject source, int amount)
        {
            if (m_LastFrameDamaged == Time.frameCount)
            {
                Debug.Log(
                    $"Already received damage from a source this frame, ignoring damage from {source.name}");
                return;
            }

            m_LastFrameDamaged = Time.frameCount;
            OnDamageReceived?.Invoke(this, new DamageEventArgs()
            {
                Source = source,
                Amount = amount
            });
        }
    }
}
