using System;
using System.Linq;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace AnotherAutoRigger
{
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
            if (gameObject.name.Contains(":"))
            {
                List<string> split = gameObject.name.Split(':').ToList();
                split.RemoveAt(split.Count - 1);
                return String.Join(":", split.ToArray()) + ":";
            }

            return "";
        }
    }

    public static class MonoBehaviourExtension
    {
        public static T GetComponentInGameObjectFromString<T>(this MonoBehaviour script, string objName)
        {
            GameObject obj = GameObject.Find(objName);
            if (obj == null)
                return default(T);

            return obj.GetComponent<T>();
        }
    }
}