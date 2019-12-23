locals {
  key_public_file_path  = "${path.root}/.terraform/outputs/${local.key_name}.pub"
  key_private_file_path = "${path.root}/.terraform/outputs/${local.key_name}"
}

resource "local_file" "key_public" {
  content  = "${local.key_public}"
  filename = "${local.key_public_file_path}"
}

resource "local_file" "key_private" {
  sensitive_content = "${local.key_private}"
  filename          = "${local.key_private_file_path}"
}
