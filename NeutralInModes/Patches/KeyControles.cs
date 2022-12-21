using HarmonyLib;
using UnityEngine;

namespace NeutralInModes.Patches
{
    [HarmonyPatch(typeof(ControllerManager), nameof(ControllerManager.Update))]
    class ControllerManagerUpdatePatch
    {
        static readonly (int, int)[] resolutions = { (480, 270), (640, 360), (800, 450), (1280, 720), (1600, 900) };
        static int resolutionIndex = 0;
        public static void Postfix(ControllerManager __instance)
        {
            //解像度変更
            if (Input.GetKeyDown(KeyCode.F9))
            {
                resolutionIndex++;
                if (resolutionIndex >= resolutions.Length) resolutionIndex = 0;
                ResolutionManager.SetResolution(resolutions[resolutionIndex].Item1, resolutions[resolutionIndex].Item2, false);
            }
            //廃村機能
            if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (!AmongUsClient.Instance.AmHost) return;
                if (StartGamePatch.GameStart.IsStart)
                {
                    GameManager.Instance.RpcEndGame(GameOverReason.HumansByTask, false);
                    
                }
            }
        }
    }
}