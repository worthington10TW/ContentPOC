#!/bin/sh

set -eu

git config --global user.email 'gocd@lexisnexis.co.uk' && git config --global user.name 'gocd'
mkdir -p deploy/releases/matt-and-andrew
gomplate -f source/gocd/release-template.yaml.tmpl -o deploy/releases/matt-and-andrew/contentpoc.yaml
cd deploy
git add . && git commit --allow-empty -m "adding ${GO_PIPELINE_LABEL} release of matt-and-andrew/contentpoc" && git pull -r && git push
