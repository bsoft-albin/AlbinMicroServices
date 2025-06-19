using AlbinMicroService.Core.Utilities;

namespace AlbinMicroService.Core.AbstractImpls
{
    public abstract class BaseInfraImpls
    {
        private int IntDataSwitcher { get; set; }
        private bool BooleanDataSwitcher { get; set; }
        private string? StringDataSwitcher { get; set; }

        public static GenericObjectSwitcher<X> ProduceGenericObjectSwitcher<X>(GenericObjectSwitcher<X> genericObjectSwitcher, Exception exception) where X : new()
        {
            genericObjectSwitcher.ErrorData = exception.ToErrorObject();
            genericObjectSwitcher.Error = exception.Message;
            genericObjectSwitcher.IsErrorHappened = Literals.Boolean.True;

            //genericObjectSwitcher.DataSwitcher = Literals.Boolean.False;   <================ main thing..........

            return genericObjectSwitcher;
        }
    }
}
