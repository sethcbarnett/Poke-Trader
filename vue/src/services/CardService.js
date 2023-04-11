import axios from 'axios';
 const apiService = axios.create({
     baseUrl: "https://localhost:44315"
 })
export default {

  getCardById(id) {
    return apiService.get(`card/${id}`);
  }

 
}
