<template>
    <div>
        {{msg}}
        <input type="button" value="点击" @click="onclick" />
    </div>
</template>
<script>
export default{
  name: 'MyFirst',
  data () {
    return {
      msg: 'MyFirst'
    }
  },
  methods: {
    onclick () {
      this.$emit('onclick', this.msg)
    }
  }
}
</script>
