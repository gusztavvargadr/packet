FROM gittools/gitversion:5.1.3-linux

WORKDIR /work

CMD [ "/showvariable", "SemVer" ]
