locals {
  configuration_name = "packet-samples-device-windows"
  deployment_name    = "${local.configuration_name}-${terraform.workspace}"

  project_name = "core"

  device_facility            = "ewr1"
  device_plan                = "t1.small.x86"
  device_os_name             = "Windows 2016 Standard"
  device_user_data_file_path = "${path.root}/device_userdata.ps1"
}

output "device_id" {
  value = "${local.device_id}"
}

output "device_name" {
  value = "${local.device_name}"
}

output "device_ip" {
  value = "${local.device_ip}"
}

output "key_id" {
  value = "${local.key_id}"
}

output "key_name" {
  value = "${local.key_name}"
}

output "key_public_file_path" {
  value = "${local.key_public_file_path}"
}

output "key_private_file_path" {
  value = "${local.key_private_file_path}"
}
