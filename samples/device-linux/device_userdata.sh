#!/bin/bash

export DEBIAN_FRONTEND=noninteractive 

apt-get update -y
apt-get -y install p7zip-full

curl -L https://omnitruck.chef.io/install.sh | bash -s -- -P chef -v 16.4.41
echo "CHEF_LICENSE=accept-silent" >> /etc/environment
