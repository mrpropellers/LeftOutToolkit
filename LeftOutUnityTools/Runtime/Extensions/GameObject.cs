using UnityEngine;

namespace LeftOut.Extensions
{
    public static class GameObjectExtensions
    {
        public static bool IsAssetOnDisk(this GameObject self) =>
            self.scene.rootCount == 0 || self.scene.name == null;

        public static bool TryGetComponentInParent<T>(this GameObject self, out T component) where T : Component
        {
            if (self.TryGetComponent(out component))
            {
                return true;
            }

            var parent = self.transform.parent;
            return parent != null && parent.gameObject.TryGetComponentInParent(out component);
        }

        public static T GetComponentInParent<T>(this GameObject self) where T : Component
        {
            return self.TryGetComponentInParent(out T component) ? component : null;
        }
    }
}
