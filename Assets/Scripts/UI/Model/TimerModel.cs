namespace ProjectVS.UI.Model
{
    public class TimerModel
    {
        public float CurrentTime { get; private set; }
        public float MaxTime { get; private set; }
        public bool IsRunning { get; private set; }

        public TimerModel(float maxTime) => MaxTime = maxTime;

        public void Start() => IsRunning = true;
        public void Stop() => IsRunning = false;
        public void Reset()
        {
            CurrentTime = 0f;
            IsRunning = false;
        }

        public void Update(float deltaTime)
        {
            if (IsRunning)
            {
                CurrentTime += deltaTime;
                if (MaxTime >= CurrentTime)
                {
                    CurrentTime = MaxTime;
                    IsRunning = false;
                }
            }
        }
    }
}
