<template>
  <div>
    {{msg}}
    <mt-swipe :auto="8000" :speed="10">
      <mt-swipe-item v-for="i in 5" :key="i">{{i}}{{i}}{{i}}{{i}}{{i}}</mt-swipe-item>
    </mt-swipe>
  </div>
</template>
<script>
// import {Toast} from 'mint-ui'
export default {
  name: 'Home',
  data () {
    return {
      msg: 'Home'
    }
  },
  methods: {
    pro () {
      return new Promise(function (resolve, reject) {
        console.log('promise')
        let a = 1
        if (a === 1) {
          resolve('123')
        } else {
          return reject(new Error('error'))
        }
      })
    }
  },
  created: function () {
    this.pro().then(
      function (data) {
        console.log(data)
      },
      function (err) {
        console.log(err)
      }
    )
  }
}
</script>

<style scoped>
.mint-swipe {
  height: 200px;
}
.mint-swipe-item:nth-child(1) {
  background-color: red;
}
.mint-swipe-item:nth-child(2) {
  background-color: moccasin;
}
.mint-swipe-item:nth-child(3) {
  background-color: olivedrab;
}
</style>
