terraform {
  required_version = ">= 0.12.0"

  backend "consul" {
    path = "packet-samples-device-windows/.terraform/terraform.tfstate"
  }
}

provider "packet" {
  version = "= 2.10.1"
}

provider "tls" {
  version = "= 2.1.1"
}

provider "local" {
  version = "= 1.4.0"
}

provider "null" {
  version = "= 2.1.2"
}
