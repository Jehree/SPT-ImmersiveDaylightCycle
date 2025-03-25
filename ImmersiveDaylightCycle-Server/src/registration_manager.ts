import { ModHelper, Utils } from "./mod_helper";
import * as fs from "fs";
import { IDCRaid, Config, IDCClientExitInfo, IDCCommand } from "./types";
import { LogTextColor } from "@spt/models/spt/logging/LogTextColor";
import { ExitStatus } from "@spt/models/enums/ExitStatis";
import { TimeManager } from "./time_manager";

export class RegistrationManager {
    private static raidPath: string = Utils.pathCombine([Utils.modPath, "runtime_data", "raid_session.json"]);

    public static onHostRaidStarted(
        url: string,
        info: any,
        sessionId: string,
        output: string,
        Helper: ModHelper
    ): void {
        const raid = this.createNewRaid(info.data);

        if (this.raidExists()) {
            const profileIds = Object.keys(Helper.profileHelper.getProfiles());
            const currentRaidSessionHostIsLoggedIn = this.getRaid().raid_id in profileIds;

            if (!currentRaidSessionHostIsLoggedIn) {
                Helper.log(
                    "Raid session cleanup needed! This usually means the someone crashed. Cleaning...",
                    LogTextColor.RED
                );
                this.cleanUpRaidSession();
            } else {
                Helper.log(
                    `Raid session already exists, raid [${raid.raid_id}] will not advance global time`,
                    LogTextColor.YELLOW
                );
                return;
            }
        }

        this.createNewRaidFile(raid);
    }

    public static onClientRaidExited(
        url: string,
        info: any,
        sessionId: string,
        output: string,
        Helper: ModHelper
    ): void {
        const exitInfo = info as IDCClientExitInfo;
        if (!this.raidExists()) return;
        const raid = this.getRaid();
        if (raid.raid_id != "singleplayer" && raid.raid_id != exitInfo.raid_id) return;

        this.addClientExitInfo(exitInfo, raid);

        if (exitInfo.is_host) {
            const config = Config.getConfig();
            const anyoneDisconnected: boolean = this.anyClientHasExitStatus(raid, ExitStatus.LEFT);

            const anyoneDied: boolean = this.anyClientHasExitStatus(raid, ExitStatus.KILLED);

            if (
                (config.time_resets_on_death && anyoneDied) ||
                (config.time_resets_on_disconnect && anyoneDisconnected)
            ) {
                TimeManager.resetCurrentTime();
            } else {
                TimeManager.addSeconds(exitInfo.seconds_in_raid);
                if (exitInfo.seconds_in_raid > 60 * 5) TimeManager.doTimeJump();
            }

            this.cleanUpRaidSession();
        }
    }

    public static onConsoleCommandReceived(
        url: string,
        info: any,
        sessionId: string,
        output: string,
        Helper: ModHelper
    ): string {
        const command = info as IDCCommand;

        switch (command.type) {
            case "idc_status": {
                if (this.raidExists()) {
                    command.message = `An ImmersiveDaylightCycle raid session with the id: ${
                        this.getRaid().raid_id
                    } exists`;
                } else {
                    command.message = "No ImmersiveDaylightCycle raid session exists";
                }
                break;
            }
            case "idc_clear": {
                if (this.raidExists()) {
                    command.message = `An ImmersiveDaylightCycle raid session with the id: ${
                        this.getRaid().raid_id
                    } has been cleared!`;
                    this.cleanUpRaidSession();
                } else {
                    command.message = "No ImmersiveDaylightCycle raid session exists to clear";
                }
                break;
            }
            default: {
                command.message = "Invalid command (This should never happen! Something is wrong!)";
            }
        }

        return JSON.stringify(command);
    }

    public static onProfileLogin(url: string, info: any, sessionId: string, output: string, Helper: ModHelper): void {
        if (!this.raidExists()) return;
        const profileId = output;
        if (profileId != this.getRaid().raid_id) return;
        Helper.log(
            "Raid session cleanup needed!  This usually means the someone crashed. Cleaning...",
            LogTextColor.RED
        );
        this.cleanUpRaidSession();
    }

    private static anyClientHasExitStatus(raid: IDCRaid, exitStatus: ExitStatus): boolean {
        for (const profileId in raid.client_exits) {
            const exitInfo = raid.client_exits[profileId];

            if (exitInfo.is_dedicated_client) continue;
            if (exitInfo.exit_status == exitStatus) return true;
        }
        return false;
    }

    private static cleanUpRaidSession() {
        if (!this.raidExists()) return;
        fs.unlinkSync(this.raidPath);
    }

    private static getRaid(): IDCRaid {
        return Utils.readJson<IDCRaid>(this.raidPath);
    }

    private static raidExists(): boolean {
        return fs.existsSync(this.raidPath);
    }

    private static createNewRaidFile(raid: IDCRaid): void {
        Utils.writeJson(JSON.stringify(raid), this.raidPath);
    }

    private static createNewRaid(raidId: string): IDCRaid {
        return {
            raid_id: raidId,
            client_exits: {},
        } as IDCRaid;
    }

    private static addClientExitInfo(info: IDCClientExitInfo, raid: IDCRaid) {
        raid.client_exits[info.profile_id] = info;
    }
}
