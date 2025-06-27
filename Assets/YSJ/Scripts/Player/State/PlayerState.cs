namespace ProjectVS.Player.State
{
    public abstract class PlayerState
    {
        protected PlayerController _controller;

        public PlayerState(PlayerController controller) => _controller = controller;

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void Exit() { }
    }
}
