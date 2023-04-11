import axios from 'axios';

export default {
  getCollectionByUser(username) {
    console.log(username);
    return axios.get(`/collection/${username}`);
  }
}
