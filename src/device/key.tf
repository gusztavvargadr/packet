locals {
  key_name      = "${local.deployment_name}"
  key_algorithm = "RSA"
  key_rsa_bits  = "2048"
}

resource "tls_private_key" "key" {
  algorithm = "${local.key_algorithm}"
  rsa_bits  = "${local.key_rsa_bits}"
}

resource "packet_project_ssh_key" "key" {
  project_id = "${local.project_id}"

  name       = "${local.key_name}"
  public_key = "${tls_private_key.key.public_key_openssh}"
}

locals {
  key_id      = "${packet_project_ssh_key.key.id}"
  key_public  = "${tls_private_key.key.public_key_openssh}"
  key_private = "${tls_private_key.key.private_key_pem}"
}
