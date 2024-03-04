using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GameCore;
using Platform;
using UnityEngine;

namespace Utils
{
    public static class UnityUtilTools
    {
        /// <summary>
        /// 分割配置表中常用的字符
        /// </summary>
        private static char[] GameSplitChars = { ',', '|', '&', ':' };

        /// <summary>
        /// 通过名字获取子节点下的物体
        /// </summary>
        /// <param name="root"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Dictionary<string, GameObject> GetGameObjectName(GameObject root, HashSet<string> names)
        {
            Dictionary<string, GameObject> dict = new Dictionary<string, GameObject>();
            if (names.Contains(root.name))
            {
                dict.Add(root.name, root.gameObject);
            }

            foreach (var item in root.GetComponentsInChildren<Transform>(true))
            {
                if (names.Contains(item.name))
                {
                    dict.Add(item.name, item.gameObject);
                }
            }

            return dict;
        }

        /// <summary>
        /// 通过名字获取子节点下的物体
        /// </summary>
        /// <param name="root"></param>
        /// <param name="nameDict">取字典中Key作为ID</param>
        /// <returns></returns>
        public static Dictionary<string, GameObject> GetGameObjectName(GameObject root, Dictionary<string, GameObject> nameDict)
        {
            Dictionary<string, GameObject> dict = new Dictionary<string, GameObject>();
            if (nameDict.ContainsKey(root.name))
            {
                dict.Add(root.name, root.gameObject);
            }

            foreach (var item in root.GetComponentsInChildren<Transform>(true))
            {
                if (nameDict.ContainsKey(item.name))
                {
                    if (dict.ContainsKey(item.name))
                    {
                        if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                        {
                            GameDebug.LogError("same key:" + item.name);
                        }
                    }
                    else
                    {
                        dict.Add(item.name, item.gameObject);
                    }
                }
            }

            return dict;
        }

        /// <summary>
        /// 获取单个GameObject
        /// </summary>
        /// <param name="root"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject GetSingleObject(GameObject root, string name)
        {
            if (root.name == name)
            {
                return root;
            }

            foreach (var item in root.GetComponentsInChildren<Transform>(true))
            {
                if (item.name == name)
                {
                    return item.gameObject;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取单个指定的<T>组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetSingleObject<T>(GameObject root, string name)
        {
            if (root.name == name)
            {
                return root.GetComponent<T>();
            }

            foreach (var item in root.GetComponentsInChildren<Transform>(true))
            {
                if (item.name == name)
                {
                    return item.gameObject.GetComponent<T>();
                }
            }

            return default(T);
        }

        public static sbyte[] GetSplitSbyte(string value, params char[] ch)
        {
            if (ch.Length == 0)
            {
                ch = GameSplitChars;
            }

            string[] strs = value.TrimStart().TrimEnd().Split(ch, StringSplitOptions.RemoveEmptyEntries);
            sbyte[] data = new sbyte[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                data[i] = sbyte.Parse(strs[i]);
            }

            return data;
        }

        public static byte[] GetSplitByte(string value, params char[] ch)
        {
            if (ch.Length == 0)
            {
                ch = GameSplitChars;
            }

            string[] strs = value.TrimStart().TrimEnd().Split(ch, StringSplitOptions.RemoveEmptyEntries);
            byte[] data = new byte[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                data[i] = byte.Parse(strs[i]);
            }

            return data;
        }

        public static bool[] GetSplitBool(string value, params char[] ch)
        {
            if (ch.Length == 0)
            {
                ch = GameSplitChars;
            }

            string[] strs = value.TrimStart().TrimEnd().Split(ch, StringSplitOptions.RemoveEmptyEntries);
            bool[] bol = new bool[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                bol[i] = bool.Parse(strs[i]);
            }

            return bol;
        }

        public static short[] GetSplitShort(string value, params char[] ch)
        {
            if (ch.Length == 0)
            {
                ch = GameSplitChars;
            }

            string[] strs = value.TrimStart().TrimEnd().Split(ch, StringSplitOptions.RemoveEmptyEntries);
            short[] data = new short[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                data[i] = short.Parse(strs[i]);
            }

            return data;
        }

        public static int[] GetSplitInt32(string value, params char[] ch)
        {
            if (ch.Length == 0)
            {
                ch = GameSplitChars;
            }

            string[] strs = value.TrimStart().TrimEnd().Split(ch, StringSplitOptions.RemoveEmptyEntries);
            int[] data = new int[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                data[i] = int.Parse(strs[i]);
            }

            return data;
        }

        public static long[] GetSplitLong(string value, params char[] ch)
        {
            if (ch.Length == 0)
            {
                ch = GameSplitChars;
            }

            string[] strs = value.TrimStart().TrimEnd().Split(ch, StringSplitOptions.RemoveEmptyEntries);
            long[] data = new long[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                data[i] = long.Parse(strs[i]);
            }

            return data;
        }

        /// <summary>
        /// String 支持[,|&]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string[] GetSplitString(string value, params char[] ch)
        {
            if (ch.Length == 0)
            {
                ch = new char[] { '|' };
            } //文字分割仅能使用 | /*GameSplitChars;*/ }

            value = value.TrimEnd(ch); //移除末尾带分割符字符.解决Excel中数字用,分割  末尾带,

            return value.TrimStart().TrimEnd().Split(ch, StringSplitOptions.RemoveEmptyEntries);
        }

        public static float[] GetSplitFloat(string value, params char[] ch)
        {
            if (ch.Length == 0)
            {
                ch = GameSplitChars;
            }

            string[] strs = value.TrimStart().TrimEnd().Split(ch, StringSplitOptions.RemoveEmptyEntries);
            float[] data = new float[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                data[i] = float.Parse(strs[i]);
            }

            return data;
        }

        public static int[] GetSplitInt32RemoveBracket(string value, params char[] ch)
        {
            string temp = Regex.Replace(value, @"\[(?<str>.*?)\]", "${str}");
            return GetSplitInt32(temp, ch);
        }
        
        
    }
}