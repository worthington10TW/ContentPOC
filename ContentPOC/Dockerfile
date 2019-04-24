FROM microsoft/dotnet:2.2-aspnetcore-runtime
RUN apt-get update && apt-get install -y gnupg

RUN echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list

RUN curl -s https://download.newrelic.com/548C16BF.gpg | apt-key add -
RUN apt-get update
RUN apt-get install newrelic-netcore20-agent

ENV CORECLR_ENABLE_PROFILING=1 \
CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-netcore20-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-netcore20-agent/libNewRelicProfiler.so

RUN addgroup --gid 1000 appuser
RUN adduser --disabled-password --gid 1000 -uid 1000 appuser

WORKDIR /home/appuser/app

COPY ContentPOC/out/ .
RUN chown appuser:appuser -R /home/appuser/app

USER appuser
ENTRYPOINT ["dotnet", "ContentPOC.dll"]
