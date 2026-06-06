import axios from 'axios'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5174',
})

// Interceptor de requisição: injeta o token JWT em todo request autenticado
api.interceptors.request.use(config => {
  const token = localStorage.getItem('cefope_token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

export default api
