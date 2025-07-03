using System.Collections;

using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;



namespace ProjectVS.Unit.Monster
{
    public class MonsterAnimationPlayer
    {
        private MonoBehaviour _runner;
        private PlayableGraph _graph;
        private AnimationClipPlayable _playableClip;
        private bool _isPlayingOverride = false;
        private bool _loop = false;
        private float _clipLength = 0f;
        private Coroutine _coroutine;

        public Animator Animator { get; private set; }

        public MonsterAnimationPlayer(Animator animator, MonoBehaviour runner)
        {
            Animator = animator;
        }


        public void PlayClip(AnimationClip clip, bool loop = false)
        {
            if (clip == null || Animator == null) return;
            if (_isPlayingOverride) return;

            _isPlayingOverride = true;
            _loop = loop;
            _clipLength = clip.length;

            _graph = PlayableGraph.Create("TempClipGraph");

            _playableClip = AnimationClipPlayable.Create(_graph, clip);
            _playableClip.SetApplyFootIK(false);
            _playableClip.SetApplyPlayableIK(false);
            _playableClip.SetTime(0f);
            _playableClip.SetSpeed(1f);

            var output = AnimationPlayableOutput.Create(_graph, "Animation", Animator);
            output.SetSourcePlayable(_playableClip);

            _graph.Play();

            _coroutine = _runner.StartCoroutine(LoopChecker());
        }

        private IEnumerator LoopChecker()
        {
            while (_isPlayingOverride)
            {
                if (!_loop) break;

                if (_playableClip.IsValid())
                {
                    double time = _playableClip.GetTime();
                    if (time >= _clipLength - Time.deltaTime * 2)
                    {
                        _playableClip.SetTime(0f);
                    }
                }

                yield return null;
            }

            if (!_loop)
            {
                yield return new WaitForSeconds(_clipLength);
                Stop(); // 자동 정지
            }
        }

        public void Stop()
        {
            if (!_isPlayingOverride) return;

            _isPlayingOverride = false;

            if (_coroutine != null)
            {
                _runner.StopCoroutine(_coroutine);
                _coroutine = null;
            }

            if (_graph.IsValid())
                _graph.Destroy();
        }
    }
}
