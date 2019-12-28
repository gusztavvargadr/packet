FROM library/consul:1.5.3

ADD ./consul.agent.hcl /consul/config/agent.hcl

CMD [ "agent" ]
