namespace PuzzleRunner.Caching
{
    public class MethodCache
    {
        public static Dictionary<object, object> Cache = new();

        public static bool MethodPrefix(ref object __result, params object[] input)
        {
            if (Cache.TryGetValue(input, out var o))
            {
                __result = o;
                return false;
            }
            return true;
        }

        public static void MethodPostfix(ref object __result, params object[] input)
        {
            if (!Cache.ContainsKey(input))
                Cache[input] = __result;
        }
    }
}
