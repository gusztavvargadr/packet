data "packet_operating_system" "operating_system" {
  name             = "${local.device_os_name}"
  provisionable_on = "${local.device_plan}"
}

locals {
  operating_system_slug = "${data.packet_operating_system.operating_system.slug}"
}
