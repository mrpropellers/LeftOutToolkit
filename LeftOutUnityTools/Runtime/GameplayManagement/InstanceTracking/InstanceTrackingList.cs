using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LeftOut.GameplayManagement;
using UnityEngine;

namespace LeftOut
{
    public static class InstanceTrackingList<T> where T : class, ITrackableInstance
    {
        static List<T> s_Instances;

        // Ensure the list of instances is initialized before returning
        static List<T> InstancesInitialized => s_Instances ??= new List<T>();

        static void Register(T instance)
        {
            if (InstancesInitialized.Contains(instance))
            {
                Debug.LogWarning($"Can't {nameof(Register)} {instance} because it is already being tracked.");
                return;
            }

            s_Instances.Add(instance);
            instance.OnDestroyed += HandleInstanceDestroyed;
        }

        static void HandleInstanceDestroyed(ITrackableInstance instance)
        {
            Debug.Assert(instance is T,
                $"Tried to handle destruction of {instance} but it's not even of type {typeof(T)}");
            Debug.Assert(s_Instances.Contains(instance),
                $"Trying to handle destruction of {instance}, which we weren't tracking in {s_Instances}");
            s_Instances.Remove((T)instance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void AssertOnNull()
        {
            Debug.Assert(!s_Instances.Contains(null), 
                $"{nameof(InstanceTrackingList<T>)} has null items - " +
                $"did we not properly clean up after a destroy?");
        }

        static List<T> GetAll()
        {
            AssertOnNull();
            return s_Instances;
        }
        
        static bool TryGetFirst(out T instance)
        {
            AssertOnNull();
            if (InstancesInitialized.Any())
            {
                instance = s_Instances[0];
                return true;
            }

            instance = default;
            return false;
        }

    }
}
