import { createContext, useContext, useState, useEffect } from 'react'
import api from '../services/api'

const AuthContext = createContext(null)

export function AuthProvider({ children }) {
  const [user,    setUser]    = useState(null)
  const [token,   setToken]   = useState(null)
  const [loading, setLoading] = useState(true)

  // Recupera sessão salva no localStorage ao recarregar a página
  useEffect(() => {
    const storedToken = localStorage.getItem('cefope_token')
    const storedUser  = localStorage.getItem('cefope_user')
    if (storedToken && storedUser) {
      setToken(storedToken)
      setUser(JSON.parse(storedUser))
    }
    setLoading(false)
  }, [])

  async function login(email, password) {
    const response = await api.post('/auth/login', { email, password })
    const { id, token: newToken, name, email: userEmail, role } = response.data

    const userData = { id, name, email: userEmail, role }

    localStorage.setItem('cefope_token', newToken)
    localStorage.setItem('cefope_user',  JSON.stringify(userData))

    setToken(newToken)
    setUser(userData)

    return userData
  }

  function logout() {
    localStorage.removeItem('cefope_token')
    localStorage.removeItem('cefope_user')
    setToken(null)
    setUser(null)
  }

  return (
    <AuthContext.Provider value={{ user, token, login, logout, loading }}>
      {children}
    </AuthContext.Provider>
  )
}

// Hook de atalho para usar o contexto em qualquer componente
export function useAuth() {
  return useContext(AuthContext)
}
