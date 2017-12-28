## Vagrantfile
Vagrant.configure(2) do |config|
    # OS
    config.vm.box = "ubuntu/trusty64"

    # Network configuration
    config.vm.network "private_network", ip: "172.28.128.5"
    config.vm.network "forwarded_port", guest: 80, host: 8080, auto_correct: true

    # Synced folder
    config.vm.synced_folder "./Provision", "/vagrant"
    config.vm.synced_folder "#{ENV['HOME']}/Dev/",
        "/data/sites",
        :owner => 'vagrant',
        :group => 'vagrant',
        :mount_options => ["dmode=777","fmode=666"]

    # Memory
    config.vm.provider "virtualbox" do |vb|
        vb.customize ["modifyvm", :id, "--memory", "1024"]
    end
  
    # Provision scripts
    $script = <<-SCRIPT
        sh -c 'echo "deb [arch=amd64] http://apt-mo.trafficmanager.net/repos/dotnet/ trusty main" > /etc/apt/sources.list.d/dotnetdev.list'
        apt-key adv --keyserver apt-mo.trafficmanager.net --recv-keys 417A0893
        apt-get update
        apt-get install dotnet -y 

        curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg
        sudo mv microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg
        sudo sh -c 'echo "deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-ubuntu-trusty-prod trusty main" > /etc/apt/sources.list.d/dotnetdev.list'

        sudo apt-get update
        sudo apt-get install -y dotnet-sdk-2.0.3

        apt-get update
        apt-get install -y nginx
        sudo apt-get clean

        cat /vagrant/nginx/default-site > /etc/nginx/sites-available/default

        service nginx restart
    SCRIPT
  
    config.vm.provision "shell", inline: $script
end
