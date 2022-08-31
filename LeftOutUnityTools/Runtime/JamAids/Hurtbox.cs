using System;
using System.Collections;
using System.Collections.Generic;
using LeftOut.Interfaces;
using UnityEngine;

namespace LeftOut
{
    public class Hurtbox : MonoBehaviour, IHaveOwner
    {
        List<Damageable> m_DamageReceivers;

        [field: SerializeField]
        public GameObject Owner { get; internal set; }
        [SerializeField]
        int DamageAmount = 1;

        bool IsOn { get; set; }

        void Start()
        {
            m_DamageReceivers = new List<Damageable>();
        }

        void LateUpdate()
        {
            if (!IsOn) return;

            // We only want to assign damage to any given receiver once per frame, even if we've had many collisions
            foreach (var damageable in m_DamageReceivers)
            {
                damageable.ReceiveDamage(gameObject, DamageAmount);
            }

            m_DamageReceivers.Clear();
        }

        public void Activate()
        {
            m_DamageReceivers.Clear();
            IsOn = true;
        }

        public void Deactivate()
        {
            //Debug.Assert(m_IsWindingUp || m_IsOn);
            IsOn = false;
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
