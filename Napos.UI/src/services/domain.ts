/** Generate by swagger-axios-codegen */
// @ts-nocheck
/* eslint-disable */

/** Generate by swagger-axios-codegen */
/* eslint-disable */
// @ts-nocheck
import axiosStatic, { AxiosInstance, AxiosRequestConfig } from 'axios';

export interface IRequestOptions extends AxiosRequestConfig {}

export interface IRequestConfig {
  method?: any;
  headers?: any;
  url?: any;
  data?: any;
  params?: any;
}

// Add options interface
export interface ServiceOptions {
  axios?: AxiosInstance;
}

// Add default options
export const serviceOptions: ServiceOptions = {};

// Instance selector
export function axios(configs: IRequestConfig, resolve: (p: any) => void, reject: (p: any) => void): Promise<any> {
  if (serviceOptions.axios) {
    return serviceOptions.axios
      .request(configs)
      .then(res => {
        resolve(res.data);
      })
      .catch(err => {
        reject(err);
      });
  } else {
    throw new Error('please inject yourself instance like axios  ');
  }
}

export function getConfigs(method: string, contentType: string, url: string, options: any): IRequestConfig {
  const configs: IRequestConfig = { ...options, method, url };
  configs.headers = {
    ...options.headers,
    'Content-Type': contentType
  };
  return configs;
}

export const basePath = '';

export interface IList<T> extends Array<T> {}
export interface List<T> extends Array<T> {}
export interface IDictionary<TValue> {
  [key: string]: TValue;
}
export interface Dictionary<TValue> extends IDictionary<TValue> {}

export interface IListResult<T> {
  items?: T[];
}

export class ListResultDto<T> implements IListResult<T> {
  items?: T[];
}

export interface IPagedResult<T> extends IListResult<T> {
  totalCount?: number;
  items?: T[];
}

export class PagedResultDto<T = any> implements IPagedResult<T> {
  totalCount?: number;
  items?: T[];
}

// customer definition
// empty

export class ContactService {
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: CreateContactForm;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<string> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/Contact/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;

      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: ContactForm;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/Contact/Update';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;

      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /** requestBody */
      body?: IdForm;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<ContactForm> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/Contact/Get';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;

      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getList(options: IRequestOptions = {}): Promise<ContactItem[]> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/Contact/GetList';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = null;

      configs.data = data;

      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /** requestBody */
      body?: IdForm;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/Contact/Delete';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;

      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static prepareSignature(
    params: {
      /** requestBody */
      body?: IdForm;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<ContactSignatureForm> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/Contact/PrepareSignature';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;

      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static applySignature(
    params: {
      /** requestBody */
      body?: ContactSignatureForm;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/Contact/ApplySignature';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;

      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static validateSignature(
    params: {
      /** requestBody */
      body?: ContactSignatureForm;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<boolean> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/Contact/ValidateSignature';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;

      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static responseSignature(
    params: {
      /** requestBody */
      body?: ContactSignatureForm;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<ContactSignatureForm> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/Contact/ResponseSignature';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;

      axios(configs, resolve, reject);
    });
  }
}

export class SettingService {
  /**
   *
   */
  static get(options: IRequestOptions = {}): Promise<SettingsForm> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/Setting/Get';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = null;

      configs.data = data;

      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static save(
    params: {
      /** requestBody */
      body?: SettingsForm;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/Setting/Save';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;

      axios(configs, resolve, reject);
    });
  }
}

export class StoreService {
  /**
   *
   */
  static create(
    params: {
      /** requestBody */
      body?: CreateStoreForm;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<string> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/Store/Create';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;

      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /** requestBody */
      body?: StoreForm;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/Store/Update';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;

      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static get(
    params: {
      /** requestBody */
      body?: IdForm;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<StoreForm> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/Store/Get';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;

      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getList(options: IRequestOptions = {}): Promise<StoreItem[]> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/Store/GetList';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = null;

      configs.data = data;

      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /** requestBody */
      body?: IdForm;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/Store/Delete';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;

      axios(configs, resolve, reject);
    });
  }
}

export class LogService {
  /**
   *
   */
  static logClientError(
    params: {
      /** requestBody */
      body?: ClientErrorVm;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/Log/LogClientError';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params.body;

      configs.data = data;

      axios(configs, resolve, reject);
    });
  }
}

export class CreateContactForm {
  /**  */
  'name'?: string;

  constructor(data: undefined | any = {}) {
    this['name'] = data['name'];
  }

  public static validationModel = {
    name: { required: true, maxLength: 50 }
  };
}

export class ContactForm {
  /**  */
  'id'?: string;

  /**  */
  'name'?: string;

  constructor(data: undefined | any = {}) {
    this['id'] = data['id'];
    this['name'] = data['name'];
  }

  public static validationModel = {
    name: { required: true, maxLength: 50 }
  };
}

export class IdForm {
  /**  */
  'id'?: string;

  constructor(data: undefined | any = {}) {
    this['id'] = data['id'];
  }

  public static validationModel = {};
}

export class ContactItem {
  /**  */
  'id'?: string;

  /**  */
  'created'?: Date;

  /**  */
  'updated'?: Date;

  /**  */
  'name'?: string;

  constructor(data: undefined | any = {}) {
    this['id'] = data['id'];
    this['created'] = data['created'];
    this['updated'] = data['updated'];
    this['name'] = data['name'];
  }

  public static validationModel = {};
}

export class ContactSignatureForm {
  /**  */
  'id'?: string;

  /**  */
  'request'?: string;

  /**  */
  'response'?: string;

  constructor(data: undefined | any = {}) {
    this['id'] = data['id'];
    this['request'] = data['request'];
    this['response'] = data['response'];
  }

  public static validationModel = {
    request: { required: true }
  };
}

export class SettingsForm {
  /**  */
  'theme'?: boolean;

  constructor(data: undefined | any = {}) {
    this['theme'] = data['theme'];
  }

  public static validationModel = {};
}

export class CreateStoreForm {
  /**  */
  'name'?: string;

  /**  */
  'description'?: string;

  constructor(data: undefined | any = {}) {
    this['name'] = data['name'];
    this['description'] = data['description'];
  }

  public static validationModel = {
    name: { required: true, maxLength: 25 },
    description: { maxLength: 150 }
  };
}

export class StoreForm {
  /**  */
  'id'?: string;

  /**  */
  'name'?: string;

  /**  */
  'description'?: string;

  constructor(data: undefined | any = {}) {
    this['id'] = data['id'];
    this['name'] = data['name'];
    this['description'] = data['description'];
  }

  public static validationModel = {
    name: { required: true, maxLength: 25 },
    description: { maxLength: 150 }
  };
}

export class StoreItem {
  /**  */
  'id'?: string;

  /**  */
  'created'?: Date;

  /**  */
  'updated'?: Date;

  /**  */
  'name'?: string;

  /**  */
  'description'?: string;

  constructor(data: undefined | any = {}) {
    this['id'] = data['id'];
    this['created'] = data['created'];
    this['updated'] = data['updated'];
    this['name'] = data['name'];
    this['description'] = data['description'];
  }

  public static validationModel = {};
}

export class ClientErrorVm {
  /**  */
  'message'?: string;

  constructor(data: undefined | any = {}) {
    this['message'] = data['message'];
  }

  public static validationModel = {};
}
