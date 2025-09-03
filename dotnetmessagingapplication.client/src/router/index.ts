import { createRouter, createWebHistory } from 'vue-router'
import Login from '../views/Login.vue'
import NewAccount from '../views/NewAccount.vue'
import Home from '../views/Home.vue'
import Settings from '../views/Settings.vue'
import Account from '../views/Account.vue'
import AudioChat from '@/views/AudioChat.vue'
import VideoChat from '@/views/VideoChat.vue'
import NotFound from '../views/NotFound.vue'

const rootPath = '/dot-net-messaging-application'
const routes = [
{
    path: `${rootPath}/`,
    name: 'Login',
    component: Login
},
{
    path: `${rootPath}/new-account`,
    name: 'NewAccount',
    component: NewAccount
},
{
    path: `${rootPath}/:username/home`,
    name: 'Home',
    component: Home,
    props: true,
},
{
    path: `${rootPath}/:username/settings`,
    name: 'Settings',
    component: Settings
},
{
    path: `${rootPath}/:username/account`,
    name: 'Account',
    component: Account
},
{
    path: `${rootPath}/:username/audio-chat`,
    name: 'AudioChat',
    component: AudioChat
},
{
    path: `${rootPath}/:username/video-chat`,
    name: 'VideoChat',
    component: VideoChat
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