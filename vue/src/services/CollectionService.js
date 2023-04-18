import axios from 'axios';

export default {
  getCollectionByUser(username) {
    return axios.get(`/collection/${username}`);
  },

  getPublicCollectionUsers() {
    return axios.get('/users/public');
  },

  addCardToCollection(username, collectionItem) {
    return axios.post(`/collection/${username}`, collectionItem);
  },
  
  updateCard(collectionItem, username) {
    console.log(collectionItem.grade);
    return axios.post(`/collection/${collectionItem.card.id}/${username}`, collectionItem)
  }
}
