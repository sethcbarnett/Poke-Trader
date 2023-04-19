<template>
  <div id="user-window">
      <h3>{{ this.username }} will give:</h3>
      <h4>Est. Value: </h4>
      <div id="cards-for-trade">
          <h5>Charizard</h5>
      </div>
      
      <div id="collection">
          <simplified-card-display 
          v-bind:availableCard="availableCard" 
          v-bind:username="username" 
          v-for="availableCard in CheckUsername"
          v-bind:key="availableCard.card.id" />
      </div>
  </div>
</template>

<script>
import SimplifiedCardDisplay from './SimplifiedCardDisplay.vue';

export default {
    name: 'user-trade-window',
    props: ['username'],
    components: { SimplifiedCardDisplay },
    data() {
        return {
            availableCard: {},
            newUsername: ''
        }
    },
    methods: {
        GetAvailableCards() {
            this.$store.commit('SET_USER_INFO', this.$store.state.user.username);
        },
        SetNewUsername() {
            this.newUsername = this.username;
        }
    },
    created() {
        this.GetAvailableCards();
        this.SetNewUsername();
    },
    computed: {
         CheckUsername() {
            let cards = []; 
            if (this.username == this.$store.state.user.username) {
                cards = this.$store.state.loginUserAvailableCards;
            }
            else {
                cards = this.$store.state.otherUserAvailableCards;
            }
            return cards;
        },
    }
}
</script>

<style scoped>
#user-window {
    border-style: solid;
    margin: 5px;
    flex-basis: 50%;
    height: 95%;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: space-around;
}
#cards-for-trade {
    flex-basis: 50%;
    border-style: solid;
    width: 90%;
}
#collection {
    flex-basis: 30%;
    border-style: solid;
    width: 90%;
}
h4 {
    margin: 0;
    align-self: flex-start;
    margin-left: 5px;
    flex-basis: 2%;
}
h3 {
    margin: 0;
    flex-basis: 5%;
}
#collection {
  display: flex;
  flex-direction: row;
  justify-content: center;
}
</style>