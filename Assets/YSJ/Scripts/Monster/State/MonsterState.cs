namespace ProjectVS.Monster.State
{
    public abstract class MonsterState
    {
        protected MonsterController controller;

        public MonsterState(MonsterController controller) => this.controller = controller;

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void Exit() { }
    }
}
