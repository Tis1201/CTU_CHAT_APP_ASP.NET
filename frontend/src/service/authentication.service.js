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

function makeAuthenticationService() {
  const baseUrl = 'http://localhost:5223/api/auth';
  async function register(customer) {
    let url = `${baseUrl}/register`;
    return await efetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(customer)
    });
  }
  async function login(email, password) {
    let url = `${baseUrl}/login`;
    return await efetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ email, password })
    });
  }
  return {
    register,
    login
  };
}

export default makeAuthenticationService;
