import { AxiosRequestConfig } from 'axios';

export default class Backend {
    request(config: AxiosRequestConfig): Promise<any> {
        return ((window as any).DotNet.invokeMethodAsync('Napos', 'CallMeFromJs', config) as Promise<any>)
            .then((data) => { return data; });
    }
}
