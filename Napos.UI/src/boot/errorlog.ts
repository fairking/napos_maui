import { boot } from 'quasar/wrappers';
import { LogService, ClientErrorVm } from 'src/services/domain';

export default boot(async ({ app }) => {
    app.config.errorHandler = function (error, _, info) {
        LogService.logClientError({ body: new ClientErrorVm({ Message: 'Client error: ' + error }) });
    };
});
