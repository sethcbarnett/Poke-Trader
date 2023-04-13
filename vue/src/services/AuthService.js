import axios from 'axios';

export default {

  login(user) {
    return axios.post('/login', user)
  },

  register(user) {
    return axios.post('/register', user)
  },
  
  changeUserToPremium(userName) {
    return axios.put(`/users/${userName}`);
  },
  toggleVisibility(userName) {
    return axios.put(`/users/toggle/${userName}`);
  }
}
