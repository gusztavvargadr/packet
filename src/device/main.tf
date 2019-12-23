variable "deployment_name" {
  default = "packet-samples-device-default"
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

output "key_public" {
  value = "${local.key_public}"
}

output "key_private" {
  value = "${local.key_private}"
}
