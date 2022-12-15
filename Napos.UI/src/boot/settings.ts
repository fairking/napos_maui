import { boot } from 'quasar/wrappers';
import { SettingService, SettingsForm } from 'src/services/domain';
import { version } from '/package.json';

declare module '@vue/runtime-core' {
    interface ComponentCustomProperties {
        $settings: SettingsForm;
        $version: string;
    }
}

export default boot(async ({ app }) => {

    app.config.globalProperties.$version = version;

    //try {
    //    await (window as any).Blazor.start();
    //}
    //catch { }

    //try {
        app.config.globalProperties.$settings = await SettingService.get();
    //}
    //catch { }

});
