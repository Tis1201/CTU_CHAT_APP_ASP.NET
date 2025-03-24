<script setup>
import coffeeService from '../service/coffe.service.js';
import { ref, computed, watch } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import { defineAsyncComponent, onMounted } from 'vue';
// Lazy load components
const ListDrinkCard = defineAsyncComponent(() => import('../components/ListDrinkCard.vue'));
const MainPagination = defineAsyncComponent(() => import('../components/MainPagination.vue'));
const DrinkSeason = defineAsyncComponent(() => import('../components/DrinkSeason.vue'));
import AppHeader from '../components/AppHeader.vue';
import 'flowbite';
const router = useRouter();
const route = useRoute();
const totalPages = ref(1);
const drinks = ref([]);
const selectedIndex = ref(-1);

const currentPage = computed(() => {
  const page = Number(route.query?.page);
  if (Number.isNaN(page) || page < 1) return 1;
  return page;
});

async function fetchData(page) {
  try {
    const data = await coffeeService.getCoffees(page); // ← bạn đã return đúng structure
    drinks.value = data?.items ?? [];
    totalPages.value = data?.metadata?.lastPage ?? 1;

    selectedIndex.value = -1;
  } catch (error) {
    console.error('Error fetching data:', error);
  }
}

function changeCurrentPage(page) {
  router.push({ name: 'home', query: { page } });
}

watch(currentPage, () => fetchData(currentPage.value), { immediate: true });

onMounted(() => {
  console.log('Home mounted');
});
</script>

<template>
  <AppHeader />
  <div class="bg-coffee2 justify-center items-center">
    <img
      src="/img/Coffee lovers!.gif"
      alt="gif"
      class="w-full h-[450px]"
    />
    <div class="flex flex-col items-center justify-center mt-20 bg-coffee2">
      <h1
        class="mb-4 text-4xl font-extrabold leading-none tracking-tight text-white md:text-5xl lg:text-6xl dark:text-white"
      >
        We invest in the world’s potential
      </h1>
      <p
        class="mb-6 text-lg font-normal text-gray-500 lg:text-xl sm:px-16 xl:px-48 dark:text-gray-400"
      >
      Here at HTV we focus on markets where technology, innovation, and capital can unlock
      long-term value and drive economic growth.
      </p>
      <a
        href="#"
        class="inline-flex items-center justify-center px-5 py-3 text-base font-medium text-center text-white bg-blue-700 rounded-lg hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 dark:focus:ring-blue-900"
      >
        Learn more
        <svg
          class="w-3.5 h-3.5 ms-2 rtl:rotate-180"
          aria-hidden="true"
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 14 10"
        >
          <path
            stroke="currentColor"
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M1 5h12m0 0L9 1m4 4L9 9"
          />
        </svg>
      </a>
    </div>
  </div>
  <div class="w-full flex flex-col justify-center items-center">
    <div class="w-full flex flex-col items-center">
      <ListDrinkCard
        :drinks="drinks"
        :selectedIndex="selectedIndex"
      />
    </div>
    <div class="mt-3 mb-6 w-full flex flex-col items-center">
      <MainPagination
        :total-pages="totalPages"
        :current-page="currentPage"
        @update:current-page="changeCurrentPage"
      />
      <div class="w-[100px]"></div>
    </div>
    <div class="w-full bg-coffee2">
      <DrinkSeason />
    </div>
  </div>
</template>
