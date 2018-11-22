<template>
  <div id="app">
    <div class="app-content">
    <mt-header fixed :title="selected"></mt-header>
    <router-view/>

    <mt-tabbar fixed v-model="selected">
      <mt-tab-item id="首页">
        <router-link tag="span" to="/">首页</router-link>
      </mt-tab-item>
      <mt-tab-item id="会员">
        <router-link tag="span" to="/Member">会员</router-link>
      </mt-tab-item>
      <mt-tab-item id="购物车">
        <router-link tag="span" to="/Cart">购物车</router-link>
      </mt-tab-item>
      <mt-tab-item id="搜索">
        <router-link tag="span" to="/Helloword">搜索</router-link>
      </mt-tab-item>
    </mt-tabbar>
    </div>
  </div>
</template>

<script>
export default {
  name: "App",
  data: function() {
    return {
      selected: "首页"
    };
  }
};
</script>

<style scoped>
#app {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}
.app-content {
  margin-top: 40px;
  overflow-x: hidden;
}
span {
  display: inline-block;
  height: 40px;
  line-height: 40px;
  width: 100%;
}
.mint-tab-item {
  padding: 0;
}

.v-enter {
  opacity: 0;
  transform: translateX(100%);
}
.v-leave-to {
  opacity: 0;
  transform: translateX(-100%);
  position: absolute;
}
.v-enter-active,
.v-leave-active {
  transition: all 1s ease;
}
</style>
