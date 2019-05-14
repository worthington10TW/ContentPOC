#!/bin/bash
curl -X POST \
-d @example.xml \
  http://localhost:64352/news \
  -H 'Content-Type: application/xml' \
  -H 'cache-control: no-cache'

