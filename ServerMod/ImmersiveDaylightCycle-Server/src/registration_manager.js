"use strict";
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    var desc = Object.getOwnPropertyDescriptor(m, k);
    if (!desc || ("get" in desc ? !m.__esModule : desc.writable || desc.configurable)) {
      desc = { enumerable: true, get: function() { return m[k]; } };
    }
    Object.defineProperty(o, k2, desc);
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.RegistrationManager = void 0;
const instance_manager_1 = require("./instance_manager");
const fs = __importStar(require("fs"));
const types_1 = require("./types");
const LogTextColor_1 = require("C:/snapshot/project/obj/models/spt/logging/LogTextColor");
const ExitStatis_1 = require("C:/snapshot/project/obj/models/enums/ExitStatis");
const time_manager_1 = require("./time_manager");
class RegistrationManager {
    static raidPath = instance_manager_1.Utils.pathCombine([instance_manager_1.Utils.modPath, "runtime_data", "raid_session.json"]);
    static onHostRaidStarted(url, info, sessionId, output, Inst) {
        const raid = this.createNewRaid(info.data);
        if (this.raidExists()) {
            const profileIds = Object.keys(Inst.profileHelper.getProfiles());
            const currentRaidSessionHostIsLoggedIn = this.getRaid().raid_id in profileIds;
            if (!currentRaidSessionHostIsLoggedIn) {
                this.cleanUpRaidSession();
            }
            else {
                Inst.log(`Raid session already exists, raid [${raid.raid_id}] will not advance global time`, LogTextColor_1.LogTextColor.YELLOW);
                return;
            }
        }
        this.createNewRaidFile(raid);
    }
    static onClientRaidExited(url, info, sessionId, output, Inst) {
        const exitInfo = info;
        if (!this.raidExists())
            return;
        const raid = this.getRaid();
        if (raid.raid_id != "singleplayer" && raid.raid_id != exitInfo.raid_id)
            return;
        this.addClientExitInfo(exitInfo, raid);
        if (exitInfo.is_host) {
            const config = types_1.Config.getConfig();
            const anyoneDisconnected = this.anyClientHasExitStatus(raid, ExitStatis_1.ExitStatus.LEFT);
            const anyoneDied = this.anyClientHasExitStatus(raid, ExitStatis_1.ExitStatus.KILLED);
            if ((config.time_resets_on_death && anyoneDied) ||
                (config.time_resets_on_disconnect && anyoneDisconnected)) {
                time_manager_1.TimeManager.resetCurrentTime();
            }
            else {
                time_manager_1.TimeManager.addSeconds(exitInfo.seconds_in_raid);
                if (exitInfo.seconds_in_raid > 60 * 5)
                    time_manager_1.TimeManager.doTimeJump();
            }
            this.cleanUpRaidSession();
        }
    }
    static onConsoleCommandReceived(url, info, sessionId, output, Inst) {
        const command = info;
        switch (command.type) {
            case "idc_status": {
                if (this.raidExists()) {
                    command.message = `An IDC raid session with the id: ${this.getRaid().raid_id} exists`;
                }
                else {
                    command.message = "No IDC raid session exists";
                }
                break;
            }
            case "idc_clear": {
                if (this.raidExists()) {
                    command.message = `An IDC raid session with the id: ${this.getRaid().raid_id} has been cleared!`;
                    this.cleanUpRaidSession();
                }
                else {
                    command.message = "No IDC raid session exists to clear";
                }
                break;
            }
            default: {
                command.message = "Invalid command (This should never happen! Something is wrong!)";
            }
        }
        return JSON.stringify(command);
    }
    static onProfileLogin(url, info, sessionId, output, Inst) {
        if (!this.raidExists())
            return;
        const profileId = output;
        if (profileId != this.getRaid().raid_id)
            return;
        Inst.log("Raid session cleanup needed! Cleaning session for profile: " + profileId, LogTextColor_1.LogTextColor.RED);
        this.cleanUpRaidSession();
    }
    static anyClientHasExitStatus(raid, exitStatus) {
        for (const profileId in raid.client_exits) {
            const exitInfo = raid.client_exits[profileId];
            if (exitInfo.is_dedicated_client)
                continue;
            if (exitInfo.exit_status == exitStatus)
                return true;
        }
        return false;
    }
    static cleanUpRaidSession() {
        if (!this.raidExists())
            return;
        fs.unlinkSync(this.raidPath);
    }
    static getRaid() {
        return instance_manager_1.Utils.readJson(this.raidPath);
    }
    static raidExists() {
        return fs.existsSync(this.raidPath);
    }
    static createNewRaidFile(raid) {
        instance_manager_1.Utils.writeJson(JSON.stringify(raid), this.raidPath);
    }
    static createNewRaid(raidId) {
        return {
            raid_id: raidId,
            client_exits: {},
        };
    }
    static addClientExitInfo(info, raid) {
        raid.client_exits[info.profile_id] = info;
    }
}
exports.RegistrationManager = RegistrationManager;
//# sourceMappingURL=registration_manager.js.map