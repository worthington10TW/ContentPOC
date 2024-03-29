#!/bin/bash
set -eu
git remote add heroku https://git.heroku.com/$1.git
wget -qO- https://cli-assets.heroku.com/install-ubuntu.sh 1 | sh
cat > ~/.netrc << EOF
machine api.heroku.com
login $HEROKU_USERNAME
password $HEROKU_API_KEY
machine git.heroku.com
login $HEROKU_USERNAME
password $HEROKU_API_KEY
EOF
chmod 600 ~/.netrc
