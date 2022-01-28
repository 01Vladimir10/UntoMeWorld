namespace UntoMeWorld.Domain.Model
{
    public class Permission
    {
        public string Resource { get; set; }
        public string ResourceType { get; set; }
        public bool Add { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Restore { get; set; }
        public bool Purge { get; set; }
    }

    public static class ResourceTypes
    {
        public const string ApiEndPoint = "ApiEndpoint";
    }
}