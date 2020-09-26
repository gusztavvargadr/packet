variable "configuration_name" {
  default = "packet-samples-device-windows"
}

variable "project_name" {
  default = "default"
}

variable "device_facilities" {
  default = [ "ams1", "ewr1", "ny5", "iad2", "dc13", "dfw2", "sjc1", "sv15", "sin3", "nrt1" ]
}

variable "device_plan" {
  default = "t1.small.x86"
}

variable "device_os_name" {
  default = "Windows 2016 Standard"
}

variable "device_user_data_file_path" {
  default = "./device_userdata.ps1"
}

variable "device_count" {
  default = 1
}

locals {
  configuration_name = "${var.configuration_name}"
  deployment_name    = "${local.configuration_name}-${terraform.workspace}"

  project_name = "${var.project_name}"

  device_facilities          = "${var.device_facilities}"
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
