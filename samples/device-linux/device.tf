locals {
  device_hostname = "${local.deployment_name}"

  device_user_data_file_path = "${path.root}/device_userdata.sh"
}

data "packet_operating_system" "device" {
  name             = "${local.device_os_name}"
  provisionable_on = "${local.device_plan}"
}

resource "packet_device" "device" {
  project_id = "${local.project_id}"

  hostname         = "${local.device_hostname}"
  facilities       = ["${local.device_facility}"]
  plan             = "${local.device_plan}"
  billing_cycle    = "${local.device_billing_cycle}"
  operating_system = "${data.packet_operating_system.device.slug}"

  project_ssh_key_ids = ["${local.key_id}"]
  user_data           = "${file("${local.device_user_data_file_path}")}"
}

locals {
  device_id = "${packet_device.device.id}"
  device_ip = "${packet_device.device.access_public_ipv4}"
}

