import axios from 'axios';

export default {
  getCollectionByUser(username) {
    return axios.get(`/collection/${username}`);
  },

  getPublicCollectionUsers() {
    return axios.get('/users/public');
  }
}
