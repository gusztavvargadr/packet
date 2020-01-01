FROM hashicorp/terraform:0.12.18

ADD ./build/docker/sample.terraform.hcl /root/.terraform.d/.terraformrc
ENV TF_CLI_CONFIG_FILE /root/.terraform.d/.terraformrc

WORKDIR /opt/packet/

ADD ./src/ ./src/

ARG SAMPLE_NAME
ADD ./samples/${SAMPLE_NAME}/ ./samples/${SAMPLE_NAME}/

WORKDIR /opt/packet/samples/${SAMPLE_NAME}/

VOLUME /opt/packet/samples/${SAMPLE_NAME}/.terraform/

RUN terraform init -backend=false

CMD [ "-help" ]
