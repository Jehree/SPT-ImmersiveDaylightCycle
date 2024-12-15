"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.mod = void 0;
const instance_manager_1 = require("./instance_manager");
const time_manager_1 = require("./time_manager");
const registration_manager_1 = require("./registration_manager");
// UI panel patch, client runs custom route to request time and caches it
// server hooks route and sets the output to give it to client
// on raid start, client sets time to cached time
// on raid start, client runs second route to register itself with server as a player of the raid
// on raid end, client runs third route to tell server to what it's exit status is
// when the server mod detects that the final client has left, it updates it's time based on:
// time spent in raid + time jump
// OR
// if ALL registered players died, reset time
// route 1: player to session registration
//  - this route will also be what's used to handle end of raid things once the last client disconnects
// route 2: time request
class Mod {
    Inst = new instance_manager_1.InstanceManager();
    preSptLoad(container) {
        this.Inst.init(container, instance_manager_1.InitStage.PRE_SPT_LOAD);
        this.Inst.registerStaticRoute("/jehree/idc/console_command", "ImmersiveDaylightCycle-ConsoleCommand", registration_manager_1.RegistrationManager.onConsoleCommandReceived, registration_manager_1.RegistrationManager, true);
        this.Inst.registerStaticRoute("/jehree/idc/host_raid_started", "ImmersiveDaylightCycle-HostRaidStarted", registration_manager_1.RegistrationManager.onHostRaidStarted, registration_manager_1.RegistrationManager);
        this.Inst.registerStaticRoute("/jehree/idc/client_exited", "ImmersiveDaylightCycle-ClientExitedRaid", registration_manager_1.RegistrationManager.onClientRaidExited, registration_manager_1.RegistrationManager);
        this.Inst.registerStaticRoute("/jehree/idc/request_time", "ImmersiveDaylightCycle-ClientTimeRequested", time_manager_1.TimeManager.onClientTimeRequest, time_manager_1.TimeManager, true);
        this.Inst.registerStaticRoute("/launcher/profile/login", "ImmeriveDaylightCycle-CleanupCrashedSession", registration_manager_1.RegistrationManager.onProfileLogin, registration_manager_1.RegistrationManager);
    }
    postDBLoad(container) {
        this.Inst.init(container, instance_manager_1.InitStage.POST_DB_LOAD);
    }
}
exports.mod = new Mod();
//# sourceMappingURL=mod.js.map