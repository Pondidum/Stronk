#! /bin/sh

set -e  # stop on errors

find=/usr/bin/find  # appveyor has find pointing to /c/windows/system32/find for some reason

MODE=${1:-Debug}  # First parameter is build mode, defaults to Debug
NAME=$(basename $(ls *.sln | head -n 1) .sln) # Find the solution file

if [ -x "$(command -v docker)" ]; then
  CONTAINER=$(docker run -d --rm -p 8600:8600 -p 8500:8500 -p 8300:8300 -e CONSUL_BIND_INTERFACE=eth0 consul)
  echo "Started Consul container: $CONTAINER"
  sleep 2
fi

dotnet build \
  --configuration $MODE

find ./src -iname "*.Tests.csproj" | xargs -L1 dotnet test \
  --no-restore \
  --no-build \
  --configuration $MODE \

find ./src -iname "Stronk*.csproj" -not -iname "*.Tests.csproj" | xargs -L1 dotnet pack \
  --no-restore \
  --no-build \
  --configuration $MODE \
  --output ../../.build

if [ -x "$(command -v docker)" ]; then
  docker stop $CONTAINER &> /dev/null
  echo "Removed Consul container"
fi
