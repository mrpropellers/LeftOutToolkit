using UnityEngine;

namespace LeftOut
{
    public class DamageAttempt : System.EventArgs
    {
        public GameObject Source { get; }
        public float DamageAmount { get; }

        public DamageAttempt(GameObject source, float damageAmount)
        {
            Source = source;
            DamageAmount = damageAmount;
        }
    }

    public class DamageResult : System.EventArgs
    {
        // This doesn't mean that AmountApplied is non-zero, just that the attempt was not discarded
        public bool AttemptWasProcessed { get; private set; }
        public int AmountApplied;

        public static DamageResult Ignored => new DamageResult(int.MinValue)
        {
            AttemptWasProcessed = true,
        };

        public DamageResult(int amountApplied)
        {
            AttemptWasProcessed = true;
            Debug.Assert(amountApplied >= 0f);
            AmountApplied = amountApplied;
        }
    }

    public interface IDamageable
    {
        public DamageResult ProcessDamage(DamageAttempt attempt);
    }
}
