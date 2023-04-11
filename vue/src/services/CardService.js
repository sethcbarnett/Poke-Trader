import axios from 'axios';
 const apiService = axios.create({
     baseUrl: "http://localhost:44315"
 })
export default {

  getCardById(id) {
    return apiService.get("/card/:id", id);
  }

 
}
