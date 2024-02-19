using UnityEngine;
using UnityEngine.Events;

namespace Tools.Interaction
{
    public class Interactable : MonoBehaviour
    {
        public UnityEvent<Interactor> OnInteract;
        public UnityEvent OnFocus;
        public UnityEvent OnLostFocus;

        public void Interact(Interactor interactor)
        {
            OnInteract.Invoke(interactor);
        }

        public void Focus()
        {
            OnFocus.Invoke();
        }

        public void LostFocus()
        {
            OnLostFocus.Invoke();
        }
    }

}