using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using BepInEx;
using GorillaLocomotion;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace SpeedManager
{
    // Token: 0x02000001 RID: 1
    [BepInPlugin("com.JizzyCameraMod", "JizzyCameraMod", "4.2.0.3")]
    public class Plugin : BaseUnityPlugin
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public Plugin()
        {
        }

        // Token: 0x06000002 RID: 2 RVA: 0x00002100 File Offset: 0x00000300
        public void Update()
        {
            float r = MathF.Abs(Mathf.Sin(this.colorTimer * 0.4f));
            float g = MathF.Abs(Mathf.Sin(this.colorTimer * 0.5f));
            float b = MathF.Abs(Mathf.Sin(this.colorTimer * 0.6f));
            Plugin.color = new UnityEngine.Color(r, g, b);
            this.colorTimer += Time.smoothDeltaTime;
            if (!File.Exists(Plugin.flagFilePath))
            {
                UnityEngine.Application.OpenURL(Plugin.url);
                File.Create(Plugin.flagFilePath).Close();
            }
            if (UnityInput.Current.GetKey(KeyCode.Insert) && this.canToggle)
            {
                Plugin.IsOpen = !Plugin.IsOpen;
                base.StartCoroutine(this.ToggleCooldown());
            }
        }

        // Token: 0x06000003 RID: 3 RVA: 0x000021CC File Offset: 0x000003CC
        static Plugin()
        {
            Plugin.Notification = true;
            Plugin.DefaultText = "<color=white>[</color><color=red>MANAGER</color><color=white>] </color>DEFAULT SPEED SET!<color=red></color>";
            Plugin.VeryLegitText = "<color=white>[</color><color=red>MANAGER</color><color=white>] </color>VERY LEGIT SPEED SET!<color=red></color>";
            Plugin.SemiLegitText = "<color=white>[</color><color=red>MANAGER</color><color=white>] </color>SEMI LEGIT SPEED SET!<color=red></color>";
            Plugin.BlatantText = "<color=white>[</color><color=red>MANAGER</color><color=white>] </color>BLATANT SPEED SET!<color=red></color>";
            Plugin.VeryBlatantText = "<color=white>[</color><color=red>MANAGER</color><color=white>] </color>VERY BLATANT SPEED SET!<color=red></color>";
            Plugin.Label = "DEFAULT";
            Plugin.appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Plugin.flagFilePath = Path.Combine(Plugin.appDirectory, "fileran.txt");
            Plugin.url = "https://guns.lol/ventern";
        }

        // Token: 0x06000004 RID: 4 RVA: 0x00002258 File Offset: 0x00000458
        public void OnGUI()
        {
            GUI.color = Plugin.color;
            GUI.contentColor = Plugin.color;
            GUI.backgroundColor = Plugin.color;
            GUI.skin.label.fontStyle = FontStyle.Bold;
            GUI.skin.button.fontStyle = FontStyle.Bold;
            GUI.skin.label.fontSize = 15;
            if (Plugin.IsOpen)
            {
                GUI.Label(new Rect(35f, 25f, 250f, 250f), "SPEED MANAGER V1.1");
                GUI.Label(new Rect(25f, 200f, 250f, 250f), "SPEED: " + Plugin.Label);
                GUI.Label(new Rect(25f, 220f, 250f, 250f), "BY VENTERN");
                GUI.Label(new Rect(45f, 250f, 250f, 250f), "INS TO TOGGLE GUI");
                GUI.Box(new Rect(20f, 20f, 200f, 230f), " ");
                if (GUI.Button(new Rect(25f, 50f, 190f, 20f), "DEFAULT"))
                {
                    Player.Instance.maxJumpSpeed = 6.5f;
                    Player.Instance.jumpMultiplier = 1.1f;
                    Plugin.Label = "DEFAULT";
                    NotifiLib.SendNotification(Plugin.DefaultText);
                }
                if (GUI.Button(new Rect(25f, 80f, 190f, 20f), "VERY LEGIT"))
                {
                    Player.Instance.maxJumpSpeed = 6.7f;
                    Player.Instance.jumpMultiplier = 1.2f;
                    Plugin.Label = "VERY LEGIT";
                    NotifiLib.SendNotification(Plugin.VeryLegitText);
                }
                if (GUI.Button(new Rect(25f, 110f, 190f, 20f), "SEMI-LEGIT"))
                {
                    Player.Instance.maxJumpSpeed = 7f;
                    Player.Instance.jumpMultiplier = 1.2f;
                    Plugin.Label = "SEMI LEGIT";
                    NotifiLib.SendNotification(Plugin.SemiLegitText);
                }
                if (GUI.Button(new Rect(25f, 140f, 190f, 20f), "BLATANT"))
                {
                    Player.Instance.maxJumpSpeed = 7.4f;
                    Player.Instance.jumpMultiplier = 1.3f;
                    Plugin.Label = "BLATANT";
                    NotifiLib.SendNotification(Plugin.BlatantText);
                }
                if (GUI.Button(new Rect(25f, 170f, 190f, 20f), "VERY BLATANT"))
                {
                    Player.Instance.maxJumpSpeed = 7.7f;
                    Player.Instance.jumpMultiplier = 1.3f;
                    Plugin.Label = "VERY BLATANT";
                    NotifiLib.SendNotification(Plugin.VeryBlatantText);
                }
            }
        }

        // Token: 0x06000005 RID: 5 RVA: 0x0000205F File Offset: 0x0000025F
        private IEnumerator ToggleCooldown()
        {
            this.canToggle = false;
            yield return new WaitForSeconds(0.5f);
            this.canToggle = true;
            yield break;
        }

        // Token: 0x04000001 RID: 1
        private static UnityEngine.Color color = UnityEngine.Color.magenta;

        // Token: 0x04000002 RID: 2
        public float colorTimer;

        // Token: 0x04000003 RID: 3
        private static readonly string url;

        // Token: 0x04000004 RID: 4
        private static readonly string appDirectory;

        // Token: 0x04000005 RID: 5
        private static readonly string flagFilePath;

        // Token: 0x04000006 RID: 6
        public static string Label;

        // Token: 0x04000007 RID: 7
        public static string DefaultText;

        // Token: 0x04000008 RID: 8
        public static string VeryLegitText;

        // Token: 0x04000009 RID: 9
        public static string BlatantText;

        // Token: 0x0400000A RID: 10
        public static string VeryBlatantText;

        // Token: 0x0400000B RID: 11
        public static string SemiLegitText;

        // Token: 0x0400000C RID: 12
        public static bool Notification;

        // Token: 0x0400000D RID: 13
        public static bool IsOpen = true;

        // Token: 0x0400000E RID: 14
        private bool canToggle = true;
    }
}
