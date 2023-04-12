import axios from 'axios';

export default {
  changeUserToPremium(user) {
    return axios.put('/users', user);
  }
}