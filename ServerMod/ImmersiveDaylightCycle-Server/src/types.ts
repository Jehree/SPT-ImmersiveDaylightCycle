import { ExitStatus } from "@spt/models/enums/ExitStatis";
import { Utils } from "./instance_manager";

export type IDCConfig = {
    raid_exit_time_jump: number;
    cycle_rate: number;
    time_resets_on_death: boolean;
    time_resets_on_disconnect: boolean;
    reset_hour: number;
    reset_minute: number;
    reset_second: number;
};

export type IDCRaid = {
    raid_id: string;
    client_exits: { [key: string]: IDCClientExitInfo };
};

export type IDCClientExitInfo = {
    raid_id: string;
    profile_id: string;
    exit_status: ExitStatus;
    is_host: boolean;
    is_dedicated_client: boolean;
    seconds_in_raid: number;
};

export type IDCTime = {
    hour: number;
    minute: number;
    second: number;
    cycle_rate: number;
};

export type IDCCommand = {
    type: string;
    message: string;
};

export class Config {
    public static getConfig(): IDCConfig {
        return JSON.parse(Utils.pathCombine([Utils.modPath, "config.json"])) as IDCConfig;
    }
}
