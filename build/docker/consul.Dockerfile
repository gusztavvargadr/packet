ARG CONSUL_VERSION

FROM library/consul:${CONSUL_VERSION}

ADD ./consul.agent.hcl /consul/config/agent.hcl

ENV CONSUL_DISABLE_PERM_MGMT=
