locals {
  device_name     = "${local.deployment_name}"
  device_hostname = "${local.device_name}"
}

resource "packet_device" "device" {
  project_id = "${local.project_id}"

  hostname         = "${local.device_hostname}"
  facilities       = ["${local.device_facility}"]
  plan             = "${local.device_plan}"
  billing_cycle    = "${local.device_billing_cycle}"
  operating_system = "${local.operating_system_slug}"

  project_ssh_key_ids = ["${local.key_id}"]
  user_data           = "${local.device_user_data}"
}

locals {
  device_id = "${packet_device.device.id}"
  device_ip = "${packet_device.device.access_public_ipv4}"
}
