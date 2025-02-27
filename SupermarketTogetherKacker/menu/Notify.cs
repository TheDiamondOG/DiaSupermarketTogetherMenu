using System.Collections.Generic;
using UnityEngine;

namespace SupermarketTogetherKacker.menu
{
    public class Notify : MonoBehaviour
    {
        private static List<Notification> notifications = new List<Notification>();
        private const float boxWidth = 300f;
        private const float boxHeight = 50f;
        private const float padding = 10f;
        private const int fontSize = 18;

        private void OnGUI()
        {
            float startY = Screen.height - 100;

            for (int i = 0; i < notifications.Count; i++)
            {
                Notification notification = notifications[i];
                if (Time.time > notification.EndTime)
                {
                    notifications.RemoveAt(i);
                    i--;
                    continue;
                }

                Rect boxRect = new Rect(10, startY - (i * (boxHeight + padding)), boxWidth, boxHeight);
                GUI.Box(boxRect, $"[{notification.Type}] {notification.Message}", GetStyle(notification.Type));
            }
        }

        public static void Send(string message, NotificationType type = NotificationType.Info, float duration = 3f)
        {
            notifications.Add(new Notification(message, type, Time.time + duration));
        }

        public static void ClearNotifications()
        {
            notifications.Clear();
        }

        private GUIStyle GetStyle(NotificationType type)
        {
            GUIStyle style = new GUIStyle(GUI.skin.box)
            {
                fontSize = fontSize,
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                normal = { textColor = GetColor(type) }
            };
            return style;
        }

        private Color GetColor(NotificationType type)
        {
            return type switch
            {
                NotificationType.Info => Color.cyan,
                NotificationType.Warning => Color.yellow,
                NotificationType.Error => Color.red,
                NotificationType.Success => Color.green,
                _ => Color.white
            };
        }

        public enum NotificationType
        {
            Info,
            Warning,
            Error,
            Success
        }

        private class Notification
        {
            public string Message { get; }
            public NotificationType Type { get; }
            public float EndTime { get; }

            public Notification(string message, NotificationType type, float endTime)
            {
                Message = message;
                Type = type;
                EndTime = endTime;
            }
        }
    }
}
