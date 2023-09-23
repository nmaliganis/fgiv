import axios from 'axios'

const buildRequest = (url, method, data, timeout) => {

    const options = {
        baseURL: 'http://localhost:5000',
        method,
        url,
        data,
        timeout,
    };
    return axios.request(options)
};

export default {
    getSuspects:
            buildRequest(`/api/v1/suspects`, 'get'),
    getSuspectById: ({ id }) => buildRequest(`/api/v1/suspects/${id}`, 'get'),
    updateSuspect: ({ id, ...data }) =>
        buildRequest(`/api/v1/suspects/${id}`, 'put', data),
    createSuspect: ({ id, ...data }) =>
        buildRequest(`/api/v1/suspects/${id}`, 'post', data)
}
