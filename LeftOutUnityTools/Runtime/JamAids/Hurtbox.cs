using System;
using System.Collections;
using System.Collections.Generic;
using LeftOut.Interfaces;
using UnityEngine;

namespace LeftOut
{
    public class Hurtbox : MonoBehaviour, IHaveOwner
    {
        bool m_IsWindingUp;
        bool m_IsOn;
        [SerializeField]
        List<Animator> m_Animators;
        List<Damageable> m_DamageReceivers;

        [field: SerializeField]
        public GameObject Owner { get; internal set; }
        [SerializeField]
        int DamageAmount = 1;

        bool IsOn
        {
            get => m_IsOn;
            set
            {
                // Whenever IsOn toggles, IsWindingUp must reset
                m_IsWindingUp = false;
                foreach (var animator in m_Animators)
                {
                    if (animator == null)
                        continue;
                    animator.SetBool(GlobalConsts.AnimatorParameters.HurtboxActive, value);
                }
                m_IsOn = value;
            }
        }

        void Start()
        {
            m_DamageReceivers = new List<Damageable>();
        }

        void LateUpdate()
        {
            if (!m_IsOn) return;

            // We only want to assign damage to any given receiver once per frame, even if we've had many collisions
            foreach (var damageable in m_DamageReceivers)
            {
                damageable.ReceiveDamage(gameObject, DamageAmount);
            }

            m_DamageReceivers.Clear();
        }

        void OnValidate()
        {
            if (!TryGetComponent<Rigidbody>(out _) && !TryGetComponent<Rigidbody2D>(out _))
            {
                Debug.LogWarning($"No rigidbody attached - may not be able to detect collisions");
            }
        }

        public void Activate(float windupTime = 0f)
        {
            m_DamageReceivers.Clear();
            Debug.Assert(!m_IsWindingUp && !m_IsOn, "Can't call Activate when already active");
            foreach (var animator in m_Animators)
            {
                animator.SetTrigger(GlobalConsts.AnimatorParameters.StartHurtboxWindup);
            }

            m_IsWindingUp = true;
            StartCoroutine(ActivateAfter(windupTime));
        }

        public void Deactivate()
        {
            //Debug.Assert(m_IsWindingUp || m_IsOn);
            IsOn = false;
        }

        IEnumerator ActivateAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            if (m_IsWindingUp)
            {
                IsOn = true;
            }
            else
            {
                Debug.LogWarning("Windup killed during wind up period - did not activate.");
            }
        }

        void AssignDamage(Damageable target)
        {
            if (!IsOn || !isActiveAndEnabled || m_DamageReceivers.Contains(target))
                return;
            m_DamageReceivers.Add(target);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent(out Damageable damageable))
            {
                AssignDamage(damageable);
            }
        }

        void OnCollisionStay(Collision collisionInfo)
        {
            if (collisionInfo.collider.TryGetComponent(out Damageable damageable))
            {
                AssignDamage(damageable);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Damageable damageable))
            {
                AssignDamage(damageable);
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out Damageable damageable))
            {
                AssignDamage(damageable);
            }
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.TryGetComponent(out Damageable damageable))
            {
                AssignDamage(damageable);
            }
        }

        void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.collider.TryGetComponent(out Damageable damageable))
            {
                AssignDamage(damageable);
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out Damageable damageable))
            {
                AssignDamage(damageable);
            }
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if (other.TryGetComponent(out Damageable damageable))
            {
                AssignDamage(damageable);
            }
        }
    }
}
