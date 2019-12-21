data "packet_project" "project" {
  name = "${local.project_name}"
}

locals {
  project_id = "${data.packet_project.project.project_id}"
}
