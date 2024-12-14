"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.TimeManager = void 0;
const types_1 = require("./types");
const instance_manager_1 = require("./instance_manager");
class TimeManager {
    static timePath = instance_manager_1.Utils.pathCombine([instance_manager_1.Utils.modPath, "runtime_data", "time.json"]);
    static onClientTimeRequest(url, info, sessionId, output, Inst) {
        return instance_manager_1.Utils.readJson(JSON.stringify(this.getCurrentTime()));
    }
    static getCurrentTime() {
        return instance_manager_1.Utils.readJson(this.timePath);
    }
    static resetCurrentTime() {
        this.setCurrentTime(this.getResetTime());
    }
    static doTimeJump() {
        const time = this.getCurrentTime();
        const config = types_1.Config.getConfig();
        time.hour += config.raid_exit_time_jump;
        this.setCurrentTime(time);
    }
    static addSeconds(seconds) {
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
    static setCurrentTime(newTime) {
        instance_manager_1.Utils.writeJson(JSON.stringify(newTime), this.timePath);
    }
    static getResetTime() {
        const config = types_1.Config.getConfig();
        return {
            hour: config.reset_hour,
            minute: config.reset_minute,
            second: config.reset_second,
            cycle_rate: config.cycle_rate,
        };
    }
}
exports.TimeManager = TimeManager;
//# sourceMappingURL=time_manager.js.map