using UnityEngine;

namespace LeftOut.Extensions
{
    public static class GameObjectExtensions
    {
        public static bool IsAssetOnDisk(this GameObject self) =>
            self.scene.rootCount == 0 || self.scene.name == null;
    }
}
