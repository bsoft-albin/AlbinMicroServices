namespace AlbinMicroService.Core.Utilities
{
    public abstract class ConstantData
    {
        public abstract class Templates
        {
            public const string API_BASE_TEMPLATE = "api/[controller]/[action]";
            public const string API_TEMPLATE = "api/[controller]/[action]";
            public const string API_VERSION_TEMPLATE = "api/v{version:apiVersion}/[controller]/[action]";
        }
        public abstract class ApiVersions
        {
            public const string DEFAULT = "1.0";
            public const string LongTermVersion = "2.0";
            public const string Latest = "3.0";
        }

        public abstract class ActionNames
        {
            public abstract class Greetings
            {
                public const string SayHello = "SayHelloAsync";
                public const string GetReligions = "GetReligionsAsync";
            }

        }


    }
}
