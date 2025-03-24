<template>
  <div>
    <AppHeader />
    <Order
      :order="order"
      :selectedIndex="selectedIndex"
      @paymentSuccess="showToast('success')"
      @deleteSuccess="showToast('danger')"
    />
    <div
      v-if="toastType === 'success'"
      class="fixed top-4 right-4 z-50 flex items-center w-full max-w-xs p-4 mb-4 text-gray-500 bg-white rounded-lg shadow dark:text-gray-400 dark:bg-gray-800"
      role="alert"
    >
      <div
        class="inline-flex items-center justify-center flex-shrink-0 w-8 h-8 text-green-500 bg-green-100 rounded-lg dark:bg-green-800 dark:text-green-200"
      >
        <svg
          class="w-5 h-5"
          xmlns="http://www.w3.org/2000/svg"
          fill="currentColor"
          viewBox="0 0 20 20"
        >
          <path
            d="M10 .5a9.5 9.5 0 1 0 9.5 9.5A9.51 9.51 0 0 0 10 .5Zm3.707 8.207-4 4a1 1 0 0 1-1.414 0l-2-2a1 1 0 0 1 1.414-1.414L9 10.586l3.293-3.293a1 1 0 0 1 1.414 1.414Z"
          />
        </svg>
      </div>
      <div class="ms-3 text-sm font-normal">Payment successful!</div>
    </div>

    <div
      v-if="toastType === 'danger'"
      class="fixed top-4 right-4 z-50 flex items-center w-full max-w-xs p-4 mb-4 text-gray-500 bg-white rounded-lg shadow dark:text-gray-400 dark:bg-gray-800"
      role="alert"
    >
      <div
        class="inline-flex items-center justify-center flex-shrink-0 w-8 h-8 text-red-500 bg-red-100 rounded-lg dark:bg-red-800 dark:text-red-200"
      >
        <svg
          class="w-5 h-5"
          xmlns="http://www.w3.org/2000/svg"
          fill="currentColor"
          viewBox="0 0 20 20"
        >
          <path
            d="M10 .5a9.5 9.5 0 1 0 9.5 9.5A9.51 9.51 0 0 0 10 .5Zm3.707 11.793a1 1 0 1 1-1.414 1.414L10 11.414l-2.293 2.293a1 1 0 0 1-1.414-1.414L8.586 10 6.293 7.707a1 1 0 0 1 1.414-1.414L10 8.586l2.293-2.293a1 1 0 0 1 1.414 1.414L11.414 10l2.293 2.293Z"
          />
        </svg>
      </div>
      <div class="ms-3 text-sm font-normal">Item deleted successfully!</div>
    </div>
  </div>
</template>

<script setup>
import AppHeader from '../components/AppHeader.vue';
import Order from '../components/Order.vue';
import orderService from '../service/order.service';
import { ref, computed, watch } from 'vue';
import { useRouter, useRoute } from 'vue-router';

const router = useRouter();
const route = useRoute();

const order = ref([]);
const totalPages = ref(1);
const selectedIndex = ref(-1);
const toastType = ref(null); // Initialize toastType

async function fetchData(page) {
  try {
    const data = await orderService.getOrders(page);
    console.log('API Response:', data);

    if (data && data.items) {
      order.value = data.items;
      totalPages.value = data.metadata?.lastPage ?? 1;
    } else {
      console.error('Unexpected data structure:', data);
      order.value = [];
      totalPages.value = 1;
    }

    selectedIndex.value = -1;
  } catch (error) {
    console.error('Error fetching data:', error);
    order.value = [];
    totalPages.value = 1;
  }
}

const showToast = (type) => {
  toastType.value = type;
  setTimeout(() => {
    toastType.value = null;
  }, 3000);
};

const currentPage = computed(() => {
  const page = Number(route.query?.page);
  return Number.isNaN(page) || page < 1 ? 1 : page;
});

watch(currentPage, () => fetchData(currentPage.value), { immediate: true });
</script>
