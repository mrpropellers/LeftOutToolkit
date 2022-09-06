using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LeftOut
{
    public class DamageAttempt : System.EventArgs
    {
        float m_RawDamage;

        public GameObject Source { get; }
        public List<float> Multipliers { get; }

        public float FinalDamageAmount
        {
            get
            {
                var finalDamage =
                    Multipliers.Aggregate(m_RawDamage, (current, factor) => current * factor);
                // TODO: This is bad practice. What's a better way to ensure Multipliers don't persist
                //       between damage attempts?
                Multipliers.Clear();
                return finalDamage;
            }
        }

        public DamageAttempt(GameObject source, float damageAmount)
        {
            Source = source;
            m_RawDamage = damageAmount;
            Multipliers = new List<float>();
        }
    }

    public class DamageResult : System.EventArgs
    {
        // This doesn't mean that AmountApplied is non-zero, just that the attempt was not discarded
        public bool AttemptWasProcessed { get; private set; }
        public int AmountApplied;

        public static DamageResult Ignored = new DamageResult();

        DamageResult()
        {
            AmountApplied = int.MinValue;
            AttemptWasProcessed = false;
        }

        public DamageResult(int amountApplied)
        {
            AttemptWasProcessed = true;
            Debug.Assert(amountApplied >= 0);
            AmountApplied = amountApplied;
        }
    }

    public interface IDamageable
    {
        public DamageResult ProcessDamage(DamageAttempt attempt);
    }
}
