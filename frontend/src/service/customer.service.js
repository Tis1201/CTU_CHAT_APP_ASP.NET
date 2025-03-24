async function efetch(url, options = {}) {
  let result = {};
  let json = {};
  try {
    result = await fetch(url, options);
    json = await result.json();
  } catch (error) {
    throw new Error(error.message);
  }
  if (!result.ok || json.status !== 'success') {
    throw new Error(json.message);
  }
  return json.data;
}

function makeCustomerService() {
  const baseUrl = 'http://localhost:5223/api/customers';
  async function getCustomers(page, limit = 12) {
    let url = `${baseUrl}?page=${page}&limit=${limit}`;
    const data = await efetch(url, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return data;
  }
  async function getCustomerById(id) {
    let url = `${baseUrl}/${id}`;
    const data = await efetch(url, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return data;
  }
  async function createCustomer(customer) {
    let url = `${baseUrl}/register`;
    console.log(customer);
    const data = await efetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(customer)
    });
    return data;
  }
  async function updateCustomer(id, customer) {
    let url = `${baseUrl}/${id}`;
    const data = await efetch(url, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(customer)
    });
    return data;
  }
  async function deleteCustomer(id) {
    let url = `${baseUrl}/${id}`;
    const data = await efetch(url, {
      method: 'DELETE',
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return data;
  }
  return {
    getCustomers,
    getCustomerById,
    createCustomer,
    updateCustomer,
    deleteCustomer
  };
}

export default makeCustomerService();
