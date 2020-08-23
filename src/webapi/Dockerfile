FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["src/webapi/VVJ.AppWithDataDog.WebApi.csproj", "src/webapi/"]
RUN dotnet restore "src/webapi/VVJ.AppWithDataDog.WebApi.csproj"
COPY . .
WORKDIR "/src/src/webapi"
RUN dotnet build "VVJ.AppWithDataDog.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VVJ.AppWithDataDog.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ARG DD_TRACE_VERSION
ENV DD_TRACE_VERSION=${DD_TRACE_VERSION:-1.19.1}

RUN curl -LO "https://github.com/DataDog/dd-trace-dotnet/releases/download/v${DD_TRACE_VERSION}/datadog-dotnet-apm_${DD_TRACE_VERSION}_amd64.deb"
RUN dpkg -i "./datadog-dotnet-apm_${DD_TRACE_VERSION}_amd64.deb"

ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER={846F5F1C-F9AE-4B07-969E-05C26BC060D8}
ENV CORECLR_PROFILER_PATH=/opt/datadog/Datadog.Trace.ClrProfiler.Native.so
ENV DD_INTEGRATIONS=/opt/datadog/integrations.json
ENV DD_DOTNET_TRACER_HOME=/opt/datadog
ENV DD_LOGS_INJECTION=true
ENV DD_TRACE_DEBUG=true
ENV DD_TRACE_ANALYTICS_ENABLED=true

# ARG DD_TRACE_AGENT_URL
# ENV DD_TRACE_AGENT_URL=${DD_TRACE_AGENT_URL:-http://localhost:8126}
# ARG DD_ENV
# ENV DD_ENV=${DD_ENV:-dev}
# ARG DD_SERVICE
# ENV DD_SERVICE=${DD_SERVICE:-VVJ.AppWithDataDog.WebApi}
# ARG DD_VERSION
# ENV DD_VERSION=${DD_VERSION:-0.1.0}
# ARG DD_TAGS
# ENV DD_TAGS=${DD_TAGS:-api:VVJ.AppWithDataDog.WebApi}

#ENTRYPOINT ["/bin/bash", "/app/init-env.sh"]
RUN /opt/datadog/createLogPath.sh
ENTRYPOINT ["dotnet", "VVJ.AppWithDataDog.WebApi.dll"]
