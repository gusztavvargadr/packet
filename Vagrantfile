Vagrant.configure("2") do |config|
  config.vm.box = "gusztavvargadr/docker-linux"

  config.vm.provider "virtualbox" do |provider|
    provider.cpus = 2
    provider.memory = 2048
  end

  config.vm.provider "hyperv" do |provider|
    provider.cpus = 2
    provider.memory = 2048
  end

  config.vm.synced_folder ".", "/vagrant", disabled: true
  config.vm.synced_folder ".", "/home/vagrant/source"

  config.vm.provision "git", type: "shell", path: "./build/vagrant/git.sh", privileged: false
  config.vm.provision "dotnet", type: "shell", path: "./build/vagrant/dotnet.sh", privileged: false
end
