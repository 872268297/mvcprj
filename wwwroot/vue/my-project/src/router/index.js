import Vue from 'vue'
import Router from 'vue-router'
import HelloWorld from '@/components/HelloWorld'
import MyFirst from '@/components/MyFirst'
import Home from '@/components/Home'
import Member from '@/components/Member'
import Cart from '@/components/Cart'
import Search from '@/components/Search'
Vue.use(Router)

export default new Router({
    routes: [{
            path: '/',
            name: 'Home',
            component: Home
        },
        {
            path: '/MyFirst',
            name: 'MyFirst',
            component: MyFirst
        },
        {
            path: '/Helloword',
            name: 'HelloWord',
            component: HelloWorld
        },
        {
            path: '/Member',
            name: 'Member',
            component: Member
        },
        {
            path: '/Cart',
            name: 'Cart',
            component: Cart
        },
        {
            path: '/Search',
            name: 'Search',
            component: Search
        }
    ]
})