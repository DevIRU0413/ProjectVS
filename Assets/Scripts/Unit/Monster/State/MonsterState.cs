using System;

using UnityEngine;

namespace ProjectVS.Monster.State
{
    public abstract class MonsterState
    {
        protected MonsterController controller;
        protected Animator animator;

        public bool UseFixedUpdate = false;

        public MonsterState(MonsterController controller, Animator animator)
        {
            this.controller = controller;
            this.animator = animator;
            Init();
        }

        protected virtual void Init() { }

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void Exit() { }
    }
}
