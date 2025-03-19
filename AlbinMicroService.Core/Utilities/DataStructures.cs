using Serilog.Sinks.PostgreSQL;

namespace AlbinMicroService.Core.Utilities
{
    public class DataStructures
    {
        public Dictionary<string, ColumnWriterBase> PostgreSqlLogsTableStruct { get; set; }

        public DataStructures()
        {
            PostgreSqlLogsTableStruct = new Dictionary<string, ColumnWriterBase>
            {
                { "timestamp", new TimestampColumnWriter() },
                { "level", new LevelColumnWriter() },
                { "message", new RenderedMessageColumnWriter() },
                { "exception", new ExceptionColumnWriter() }
            };
        }
    }
}
