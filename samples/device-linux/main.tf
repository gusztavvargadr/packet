variable "configuration_name" {
  default = "packet-samples-device-linux"
}

variable "project_name" {
  default = "default"
}

variable "device_facility" {
  default = "ams1"
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

variable "device_count" {
  default = 1
}

locals {
  configuration_name = "${var.configuration_name}"
  deployment_name    = "${local.configuration_name}-${terraform.workspace}"

  project_name = "${var.project_name}"

  device_facility            = "${var.device_facility}"
  device_plan                = "${var.device_plan}"
  device_os_name             = "${var.device_os_name}"
  device_user_data_file_path = "${path.root}/${var.device_user_data_file_path}"
  device_count               = "${var.device_count}"
}

output "device_ids" {
  value = "${local.device_ids}"
}

output "device_names" {
  value = "${local.device_names}"
}

output "device_ips" {
  value = "${local.device_ips}"
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
