using AlbinMicroService.Core;

namespace AlbinMicroService.DataMappers.Contracts
{
    public interface IDataMapperExtensions
    {
        ErrorObject ToErrorObject(Exception ex);
        Task<PaginatedResponse> ToPaginatedResponseAsync<DataSource>(IQueryable<DataSource> source, int page, int pageSize, bool includeMetaData = false, CancellationToken cancellationToken = default);
    }
}
