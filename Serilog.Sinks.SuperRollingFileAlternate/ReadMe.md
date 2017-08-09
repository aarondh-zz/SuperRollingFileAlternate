# Serilog Super Rolling File Sink (roll-on-file-size alternative) [![NuGet Version](http://img.shields.io/nuget/v/Serilog.Sinks.SuperRollingFileAlternate.svg?style=flat)](https://www.nuget.org/packages/Serilog.Sinks.SuperRollingFileAlternate/)

This is a rolling file sink that is an extension to [Serilog.Sinks.RollingFileAlternate](https://github.com/BedeGaming/sinks-rollingfile) that allows you to easily configure async, log file prefix, and formatters.  Including the ability to specify renderMessage to the JsonFormatter.  Note that this project does not replace RollingFileAlternate, rather it is an addtional extension that uses Serilog.Sinks.RollingFileAlternate, and Serilog.Sinks.Async, the new Serilog.Formatting.CompactJson projects to make configuring enterprise suitable logs.",


### Getting started

Install the [Serilog.Sinks.RollingFileAlternate](https://nuget.org/packages/serilog.sinks.superRollingFileAlternate) package from NuGet:

```powershell
Install-Package Serilog.Sinks.RollingFileAlternate
```

To configure the sink in C# code, call `WriteTo.RollingFileAlternate()` during logger configuration:

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

### File size limit

The file size limit, beyond which a new log file will be created, is specified with the `fileSizeLimitBytes` parameter.

```csharp
    .WriteTo.SuperRollingFileAlternate(".\\logs", fileSizeLimitBytes: 1024 * 1024)
```

The default if no limit is specified is to roll every two megabytes.

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
    <add key="serilog:minimum-level" value="Debug" />
    <add key="serilog:using:SuperRollingFileAlternate" value="Starbucks.Mop.Utils.Serilog.SuperRollingFileAlternate" />
    <add key="serilog:write-to:SuperRollingFileAlternate.logDirectory" value=“%BASEDIR%\logs" />
    <add key="serilog:write-to:SuperRollingFileAlternate.logFilePrefix" value="Starbucks.ExpressOrder.WebApi" />
    <add key="serilog:writeto:SuperRollingFileAlternate.formatter" 
       value="Serilog.Formatting.Compact.CompactJsonFormatter, Serilog.Formatting.Compact" />
    <add key="serilog:write-to:SuperRollingFileAlternate.restrictedToMinimumLevel" value="Debug" />
    <add key="serilog:enrich:with-property:Title" value="APILog" />
    <add key="serilog:enrich:with-property:Category" value="ExpressOrder" /> 
. . .
</appSettings>
```
