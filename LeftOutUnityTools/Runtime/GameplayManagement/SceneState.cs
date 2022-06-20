using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace LeftOut.GameplayManagement
{
    public enum LevelStateType
    {
        Unknown,
        NotStarted,
        Active,
        Paused,
        Complete
    }

    [CreateAssetMenu(fileName = "LevelState", menuName = "Left Out/Level State", order = 0)]
    public class SceneState : StateMachine<LevelStateType>
    {
        enum LevelEndType
        {
            Unknown,
            Exited,
            Success,
            Failed
        }

        [SerializeField]
        bool m_DEBUG_StartLevel;
        [SerializeField]
        bool m_DEBUG_Reset;

        [SerializeField]
        LevelStateType m_DEBUG_CurrentState;

        protected override LevelStateType DefaultState => LevelStateType.Unknown;

        void OnValidate()
        {
            m_DEBUG_CurrentState = Current;
            if (m_DEBUG_StartLevel)
            {
                Debug.Log("Transitioning to Level Start...");
                TryTransitionTo(LevelStateType.Active);
                m_DEBUG_StartLevel = false;
            }

            if (m_DEBUG_Reset)
            {
                Reset();
                m_DEBUG_Reset = false;
            }
        }

        public void AutoBind(UnityEngine.Object obj)
        {
            if (obj is IDeactivateOnPause deactivateOnPause)
            {
                Debug.Log($"Binding {obj.name} to pause/unpause.");
                BindSpecificTransition(LevelStateType.Active, LevelStateType.Paused,
                    () => deactivateOnPause.DeactivateThisOnPause.enabled = false);
                BindSpecificTransition(LevelStateType.Paused, LevelStateType.Active,
                    () => deactivateOnPause.DeactivateThisOnPause.enabled = true);
            }

            if (obj is IBindToLevelStart bindToLevelStart)
            {
                Debug.Log($"Binding {obj.name} to level start.");
                BindSpecificTransition(LevelStateType.NotStarted, LevelStateType.Active,
                    bindToLevelStart.OnLevelStart);
            }
        }
    }
}
