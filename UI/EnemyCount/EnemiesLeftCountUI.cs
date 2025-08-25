using Main.Core;
using Main.Runtime.Core.Events;
using Main.Runtime.Manager;
using Main.Shared;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace KHJ.UI
{
    public class EnemiesLeftCountUI : MonoBehaviour
    {
        private GameEventChannelSO _gameEventChannel;
        [SerializeField] private TextMeshProUGUI _enemyCountText;
        [SerializeField] private OffScreenIndicatorUI _offScreenIndicator;
        private int _enemyMaxCount;

        private void Awake()
        {
            _gameEventChannel = AddressableManager.Load<GameEventChannelSO>("GameEventChannel");

        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => SceneManagerEx.Instance.CurrentScene != null);
            IBattleScene battle = (SceneManagerEx.Instance.CurrentScene as IBattleScene);
            battle.OnChangedBattleZoneController += Init;
            yield return new WaitUntil(() => battle.CurrentBattleZoneController != null);
            Init(currentBattleZoneController: battle.CurrentBattleZoneController);
        }

        private void Init(IBattleZoneController prevBattleZoneController = null, IBattleZoneController currentBattleZoneController = null)
        {
            if (prevBattleZoneController != null)
            {
                prevBattleZoneController.OnChangedRemainingEnemy -= HandleChangeEnemyEvent;
            }

            currentBattleZoneController.OnChangedRemainingEnemy += HandleChangeEnemyEvent;
            _enemyMaxCount = currentBattleZoneController.RemainingEnemy;
            Debug.Log($"적 개수 : {currentBattleZoneController.RemainingEnemy}");
            HandleChangeEnemyEvent(_enemyMaxCount);
        }

        private void HandleChangeEnemyEvent(int cnt)
        {
            Debug.Log($"적 바뀜 : {cnt}");

            if (cnt <= 0)
            {
                _enemyCountText.text = $"<s>남은 적 수 : 0 / {_enemyMaxCount}</s>";
                _enemyCountText.color = Color.gray;

                _offScreenIndicator.EnableImage(true);
            }
            else
            {
                _enemyCountText.text = $"남은 적 수 : {cnt.ToString()} / {_enemyMaxCount}";
                _enemyCountText.color = Color.white;
            }
        }
    }
}
