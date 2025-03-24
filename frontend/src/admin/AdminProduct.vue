<template>
  <div class="bg-white p-6">
    <!-- Header -->
    <h1 class="text-2xl font-bold mb-4">Admin Panel - Coffee Management</h1>

    <!-- Search and Dropdown -->
    <div class="flex items-center gap-4 mb-6">
      <select
        v-model="selectedCategory"
        @change="fetchData"
        class="border p-2 rounded"
      >
        <option value="All">All Categories</option>
        <option
          v-for="category in categories"
          :key="category"
          :value="category"
        >
          {{ category }}
        </option>
      </select>
      <input
        v-model="searchQuery"
        type="text"
        placeholder="Search by name"
        class="border p-2 rounded flex-1"
      />
    </div>

    <!-- Coffee Table -->
    <table class="w-full border mb-6">
      <thead>
        <tr>
          <th class="border px-4 py-2">ID</th>
          <th class="border px-4 py-2">Name</th>
          <th class="border px-4 py-2">Category</th>
          <th class="border px-4 py-2">Price</th>
          <th class="border px-4 py-2">Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="coffee in coffees"
          :key="coffee.productId"
        >
          <td class="border px-4 py-2">{{ coffee.productId }}</td>
          <td class="border px-4 py-2">{{ coffee.name }}</td>
          <td class="border px-4 py-2">{{ coffee.category }}</td>
          <td class="border px-4 py-2">{{ coffee.price }}</td>
          <td class="border px-4 py-2">
            <button
              @click="editCoffee(coffee)"
              class="text-blue-500"
            >
              Edit
            </button>
            <button
              @click="deleteCoffee(coffee.productId)"
              class="text-red-500 ml-2"
            >
              Delete
            </button>
          </td>
        </tr>
      </tbody>
    </table>

    <!-- Pagination -->
    <div class="flex justify-between mb-6">
      <button
        @click="changePage(currentPage - 1)"
        :disabled="currentPage === 1"
      >
        Previous
      </button>
      <button
        @click="changePage(currentPage + 1)"
        :disabled="currentPage === totalPages"
      >
        Next
      </button>
    </div>

    <!-- Coffee Form -->
    <div class="border p-4 rounded mb-6">
      <h2 class="text-xl font-bold mb-4">{{ isEdit ? 'Edit Coffee' : 'Add New Coffee' }}</h2>
      <form
        @submit.prevent="handleSubmit"
        enctype="multipart/form-data"
      >
        <!-- Name -->
        <input
          v-model="form.name"
          type="text"
          placeholder="Name"
          class="border p-2 w-full mb-2"
          required
        />

        <!-- Description -->
        <textarea
          v-model="form.description"
          placeholder="Description"
          class="border p-2 w-full mb-2"
          required
        ></textarea>

        <!-- Price -->
        <input
          v-model.number="form.price"
          type="number"
          placeholder="Price"
          class="border p-2 w-full mb-2"
          required
        />

        <!-- Category -->
        <select
          v-model="form.category"
          class="border p-2 w-full mb-2"
          required
        >
          <option value="">Select Category</option>
          <option
            v-for="category in categories"
            :key="category"
            :value="category"
          >
            {{ category }}
          </option>
        </select>

        <!-- Product Image -->
        <input
          @change="handleFileUpload"
          type="file"
          accept="image/*"
          class="border p-2 w-full mb-2"
          :required="!isEdit"
        />

        <!-- Submit Button -->
        <button
          type="submit"
          class="bg-blue-500 text-white px-4 py-2 rounded"
        >
          {{ isEdit ? 'Update Coffee' : 'Add Coffee' }}
        </button>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue';
import coffeeService from '@/service/coffe.service';

const coffees = ref([]);
const currentPage = ref(1);
const totalPages = ref(1);
const categories = ref(['Coffee', 'Tea', 'Soda', 'Juice Fruits']);
const selectedCategory = ref('All');
const searchQuery = ref('');
const form = ref({
  productId: null,
  name: '',
  category: '',
  price: '',
  description: '',
  productImg: null
});

const isEdit = ref(false);
const debounceTimeout = ref(null);

async function fetchData() {
  try {
    const data = await coffeeService.searchCoffee(
      currentPage.value,
      selectedCategory.value,
      searchQuery.value
    );
    if (data && data.items) {
      coffees.value = data.items;
      totalPages.value = data.metadata?.lastPage ?? 1;
    } else {
      coffees.value = [];
      totalPages.value = 1;
    }
  } catch (error) {
    console.error('Error fetching data:', error);
  }
}

function changePage(page) {
  if (page > 0 && page <= totalPages.value) {
    currentPage.value = page;
    fetchData();
  }
}

function editCoffee(coffee) {
  Object.assign(form.value, coffee);
  form.value.productImg = null;
  isEdit.value = true;
}

async function deleteCoffee(id) {
  try {
    await coffeeService.deleteCoffee(id);
    fetchData();
  } catch (error) {
    console.error('Error deleting coffee:', error);
  }
}

async function handleSubmit() {
  try {
    const formData = new FormData();
    formData.append('name', form.value.name);
    formData.append('description', form.value.description);
    formData.append('price', form.value.price);
    formData.append('category', form.value.category);
    if (form.value.productImg) {
      formData.append('productImg', form.value.productImg);
    }

    if (isEdit.value) {
      await coffeeService.updateCoffee(form.value.productId, formData);
    } else {
      await coffeeService.createCoffee(formData);
    }

    resetForm();
    fetchData();
  } catch (error) {
    console.error('Error saving coffee:', error);
  }
}

function resetForm() {
  form.value = {
    productId: null,
    name: '',
    category: '',
    price: '',
    description: '',
    productImg: null
  };
  isEdit.value = false;
}

function handleFileUpload(event) {
  form.value.productImg = event.target.files[0];
}

watch([selectedCategory, searchQuery], () => {
  clearTimeout(debounceTimeout.value);
  debounceTimeout.value = setTimeout(() => fetchData(), 300);
});

onMounted(() => fetchData());
</script>

<style>
body {
  background-color: white;
}
</style>
