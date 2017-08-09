using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Display;
using Serilog.Formatting.Json;
using Serilog.Sinks.RollingFileAlternate.Sinks.SizeRollingFileSink;
using System;
using System.Text;

namespace Serilog.Sinks.JsonRollingFileAlternate
{
    public enum JsonRollingFileFormats
    {
        Text,
        Json,
        CompactJson
    }
    public static class SuperRollingFileAlternateExtensions
    {
        const int DefaultRetainedFileCountLimit = 31; // A long month of logs
        const long DefaultFileSizeLimitBytes = 1L * 1024 * 1024 * 1024;
        const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}";
        public static LoggerConfiguration SuperRollingFileAlternate(
            this LoggerSinkConfiguration configuration,
            string logDirectory, 
            string logFilePrefix = "",
            JsonRollingFileFormats format = JsonRollingFileFormats.Text,
            ITextFormatter formatter = null,
            LogEventLevel minimumLevel = LevelAlias.Minimum,
            long fileSizeLimitBytes = DefaultFileSizeLimitBytes,
            int? retainedFileCountLimit = DefaultRetainedFileCountLimit,
            Encoding encoding = null,
            bool renderMessage = false,
            bool async = false,
            string outputTemplate = DefaultOutputTemplate,
            IFormatProvider formatProvider = null)
        {
            if ( formatter == null )
            {
                switch (format)
                {
                    case JsonRollingFileFormats.Json:
                        formatter = new JsonFormatter(renderMessage: renderMessage);
                        break;
                    case JsonRollingFileFormats.CompactJson:
                        formatter = new CompactJsonFormatter();
                        break;
                    case JsonRollingFileFormats.Text:
                    default:
                        formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
                        break;
                }
            }
            var sink = new AlternateRollingFileSink(logDirectory, formatter, fileSizeLimitBytes,
                            retainedFileCountLimit,encoding,logFilePrefix);
            if ( async )
            {
                return configuration.Async(c => c.Sink(sink, minimumLevel));
            }
            else
            {
                return configuration.Sink(sink, minimumLevel);
            }
        }
    }
}