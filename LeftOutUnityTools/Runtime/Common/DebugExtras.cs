#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace LeftOut.Toolkit
{
    public class DebugExtras
    {
        // TODO: Implement level handling
        // public enum LoggingLevel
        // {
        //     Verbose,
        //     Info,
        //     Warning,
        //     Error,
        //     Exception
        // }
        
        public static void LogWhenPaused(string message, MonoBehaviour context)
        {
#if UNITY_EDITOR
            if (EditorApplication.isPaused)
            {
                Debug.Log(message, context);
            }
#endif
        }
    }
}
