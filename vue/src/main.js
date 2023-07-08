import { createApp } from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify'
import router from "./router";
import axios from "axios";

import { loadFonts } from './plugins/webfontloader'

loadFonts()

createApp(App)
    .use(vuetify)
    .use(router)
    .provide('axios', axios)
    .mount('#app')
