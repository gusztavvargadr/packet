resource "null_resource" "provision" {
  triggers = {
    device_id = "${local.device_id}"
  }

  connection {
    type        = "ssh"
    host        = "${local.device_ip}"
    user        = "Admin"
    private_key = "${local.key_private}"
  }

  provisioner "remote-exec" {
    inline = [
      "echo 'Hello World!'",
    ]
  }
}
