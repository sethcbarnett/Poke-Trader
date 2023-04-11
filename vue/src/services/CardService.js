import axios from 'axios';

export default {

  getCardById(id) {
    return axios.get(`card/${id}`);
  }

 
}
