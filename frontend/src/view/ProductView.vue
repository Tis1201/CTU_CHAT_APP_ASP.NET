<template>
  <div class="bg-coffee2">
    <AppHeader />
    <div class="mb-20">
      <img
        src="https://cdn.discordapp.com/attachments/1162425838206189662/1353671776692998224/image.png?ex=67e28095&is=67e12f15&hm=9776892e4f894b7cf7b839a9fae5dcf73082a78ee80767f466885e91276a1b5b&"
        alt="Order"
        class="w-full h-[300px]"
      />
    </div>
    <div class="flex flex-col items-center justify-center mb-10">
      <h1
        class="mb-4 text-4xl font-extrabold leading-none tracking-tight text-white md:text-5xl lg:text-6xl dark:text-white"
      >
      We serve
        <span
          class="underline underline-offset-3 decoration-8 decoration-amber-600 dark:decoration-amber-800"
          >the best coffee</span
        >
      </h1>
      <p class="text-lg font-normal text-gray-500 lg:text-xl dark:text-gray-400">
        Here at HTV we focus on markets where technology, innovation, and capital can unlock
        long-term value and drive economic growth.
      </p>
    </div>
    <div class="mb-10">
      <form class="max-w-lg mx-auto">
        <div class="flex">
          <label
            for="search-dropdown"
            class="mb-2 text-sm font-medium text-gray-900 sr-only dark:text-white"
            >Your Email</label
          >
          <button
            id="dropdown-button"
            @click="toggleDropdown"
            data-dropdown-toggle="dropdown"
            class="flex-shrink-0 z-10 inline-flex items-center py-2.5 px-4 text-sm font-medium text-center text-gray-900 bg-gray-100 border border-gray-300 rounded-s-lg hover:bg-gray-200 focus:ring-4 focus:outline-none focus:ring-gray-100 dark:bg-gray-700 dark:hover:bg-gray-600 dark:focus:ring-gray-700 dark:text-white dark:border-gray-600"
            type="button"
          >
            {{ selectedCategory }}
            <svg
              class="w-2.5 h-2.5 ms-2.5"
              aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 10 6"
            >
              <path
                stroke="currentColor"
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="m1 1 4 4 4-4"
              />
            </svg>
          </button>
          <div
            id="dropdown"
            v-show="isDropdownOpen"
            class="z-10 hidden bg-white divide-y divide-gray-100 rounded-lg shadow w-44 dark:bg-gray-700"
          >
            <ul
              class="py-2 text-sm text-gray-700 dark:text-gray-200"
              aria-labelledby="dropdown-button"
            >
              <li
                v-for="category in categories"
                :key="category"
              >
                <button
                  type="button"
                  class="inline-flex w-full px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white"
                  @click="selectCategory(category)"
                >
                  {{ category }}
                </button>
              </li>
            </ul>
          </div>
          <div class="relative w-full">
            <input
              type="search"
              id="search-dropdown"
              v-model="searchQuery"
              class="block p-2.5 w-full z-20 text-sm text-gray-900 bg-gray-50 rounded-e-lg border-s-gray-50 border-s-2 border border-gray-300 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-s-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:border-blue-500"
              placeholder="Search products..."
              required
            />
            <button
              type="submit"
              class="absolute top-0 end-0 p-2.5 text-sm font-medium h-full text-white bg-blue-700 rounded-e-lg border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
            >
              <svg
                class="w-4 h-4"
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 20 20"
              >
                <path
                  stroke="currentColor"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z"
                />
              </svg>
              <span class="sr-only">Search</span>
            </button>
          </div>
        </div>
      </form>
    </div>
    <Order
      :drinks="drinks"
      :selectedIndex="selectedIndex"
    />
  </div>
  <div class="flex justify-center items-center mt-10">
    <MainPagination
      :totalPages="totalPages"
      :current-page="currentPage"
      @update:current-page="changeCurrentPage"
    />
  </div>
</template>

<script setup>
import { ref, computed, watch, onMounted } from 'vue';
import AppHeader from '../components/AppHeader.vue';
import Order from '../components/Product.vue';
import MainPagination from '../components/MainPagination.vue';
import coffeeService from '../service/coffe.service';
import { useRouter, useRoute } from 'vue-router';

const router = useRouter();
const route = useRoute();

// Dropdown state and logic
const selectedCategory = ref('All'); // Initialize selected category
const categories = ref(['All', 'Coffee', 'Tea', 'Soda', 'Juice fruits']);
const isDropdownOpen = ref(false);

const toggleDropdown = () => {
  isDropdownOpen.value = !isDropdownOpen.value;
};

const selectCategory = (category) => {
  selectedCategory.value = category;
  localStorage.setItem('selectedCategory', category); // Save the selected category to localStorage
  isDropdownOpen.value = false; // Close the dropdown
};

// Restore category from localStorage on component mount
onMounted(() => {
  const storedCategory = localStorage.getItem('selectedCategory');
  if (storedCategory) {
    selectedCategory.value = storedCategory;
  }
});

// Drinks and pagination
const drinks = ref([]);
const totalPages = ref(1);
const selectedIndex = ref(-1);
const searchQuery = ref(''); // Track the search input

const currentPage = computed(() => {
  const page = Number(route.query?.page);
  if (Number.isNaN(page) || page < 1) return 1;
  return page;
});

// Fetch data logic
async function fetchData(page) {
  try {
    console.log('Selected Category:', selectedCategory.value);
    console.log('Search Query:', searchQuery.value);

    const data = await coffeeService.searchCoffee(page, selectedCategory.value, searchQuery.value);
    console.log('API Response:', data); // Log the API response

    if (data && data.items) {
      totalPages.value = data.metadata?.lastPage ?? 1;
      drinks.value = data.items ?? [];
    } else {
      console.error('Unexpected data structure:', data);
      totalPages.value = 1;
      drinks.value = [];
    }
  } catch (error) {
    console.error('Error fetching data:', error);
  }
}

function changeCurrentPage(page) {
  router.push({ name: 'productview', query: { page } });
}

// Watchers
watch(currentPage, () => fetchData(currentPage.value), { immediate: true });

watch(selectedCategory, () => {
  console.log('Category changed to:', selectedCategory.value);
  fetchData(currentPage.value); // Fetch data when category changes
});

let debounceTimeout;
watch(searchQuery, () => {
  clearTimeout(debounceTimeout);
  debounceTimeout = setTimeout(() => fetchData(currentPage.value), 300); // Debounce API calls
});
</script>
