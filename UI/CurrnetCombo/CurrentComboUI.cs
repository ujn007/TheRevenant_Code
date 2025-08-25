using BIS.Data;
using Main.Core;
using Main.Runtime.Manager;
using PJH.Runtime.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace KHJ.UI
{
    public class CurrentComboUI : MonoBehaviour
    {
        public RectTransform rect => transform as RectTransform;

        [SerializeField] private TextMeshProUGUI _numberText;

        private CanvasGroup _canvasGroup; public CanvasGroup CanvasGroup { get { return _canvasGroup; } set { _canvasGroup = value; } }
        private CurrentEquipComboSO _currentEquipComboSO;
        private List<ComboPieceUI> _comboPieceList = new();
        private PlayerAttack _playerAttack;

        private void Awake()
        {
            _currentEquipComboSO = AddressableManager.Load<CurrentEquipComboSO>("CurrentEquipComboSO");
            _canvasGroup = GetComponent<CanvasGroup>();
            _comboPieceList = GetComponentsInChildren<ComboPieceUI>().ToList();

            _playerAttack = (PlayerManager.Instance.Player as Player).GetCompo<PlayerAttack>();
            _playerAttack.OnUseCommandActionPiece += HandleUseComboEvent;
        }

        private void OnDestroy()
        {
            _playerAttack.OnUseCommandActionPiece -= HandleUseComboEvent;
            
        }

        public void DestroyCombo()
        {
            _playerAttack.OnUseCommandActionPiece -= HandleUseComboEvent;
        }

        private void HandleUseComboEvent(CommandActionPieceSO comSO)
        {
            for (int i = 0; i < _comboPieceList.Count; i++)
            {
                if (_comboPieceList[i].CurrentPieceSO == comSO)
                {
                    _comboPieceList[i].HighLightIcon();
                }
            }
        }

        public void SetComboIcon(int index)
        {
            _numberText.text = (index + 1).ToString();

           print($"현재 콤보 개수 : {_currentEquipComboSO.CurrentEquipCommandSOs[index].CommandActionPieceSOs.Count}");
            for (int i = 0; i < _comboPieceList.Count; i++)
            {
                CommandActionPieceSO piece = _currentEquipComboSO.CurrentEquipCommandSOs[index].CommandActionPieceSOs[i];

                _comboPieceList[i].Init(piece);
            }
        }
    }
}