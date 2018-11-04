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
            // get keys from JSONNode dictionary as a list
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
            // add items to list in one liner
            list.AddRange(elements);
        }
    }

    public static class GameObjectExtension
    { 
        public static string GetMayaStyleNamespace(this GameObject gameObject)
        {
            // get Maya style namespace from name of gameobject
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
            // get component from gameobject which is found with a string
            GameObject obj = GameObject.Find(objName);
            if (obj == null)
                return default(T);

            return obj.GetComponent<T>();
        }
    }
}