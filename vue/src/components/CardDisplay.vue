<template>
  <div id = "display-area">
    <p>Hello</p>
    <div id = "card-display"> 
      <div class ="toptext" >
        <h1>{{card.name}}</h1>
      </div>
      <div class="card-image">
        <img v-bind:src="card.img"/>
      </div>
      <div class ="bottom">
        <h3>{{card.id}}</h3>
      </div>
    </div>
  </div>
</template>

<script>
import CardService from "../services/CardService.js";
export default {
    name: 'card-display',
    data() {
        return {
            card:{ 
                id:"xy10-117",
                name: "",
                img: ""

            }
        }
    },
    created() {
      CardService.getCardById(this.card.id).then((response)=>{
        this.card.name = response.data.name;
        this.card.img = response.data.imgUrl;
        this.card.id = response.data.id;
      });
    }

}
</script>

<style>

#card-display{
  background-color: yellow;
  border: 2px solid black;
  border-radius: 20px;
  padding: 20px;
  display: flex;
  flex-direction: column;
  margin: 20px;
}
#display-area {
  background-color: blue;
  margin-top: 1px;
}
</style>