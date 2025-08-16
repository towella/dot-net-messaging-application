import { createRouter, createWebHistory } from 'vue-router'
import Login from '../views/Login.vue'
import Home from '../views/Home.vue'
import Settings from '../views/Settings.vue'
import Account from '../views/Account.vue'

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
    component: Home,
    children: [{
        path: 'settings',
        name: 'Settings',
        component: Settings
    },
    {
        path: 'account',
        name: 'Account',
        component: Account
    }]
}]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router