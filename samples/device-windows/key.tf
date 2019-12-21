locals {
  key_name      = "${local.deployment_name}"
  key_file_path = "${path.root}/.terraform/outputs/${local.key_name}.pem"
}

resource "tls_private_key" "key" {
  algorithm = "RSA"
  rsa_bits  = "2048"
}

resource "local_file" "key" {
  sensitive_content = "${tls_private_key.key.private_key_pem}"
  filename          = "${local.key_file_path}"
}

resource "packet_project_ssh_key" "key" {
  project_id = "${local.project_id}"

  name = "${local.key_name}"

  public_key = "${tls_private_key.key.public_key_openssh}"
}

locals {
  key_id = "${packet_project_ssh_key.key.id}"
}
