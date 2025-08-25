using DG.Tweening;
using PJH.Runtime.Players;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace KHJ.UI
{
    public class ComboPieceUI : MonoBehaviour
    {
        [ColorUsage(true, false)] [SerializeField]
        private Color _prevColor;

        [ColorUsage(true, false)] [SerializeField]
        private Color _changeColor;

        [SerializeField] private float _changeScale;
        [SerializeField] private float _changeDuration;

        private Image _comboImage;

        public Image ComboImage
        {
            get { return _comboImage; }
            set { _comboImage = value; }
        }

        private CommandActionPieceSO _currentPieceSO;

        public CommandActionPieceSO CurrentPieceSO
        {
            get { return _currentPieceSO; }
        }

        private Transform _frame;

        private void Awake()
        {
            _comboImage = GetComponent<Image>();
        }

        private void Start()
        {
            _comboImage.color = _prevColor;
        }

        public void Init(CommandActionPieceSO piece)
        {
            _comboImage.DOFade(piece == null ? 0 : 1, 0).SetLink(_comboImage.gameObject);

            if (piece == null) return;

            _currentPieceSO = piece;
            _comboImage.sprite = piece.pieceIcon;
        }

        public void HighLightIcon()
        {
            _frame = transform.parent.parent;

            _comboImage.color = _changeColor;
            _frame.localScale = Vector3.one * _changeScale;

            _comboImage.DOColor(_prevColor, _changeDuration).SetLink(_comboImage.gameObject);
            _frame.DOScale(Vector3.one, _changeDuration).SetLink(_frame.gameObject);
        }
    }
}