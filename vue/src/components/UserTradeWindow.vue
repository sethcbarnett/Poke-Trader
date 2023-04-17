<template>
  <div id="user-window">
      <h3>{{ this.username }} will give:</h3>
      <h4>Est. Value: </h4>
      <div id="cards-for-trade">
          <h5>Charizard</h5>
      </div>
      
      <div id="collection">
          <h5>Alakazam</h5>
      </div>
  </div>
</template>

<script>
export default {
    name: 'user-trade-window',
    props: ['username']
}
</script>

<style>
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
</style>