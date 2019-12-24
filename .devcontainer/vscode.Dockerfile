
#-------------------------------------------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See https://go.microsoft.com/fwlink/?linkid=2090316 for license information.
#-------------------------------------------------------------------------------------------------------------

FROM library/ubuntu:16.04

ENV DEBIAN_FRONTEND=noninteractive

ARG COMPOSE_VERSION=1.24.0

ARG USERNAME=vscode
ARG USER_UID=1000
ARG USER_GID=$USER_UID

RUN apt-get update \
  && apt-get -y install --no-install-recommends apt-utils dialog 2>&1 \
  && apt-get -y install git iproute2 procps \
  && apt-get install -y apt-transport-https ca-certificates curl unzip gnupg-agent software-properties-common lsb-release \
  && curl -fsSL https://download.docker.com/linux/$(lsb_release -is | tr '[:upper:]' '[:lower:]')/gpg | (OUT=$(apt-key add - 2>&1) || echo $OUT) \
  && add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/$(lsb_release -is | tr '[:upper:]' '[:lower:]') $(lsb_release -cs) stable" \
  && apt-get update \
  && apt-get install -y docker-ce-cli \
  && curl -sSL "https://github.com/docker/compose/releases/download/${COMPOSE_VERSION}/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose \
  && chmod +x /usr/local/bin/docker-compose \
  && groupadd --gid $USER_GID $USERNAME \
  && useradd -s /bin/bash --uid $USER_UID --gid $USER_GID -m $USERNAME \
  && apt-get install -y sudo \
  && echo $USERNAME ALL=\(root\) NOPASSWD:ALL > /etc/sudoers.d/$USERNAME\
  && chmod 0440 /etc/sudoers.d/$USERNAME \
  && apt-get autoremove -y \
  && apt-get clean -y \
  && rm -rf /var/lib/apt/lists/*

ENV DEBIAN_FRONTEND=dialog

RUN curl -sSL https://releases.hashicorp.com/terraform/0.12.18/terraform_0.12.18_linux_amd64.zip -o /tmp/terraform.zip \
  && unzip -qq /tmp/terraform.zip -d /usr/local/bin/ \
  && rm /tmp/terraform.zip

ADD ./vscode.terraform.hcl $HOME/.terraform.d/.terraformrc
ENV TF_CLI_CONFIG_FILE=$HOME/.terraform.d/.terraformrc

RUN curl -sSL https://github.com/terraform-linters/tflint/releases/download/v0.13.2/tflint_linux_amd64.zip -o /tmp/tflint.zip \
  && unzip -qq /tmp/tflint.zip -d /usr/local/bin/ \
  && rm /tmp/tflint.zip
