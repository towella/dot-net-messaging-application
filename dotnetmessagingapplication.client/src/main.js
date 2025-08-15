import './assets/main.css'

import { createApp } from 'vue'
import router from "./router/index"
import App from './App.vue'

const app = createApp(App)
//app.component('widget', Widget)
app.use(router).mount('#app')
