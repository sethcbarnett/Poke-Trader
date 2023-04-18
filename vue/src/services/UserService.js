import axios from 'axios';

export default {
  getUsersThatContainSearch(search) {
    return axios.get(`users/${search}`);
  }
}