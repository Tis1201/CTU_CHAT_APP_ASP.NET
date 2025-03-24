<template>
  <div class="bg-white p-6">
    <h1 class="text-2xl font-bold mb-4">Admin Panel - Order Management</h1>

    <!-- Search -->
    <div class="flex items-center gap-4 mb-6">
      <input
        v-model="searchQuery"
        type="text"
        placeholder="Search by payment method"
        class="border p-2 rounded flex-1"
      />
    </div>

    <!-- Order Table -->
    <table class="w-full border mb-6">
      <thead>
        <tr>
          <th class="border px-4 py-2">Order ID</th>
          <th class="border px-4 py-2">Customer ID</th>
          <th class="border px-4 py-2">Order Date</th>
          <th class="border px-4 py-2">Payment Method</th>
          <th class="border px-4 py-2">Total Price</th>
          <th class="border px-4 py-2">Status</th>
          <th class="border px-4 py-2">Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="order in filteredOrders"
          :key="order.orderItemId"
        >
          <td class="border px-4 py-2">{{ order.orderItemId }}</td>
          <td class="border px-4 py-2">{{ order.customerId }}</td>
          <td class="border px-4 py-2">{{ formatDate(order.orderDate) }}</td>
          <td class="border px-4 py-2">{{ order.paymentMethod }}</td>
          <td class="border px-4 py-2">{{ order.totalPrice }}</td>
          <td class="border px-4 py-2">{{ order.status ? 'Completed' : 'Pending' }}</td>
          <td class="border px-4 py-2">
            <button
              @click="editOrder(order)"
              class="text-blue-500"
            >
              Edit
            </button>
            <button
              @click="deleteOrder(order.orderItemId)"
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

    <!-- Order Form -->
    <div class="border p-4 rounded mb-6">
      <h2 class="text-xl font-bold mb-4">{{ isEdit ? 'Edit Order' : 'Add New Order' }}</h2>
      <form @submit.prevent="handleSubmit">
        <input
          v-model="form.customerId"
          type="number"
          placeholder="Customer ID"
          class="border p-2 w-full mb-2"
          required
        />
        <input
          v-model="form.productId"
          type="number"
          placeholder="Product ID"
          class="border p-2 w-full mb-2"
          required
        />
        <input
          v-model="form.quantity"
          type="number"
          placeholder="Quantity"
          class="border p-2 w-full mb-2"
          required
        />
        <input
          v-model="form.paymentMethod"
          type="text"
          placeholder="Payment Method"
          class="border p-2 w-full mb-2"
          required
        />

        <select
          v-model="form.status"
          class="border p-2 w-full mb-2"
          required
        >
          <option :value="false">Pending</option>
          <option :value="true">Completed</option>
        </select>

        <button
          type="submit"
          class="bg-blue-500 text-white px-4 py-2 rounded"
        >
          {{ isEdit ? 'Update Order' : 'Add Order' }}
        </button>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch, computed } from 'vue';
import orderService from '@/service/order.service';

const orders = ref([]);
const currentPage = ref(1);
const totalPages = ref(1);
const searchQuery = ref('');
const isEdit = ref(false);
const debounceTimeout = ref(null);

const form = ref({
  orderItemId: null,
  customerId: null,
  productId: null,
  quantity: null,
  paymentMethod: '',
  status: false
});

// Lọc đơn hàng theo payment method nếu có searchQuery
const filteredOrders = computed(() => {
  if (!searchQuery.value.trim()) return orders.value;
  return orders.value.filter((order) =>
    order.paymentMethod.toLowerCase().includes(searchQuery.value.toLowerCase())
  );
});

async function fetchData() {
  try {
    const data = await orderService.getOrdersAdmin(currentPage.value);
    if (data && data.items) {
      orders.value = data.items;
      totalPages.value = data.metadata?.lastPage ?? 1;
    } else {
      orders.value = [];
      totalPages.value = 1;
    }
  } catch (error) {
    console.error('Error fetching orders:', error);
  }
}

function changePage(page) {
  if (page > 0 && page <= totalPages.value) {
    currentPage.value = page;
    fetchData();
  }
}

function editOrder(order) {
  Object.assign(form.value, order);
  isEdit.value = true;
}

async function deleteOrder(id) {
  try {
    await orderService.deleteOrder(id);
    fetchData();
  } catch (error) {
    console.error('Error deleting order:', error);
  }
}

async function handleSubmit() {
  try {
    const payload = { ...form.value };
    if (isEdit.value) {
      await orderService.updateOrder(payload.orderItemId, payload);
    } else {
      await orderService.createOrder(payload);
    }
    resetForm();
    fetchData();
  } catch (error) {
    console.error('Error saving order:', error);
  }
}

function resetForm() {
  form.value = {
    orderItemId: null,
    customerId: null,
    productId: null,
    quantity: null,
    paymentMethod: '',
    status: false
  };
  isEdit.value = false;
}

function formatDate(dateString) {
  const date = new Date(dateString);
  return date.toLocaleDateString();
}

// Watch for search
watch(searchQuery, () => {
  clearTimeout(debounceTimeout.value);
  debounceTimeout.value = setTimeout(fetchData, 300);
});

onMounted(fetchData);
</script>

<style>
body {
  background-color: white;
}
</style>
