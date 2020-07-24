variable "deployment_name" {
  default = "packet-device"
}

variable "project_name" {
  type = string
}

variable "device_facility" {
  type = string
}

variable "device_plan" {
  type = string
}

variable "device_os_name" {
  type = string
}

variable "device_user_data" {
  default = ""
}

variable "device_count" {
  default = 1
}

locals {
  deployment_name = "${var.deployment_name}"

  project_name = "${var.project_name}"

  device_facility      = "${var.device_facility}"
  device_plan          = "${var.device_plan}"
  device_billing_cycle = "hourly"
  device_os_name       = "${var.device_os_name}"
  device_user_data     = "${var.device_user_data}"
  device_count         = "${var.device_count}"
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

output "key_public" {
  value = "${local.key_public}"
}

output "key_private" {
  value = "${local.key_private}"
}
