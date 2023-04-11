<template>
  <div> 
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

</style>