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

function makeCoffeeService() {
  const baseUrl = 'http://localhost:5223/api/v1/products';
  async function getCoffees(page, limit = 12) {
    let url = `${baseUrl}?page=${page}&limit=${limit}`;
    const data = await efetch(url);
    return data;
  }

  async function getCoffeeById(id) {
    let url = `${baseUrl}/${id}`;
    const data = await efetch(url);
    return data;
  }

  async function createCoffee(formData) {
    let url = `${baseUrl}`;
    const data = await efetch(url, {
      method: 'POST',
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`, // Không thêm Content-Type vì fetch tự thêm cho FormData
      },
      body: formData, // Đảm bảo gửi đúng FormData
    });
    return data;
  }
  

  async function updateCoffee(id, coffee) {
    let url = `${baseUrl}/${id}`;
    const data = await efetch(url, {
      method: 'PUT',
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: coffee
    });
  }

  async function deleteCoffee(id) {
    let url = `${baseUrl}/${id}`;
    const data = await efetch(url, {
      method: 'DELETE',
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
  }

  async function deleteAllCoffees() {
    let url = `${baseUrl}`;
    const data = await efetch(url, {
      method: 'DELETE',
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
  }

  async function searchCoffee(page, category, name) {
    try {
      let nameQuery = '';
      if (name) {
        nameQuery = `&name=${name}`;
      }
      let limit = 12;
      console.log('Category:', category);
      let url = `${baseUrl}/cate/${category}?page=${page}&limit=${limit}${nameQuery}`;
      console.log('Fetching URL:', url);

      const data = await efetch(url);
      return data;
    } catch (error) {
      console.error('Error fetching coffee data:', error);
      throw new Error('Failed to fetch coffee data');
    }
  }

  return {
    getCoffees,
    getCoffeeById,
    createCoffee,
    updateCoffee,
    deleteCoffee,
    deleteAllCoffees,
    searchCoffee
  };
}

export default makeCoffeeService();
