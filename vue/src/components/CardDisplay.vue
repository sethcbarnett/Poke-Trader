<template>
    <div id = "card-display"> 
      <div id ="top-text" >
        <h4>{{card.name}}</h4>
      </div>
      <div id="card-image">
        <img v-bind:src="card.imgUrl"/>
      </div>
      <div id ="bottom-text">
        <h4>{{card.id}}</h4>
      </div>
    </div>
</template>

<script>
//import CardService from "../services/CardService.js";
export default {
    name: 'card-display',
    props: {
      card: {
        id: "",
        name: "",
        imgUrl: ""
      }
    },
    created() {
      // CardService.getCardById(this.card.id).then((response)=>{
      //   this.card.name = response.data.name;
      //   this.card.img = response.data.imgUrl;
      //   this.card.id = response.data.id;
      // });
    }

}
</script>

<style>

#card-display{
  background-color: rgb(207, 200, 177);
  border: 2px solid black;
  border-radius: 20px;
  padding: 20px;
  display: flex;
  flex-direction: column;
  margin: 20px;
  padding-bottom: 5px;
  padding-top: 5px;
}
#display-area {
  background-color: blue;
  margin-top: 1px;
}
#top-text h4{
  margin: 5px;
  text-align: center;
}
#card-image {
  display: block;
}
#bottom-text h4 {
  margin: 5px;
  text-align: center;
}
</style>