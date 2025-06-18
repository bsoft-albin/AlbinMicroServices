using AlbinMicroService.Core.Utilities;

namespace AlbinMicroService.Core.AbstractImpls
{
    public abstract class BaseAppImpls
    {
        private short StatusCode { get; set; } = HttpStatusCodes.Status500InternalServerError;
        private string StatusMessage { get; set; } = HttpStatusMessages.Status500InternalServerError;

        public RuntimeErrorResponse ProduceRuntimeErrorResponse<X>(GenericObjectSwitcher<X> genericObject) where X : new()
        {
            RuntimeErrorResponse runtimeErrorResponse = new()
            {
                Error = genericObject.ErrorData,
                StatusCode = this.StatusCode,
                StatusMessage = this.StatusMessage
            };

            return runtimeErrorResponse;
        }
    }
}
