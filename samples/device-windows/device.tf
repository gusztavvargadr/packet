module "device" {
  source = "../../src/device"

  deployment_name = "${local.deployment_name}"

  project_name = "${local.project_name}"

  device_facility  = "${local.device_facility}"
  device_plan      = "${local.device_plan}"
  device_os_name   = "${local.device_os_name}"
  device_user_data = "${file("${local.device_user_data_file_path}")}"
  device_count     = "${local.device_count}"
}

locals {
  device_ids   = "${module.device.device_ids}"
  device_names = "${module.device.device_names}"
  device_ips   = "${module.device.device_ips}"

  key_id      = "${module.device.key_id}"
  key_name    = "${module.device.key_name}"
  key_public  = "${module.device.key_public}"
  key_private = "${module.device.key_private}"
}
