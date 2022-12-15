import { boot } from 'quasar/wrappers';
import { serviceOptions as domain_services } from 'src/services/domain';

export default boot(({ app }) => {
  domain_services.axios = app.config.globalProperties.$api;
});
