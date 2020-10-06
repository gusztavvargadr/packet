ARG GITVERSION_VERSION

FROM gittools/gitversion:${GITVERSION_VERSION}

WORKDIR /opt/gitversion/
