using LJS.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KHJ.UI
{
    public class RoomUI : MonoBehaviour
    {
        public RectTransform Rect => transform as RectTransform;
        [SerializeField] private Image outLine;
        [ColorUsage(true)] [SerializeField] private Color roomColor;

        private Dictionary<SpecialRoomType, Image> typeImageDic = new();
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();

            Initialize();
        }

        private void Start()
        {
            SetColorUI(false);
        }

        public void Initialize()
        {
            foreach (SpecialRoomType roomType in Enum.GetValues(typeof(SpecialRoomType)))
            {
                if (roomType == SpecialRoomType.None) continue;

                string iconName = roomType.ToString() + "RoomIcon";
                Image image = transform.Find(iconName).GetComponent<Image>();
                image.enabled = false;
                typeImageDic[roomType] = image;
            }
        }

        public void SetColorUI(bool v)
        {
            Color color = v ? Color.white : roomColor;
            image.color = color;
        }

        public void SetStartRoomOutLine(bool v)
        {
            print("켜ㅛ졌닫다다다다ㅏ다다다다ㅏ다다ㅏ");
            print(outLine);
            print(v);
            outLine.enabled = v;
        }

        public void ResetIcons()
        {
            foreach (var item in typeImageDic)
            {
                item.Value.enabled = false;
            }
        }

        public void SetTypeIcon(SpecialRoomType type)
        {
            if (type == SpecialRoomType.None) return;
            print(type.ToString());
            typeImageDic[type].enabled = true;
        }
    }
}
