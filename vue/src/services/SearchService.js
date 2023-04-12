import axios from 'axios';

export default {
  getCardsBySearch(parameters) {
    return axios.get(`search/params/${parameters}`);
  }
}