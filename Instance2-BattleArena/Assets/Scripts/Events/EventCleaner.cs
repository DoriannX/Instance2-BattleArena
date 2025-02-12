using System;
using System.Reflection;
using UnityEngine;

namespace Events
{
    public class EventCleaner : MonoBehaviour
    {
        private void OnDestroy()
        {
            Type eventManagerType = typeof(EventManager);
            FieldInfo[] fields = eventManagerType.GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(Action))
                {
                    field.SetValue(null, null);
                }
            }
        }
    }
}