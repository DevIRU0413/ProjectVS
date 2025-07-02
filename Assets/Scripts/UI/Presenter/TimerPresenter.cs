using ProjectVS.Interface.UI;
using ProjectVS.UI.Model;

using UnityEngine;

namespace ProjectVS.UI.Presenter
{
    public class TimerPresenter
    {
        private readonly TimerModel model;
        private readonly ITimerView view;

        public TimerPresenter(TimerModel model, ITimerView view)
        {
            this.model = model;
            this.view = view;
        }

        public void StartTimer() => model.Start(); // 타임 온
        public void StopTimer() => model.Stop(); // 타임 스탑
        public void ResetTimer() => model.Reset(); // 타임 리셋

        public bool IsTimeOver() => model.CurrentTime >= model.MaxTime;

        public void Update(float deltaTime)
        {
            model.Update(deltaTime);

            int minutes = Mathf.FloorToInt(model.CurrentTime / 60f);
            int seconds = Mathf.FloorToInt(model.CurrentTime % 60f);

            view.SetTimeText(string.Format("{0:00}:{1:00}", minutes, seconds));
        }
    }
}
