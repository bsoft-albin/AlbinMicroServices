namespace AlbinMicroService.Core.Utilities
{

    public abstract class ApiRoutes
    {
        public const string API_BASE_TEMPLATE = "api/[controller]";
        public const string API_TEMPLATE = "api/[controller]/[action]";
        public const string API_VERSION_TEMPLATE = "api/v{version:apiVersion}/[controller]/[action]";
    }

    public abstract class ApiVersions
    {
        public const string DEFAULT = "1.0";
        public const string LONG_TERM_SUPPORT = "2.0";
        public const string STANDARD_TERM_SUPPORT = "3.0";
    }

    public abstract class ApiAuthorization
    {
        public abstract class Policies
        {
            public const string AdminOnly = nameof(AdminOnly);
        }

        public abstract class Scopes
        {
            public const string MASTER_READ = "master.read";
            public const string MASTER_WRITE = "master.write";
            public const string USER_READ = "user.read";
            public const string USER_WRITE = "user.write";
            public const string ADMIN_READ = "admin.read";
            public const string ADMIN_WRITE = "admin.write";
            public const string OFFLINE_ACCESS = "offline_access";
        }

        public abstract class TokenRequestKeys
        {
            public const string grant_type = nameof(grant_type);
            public const string client_id = nameof(client_id);
            public const string password = nameof(password);
            public const string username = nameof(username);
            public const string client_secret = nameof(client_secret);
        }

        public abstract class GrantTypes
        {
            public const string password = nameof(password);
            public const string code = nameof(code);
        }
    }

    public abstract class Literals
    {
        public abstract class Boolean
        {
            public const bool True = true;
            public const bool False = false;
        }

        public abstract class Integer
        {
            public const short Zero = 0;
            public const short One = 1;
            public const short Two = 2;
            public const short Three = 3;
            public const short Four = 4;
            public const short Five = 5;
            public const short Six = 6;
            public const short Seven = 7;
            public const short Eight = 8;
            public const short Nine = 9;
        }
    }

    public abstract class SystemRoles
    {
        public const string USER = "User";
        public const string ADMIN = "Admin";
        public const string MANAGER = "Manager";
        public const string STAFF = "Staff";
        public const string SUPER_ADMIN = "SuperAdmin";
    }

    public abstract class SystemClientIds
    {
        public const string USER = "public-client";
        public const string ADMIN = "admin-client";
        public const string MANAGER = "manager-client";
        public const string STAFF = "staff-client";
        public const string SUPER_ADMIN = "superadmin-client";
    }

    public abstract class SystemClientSecrets
    {
        public const string ADMIN = "admin-secret";
        public const string SUPER_ADMIN = "superadmin-secret";
    }

    public abstract class DatabaseTypes
    {
        public const string SqlServer = nameof(SqlServer);
        public const string MySql = nameof(MySql);
        public const string Oracle = nameof(Oracle);
        public const string PostgreSql = nameof(PostgreSql);
        public const string MariaDb = nameof(MariaDb);
        public const string MongoDb = nameof(MongoDb);
        public const string Redis = nameof(Redis);
    }

    public abstract class CommonActionNames
    {
        public abstract class Greetings
        {
            public const string SayHello = "SayHelloAsync";
        }
    }

    public static class CustomHttpStatusCodes
    {
        public const short ServerShutdown = 800;
        public const short UnXpectedError = 520;
        public const short ResourceDeleted = 204;
        public const short ResourceUpdated = 202;
        public const short ResourceInserted = 201;
    }

    public static class CustomHttpStatusMessages
    {
        public const string ServerShutdown = "Temporarily the Server Shutdown";
        public const string ResourceDeleted = "Resource Deleted";
        public const string ResourceUpdated = "Resource Updated";
        public const string ResourceInserted = "Resource Inserted";
        public const string UsernameExists = "Username Already Exists";
        public const string EmailExists = "Email already Registered with us";
        public const string UnXpectedError = "Something went wrong please try again later";
        public const string UnKnownError = "UNKNOWN_ERROR";
    }

    public static class AllowedHttpMethods
    {
        public static readonly string[] Value = ["GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS"];
    }

    public abstract class HttpStatusCodes
    {
        public const short Status100Continue = 100;
        public const short Status101SwitchingProtocols = 101;
        public const short Status102Processing = 102;
        public const short Status200OK = 200;
        public const short Status201Created = 201;
        public const short Status202Accepted = 202;
        public const short Status203NonAuthoritative = 203;
        public const short Status204NoContent = 204;
        public const short Status205ResetContent = 205;
        public const short Status206PartialContent = 206;
        public const short Status207MultiStatus = 207;
        public const short Status208AlreadyReported = 208;
        public const short Status226IMUsed = 226;
        public const short Status300MultipleChoices = 300;
        public const short Status301MovedPermanently = 301;
        public const short Status302Found = 302;
        public const short Status303SeeOther = 303;
        public const short Status304NotModified = 304;
        public const short Status305UseProxy = 305;
        public const short Status306SwitchProxy = 306;
        public const short Status307TemporaryRedirect = 307;
        public const short Status308PermanentRedirect = 308;
        public const short Status400BadRequest = 400;
        public const short Status401Unauthorized = 401;
        public const short Status402PaymentRequired = 402;
        public const short Status403Forbidden = 403;
        public const short Status404NotFound = 404;
        public const short Status405MethodNotAllowed = 405;
        public const short Status406NotAcceptable = 406;
        public const short Status407ProxyAuthenticationRequired = 407;
        public const short Status408RequestTimeout = 408;
        public const short Status409Conflict = 409;
        public const short Status410Gone = 410;
        public const short Status411LengthRequired = 411;
        public const short Status412PreconditionFailed = 412;
        public const short Status413RequestEntityTooLarge = 413;
        public const short Status413PayloadTooLarge = 413;
        public const short Status414RequestUriTooLong = 414;
        public const short Status414UriTooLong = 414;
        public const short Status415UnsupportedMediaType = 415;
        public const short Status416RequestedRangeNotSatisfiable = 416;
        public const short Status416RangeNotSatisfiable = 416;
        public const short Status417ExpectationFailed = 417;
        public const short Status418ImATeapot = 418;
        public const short Status419AuthenticationTimeout = 419;
        public const short Status421MisdirectedRequest = 421;
        public const short Status422UnProcessableEntity = 422;
        public const short Status423Locked = 423;
        public const short Status424FailedDependency = 424;
        public const short Status426UpgradeRequired = 426;
        public const short Status428PreconditionRequired = 428;
        public const short Status429TooManyRequests = 429;
        public const short Status431RequestHeaderFieldsTooLarge = 431;
        public const short Status451UnavailableForLegalReasons = 451;
        public const short Status500InternalServerError = 500;
        public const short Status501NotImplemented = 501;
        public const short Status502BadGateway = 502;
        public const short Status503ServiceUnavailable = 503;
        public const short Status504GatewayTimeout = 504;
        public const short Status505HttpVersionNotSupported = 505;
        public const short Status506VariantAlsoNegotiates = 506;
        public const short Status507InsufficientStorage = 507;
        public const short Status508LoopDetected = 508;
        public const short Status510NotExtended = 510;
        public const short Status511NetworkAuthenticationRequired = 511;
    }

    public abstract class HttpStatusMessages
    {
        public const string Status100Continue = "Continue";
        public const string Status101SwitchingProtocols = "Switching Protocols";
        public const string Status102Processing = "Processing";
        public const string Status200OK = "OK";
        public const string Status201Created = "Created";
        public const string Status202Accepted = "Accepted";
        public const string Status203NonAuthoritative = "Non-Authoritative Information";
        public const string Status204NoContent = "No Content";
        public const string Status205ResetContent = "Reset Content";
        public const string Status206PartialContent = "Partial Content";
        public const string Status207MultiStatus = "Multi-Status";
        public const string Status208AlreadyReported = "Already Reported";
        public const string Status226IMUsed = "IM Used";
        public const string Status300MultipleChoices = "Multiple Choices";
        public const string Status301MovedPermanently = "Moved Permanently";
        public const string Status302Found = "Found";
        public const string Status303SeeOther = "See Other";
        public const string Status304NotModified = "Not Modified";
        public const string Status305UseProxy = "Use Proxy";
        public const string Status306SwitchProxy = "Switch Proxy";
        public const string Status307TemporaryRedirect = "Temporary Redirect";
        public const string Status308PermanentRedirect = "Permanent Redirect";
        public const string Status400BadRequest = "Bad Request";
        public const string Status401Unauthorized = "Unauthorized";
        public const string Status402PaymentRequired = "Payment Required";
        public const string Status403Forbidden = "Forbidden";
        public const string Status404NotFound = "Not Found";
        public const string Status405MethodNotAllowed = "Method Not Allowed";
        public const string Status406NotAcceptable = "Not Acceptable";
        public const string Status407ProxyAuthenticationRequired = "Proxy Authentication Required";
        public const string Status408RequestTimeout = "Request Timeout";
        public const string Status409Conflict = "Conflict";
        public const string Status410Gone = "Gone";
        public const string Status411LengthRequired = "Length Required";
        public const string Status412PreconditionFailed = "Precondition Failed";
        public const string Status413RequestEntityTooLarge = "Request Entity Too Large";
        public const string Status413PayloadTooLarge = "Payload Too Large";
        public const string Status414RequestUriTooLong = "Request-URI Too Long";
        public const string Status414UriTooLong = "URI Too Long";
        public const string Status415UnsupportedMediaType = "Unsupported Media Type";
        public const string Status416RequestedRangeNotSatisfiable = "Requested Range Not Satisfiable";
        public const string Status416RangeNotSatisfiable = "Range Not Satisfiable";
        public const string Status417ExpectationFailed = "Expectation Failed";
        public const string Status418ImATeapot = "I'm a teapot";
        public const string Status419AuthenticationTimeout = "Authentication Timeout";
        public const string Status421MisdirectedRequest = "Misdirected Request";
        public const string Status422UnProcessableEntity = "UnProcessable Entity";
        public const string Status423Locked = "Locked";
        public const string Status424FailedDependency = "Failed Dependency";
        public const string Status426UpgradeRequired = "Upgrade Required";
        public const string Status428PreconditionRequired = "Precondition Required";
        public const string Status429TooManyRequests = "Too Many Requests";
        public const string Status431RequestHeaderFieldsTooLarge = "Request Header Fields Too Large";
        public const string Status451UnavailableForLegalReasons = "Unavailable For Legal Reasons";
        public const string Status500InternalServerError = "Internal Server Error";
        public const string Status501NotImplemented = "Not Implemented";
        public const string Status502BadGateway = "Bad Gateway";
        public const string Status503ServiceUnavailable = "Service Unavailable";
        public const string Status504GatewayTimeout = "Gateway Timeout";
        public const string Status505HttpVersionNotSupported = "HTTP Version Not Supported";
        public const string Status506VariantAlsoNegotiates = "Variant Also Negotiates";
        public const string Status507InsufficientStorage = "Insufficient Storage";
        public const string Status508LoopDetected = "Loop Detected";
        public const string Status510NotExtended = "Not Extended";
        public const string Status511NetworkAuthenticationRequired = "Network Authentication Required";
    }

}
