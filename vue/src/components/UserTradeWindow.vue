<template>
  <div id="user-window">
      <h3>{{ this.username }}'s Cards here</h3>
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
    height: 70vh;
}
h3 {
    text-align: center;
}
</style>