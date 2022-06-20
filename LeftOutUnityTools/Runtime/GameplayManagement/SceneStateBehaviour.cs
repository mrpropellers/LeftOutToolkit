using UnityEngine;

namespace LeftOut.GameplayManagement
{
    // TODO? Maybe this shouldn't be a Singleton
    public class SceneStateBehaviour : SingletonBehaviour<SceneStateBehaviour>
    {
        [SerializeField]
        SceneState m_State;

        public SceneState State
        {
            get
            {
                if (m_State == null)
                {
                    m_State = ScriptableObject.CreateInstance<SceneState>();
                    m_State.TryTransitionTo(LevelStateType.NotStarted);
                }
                return m_State;
            }
            private set => m_State = value;
        }
    }
}
