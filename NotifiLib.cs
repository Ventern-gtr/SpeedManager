using System;
using System.Linq;
using BepInEx;
using GorillaLocomotion;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace SpeedManager
{
    // Token: 0x02000004 RID: 4
    [BepInPlugin("com.JizyyLib", "JizzyLib", "2.1")]
    public class NotifiLib : BaseUnityPlugin
    {
        // Token: 0x06000012 RID: 18 RVA: 0x000025E4 File Offset: 0x000007E4
        private static void Init()
        {
            NotifiLib.MainCamera = GameObject.Find("Main Camera");
            NotifiLib.HUDObj = new GameObject();
            NotifiLib.HUDObj2 = new GameObject();
            NotifiLib.HUDObj2.name = "NOTIFICATIONLIB_HUD_OBJ";
            NotifiLib.HUDObj.name = "NOTIFICATIONLIB_HUD_OBJ";
            NotifiLib.HUDObj.AddComponent<Canvas>();
            NotifiLib.HUDObj.AddComponent<CanvasScaler>();
            NotifiLib.HUDObj.AddComponent<GraphicRaycaster>();
            NotifiLib.HUDObj.GetComponent<Canvas>().enabled = true;
            NotifiLib.HUDObj.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
            NotifiLib.HUDObj.GetComponent<Canvas>().worldCamera = NotifiLib.MainCamera.GetComponent<Camera>();
            NotifiLib.HUDObj.GetComponent<RectTransform>().sizeDelta = new Vector2(5f, 5f);
            NotifiLib.HUDObj.GetComponent<RectTransform>().position = new Vector3(NotifiLib.MainCamera.transform.position.x, NotifiLib.MainCamera.transform.position.y, NotifiLib.MainCamera.transform.position.z);
            NotifiLib.HUDObj2.transform.position = new Vector3(NotifiLib.MainCamera.transform.position.x, NotifiLib.MainCamera.transform.position.y, NotifiLib.MainCamera.transform.position.z - 4.6f);
            NotifiLib.HUDObj.transform.parent = NotifiLib.HUDObj2.transform;
            NotifiLib.HUDObj.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 1.6f);
            Vector3 eulerAngles = NotifiLib.HUDObj.GetComponent<RectTransform>().rotation.eulerAngles;
            eulerAngles.y = -270f;
            NotifiLib.HUDObj.transform.localScale = new Vector3(1f, 1f, 1f);
            NotifiLib.HUDObj.GetComponent<RectTransform>().rotation = Quaternion.Euler(eulerAngles);
            NotifiLib.Testtext = new GameObject
            {
                transform =
                {
                    parent = NotifiLib.HUDObj.transform
                }
            }.AddComponent<Text>();
            NotifiLib.Testtext.text = "";
            NotifiLib.Testtext.fontSize = 10;
            NotifiLib.Testtext.font = GameObject.Find("COC Text").GetComponent<Text>().font;
            NotifiLib.Testtext.rectTransform.sizeDelta = new Vector2(260f, 70f);
            NotifiLib.Testtext.alignment = TextAnchor.LowerLeft;
            NotifiLib.Testtext.rectTransform.localScale = new Vector3(0.01f, 0.01f, 1f);
            NotifiLib.Testtext.rectTransform.localPosition = new Vector3(-1.5f, -0.9f, -0.6f);
            NotifiLib.Testtext.material = NotifiLib.AlertText;
            NotifiLib.NotifiText = NotifiLib.Testtext;
        }

        // Token: 0x06000013 RID: 19 RVA: 0x000028D0 File Offset: 0x00000AD0
        private static void FixedUpdate()
        {
            if (!NotifiLib.HasInit && GameObject.Find("Main Camera") != null)
            {
                NotifiLib.Init();
                NotifiLib.HasInit = true;
            }
            NotifiLib.HUDObj2.transform.position = new Vector3(NotifiLib.MainCamera.transform.position.x, NotifiLib.MainCamera.transform.position.y, NotifiLib.MainCamera.transform.position.z);
            NotifiLib.HUDObj2.transform.rotation = NotifiLib.MainCamera.transform.rotation;
            if (NotifiLib.Testtext.text != "")
            {
                NotifiLib.NotificationDecayTimeCounter++;
                if (NotifiLib.NotificationDecayTimeCounter > NotifiLib.NotificationDecayTime)
                {
                    NotifiLib.Notifilines = null;
                    NotifiLib.newtext = "";
                    NotifiLib.NotificationDecayTimeCounter = 0;
                    NotifiLib.Notifilines = NotifiLib.Testtext.text.Split(Environment.NewLine.ToCharArray()).Skip(1).ToArray<string>();
                    foreach (string text in NotifiLib.Notifilines)
                    {
                        if (text != "")
                        {
                            NotifiLib.newtext = NotifiLib.newtext + text + "\n";
                        }
                    }
                    NotifiLib.Testtext.text = NotifiLib.newtext;
                    return;
                }
            }
            else
            {
                NotifiLib.NotificationDecayTimeCounter = 0;
            }
        }

        // Token: 0x06000014 RID: 20 RVA: 0x00002A34 File Offset: 0x00000C34
        public static void SendNotification(string NotificationText)
        {
            if (NotifiLib.IsEnabled)
            {
                if (!NotificationText.Contains(Environment.NewLine))
                {
                    NotificationText += Environment.NewLine;
                }
                NotifiLib.NotifiText.text = NotifiLib.NotifiText.text + NotificationText;
                NotifiLib.PreviousNotifi = NotificationText;
            }
        }

        // Token: 0x06000015 RID: 21 RVA: 0x000020A5 File Offset: 0x000002A5
        public static void ClearAllNotifications()
        {
            NotifiLib.NotifiText.text = "";
        }

        // Token: 0x06000016 RID: 22 RVA: 0x00002A84 File Offset: 0x00000C84
        public static void ClearPastNotifications(int amount)
        {
            string text = "";
            foreach (string text2 in NotifiLib.NotifiText.text.Split(Environment.NewLine.ToCharArray()).Skip(amount).ToArray<string>())
            {
                if (text2 != "")
                {
                    text = text + text2 + "\n";
                }
            }
            NotifiLib.NotifiText.text = text;
        }

        // Token: 0x06000017 RID: 23 RVA: 0x000020B6 File Offset: 0x000002B6
        [HarmonyPatch(typeof(Player), "FixedUpdate")]
        [HarmonyPrefix]
        private static bool FixedUpdateHook()
        {
            NotifiLib.FixedUpdate();
            return true;
        }

        // Token: 0x06000018 RID: 24 RVA: 0x000020BE File Offset: 0x000002BE
        public NotifiLib()
        {
        }

        // Token: 0x06000019 RID: 25 RVA: 0x000020C6 File Offset: 0x000002C6
        // Note: this type is marked as 'beforefieldinit'.
        static NotifiLib()
        {
        }

        // Token: 0x04000015 RID: 21
        private static GameObject MainCamera;

        // Token: 0x04000016 RID: 22
        private static GameObject HUDObj;

        // Token: 0x04000017 RID: 23
        private static GameObject HUDObj2;

        // Token: 0x04000018 RID: 24
        private static Text Testtext;

        // Token: 0x04000019 RID: 25
        private static Material AlertText = new Material(Shader.Find("GUI/Text Shader"));

        // Token: 0x0400001A RID: 26
        private static int NotificationDecayTime = 150;

        // Token: 0x0400001B RID: 27
        private static int NotificationDecayTimeCounter = 0;

        // Token: 0x0400001C RID: 28
        public static int NoticationThreshold = 30;

        // Token: 0x0400001D RID: 29
        private static string[] Notifilines;

        // Token: 0x0400001E RID: 30
        private static string newtext;

        // Token: 0x0400001F RID: 31
        public static string PreviousNotifi;

        // Token: 0x04000020 RID: 32
        private static bool HasInit = false;

        // Token: 0x04000021 RID: 33
        private static Text NotifiText;

        // Token: 0x04000022 RID: 34
        public static bool IsEnabled = true;
    }
}
