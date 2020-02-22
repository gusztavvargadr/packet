resource "null_resource" "provision" {
  triggers = {
    device_id = local.device_ids[count.index]
  }

  connection {
    type        = "ssh"
    host        = local.device_ips[count.index]
    user        = "root"
    private_key = local.key_private
  }

  provisioner "remote-exec" {
    inline = [
      "echo 'Hello World!'",
    ]
  }

  count = local.device_count
}
