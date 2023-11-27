using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public interface IState
    {
        public void OnEnter();
        public void OnExit();
        public void OnHandleInput();
        public void OnUpdate();
        public void OnFixUpdate();
        public void OnTriggerEnter(Collider collider);
        public void OnTriggerExit(Collider collider);
        public void OnAnimationEnterEvent();
        public void OnAnimationExitEvent();
        public void OnAnimationTransitionEvent();
    }

