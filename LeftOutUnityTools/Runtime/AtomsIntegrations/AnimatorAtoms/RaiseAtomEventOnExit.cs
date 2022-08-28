using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace AtomsIntegrations.AnimatorAtoms
{
    public class RaiseAtomEventOnExit : StateMachineBehaviour
    {
        [SerializeField]
        GameObjectEvent m_Event;

        public override void OnStateExit(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_Event.Raise(animator.gameObject);
        }

    }
}
