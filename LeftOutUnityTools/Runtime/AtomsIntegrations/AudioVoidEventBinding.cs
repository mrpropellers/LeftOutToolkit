using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LeftOut.Atoms
{
    public class AudioVoidEventBinding : MonoBehaviour
    {
        [System.Serializable]
        class EventClipBinding
        {
            int ClipIndex = 0;
            public VoidEvent Event;
            public AudioSource AudioSource;
            public AudioClip[] AudioClips;
            [Range(0f, 2f)]
            public float BaseVolume = 1f;
            [Range(0f, 0.5f)]
            public float VolumeVariance;
            [Range(0f, 4f)]
            public float BasePitch = 1f;
            [Range(0f, 2.0f)]
            public float PitchVariance;

            public AudioClip NextClip
            {
                get
                {
                    var clip = AudioClips[ClipIndex];
                    ClipIndex++;
                    if (ClipIndex >= AudioClips.Length)
                    {
                        ClipIndex = 0;
                    }

                    return clip;
                }
            }
        }

        [SerializeField]
        EventClipBinding[] m_Bindings;

        void Awake()
        {
            var numAdded = 0;
            for (var i = 0; i < m_Bindings.Length; i++)
            {
                var binding = m_Bindings[i];
                if (binding.Event == null || binding.AudioClips.Length == 0)
                    continue;
                var clipNum = i;
                binding.Event.Register(() => PlayClip(clipNum));
                numAdded++;
            }

            if (numAdded < m_Bindings.Length)
            {
                Debug.LogWarning($"Only added {numAdded} of {m_Bindings.Length} bindings", this);
            }

        }

        void PlayClip(int bindingIndex)
        {
            var binding = m_Bindings[bindingIndex];
            // >>> TODO: Handle pitch/volume variance
            binding.AudioSource.pitch =
                binding.BasePitch + Random.Range(-binding.PitchVariance, binding.PitchVariance);
            binding.AudioSource.PlayOneShot(binding.NextClip,
                binding.BaseVolume + Random.Range(-binding.VolumeVariance, binding.VolumeVariance));
        }
    }
}
