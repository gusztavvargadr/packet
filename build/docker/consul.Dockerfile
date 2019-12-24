FROM library/consul:1.5.3

ADD ./build/docker/consul.agent.hcl /consul/config/agent.hcl

CMD [ "agent" ]
