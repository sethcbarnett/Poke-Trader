

import axios from 'axios';

export default {

  getCollectionByUser(username) {
    return axios.get(`${username}/collection`);
  }

 
}
