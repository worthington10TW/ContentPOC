#!/bin/sh

set -eu

dotnet-sonarscanner begin /k:"ContentPOC" /d:sonar.host.url=https://sonarqube.rosprod.lnawsapps.co.uk /d:sonar.login=${SONAR_LOGIN} /d:sonar.cs.opencover.reportsPaths="coverage.opencover.xml" /d:sonar.exclusions="**/wwwroot/**/*"
dotnet build -c Release -property:Version=${GO_PIPELINE_LABEL}
cd ContentPOC.Test
dotnet test -c Release --no-build /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput='../' --logger "trx;LogFileName=result.trx" && xsltproc ../gocd/MSBuild-to-NUnit.xslt TestResults/result.trx > TestResults/cs_unit_result.xml
cd ..
cd ContentPOC.Integration
dotnet test -c Release --no-build --logger "trx;LogFileName=result.trx" && xsltproc ../gocd/MSBuild-to-NUnit.xslt TestResults/result.trx > TestResults/cs_integration_result.xml
cd ..
dotnet-sonarscanner end /d:sonar.login=${SONAR_LOGIN}
dotnet publish -c Release -o out --no-build
