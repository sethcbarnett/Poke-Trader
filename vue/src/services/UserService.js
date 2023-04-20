import axios from 'axios';

export default {
  getUsersThatContainSearch(search) {
    return axios.get(`users/search/${search}`);
  },

  getTradesInProgress(username) {
    return axios.get(`users/${username}/active_trades`);
  },

  getTrade(usernameFrom, usernameTo) {
    return axios.get(`/trade/${usernameFrom}/${usernameTo}`);
  },

  postTrade(tradeObject) {
    return axios.post('trade', tradeObject);
  },

  acceptTrade(user, otherUser) {
    return axios.put(`trade/${user}/${otherUser}/accepted`);
  },

  rejectTrade(user, otherUser) {
    return axios.put(`trade/${user}/${otherUser}/rejected`);
  }
}