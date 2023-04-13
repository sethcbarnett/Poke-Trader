<template>
  <div id="register" class="text-center">
    <form>
      <h1 class="headline">Upgrade to Premium</h1>
        <div role="alert" v-if="PremRegErrors">
        {{ PremRegErrorMsg }}
      </div>
      <div class="form-input-group">
        <label for="cc-number">Credit Card Number:</label>
        <input type="text" id="cc-number"  required maxlength="16" size="16"/>
      </div>
      <div class="form-input-group">
        <label for="csc">CSC:</label>
        <input type="text" id="csc" required maxlength="3" size="3"/>
      </div>
      <label for="address-checkbox"
        ><h3 id="checkbox-text">Billing Address Different From Shipping Address</h3></label
      >
      <input
        v-on:click="
          isChecked === false ? (isChecked = true) : (isChecked = false)
        "
        id="address-checkbox"
        type="checkbox"
      />
      <div v-show="isChecked" id="address">
        <div class="form-input-group">
          <label for="streetAddress">Street Address:</label>
          <input
            type="text"
            id="streetAddress"
           
            required
          />
        </div>
        <div class="form-input-group">
          <label for="city">City:</label>
          <input type="text" id="city"  required />
        </div>
        <div class="form-input-group">
          <label for="stateAbbreviation">State:</label>
          <input
            type="text"
            id="stateAbbreviation"
            
            required
          />
        </div>
        <div class="form-input-group">
          <label for="zipCode">Zip Code:</label>
          <input type="number" id="zipCode"  required />
        </div>
      </div>
      <button v-on:click.prevent="redirectToCollection">CATCH 'EM ALL</button>
    </form>
  </div>
</template>

<script>
import authService from "../services/AuthService";
//import PremiumService from "../services/PremiumService.js"
export default {
  name: "premium",
  data() {
    return {
      isChecked: false,

      user: {
        username: "",
        
        isPremium: false
      },
      PremRegErrors: false,
      PremRegErrorMsg: "There were problems upgrading to Premium.",
    };
  },
  methods: {
    redirectToCollection(){
      authService.changeUserToPremium(this.$store.state.user.username).then((response) =>{
            console.log(response);
            if (response.status == 200) {
              this.$store.commit('GO_PREMIUM');
              this.$router.push({ name: 'collection', params: {username: `${this.$store.state.user.username}`}});
            }
            else{ this.PremRegErrors = true;
            }
        })
      
    }
  },
};
</script>

<style scoped>
.text-center {
  background-image: none;
  width: 100vw;
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  background: url('../assets/pokeball wallpaper.jpg');
}
form {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  border-radius: 100px;
  width: 500px;
  height: 500px;
  color: #3466af;
  background-color: #ffcb05;
  border-width: 20px;
  border-style: solid;
  border-color: #3466af;
  font-size: 1.25em;
  padding-bottom: 10px;
  line-height: 0.55em;
  text-align: top;
}
h1 {
  font-size: 2.5em;
  padding-bottom: 20px;
}
.form-input-group {
  margin-bottom: 1rem;
}
label {
  margin-right: 0.5rem;
}
.login-link {
  color: #3466af;
}
button {
  background-color: #3466af;
  color: white;
  border: none;
  border-radius: 5px;
  font-family: "Pokemon Solid", sans-serif;
  text-align: center;
  text-justify: auto;
  letter-spacing: 1px;
  padding-bottom: 5px;
  cursor: pointer;
}
.headline{
    font-size: 35px;
}
#checkbox-text{
    font-size: 12px;
}
/* #csc{
  size: 3;
} */


</style>
