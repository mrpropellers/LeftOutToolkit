using UnityEngine;

namespace LeftOut.GameplayManagement
{
    // TODO? Maybe this shouldn't be a Singleton
    public class SceneStateBehaviour : SingletonBehaviour<SceneStateBehaviour>
    {
        [SerializeField]
        SceneStateMachine m_State;

        public SceneStateMachine State
        {
            get
            {
                if (m_State == null)
                {
                    Debug.LogWarning($"No {nameof(SceneStateMachine)} set - creating a blank one.");
                    m_State = ScriptableObject.CreateInstance<SceneStateMachine>();
                }
                return m_State;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            InitializeState();
        }

        void InitializeState()
        {
            Debug.Log("Initializing Scene state.");
            m_State.Initialize();
            m_State.TryTransitionTo(SceneState.NotStarted);
        }
    }
}
