namespace ProjectVS.Interface
{
    interface IGroggyTrackable
    {
        int GroggyThreshold { get; } // 그로기 걸릴 카운트
        bool IsFaild { get; } // 실패 유무
    }
}
