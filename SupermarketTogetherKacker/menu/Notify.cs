using System.Collections.Generic;
using UnityEngine;

namespace SupermarketTogetherKacker.menu
{
    public class Notify : MonoBehaviour
    {
        private static List<Notification> notifications = new List<Notification>();
        private const float notifyWidth = 400f;
        private const float notifyHeight = 100f;
        private const int notifyRoundness = 10;
        private const float padding = 10f;
        private const int titleSize = 16;
        private const int statusSize = 12;
        private const float slideSpeed = 0.1f;

        public static bool enabled = true;

        private void OnGUI()
        {
            float startY = Screen.height - 100;

            for (int i = 0; i < notifications.Count; i++)
            {
                Notification notification = notifications[i];
                
                if (Time.time > notification.EndTime + slideSpeed)
                {
                    notifications.RemoveAt(i);
                    i--;
                    continue;
                }
                
                float slideInX = Mathf.Lerp(-notifyWidth, 10, Mathf.Clamp01((Time.time - notification.StartTime) / slideSpeed));
                
                if (Time.time > notification.EndTime)
                {
                    slideInX = Mathf.Lerp(10, -notifyWidth, Mathf.Clamp01((Time.time - notification.EndTime) / slideSpeed));
                }

                if (slideInX > 10) slideInX = 10;

                Rect notifyErection = new Rect(slideInX, startY - (i * (notifyHeight + padding)), notifyWidth, notifyHeight);

                GUIStyle notifyStyle = new GUIStyle(GUI.skin.box)
                {
                    normal = { background = MakeRoundedTexture(notifyRoundness, Color.black) }
                };
                GUI.Box(notifyErection, "", notifyStyle);

                //DrawOutline(notifyErection);

                GUIStyle statusStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = titleSize,
                    fontStyle = FontStyle.Bold,
                    normal = { textColor = GetColor(notification.Type) }
                };
                GUI.Label(new Rect(notifyErection.x + 10, notifyErection.y + 5, notifyWidth - 20, 25), notification.Type.ToString(), statusStyle);

                GUIStyle messageStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = statusSize,
                    normal = { textColor = Color.white }
                };
                GUI.Label(new Rect(notifyErection.x + 10, notifyErection.y + 25, notifyWidth - 20, notifyHeight - 25), notification.Message, messageStyle);
            }
        }

        public static void Send(string message, NotificationType type = NotificationType.Info, float duration = 3f)
        {
            if (enabled)
            {
                notifications.Add(new Notification(message, type, Time.time + duration));
            }
        }

        public static void ClearNotifications()
        {
            notifications.Clear();
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

        private Texture2D MakeTexture(int width, int height, Color color)
        {
            Texture2D texture = new Texture2D(width, height);
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = color;
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }

        private Texture2D MakeRoundedTexture(float radius, Color color)
        {
            int width = Mathf.RoundToInt(radius * 2);
            int height = Mathf.RoundToInt(radius * 2);
            Texture2D tex = new Texture2D(width, height);
            Color[] pixels = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float dx = x - radius;
                    float dy = y - radius;
                    if (dx * dx + dy * dy <= radius * radius)
                    {
                        pixels[y * width + x] = color;
                    }
                    else
                    {
                        pixels[y * width + x] = new Color(0, 0, 0, 0);
                    }
                }
            }
            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }

        private void DrawOutline(Rect rect)
        {
            GUI.color = Color.cyan;
            GUI.DrawTexture(new Rect(rect.x - 2, rect.y - 2, 4-rect.width, 4-rect.height), MakeTexture(1, 1, Color.cyan));
            GUI.color = Color.white;
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
            public float StartTime { get; }

            public Notification(string message, NotificationType type, float endTime)
            {
                Message = message;
                Type = type;
                EndTime = endTime;
                StartTime = Time.time;
            }
        }
    }
}
