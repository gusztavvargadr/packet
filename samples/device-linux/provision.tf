resource "null_resource" "provision" {
  triggers = {
    device_id = local.device_ids[count.index]
  }

  provisioner "local-exec" {
    command = "ssh -oStrictHostKeyChecking=no -i ${local.key_private_file_path} root@${local.device_ips[count.index]} echo 'Hello World!'"
  }

  count = local.device_count
}
