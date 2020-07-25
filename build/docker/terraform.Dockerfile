ARG TERRAFORM_VERSION

FROM hashicorp/terraform:${TERRAFORM_VERSION}

WORKDIR /opt/terraform/

ADD ./build/docker/terraform.cli.hcl /root/.terraform.d/.terraformrc
ENV TF_CLI_CONFIG_FILE /root/.terraform.d/.terraformrc
