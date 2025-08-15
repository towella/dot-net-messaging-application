import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'
import Login from '../views/Login.vue'

const rootPath = '/dot-net-messaging-application'
const routes = [
{
    path: `${rootPath}/`,
    name: 'Login',
    component: Login
},
{
    path: `${rootPath}/home`,
    name: 'Home',
    component: Home
}]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router