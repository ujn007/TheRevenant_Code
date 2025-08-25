using DG.Tweening;
using FIMSpace.FProceduralAnimation;
using Main.Core;
using Main.Runtime.Core.Events;
using Main.Runtime.Manager;
using OccaSoftware.RadialBlur.Runtime;
using PJH.Runtime.Core.PlayerCamera;
using PJH.Runtime.Players;
using System.Collections;
using FIMSpace;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using SceneManagerEx = Main.Runtime.Manager.SceneManagerEx;

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
        [SerializeField] private PlayerCamera playerCam;
        [SerializeField] private LegsAnimator playerLegAnim;
        [SerializeField] private LeaningAnimator playerLeaningAnim;
        [SerializeField] private CanvasGroup gameSceneGroup;
        [SerializeField] private bool isNotSeeCutScene;
        [SerializeField] private TimelineSkipper skiper;

        private Player _player;
        private PlayerCamera _playerCamera;

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
            _player = PlayerManager.Instance.Player as Player;
            _playerCamera = PlayerManager.Instance.PlayerCamera;
            playerLeaningAnim.enabled = false;
            playerLegAnim.enabled = false;
            _gameEventChannelSO = AddressableManager.Load<GameEventChannelSO>("GameEventChannel");
        }

        private void Start()
        {
            _player.GetCompo<PlayerAnimator>().Animancer.enabled = false;
            _player.GetCompo<PlayerMovement>().CC.enabled = false;

            if (!isNotSeeCutScene)
                StartCoroutine(WaitOneFrame());
        }

        private IEnumerator WaitOneFrame()
        {
            yield return null;
            SceneManagerEx.Instance.CurrentScene.SettingForTimeline();
            SceneManagerEx.Instance.CurrentScene.VolumeForTimeline();
            playableDirector.Play();
        }

        public void EnableInputs(bool isEn)
        {
            _player.PlayerInput.EnablePlayerInput(isEn);
            _player.PlayerInput.EnableUIInput(isEn);
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
            SceneManagerEx.Instance.CurrentScene.SettingForEndTimeline();
            SceneManagerEx.Instance.CurrentScene.VolumeForEndTimeline();
            pp.profile = vp;
            skiper.TimeLineEnd();
            _player.transform.localPosition = Vector3.zero;
            _player.GetCompo<PlayerMovement>().CC.enabled = true;
            playerLeaningAnim.User_AfterTeleport();
            playerLeaningAnim.enabled = true;
            _player.GetCompo<PlayerAnimator>().Animancer.enabled = true;
            _player.ModelTrm.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            _playerCamera.enabled = true;
            _gameEventChannelSO.RaiseEvent(GameEvents.StartTutorial);
        }

        public void GameStart()
        {
            GameEvents.SceneChangeEvent.changeSceneName = _sceneName;
            _gameEventChannelSO.RaiseEvent(GameEvents.SceneChangeEvent);
        }
    }
}