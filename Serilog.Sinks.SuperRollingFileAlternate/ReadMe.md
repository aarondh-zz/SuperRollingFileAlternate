# Serilog Super Rolling File Sink (super roll-on-file-size alternative) [![NuGet Version](http://img.shields.io/nuget/v/Serilog.Sinks.SuperRollingFileAlternate.svg?style=flat)](https://www.nuget.org/packages/Serilog.Sinks.SuperRollingFileAlternate/)

This is a rolling file sink that is an extension to [Serilog.Sinks.RollingFileAlternate](https://github.com/BedeGaming/sinks-rollingfile) that allows you to easily configure async, log file prefix, and formatters.  Including the ability to specify renderMessage to the JsonFormatter.  Note that this project does not replace RollingFileAlternate, rather it is an addtional extension that uses Serilog.Sinks.RollingFileAlternate, and Serilog.Sinks.Async, the new Serilog.Formatting.Compact packages to make configuring enterprise suitable logs.


## Getting started

Install the [Serilog.Sinks.RollingFileAlternate](https://nuget.org/packages/serilog.sinks.superRollingFileAlternate) package from NuGet:

```powershell
Install-Package Serilog.Sinks.SuperRollingFileAlternate
```

To configure the sink in C# code, call `WriteTo.SuperRollingFileAlternate()` during logger configuration:

```csharp
var log = new LoggerConfiguration()
    .WriteTo.SuperRollingFileAlternate(".\\logs")
    .CreateLogger();
    
log.Information("This will be written to the rolling file set");
```

The sink is configured with the path of a folder for the log file set:

```
logs\20160631-00001.txt
logs\20160701-00001.txt
logs\20160701-00002.txt
```

> **Important:** Only one process may write to a log file at a given time. For multi-process scenarios, either use separate files or [one of the non-file-based sinks](https://github.com/serilog/serilog/wiki/Provided-Sinks).

## Parameters

The following parameters can be set via code or using the AppSettings (if the Serilog.AppSettings package is installed, see below)

###logDirectory

The `logDirectory` parameter specifies where the log file sets will be written to.  There is no default.

### Format

There are three "built in" file formats that can be specified with the `format` parameter.

* SuperRollingFileAlternateFormats.Json 
* SuperRollingFileAlternateFormats.CompactJson
* SuperRollingFileAlternateFormats.Text

#### SuperRollingFileAlternateFormats.Json
This format utilizes the Serilog JsonFormatter. Then you specify the Json you can also specify true or false in the `renderMessage` parameter to turn on or off the rendering of the message in the json stream.

#### SuperRollingFileAlternateFormats.CompactJson
This format utilizes the [Serilog.Formatting.Compact](https://github.com/serilog/serilog-formatting-compact) package, this format greatly reduces the size of events by simplifying the Json generated.  Good for high volumn event logging.

#### SuperRollingFileAlternateFormats.Text
This is the default format.  It uses the serilog "build in" MessageTemplateTextFormatter.  When using this option, the output template is specified using the `outputTemplate` parameter.

### Formatter
The `formatter` parameter provides the ability to specify custom serilog formatters if the `format` parameter does not satisfy your requirements.  When the `formatter` parameter is set it overrides whatever is set by the `format` parameter.

### Format Provider

The `formatProvider` parameter allows you to specify the serilog format provider, used by the MessageTemplateTextFormatter when the `format` parameter is set to SuperRollingFileAlternateFormats.Text


### Minimum Level
The `formatProvider` parameter specifies the minimum level of logging to this sink.  The default is LevelAlias.Minimum.

### Retained File Count Limit

The `retainedFileCountLimit` parameter specifies the maximum number of log files to retain.  The default is 31 files.  (One month of the files if the fileSizeLimitBytes is set high enough)

### Render Message

When the `format` parameter is set to SuperRollingFileAlternateFormats.Json, the `renderMessage` parameter is passed to the JsonFormatter to turn on or off the rendering of message to the log file.  The default is false.

### Async

The `async` parameter specifies that the superRollingFileAlternate sink should be wrapped in the [Serilog.Sinks.Async](https://github.com/serilog/serilog-sinks-async) function which causes logs to be written to disk on a background thread.  The default is false.

### Output Template

When the `format` parameter is set to SuperRollingFileAlternateFormats.Text, the `outputTemplate` parameter specifies the text template to be used to format the log.  
The default is set to 

```
   {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}

```

### Log File Prefix

The log file name can be prepended with text to make the file name more usable, is specified with the `logFilePrefix` parameter.

```csharp
    .WriteTo.SuperRollingFileAlternate(".\\logs", logFilePrefix: "MyProductionApp-")
```

The above configuration would product log files as follows:

```
logs\MyProductionApp-20160631-00001.txt
logs\MyProductionApp20160701-00001.txt
logs\MyProductionApp20160701-00002.txt
```

### File size limit

The file size limit, beyond which a new log file will be created, is specified with the `fileSizeLimitBytes` parameter.

```csharp
    .WriteTo.SuperRollingFileAlternate(".\\logs", fileSizeLimitBytes: 1024 * 1024)
```

The default if no limit is specified is to roll every 1 gigabytes.

### XML `<appSettings>` configuration

To use the super rolling file alternate sink with the [Serilog.Settings.AppSettings](https://github.com/serilog/serilog-settings-appsettings) package, first install that package if you haven't already done so:

```powershell
Install-Package Serilog.Settings.AppSettings
```

Instead of configuring the logger in code, call `ReadFrom.AppSettings()`:

```csharp
var log = new LoggerConfiguration()
    .ReadFrom.AppSettings()
    .CreateLogger();
```

```XML
<appSettings> 
    . . .
    <add key="serilog:minimum-level" value="Information" />
    <add key="serilog:using:SuperRollingFileAlternate" value="Serilog.Sinks.SuperRollingFileAlternate" />
    <add key="serilog:write-to:SuperRollingFileAlternate.logDirectory" value=".\logs" />
    <add key="serilog:write-to:SuperRollingFileAlternate.logFilePrefix" value="Consto.ExpressOrder.WebApi" />
    <add key="serilog:write-to:SuperRollingFileAlternate.format" value="CompactJson" />
    <add key="serilog:write-to:SuperRollingFileAlternate.restrictedToMinimumLevel" value="Information" />
. . .
</appSettings>
```
