namespace LeftOut.GlobalConsts
{
    public static class ShaderProperty
    {
        // TODO: Make this renderer agnostic with pre-processor defines
        public const string MainColor = "_Color";
        public const string EmissiveColor = "_EmissionColor";
    }

    public static class Tags
    {
        public const string Untagged = "Untagged";
        public const string Respawn = "Respawn";
        public const string Finish = "Finish";
        public const string EditorOnly = "EditorOnly";
        public const string MainCamera = "MainCamera";
        public const string Player = "Player";
        public const string GameController = "GameController";
    }
}
