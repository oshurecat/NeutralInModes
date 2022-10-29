using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using NeutralInModes.Patches;
using System;
using System.IO;
using System.Reflection;
using UnhollowerBaseLib;
using UnityEngine;

namespace NeutralInModes
{
    public static class Helpers
    {
        private delegate bool DelegateLoadImage(IntPtr tex, IntPtr data, bool markNonReadable);
        private static DelegateLoadImage _callLoadImage;
        private static bool LoadImage(Texture2D tex, byte[] data, bool markNonReadable)
        {
            _callLoadImage ??= IL2CPP.ResolveICall<DelegateLoadImage>("UnityEngine.ImageConversion::LoadImage");
            var il2cppArray = (Il2CppStructArray<byte>)data;

            return _callLoadImage.Invoke(tex.Pointer, il2cppArray.Pointer, markNonReadable);
        }
        public static unsafe Texture2D loadTextureFromResources(string path)
        {
            try
            {
                Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream(path);
                var byteTexture = new byte[stream.Length];
                var read = stream.Read(byteTexture, 0, (int)stream.Length);
                LoadImage(texture, byteTexture, false);
                return texture;
            }
            catch
            {
                System.Console.WriteLine("Error loading texture from resources: " + path);
            }
            return null;
        }
        public static Sprite LoadSpriteFromResources(string name, float size)
        {

            try
            {
                var texture = Helpers.loadTextureFromResources(name);
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), size);
            }
            catch
            {

            }
            return null;
        }

        public static string cs(Color c, string s)
        {
            return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>", ToByte(c.r), ToByte(c.g), ToByte(c.b), ToByte(c.a), s);
        }

        private static byte ToByte(float f)
        {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }
        public static PlayerControl playerById(byte id)
        {
            foreach (PlayerControl player in CachedPlayer.AllPlayers)
                if (player.PlayerId == id)
                    return player;
            return null;
        }
    }
}