import { createWebHistory, createRouter } from 'vue-router';
import Home from '../view/Home.vue';
import ProductView from '../view/ProductView.vue';
import OrderView from '../view/OrderView.vue';
import LoginView from '../components/Login.vue';
import Admin from '../view/Admin.vue';
const routes = [
  {
    path: '/Product',
    name: 'productview',
    component: ProductView
  },
  {
    path: '/',
    name: 'home',
    component: Home
  },
  {
    path: '/Order',
    name: 'orderview',
    component: OrderView
  },
  {
    path: '/login',
    name: 'login',
    component: LoginView
  },
  {
    path: '/admin',
    name: 'admin',
    component: Admin
  }
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
  linkActiveClass: 'router-link-active'
});
export default router;
