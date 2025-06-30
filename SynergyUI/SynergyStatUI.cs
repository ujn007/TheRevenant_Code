using Main.Runtime.Core.Events;
using Main.Runtime.Core.StatSystem;
using Main.Runtime.Manager;
using PJH.Runtime.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Main.Core;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace KHJ
{
    public class SynergyStatUI : MonoBehaviour
    {
        private GameEventChannelSO eventSO;
        [SerializeField] private StatOverrideListSO statOverrideListSO;
        [SerializeField] private TextMeshProUGUI statTextUI;
        private StatSO statSO;

        [SerializeField] private TextMeshProUGUI blockInfoText;


        private void Awake()
        {
            eventSO = AddressableManager.Load<GameEventChannelSO>("GameEventChannel");

            eventSO.AddListener<ModifyPlayerStat>(HandleIncreasePlayerStat);
            eventSO.AddListener<CurrentBlockInfo>(HandleCurrentBlockInfo);
        }

        private void OnDestroy()
        {
            eventSO.RemoveListener<ModifyPlayerStat>(HandleIncreasePlayerStat);
            eventSO.RemoveListener<CurrentBlockInfo>(HandleCurrentBlockInfo);
        }

        private void HandleIncreasePlayerStat(ModifyPlayerStat stat)
        {
            Debug.LogError($"rrrr");

            StartCoroutine(IncreaseStatCor());
        }

        private void HandleCurrentBlockInfo(CurrentBlockInfo info)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"단계 : {info.grade}");
            sb.AppendLine();
            sb.Append($"증가 스탯 : {info.stat.DisplayName}");
            sb.AppendLine();
            sb.Append($"증가 수치 : {info.statValue}");

            blockInfoText.text = sb.ToString();
        }

        private IEnumerator IncreaseStatCor()
        {
            yield return null;

            Player player = PlayerManager.Instance.Player as Player;
            PlayerStat statCompo = player.GetCompo<PlayerStat>();
            List<StatOverride> statOverride = statOverrideListSO.StatOverrides;
            StringBuilder sb = new StringBuilder();

            statOverride.ForEach(x =>
            {
                statSO = statCompo.GetStat(x.CreateStat());
                sb.Append($"{statSO.DisplayName} : {statSO.Value} + ");

                float v = Mathf.Round((statSO.Value - statSO.BaseValue) * 10f) / 10f;
                if (!statSO.IsMax) sb.Append($"(<color=#00FF00>{(v).ToString("F1")}</color>)");
                else sb.Append($"(<color=#00FF00>MAX</color>)");

                sb.AppendLine();
            });
            Debug.LogError(sb.ToString());
            statTextUI.text = sb.ToString();
        }
    }
}