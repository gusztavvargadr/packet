FROM gittools/gitversion:5.1.2-linux

WORKDIR /work

CMD [ "/showvariable", "SemVer" ]
