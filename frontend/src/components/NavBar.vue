<template>
  <div class="bg-coffee2">
   
    <AppHeader v-if="isHome" />

  </div>
</template>

<script>
import AppHeader from './AppHeader.vue';
import { onMounted } from 'vue';
export default {
  name: 'NavBar',
  components: {
    AppHeader
  },
  computed: {
    isHome() {
      return this.$route.path === '/';
    }
  },
  setup() {
    onMounted(() => {
      console.log('NavBar mounted');
    });
  }
};
</script>

<style>
/* Bạn có thể thêm các kiểu tùy chỉnh ở đây nếu cần */
.router-link-active {
  color: #1d4ed8; /* Màu active */
}

.active {
  color: #1d4ed8; /* Màu active */
}
</style>
