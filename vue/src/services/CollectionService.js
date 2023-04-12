import axios from 'axios';

export default {
  getCollectionByUser(username) {
    console.log(username);
    return axios.get(`/collection/${username}`);
  },

  getPublicCollectionUsers() {
    return axios.get('/users/public');
  }
}
