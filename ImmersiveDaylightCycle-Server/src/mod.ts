/* eslint-disable @typescript-eslint/naming-convention */
import { DependencyContainer } from "tsyringe";
import { IPreSptLoadMod } from "@spt/models/external/IPreSptLoadMod";
import { InitStage, ModHelper } from "./mod_helper";
import { TimeManager } from "./time_manager";
import { RegistrationManager } from "./registration_manager";

class Mod implements IPreSptLoadMod {
    public Helper = new ModHelper();

    public preSptLoad(container: DependencyContainer): void {
        this.Helper.init(container, InitStage.PRE_SPT_LOAD);

        this.Helper.registerStaticRoute(
            "/jehree/idc/console_command",
            "ImmersiveDaylightCycle-ConsoleCommand",
            RegistrationManager.onConsoleCommandReceived,
            RegistrationManager,
            true
        );
        this.Helper.registerStaticRoute(
            "/jehree/idc/host_raid_started",
            "ImmersiveDaylightCycle-HostRaidStarted",
            RegistrationManager.onHostRaidStarted,
            RegistrationManager
        );
        this.Helper.registerStaticRoute(
            "/jehree/idc/client_exited",
            "ImmersiveDaylightCycle-ClientExitedRaid",
            RegistrationManager.onClientRaidExited,
            RegistrationManager
        );
        this.Helper.registerStaticRoute(
            "/jehree/idc/request_time",
            "ImmersiveDaylightCycle-ClientTimeRequested",
            TimeManager.onClientTimeRequest,
            TimeManager,
            true
        );
        this.Helper.registerStaticRoute(
            "/launcher/profile/login",
            "ImmeriveDaylightCycle-CleanupCrashedSession",
            RegistrationManager.onProfileLogin,
            RegistrationManager
        );
    }
}

export const mod = new Mod();
