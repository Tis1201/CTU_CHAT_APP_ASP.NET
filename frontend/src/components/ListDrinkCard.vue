<template>
  <div class="w-full flex justify-center mt-20 mb-10">
    <div class="flex items-center justify-center flex-wrap">
      <div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 p-4">
        <div
          v-for="(drink, index) in drinks"
          :key="drink.id"
          :class="{ 'opacity-50': index === selectedIndex }"
          class="bg-coffee border border-gray-700 rounded-lg dark:bg-gray-800 dark:border-gray-700 h-[320px] w-[250px] overflow-hidden shadow-lg shadow-gray-700 hover:shadow-xl hover:shadow-gray-600 transition-shadow duration-300"
        >
          <a href="#">
            <img
              class="rounded-t-lg w-full h-[200px] object-cover"
              :src="resolveImage(drink.productImg)"
              alt="Drink Image"
            />
          </a>
          <div class="p-2 mt-2">
            <a href="#">
              <h5
                class="mb-1 text-sm font-bold tracking-tight text-white dark:text-white text-[20px]"
              >
                {{ drink.name }}
              </h5>
            </a>
            <p
              class="mb-2 text-xs font-normal text-white dark:text-gray-400 mt-5 overflow-ellipsis overflow-hidden"
            >
              ${{ drink.price }}
            </p>
            <div>
              <!-- Buy Now Button -->
              <a
                href="#"
                @click.prevent="showProduct(drink.productId)"
                class="inline-flex h-[30px] items-center px-2 py-1 text-xs font-medium text-center text-white bg-blue-700 rounded-lg hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
              >
                Buy Now
                <svg
                  class="rtl:rotate-180 w-2.5 h-2.5 ms-1"
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

              <!-- Overlay for ProductId component -->
              <div
                v-if="isProductVisible"
                class="fixed inset-0 z-50 flex items-center justify-center bg-gray-900 bg-opacity-50"
              >
                <div
                  class="relative bg-white rounded-lg shadow-lg p-6"
                  :class="modalSize"
                >
                  <!-- Close Button -->
                  <button
                    @click="hideProduct"
                    class="absolute top-2 right-2 text-gray-600 hover:text-gray-800"
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      fill="none"
                      viewBox="0 0 24 24"
                      stroke-width="2"
                      stroke="currentColor"
                      class="w-6 h-6"
                    >
                      <path
                        stroke-linecap="round"
                        stroke-linejoin="round"
                        d="M6 18L18 6M6 6l12 12"
                      />
                    </svg>
                  </button>

                  <!-- ProductId Component (Displayed as Modal Content) -->
                  <ProductId :id="selectedProductId" />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import ProductId from './ProductId.vue';
import { ref } from 'vue';

defineProps({
  drinks: {
    type: Array,
    required: true,
    default: () => []
  },
  selectedIndex: { type: Number, default: -1 }
});

const isProductVisible = ref(false);
const selectedProductId = ref(null);

const showProduct = (id) => {
  selectedProductId.value = id;
  isProductVisible.value = true;
};

const hideProduct = () => {
  isProductVisible.value = false;
  selectedProductId.value = null;
};


const resolveImage = (img) => {
  if (!img) return '/img/no-image.jpg'; 
  return img.startsWith('http') ? img : `http://localhost:5223${img}`;
};

const modalSize = 'w-[70%] h-[90%] max-w-4xl max-h-[100vh]';
</script>
