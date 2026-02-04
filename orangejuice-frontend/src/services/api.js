import axios from 'axios';

const api = axios.create({
    baseURL: "http://localhost:5198/"
});

export default api;