module "device" {
  source = "../../src/device"

  deployment_name = "${local.deployment_name}"

  project_name = "${local.project_name}"

  device_facility  = "${local.device_facility}"
  device_plan      = "${local.device_plan}"
  device_os_name   = "${local.device_os_name}"
  device_user_data = "${file("${local.device_user_data_file_path}")}"
}

locals {
  device_id   = "${module.device.device_id}"
  device_name = "${module.device.device_name}"
  device_ip   = "${module.device.device_ip}"

  key_id      = "${module.device.key_id}"
  key_name    = "${module.device.key_name}"
  key_public  = "${module.device.key_public}"
  key_private = "${module.device.key_private}"
}
