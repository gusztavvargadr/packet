ARG TERRAFORM_VERSION

FROM hashicorp/terraform:${TERRAFORM_VERSION}

WORKDIR /opt/terraform/

ADD ./build/docker/sample.terraform.hcl /root/.terraform.d/.terraformrc
ENV TF_CLI_CONFIG_FILE /root/.terraform.d/.terraformrc

ADD ./src/ ./src/

ARG SAMPLE_NAME

ADD ./samples/${SAMPLE_NAME}/ ./samples/${SAMPLE_NAME}/

WORKDIR /opt/terraform/samples/${SAMPLE_NAME}/

RUN terraform init -backend=false

CMD [ "-help" ]
