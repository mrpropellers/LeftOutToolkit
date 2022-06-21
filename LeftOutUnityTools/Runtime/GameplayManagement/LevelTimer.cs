using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace LeftOut.GameplayManagement
{
    public class LevelTimer : SingletonBehaviour<LevelTimer>, IDeactivateOnPause
    {
        [SerializeField]
        float m_CompletionTime;

        [SerializeField]
        AnimationCurve m_DifficultyCurve;

        [field: SerializeField]
        public UnityEvent OnTimerComplete;

        [SerializeField]
        EventChannel m_StartChannel;

        public bool TimerHasFinished { get; private set; }
        [field: SerializeField]
        public float ElapsedTime { get; private set; }
        public bool HasStarted { get; private set; }
        public bool IsRunning { get; private set; }
        public bool CanBeStarted => !HasStarted && !(TimerHasFinished || IsRunning);
        public float CompletionPercentage => Mathf.Clamp01(ElapsedTime / m_CompletionTime);
        public float CurrentDifficulty => Mathf.Clamp01(m_DifficultyCurve.Evaluate(CompletionPercentage));

        public MonoBehaviour DeactivateThisOnPause => this;

        protected override void Awake()
        {
            base.Awake();
            m_StartChannel.OnEvent.AddListener(HandleStartEvent);
            ElapsedTime = 0f;
        }

        public void StartTimer()
        {
            Assert.IsFalse(IsRunning);
            Assert.IsFalse(HasStarted);
            HasStarted = true;
            IsRunning = true;
        }

        void Update()
        {
            if (IsRunning)
            {
                Assert.IsFalse(TimerHasFinished);
                ElapsedTime += Time.deltaTime;
                if (ElapsedTime >= m_CompletionTime)
                {
                    BroadcastTimeUp();
                }
            }
        }

        void BroadcastTimeUp()
        {
            IsRunning = false;
            TimerHasFinished = true;
            OnTimerComplete?.Invoke();
        }

        void HandleStartEvent()
        {
            if (!CanBeStarted)
            {
                Debug.LogError("Start event was raised after we already started.");
                return;
            }

            StartTimer();
        }
    }
}
