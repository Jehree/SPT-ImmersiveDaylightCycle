import { InstanceManager, Utils } from "./instance_manager";
import * as fs from "fs";
import { IDCRaid, Config, IDCClientExitInfo, IDCCommand } from "./types";
import { LogTextColor } from "@spt/models/spt/logging/LogTextColor";
import { ExitStatus } from "@spt/models/enums/ExitStatis";
import { TimeManager } from "./time_manager";
import { DateTime } from "@spt/models/enums/DateTime";

export class RegistrationManager {
    private static raidPath: string = Utils.pathCombine([Utils.modPath, "runtime_data", "raid_session.json"]);

    public static onHostRaidStarted(
        url: string,
        info: any,
        sessionId: string,
        output: string,
        Inst: InstanceManager
    ): void {
        const raid = this.createNewRaid(output);

        if (this.raidExists()) {
            Inst.log(
                `Raid session already exists, raid [${raid.raid_id}] will not advance global time`,
                LogTextColor.YELLOW
            );
            return;
        }

        this.createNewRaidFile(raid);
    }

    public static onClientRaidExited(
        url: string,
        info: any,
        sessionId: string,
        output: string,
        Inst: InstanceManager
    ): void {
        const exitInfo = JSON.parse(output) as IDCClientExitInfo;
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
            }

            this.cleanUpRaidSession();
        }
    }

    public static onConsoleCommandReceived(
        url: string,
        info: any,
        sessionId: string,
        output: string,
        Inst: InstanceManager
    ): string {
        console.log(output);

        const command = JSON.parse(output) as IDCCommand;

        switch (command.type) {
            case "idc_status": {
                if (this.raidExists()) {
                    command.message = `An IDC raid session with the id: ${this.getRaid().raid_id} exists`;
                } else {
                    command.message = "No IDC raid sessions exist";
                }
            }
            case "idc_clear": {
                if (this.raidExists()) {
                    this.cleanUpRaidSession();
                    command.message = `An IDC raid session with the id: ${this.getRaid().raid_id} has been cleared!`;
                } else {
                    command.message = "No IDC raid sessions exists to clear";
                }
            }
        }
        command.message = "Invalid command";

        return JSON.stringify(command);
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
