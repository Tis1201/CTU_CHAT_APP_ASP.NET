<template>
  <div class="bg-white p-6">
    <!-- Header -->
    <h1 class="text-2xl font-bold mb-4">Admin Panel - Customer Management</h1>

    <!-- Search -->
    <div class="flex items-center gap-4 mb-6">
      <input
        v-model="searchQuery"
        type="text"
        placeholder="Search by name or email"
        class="border p-2 rounded flex-1"
      />
    </div>

    <!-- Customer Table -->
    <table class="w-full border mb-6">
      <thead>
        <tr>
          <th class="border px-4 py-2">ID</th>
          <th class="border px-4 py-2">Full Name</th>
          <th class="border px-4 py-2">Email</th>
          <th class="border px-4 py-2">Phone</th>
          <th class="border px-4 py-2">Role</th>
          <th class="border px-4 py-2">Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="customer in customers"
          :key="customer.customer_id"
        >
          <td class="border px-4 py-2">{{ customer.customer_id }}</td>
          <td class="border px-4 py-2">{{ customer.full_name }}</td>
          <td class="border px-4 py-2">{{ customer.email }}</td>
          <td class="border px-4 py-2">{{ customer.phone_number }}</td>
          <td class="border px-4 py-2">{{ customer.role ? 'Admin' : 'User' }}</td>
          <td class="border px-4 py-2">
            <button
              @click="editCustomer(customer)"
              class="text-blue-500"
            >
              Edit
            </button>
            <button
              @click="deleteCustomer(customer.customer_id)"
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

    <!-- Customer Form -->
    <div class="border p-4 rounded mb-6">
      <h2 class="text-xl font-bold mb-4">{{ isEdit ? 'Edit Customer' : 'Add New Customer' }}</h2>
      <form @submit.prevent="handleSubmit">
        <!-- Full Name -->
        <input
          v-model="form.full_name"
          type="text"
          placeholder="Full Name"
          class="border p-2 w-full mb-2"
          required
        />

        <!-- Email -->
        <input
          v-model="form.email"
          type="email"
          placeholder="Email"
          class="border p-2 w-full mb-2"
          required
        />

        <!-- Phone -->
        <input
          v-model="form.phone_number"
          type="text"
          placeholder="Phone Number"
          class="border p-2 w-full mb-2"
          required
        />

        <!-- Password -->
        <input
          v-model="form.password"
          type="password"
          placeholder="Password"
          class="border p-2 w-full mb-2"
          :required="!isEdit"
        />

        <!-- Address -->
        <input
          v-model="form.address"
          type="text"
          placeholder="Address"
          class="border p-2 w-full mb-2"
          required
        />

        <!-- Role -->
        <select
          v-model="form.role"
          class="border p-2 w-full mb-2"
          required
        >
          <option value="false">User</option>
          <option value="true">Admin</option>
        </select>

        <!-- Submit Button -->
        <button
          type="submit"
          class="bg-blue-500 text-white px-4 py-2 rounded"
        >
          {{ isEdit ? 'Update Customer' : 'Add Customer' }}
        </button>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue';
import customerService from '@/service/customer.service';

const customers = ref([]);
const currentPage = ref(1);
const totalPages = ref(1);
const searchQuery = ref('');
const form = ref({
  full_name: '',
  phone_number: '',
  email: '',
  password: '',
  address: '',
  role: false
});
const isEdit = ref(false);
const debounceTimeout = ref(null);

async function fetchData() {
  try {
    const data = await customerService.getCustomers(currentPage.value, 10);

    if (data && data.items && data.metadata) {
      customers.value = data.items;
      totalPages.value = data.metadata.lastPage || 1;
    } else {
      console.error('Unexpected data structure:', data);
      customers.value = [];
      totalPages.value = 1;
    }
  } catch (error) {
    console.error('Error fetching data:', error);
    customers.value = [];
    totalPages.value = 1;
  }
}

function changePage(page) {
  if (page > 0 && page <= totalPages.value) {
    currentPage.value = page;
    fetchData();
  }
}

function editCustomer(customer) {
  Object.assign(form.value, customer);
  isEdit.value = true;
}

async function deleteCustomer(email) {
  try {
    await customerService.deleteCustomer(email); // Nếu không có ID thì dùng email làm key
    fetchData();
  } catch (error) {
    console.error('Error deleting customer:', error);
  }
}

async function handleSubmit() {
  try {
    if (!form.value.full_name || !form.value.email || (!isEdit.value && !form.value.password)) {
      alert('Please fill all the required fields.');
      return;
    }

    const customer = {
      full_name: form.value.full_name,
      phone_number: form.value.phone_number,
      email: form.value.email,
      password: form.value.password,
      address: form.value.address,
      role: form.value.role === 'true' || form.value.role === true
    };

    if (isEdit.value) {
      await customerService.updateCustomer(form.value.customer_id, customer);
    } else {
      await customerService.createCustomer(customer);
    }

    resetForm();
    fetchData();
  } catch (error) {
    console.error('Error saving customer:', error);
    alert(error.message);
  }
}

function resetForm() {
  form.value = {
    full_name: '',
    phone_number: '',
    email: '',
    password: '',
    address: '',
    role: false
  };
  isEdit.value = false;
}

watch(searchQuery, () => {
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
