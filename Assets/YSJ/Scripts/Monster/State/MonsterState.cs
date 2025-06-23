namespace PVS.Monster.State
{
    public abstract class MonsterState
    {
        protected MonsterController _controller;

        public MonsterState(MonsterController controller) => _controller = controller;

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void Exit() { }
    }
}
