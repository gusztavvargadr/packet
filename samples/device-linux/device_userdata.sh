#!/bin/bash

export DEBIAN_FRONTEND=noninteractive 

curl -L https://omnitruck.chef.io/install.sh | bash -s -- -P chef -v 16.1.16
echo "CHEF_LICENSE=accept-silent" >> /etc/environment

apt-get update -y
apt-get -y install p7zip-full
