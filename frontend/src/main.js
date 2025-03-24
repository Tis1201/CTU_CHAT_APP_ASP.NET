import { createApp } from 'vue';
import App from './App.vue';
import './assets/style.css';
import './assets/main.css';
// import './assets/login.css';
import 'flowbite';
import router from './router';
// import 'bootstrap/dist/css/bootstrap.min.css';
// import '@fortawesome/fontawesome-free/css/all.min.css';
import '@fortawesome/fontawesome-free/css/all.min.css';

createApp(App).use(router).mount('#app');
