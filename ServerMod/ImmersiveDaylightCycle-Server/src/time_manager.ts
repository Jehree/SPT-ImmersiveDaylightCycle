import { Config, IDCTime } from "./types";
import { Utils } from "./mod_helper";
import { ModHelper } from "./mod_helper";

export class TimeManager {
    private static timePath = Utils.pathCombine([Utils.modPath, "runtime_data", "time.json"]);

    public static onClientTimeRequest(
        url: string,
        info: any,
        sessionId: string,
        output: string,
        Helper: ModHelper
    ): string {
        return JSON.stringify(this.getCurrentTime());
    }

    public static getCurrentTime(): IDCTime {
        return Utils.readJson<IDCTime>(this.timePath);
    }

    public static resetCurrentTime(): void {
        this.setCurrentTime(this.getResetTime());
    }

    public static doTimeJump(): void {
        const time = this.getCurrentTime();
        const config = Config.getConfig();
        time.hour += config.raid_exit_time_jump;
        this.setCurrentTime(time);
    }

    public static addSeconds(seconds: number) {
        const time = this.getCurrentTime();

        const totalSeconds = time.hour * 3600 + time.minute * 60 + time.second + seconds;
        const newHour = Math.floor((totalSeconds / 3600) % 24);
        const newMinute = Math.floor((totalSeconds / 60) % 60);
        const newSecond = totalSeconds % 60;

        time.hour = newHour;
        time.minute = newMinute;
        time.second = newSecond;

        this.setCurrentTime(time);
    }

    private static setCurrentTime(newTime: IDCTime): void {
        Utils.writeJson(JSON.stringify(newTime), this.timePath);
    }

    private static getResetTime(): IDCTime {
        const config = Config.getConfig();
        return {
            hour: config.reset_hour,
            minute: config.reset_minute,
            second: config.reset_second,
            cycle_rate: config.cycle_rate,
        };
    }
}
