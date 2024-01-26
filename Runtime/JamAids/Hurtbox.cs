using System;
using UnityEngine;

namespace LeftOut
{
    public class Hurtbox : MonoBehaviour, IDamageable
    {
        bool m_HasParent;
        IDamageable m_Parent;

        [field: SerializeField]
        public float DamageMultiplier { get; private set; }

        void Start()
        {
            m_Parent = transform.parent.gameObject.GetComponentInParent<IDamageable>();
            m_HasParent = m_Parent != null;
            if (!m_HasParent)
            {
                Debug.LogError(
                    $"{nameof(Hurtbox)} has no parent {nameof(IDamageable)} to pass damage to", this);
            }
        }

        public DamageResult ProcessDamage(DamageAttempt attempt)
        {
            if (!m_HasParent) return DamageResult.Ignored;
            attempt.SetMultiplier(this);
            return m_Parent.ProcessDamage(attempt);
        }
    }
}
