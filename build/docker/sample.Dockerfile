FROM hashicorp/terraform:0.12.18

ADD ./build/docker/sample.terraform.hcl $HOME/.terraform.d/.terraformrc
ENV TF_CLI_CONFIG_FILE=$HOME/.terraform.d/.terraformrc

WORKDIR /opt/gusztavvargadr/packet/

ADD ./src/ ./src/

ARG name
ADD ./samples/${name}/ ./samples/${name}/

WORKDIR /opt/gusztavvargadr/packet/samples/${name}/

RUN terraform init -backend=false

CMD [ "version" ]
