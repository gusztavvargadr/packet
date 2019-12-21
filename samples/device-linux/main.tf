locals {
  configuration_name = "packet-samples-device-linux"
  deployment_name    = "${local.configuration_name}-${terraform.workspace}"

  project_name = "core"

  device_facility      = "ewr1"
  device_plan          = "t1.small.x86"
  device_billing_cycle = "hourly"
  device_os_name       = "Ubuntu 16.04 LTS"
}

output "device_id" {
  value = "${local.device_id}"
}

output "device_ip" {
  value = "${local.device_ip}"
}

output "key_file_path" {
  value = "${local.key_file_path}"
}
