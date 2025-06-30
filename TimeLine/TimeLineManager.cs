using DG.Tweening;
using FIMSpace.FProceduralAnimation;
using Main.Core;
using Main.Runtime.Core.Events;
using Main.Runtime.Manager;
using OccaSoftware.RadialBlur.Runtime;
using PJH.Runtime.Core.PlayerCamera;
using PJH.Runtime.Players;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;

namespace KHJ
{
    public class TimeLineManager : MonoSingleton<TimeLineManager>
    {
        private GameEventChannelSO _gameEventChannelSO;

        [SerializeField] private string _sceneName;
        [SerializeField] private PlayableDirector playableDirector;
        public bool isStart { get; private set; }

        [SerializeField] private Volume pp;
        [SerializeField] private VolumeProfile vp;
   private PlayerInputSO inputSO;
        [SerializeField] private PlayerCamera playerCam;
        [SerializeField] private LegsAnimator playerLegAnim;
        [SerializeField] private CanvasGroup gameSceneGroup;
        [SerializeField] private bool isNotSeeCutScene;
        [SerializeField] private TimelineSkipper skiper;

        private Player player;

        #region Volume

        public void BlinkClose(string str)
        {
            string[] value = str.Split(',');
            float endValue = float.Parse(value[0]);
            float duration = float.Parse(value[1]);

            if (pp.profile.TryGet<Beautify.Universal.Beautify>(out var bloom))
            {
                DOTween.To(() => bloom.vignettingBlink.value, x => bloom.vignettingBlink.value = x, endValue, duration);
            }
        }

        public void BlinkCloseOpen(string str)
        {
            string[] value = str.Split(',');
            float endValue = float.Parse(value[0]);
            float duration = float.Parse(value[1]);

            if (pp.profile.TryGet<Beautify.Universal.Beautify>(out var bloom))
            {
                DOTween.To(() => bloom.vignettingBlink.value, x => bloom.vignettingBlink.value = x, endValue, duration)
                    .SetLoops(2, LoopType.Yoyo);
            }
        }

        public void SetFade(string str)
        {
            string[] value = str.Split(',');
            float endValue = float.Parse(value[0]);
            float duration = float.Parse(value[1]);

            if (pp.profile.TryGet<Beautify.Universal.Beautify>(out var bloom))
            {
                DOTween.To(() => bloom.vignettingFade.value, x => bloom.vignettingFade.value = x, endValue, duration);
            }
        }

        public void RadialBlur(float value)
        {
            if (pp.profile.TryGet(out RadialBlurPostProcess radialBlur))
            {
                radialBlur.intensity.value = value;
            }
        }

        #endregion

        private void Awake()
        {
            inputSO = AddressableManager.Load<PlayerInputSO>("PlayerInputSO");
            player = PlayerManager.Instance.Player as Player;
            player.GetCompo<PlayerAnimator>().Animancer.enabled = false;
        }

        private void Start()
        {
            if (!isNotSeeCutScene)
                StartCoroutine(WaitOneFrame());
        }

        private IEnumerator WaitOneFrame()
        {
            yield return null;
            playableDirector.Play();
        }

        public void EnableInputs(bool isEn)
        {
            inputSO.EnablePlayerInput(isEn);
            inputSO.EnableUIInput(isEn);
        }

        public void EndPlayerSet()
        {
            playerLegAnim.enabled = true;
            gameSceneGroup.DOFade(1, 1).SetEase(Ease.Linear);
        }

        public void EnablePlayerCam(bool isEn)
        {
            playerCam.enabled = isEn;
        }

        public void TimelineStart()
        {
            isStart = true;
            playableDirector.Play();
        }

        public void GameEnd()
        {
            pp.profile = vp;
            skiper.TimeLineEnd();
            player.GetCompo<PlayerAnimator>().Animancer.enabled = true;
        }

        public void GameStart()
        {
            _gameEventChannelSO = AddressableManager.Load<GameEventChannelSO>("GameEventChannel");
            GameEvents.SceneChangeEvent.changeSceneName = _sceneName;
            _gameEventChannelSO.RaiseEvent(GameEvents.SceneChangeEvent);
        }
    }
}