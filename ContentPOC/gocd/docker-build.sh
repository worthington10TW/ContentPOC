#!/bin/sh

set -eu

# Don't tag with ECR info to prevent upload if scan fails
docker build -t matt-and-andrew/contentpoc:${GO_REVISION_SOURCE} .
mkdir scan-results
export AQUASEC_SCAN_PASSWORD=$(cat /home/go/aquasec-scanners/build_prod_password)

echo "Info - Docker run with scan"
docker run -v /var/run/docker.sock:/var/run/docker.sock -v $(pwd)/scan-results:/tmp 230575338114.dkr.ecr.us-east-1.amazonaws.com/aquasec/scanner-cli:3.5 scan -H http://aqua-web.aquasec35:8080 -U ${AQUASEC_SCAN_USERNAME} -P ${AQUASEC_SCAN_PASSWORD} --jsonfile /tmp/scan.json --htmlfile /tmp/scan.html --local matt-and-andrew/contentpoc:${GO_REVISION_SOURCE} --policies release-scan

# check we have a results file
FILENAME="./scan-results/scan.json"
if [ ! -f ${FILENAME} ]; then
  echo "AQUASEC report ${FILENAME} not found"
  exit 1
fi

# parse the file to get the policy result
POLICY_RESULT=$(jq '.image_assurance_results.disallowed' ./scan-results/scan.json)
echo "Policy result $POLICY_RESULT"
if [ -z "$POLICY_RESULT" ] || [ "$POLICY_RESULT" == "true" ]; then
  echo "Policy violation - check the Aquasec scan results"
  exit 1
fi

echo "Info - Docker tag"
docker tag matt-and-andrew/contentpoc:${GO_REVISION_SOURCE} 230575338114.dkr.ecr.us-east-1.amazonaws.com/matt-and-andrew/contentpoc:${GO_REVISION_SOURCE}

echo "Info - Aquasec register"
docker run -v /var/run/docker.sock:/var/run/docker.sock -v $(pwd)/scan-results:/tmp 230575338114.dkr.ecr.us-east-1.amazonaws.com/aquasec/scanner-cli:3.5 scan -H http://aqua-web.aquasec35:8080 -U scanner -P ${AQUASEC_SCAN_PASSWORD} --local 230575338114.dkr.ecr.us-east-1.amazonaws.com/matt-and-andrew/contentpoc:${GO_REVISION_SOURCE} --register --registry ECR --policies release-scan

