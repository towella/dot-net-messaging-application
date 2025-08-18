import { createRouter, createWebHistory } from 'vue-router'
import Login from '../views/Login.vue'
import Home from '../views/Home.vue'
import Settings from '../views/Settings.vue'
import Account from '../views/Account.vue'
import NotFound from '../views/NotFound.vue'

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
},
{
    path: `${rootPath}/settings`,
    name: 'Settings',
    component: Settings
},
{
    path: `${rootPath}/account`,
    name: 'Account',
    component: Account
},
{ 
    path: '/:pathMatch(.*)*',
    name: 'NotFound',
    component: NotFound 

}]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router