<template>
  <div class="hello">
    <ul>
      <li v-for="(item,index) in list" :key="item.id">
        {{++index}}-----{{item.id}}-----{{item.name}}
      </li>
    </ul>
    <input type="button" value="点击" @click="btnclick"/>
    <br/>
    <h1>{{msg}}</h1>
    <hr>
    <my-fist v-on:onclick="childClick"></my-fist>
    <hr>
    <a href="#/MyFirst">点击</a>
    <hr>
    <router-link to="/MyFirst" tag="span">点击</router-link>
  </div>
</template>
<script>
import myFist from '@/components/MyFirst'
export default {
  name: 'HelloWorld',
  data () {
    return {
      msg: 0,
      list: []
    }
  },
  components: {
    myFist: myFist
  },
  methods: {
    loadData () {
      this.$http
        .get('https://localhost:5001/api/First/GetData')
        .then(result => {
          this.list = result.body
          this.list.push({ id: 6, name: '123' })
        })
        .catch(err => {
          console.log(err)
        })
    },
    btnclick: function () {
      this.msg++
      this.$emit('onclick')
    },
    childClick: function (data) {
      this.msg = data
    }
  },
  created () {
    console.log(123)
    this.loadData()
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h1,
h2 {
  font-weight: normal;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: block;
  margin: 0 10px;
}

a {
  color: #42b983;
}
</style>
