using Newtonsoft.Json;

namespace NiceExtensions.Json
{
    public static class Extensions
    {
        public static string ToJson(this object o, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(o, formatting);
        }
    }
}