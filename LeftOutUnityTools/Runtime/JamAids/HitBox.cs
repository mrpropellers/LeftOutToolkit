using System;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace LeftOut.JamAids
{
    public class HitBox : MonoBehaviour, IDamageable
    {
        bool m_HasParent;
        IDamageable m_Parent;

        [SerializeField]
        FloatReference m_DamageMultiplier;

        void Start()
        {
            m_Parent = transform.parent.gameObject.GetComponentInParent<IDamageable>();
            m_HasParent = m_Parent != null;
            if (!m_HasParent)
            {
                Debug.LogError(
                    $"{nameof(HitBox)} has no parent {nameof(IDamageable)} to pass damage to", this);
            }
        }

        void OnValidate()
        {
            // Presumably we'd just use the Unbreakable Component if we wanted a Damageable that doesn't take damage
            // This can be worked around by using a Constant initialized to 0
            if (m_DamageMultiplier.Value == 0
                && m_DamageMultiplier.Usage == AtomReferenceUsage.VALUE)
            {
                Debug.LogWarning("Detected uninitialized damage multiplier -- defaulting to 1");
                m_DamageMultiplier.Value = 1;
            }
        }

        public DamageResult ProcessDamage(DamageAttempt attempt)
        {
            if (!m_HasParent) return DamageResult.Ignored;
            attempt.Multipliers.Add(m_DamageMultiplier.Value);
            return m_Parent.ProcessDamage(attempt);
        }
    }
}
