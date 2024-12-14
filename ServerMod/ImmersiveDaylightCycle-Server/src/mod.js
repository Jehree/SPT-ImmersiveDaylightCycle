"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.mod = void 0;
const instance_manager_1 = require("./instance_manager");
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
        /*
        this.Inst.staticRouter.registerStaticRouter(
            "MyStaticModRouter",
            [
                {
                    url: "/jehree/idc/console_command",
                    action: async (url, info, sessionId, output) => {
                        console.log(output);
                        return output;
                    },
                },
            ],
            "custom-static-my-mod"
        );
        */
        /*
        this.Inst.registerStaticRoute(
            "/jehree/idc/request_time",
            "ImmersiveDaylightCycle-ClientTimeRequested",
            TimeManager.onClientTimeRequest,
            undefined,
            true
        );
        this.Inst.registerStaticRoute(
            "/jehree/idc/host_raid_started",
            "ImmersiveDaylightCycle-HostRaidStarted",
            RegistrationManager.onHostRaidStarted,
            undefined
        );
        this.Inst.registerStaticRoute(
            "/jehree/idc/client_exited",
            "ImmersiveDaylightCycle-ClientExitedRaid",
            RegistrationManager.onClientRaidExited,
            undefined
        );
        */
        this.Inst.registerStaticRoute("/jehree/idc/console_command", "ImmersiveDaylightCycle-ConsoleCommand", registration_manager_1.RegistrationManager.onConsoleCommandReceived, undefined, true);
    }
    postDBLoad(container) {
        this.Inst.init(container, instance_manager_1.InitStage.POST_DB_LOAD);
    }
}
exports.mod = new Mod();
//# sourceMappingURL=mod.js.map