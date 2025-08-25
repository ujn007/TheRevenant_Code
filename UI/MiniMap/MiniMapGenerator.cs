using LJS.Map;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KHJ.UI
{
    public class MiniMapGenerator : MonoBehaviour
    {
        [SerializeField] private RoomUI roomImage;
        [SerializeField] private Image roomLine;
        [SerializeField] private RectTransform mapGroup;
        [SerializeField] private float interval;

        private Dictionary<(int x, int y), RoomUI> roomUIDic = new();

        private RoomManager roomManager => RoomManager.Instance;
        private RoomGenrator roomGenrator;

        private RoomUI beforeRoomUI;

        private void Awake()
        {
            roomGenrator = FindFirstObjectByType<RoomGenrator>();
            roomManager.CurrentRoomChangeActon += HandleChangeCurrentRoomEvent;
        }

        private void Start()
        {
            GenerateMapRoom();
            Initialize();
        }

        private void OnDestroy()
        {
            roomManager.CurrentRoomChangeActon -= HandleChangeCurrentRoomEvent;
        }

        private void Initialize()
        {
            RoomUI roomUI = roomUIDic[(4, 4)];
            roomUI.SetStartRoomOutLine(true);
            mapGroup.position += -(roomUI.Rect.position - mapGroup.position);
        }

        private void HandleChangeCurrentRoomEvent(RoomComponent roomCompo)
        {
            MapInfo info = roomCompo.Info;

            RoomUI roomUI = roomUIDic[(info.x, info.y)];
            roomUI.SetColorUI(true);

            if (beforeRoomUI)
                mapGroup.position += -(roomUI.Rect.position - beforeRoomUI.Rect.position);

            beforeRoomUI = roomUIDic[(info.x, info.y)];
        }

        private void GenerateMapRoom()
        {
            roomUIDic.Clear();

            int minX = int.MaxValue, minY = int.MaxValue;
            int maxX = int.MinValue, maxY = int.MinValue;

            print(roomGenrator.SpawnedRoomList);
            foreach (MapInfo map in roomGenrator.SpawnedRoomList)
            {
                if (map.x < minX) minX = map.x;
                if (map.y < minY) minY = map.y;
                if (map.x > maxX) maxX = map.x;
                if (map.y > maxY) maxY = map.y;
            }

            RectTransform sampleTrm = roomImage.transform as RectTransform;
            float roomWidth = sampleTrm.sizeDelta.x + interval;
            float roomHeight = sampleTrm.sizeDelta.y + interval;

            float offsetX = ((maxX + minX) / 2f) * roomWidth;
            float offsetY = ((maxY + minY) / 2f) * roomHeight;

            foreach (MapInfo map in roomGenrator.SpawnedRoomList)
            {
                RoomUI roomUI = Instantiate(roomImage, mapGroup);
                RectTransform trm = roomUI.transform as RectTransform;
                RoomComponent roomCompo = roomManager.FindRoomComponent(map);

                float posX = (map.x * roomWidth) - offsetX;
                float posY = (map.y * roomHeight) - offsetY;

                roomUI.SetTypeIcon(roomCompo.SpecialRoomType);

                trm.anchoredPosition = new Vector3(posX, posY, 0);
                roomUIDic[(map.x, map.y)] = roomUI;
            }

            DrawRoomConnections();
        }


        private void DrawRoomConnections()
        {
            foreach (var kvp in roomUIDic)
            {
                var (x, y) = kvp.Key;
                RectTransform fromTrm = kvp.Value.transform as RectTransform;

                (int dx, int dy)[] directions = { (1, 0), (0, 1) };

                foreach (var (dx, dy) in directions)
                {
                    var neighborKey = (x + dx, y + dy);
                    if (roomUIDic.TryGetValue(neighborKey, out RoomUI toTrm))
                    {
                        DrawLineBetween(fromTrm, toTrm.transform as RectTransform);
                    }
                }
            }
        }

        private void DrawLineBetween(RectTransform from, RectTransform to)
        {
            Vector3 start = from.anchoredPosition;
            Vector3 end = to.anchoredPosition;

            Vector3 direction = end - start;
            float distance = direction.magnitude;

            Image line = Instantiate(roomLine, mapGroup);
            RectTransform lineTrm = line.rectTransform;

            lineTrm.sizeDelta = new Vector2(distance, lineTrm.sizeDelta.y);
            lineTrm.anchoredPosition = start + direction / 2;
            lineTrm.localRotation = Quaternion.FromToRotation(Vector3.right, direction.normalized);
            line.transform.SetAsFirstSibling();
        }
    }
}
