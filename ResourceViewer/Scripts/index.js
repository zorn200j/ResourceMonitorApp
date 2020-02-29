$(function () {

   
    var ViewModel = function () {
        var self = this;

        
        self.connected = ko.observable(false);

        
        self.machines = ko.observableArray();
    };

    
    var vm = new ViewModel();

    
    ko.applyBindings(vm, $("#computerInfo")[0]);

    
    var hub = $.connection.pcInfo;

    
    hub.client.pcInfoMessage = function (machineName, cpu, memUsage, upTime, processes, disk, interrupts, mutexes, memTotal) {

        var machine = {
            machineName: machineName,
            cpu: cpu.toFixed(0),
            memUsage: (memUsage / 1024).toFixed(2) + " Mb",
            memTotal: (memTotal / 1024).toFixed(2) + " Mb",
            memPercent: ((memUsage / memTotal) * 100).toFixed(1) + "%",
            upTime: (upTime / 3600).toFixed(2) + " Hrs",
            processes: processes,
            disk: disk.toFixed(0) + " Bytes/sec",
            interrupts: interrupts.toFixed(0),
            mutexes: mutexes.toFixed(0)
            
        };

        var machineModel = ko.mapping.fromJS(machine);

        
        var match = ko.utils.arrayFirst(vm.machines(), function (item) {
            return item.machineName() == machineName;
        });

        if (!match)
            vm.machines.push(machineModel);
        else {
            var index = vm.machines.indexOf(match);
            vm.machines.replace(vm.machines()[index], machineModel);
        }
    };

    
    $.connection.hub.start().done(function () {
        vm.connected(true);
    });
});