const { codegen } = require('swagger-axios-codegen');

// See docs: https://www.npmjs.com/package/swagger-axios-codegen
codegen({
  serviceNameSuffix: '',
  enumNamePrefix: '',
  methodNameMode: 'operationId', // "[operationId|path]"
  source: require('../Napos.Domain/schema.json'),
  //remoteUrl: "http://localhost:8050/api/v1/swagger.json",
  outputDir: './src/services',
  fileName: 'domain.ts',
  strictNullChecks: false,
  useStaticMethod: true,
  useCustomerRequestInstance: false,
  /* List of api services which need to be generated in TS. */
  //include: [
  //  "StoreService",
  //],
  /* Additional types/enums not included in the api methods. Those types must have attribute [ApiModel] */
  includeTypes: [
    'IdForm',
  ],
  modelMode: 'class',
  extendGenericType: [],
  generateValidationModel: true,
});
