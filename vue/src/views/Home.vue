<template>
  <div class="home">
    <div id="top">
      <h1>Home</h1>
    </div>
    <div id="middle">
      <h2>Middle</h2>
    </div>
    <div id="bottom">
      <h3>Bottom</h3>
    </div>
  </div>
</template>

<script>
export default {
  name: "home"
};
</script>

<style scoped>
.home{
  display: flex;
  flex-direction: column;
}
</style>