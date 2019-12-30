variable "configuration_name" {
  default = "gusztavvargadr-packet-samples-device-linux"
}

variable "project_name" {
  default = "default"
}

variable "device_facility" {
  default = "ewr1"
}

variable "device_plan" {
  default = "t1.small.x86"
}

variable "device_os_name" {
  default = "Ubuntu 16.04 LTS"
}

variable "device_user_data_file_path" {
  default = "device_userdata.sh"
}

locals {
  configuration_name = "${var.configuration_name}"
  deployment_name    = "${local.configuration_name}-${terraform.workspace}"

  project_name = "${var.project_name}"

  device_facility            = "${var.device_facility}"
  device_plan                = "${var.device_plan}"
  device_os_name             = "${var.device_os_name}"
  device_user_data_file_path = "${path.root}/${var.device_user_data_file_path}"
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
