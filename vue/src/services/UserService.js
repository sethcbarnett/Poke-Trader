import axios from 'axios';

export default {
  getUsersThatContainSearch(search) {
    return axios.get(`users/search/${search}`);
  },

  getTradesInProgress(username) {
    return axios.get(`users/${username}/active_trades`);
  }
}