terraform {
  required_version = ">= 0.12.18"

  backend "remote" {
    hostname     = "app.terraform.io"
    organization = "gusztavvargadr"

    workspaces {
      prefix = "packet-samples-device-linux-"
    }
  }
}

provider "packet" {
  version = ">= 2.7.3"
}

provider "tls" {
  version = ">= 2.1.1"
}

provider "local" {
  version = ">= 1.4.0"
}

provider "null" {
  version = ">= 2.1.2"
}
