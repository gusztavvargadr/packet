FROM gittools/gitversion:5.1.3-linux

WORKDIR /opt/gitversion

CMD [ "/showvariable", "SemVer" ]
