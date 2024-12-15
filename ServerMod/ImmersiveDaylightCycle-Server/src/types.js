"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Config = void 0;
const instance_manager_1 = require("./instance_manager");
class Config {
    static configPath = instance_manager_1.Utils.pathCombine([instance_manager_1.Utils.modPath, "config.json"]);
    static getConfig() {
        return instance_manager_1.Utils.readJson(this.configPath);
    }
}
exports.Config = Config;
//# sourceMappingURL=types.js.map