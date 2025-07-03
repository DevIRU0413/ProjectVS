namespace ProjectVS.Interface
{
    public interface IPatternLifecycle
    {
        void Enter();
        void Action();
        void Exit();
        void Clear();
    }
}

