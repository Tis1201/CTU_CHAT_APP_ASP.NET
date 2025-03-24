async function efetch(url, options = {}) {
  let result = {};
  let json = {};
  try {
    result = await fetch(url, options);
    json = await result.json();
  } catch (error) {
    throw new Error(error.message);
  }
  if (!result.ok) {
    console.error('Server response:', json); // Log the exact server response
    throw new Error(JSON.stringify(json)); // Include detailed error message
  }
  return json.data;
}

function makeOrderService() {
  const baseUrl = 'http://localhost:5223/api/v1/order_items';

  async function getOrders(page, limit = 20) {
    let url = `${baseUrl}?page=${page}&limit=${limit}`;
    const data = await efetch(url, {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    });
    return data;
  }

  async function getOrdersAdmin(page, limit = 20) {
    let url = `${baseUrl}/admin?page=${page}&limit=${limit}`;
    const data = await efetch(url, {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    });
    return data;
  } 

  async function getOrderById(id) {
    let url = `${baseUrl}/${id}`;
      const data = await efetch(url, {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    });
    return data;
  }

  async function createOrder(data) {
    let url = `${baseUrl}`;
    const response = await efetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      },
      body: JSON.stringify(data)
    });
    return response;
  }

  async function updateOrder(id, data) {
    let url = `${baseUrl}/${id}`;
    const response = await efetch(url, {
      method: 'PATCH',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      },
      body: JSON.stringify(data)
    });
    return response;
  }

  async function deleteOrder(id) {
    let url = `${baseUrl}/${id}`;
    const response = await efetch(url, {
      method: 'DELETE',
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    });
    return response;
  }



  return {
    getOrders,
    getOrderById,
    createOrder,
    updateOrder,
    deleteOrder,
    getOrdersAdmin
  };
}

export default makeOrderService();
