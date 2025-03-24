<script setup>
import { computed } from 'vue';
const props = defineProps({
  totalPages: {
    type: Number,
    required: true
  },
  length: {
    type: Number,
    default: 3
  },
  currentPage: {
    type: Number,
    default: 1
  }
});
const $emit = defineEmits(['update:currentPage']);
const pages = computed(() => {
  const pages = [];
  const half = Math.floor(props.length / 2);
  let start = props.currentPage - half;
  let end = props.currentPage + half;
  if (start <= 0) {
    start = 1;
    end = props.length;
  }
  if (end > props.totalPages) {
    end = props.totalPages;
    start = end - props.length + 1;
    if (start <= 0) start = 1;
  }
  for (let i = start; i <= end; i++) {
    pages.push(i);
  }
  return pages;
});
</script>
<template>
    <nav>
      <ul class="inline-flex -space-x-px text-sm">
        <!-- Previous Button -->
        <li :class="{ 'disabled': currentPage === 1 }">
          <a
            role="button"
            class="flex items-center justify-center px-3 h-8 text-gray-500 bg-white border border-gray-300 rounded-s-lg hover:bg-gray-100 hover:text-gray-700 dark:bg-gray-800 dark:border-gray-700 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-white"
            @click.prevent="$emit('update:currentPage', currentPage - 1)"
          >
            <span>&laquo;</span>
          </a>
        </li>
  
        <!-- Page Numbers -->
        <li
          v-for="page in pages"
          :key="page"
          :class="{ 'active': currentPage === page }"
          class="flex items-center justify-center"
        >
          <a
            role="button"
            class="px-3 h-8 text-gray-500 bg-white border border-gray-300 hover:bg-gray-100 hover:text-gray-700 dark:bg-gray-800 dark:border-gray-700 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-white"
            @click.prevent="$emit('update:currentPage', page)"
          >
            {{ page }}
          </a>
        </li>
  
        <!-- Next Button -->
        <li :class="{ 'disabled': currentPage === totalPages }">
          <a
            role="button"
            class="flex items-center justify-center px-3 h-8 text-gray-500 bg-white border border-gray-300 rounded-e-lg hover:bg-gray-100 hover:text-gray-700 dark:bg-gray-800 dark:border-gray-700 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-white"
            @click.prevent="$emit('update:currentPage', currentPage + 1)"
          >
            <span>&raquo;</span>
          </a>
        </li>
      </ul>
    </nav>
  </template>
  