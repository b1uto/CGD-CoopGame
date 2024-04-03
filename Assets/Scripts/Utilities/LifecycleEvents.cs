using UnityEngine.Events;
using UnityEngine;

namespace CGD.Events
{
    public class LifecycleEvents : MonoBehaviour
    {
        public UnityEvent OnAwake = new UnityEvent();
        public UnityEvent OnStart = new UnityEvent();
        public UnityEvent OnEnabled = new UnityEvent();
        public UnityEvent OnDisabled = new UnityEvent();

        private void Awake()
        {
            OnAwake?.Invoke();
        }

        private void OnEnable()
        {
            OnEnabled?.Invoke();
        }
        private void OnDisable()
        {
            OnDisabled?.Invoke();
        }

        private void Start()
        {
            OnStart?.Invoke();
        }
    }
}
