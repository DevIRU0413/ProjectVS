namespace PVS
{
    public interface IStateLifecycle
    {
        void Enter();
        void Action();
        void Exit();
        void Clear();
    }
}

