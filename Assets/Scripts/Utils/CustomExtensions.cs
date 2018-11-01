using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace AnotherAutoRigger
{
    public static class StringExtension
    {
        public static bool IsDigit(this string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
    }

    public static class JSONNode
    {
        public static List<string> GetKeys(this SimpleJSON.JSONNode dict)
        {
            List<string> keys = new List<string>();
            foreach (var key in dict.Keys)
                keys.Add(key);

            return keys;
        }
    }

    public static class ListExtenstions
    {
        public static void AddMany<T>(this List<T> list, params T[] elements)
        {
            list.AddRange(elements);
        }
    }

    public static class GameObjectExtension
    { 
        public static string GetNamespace(this GameObject gameObject)
        {
            // validate of name contains a namespace
            if (gameObject.name.Contains(":"))
            {
                List<string> split = gameObject.name.Split(':').ToList();
                split.RemoveAt(split.Count - 1);
                return String.Join(":", split.ToArray()) + ":";
            }

            // return empty namespace
            return "";
        }
    }
}