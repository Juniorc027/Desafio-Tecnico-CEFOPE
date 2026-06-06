import { Navigate, Outlet } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

// Protege rotas que exigem autenticação
// Se não há usuário logado, redireciona para /login
export default function PrivateRoute() {
  const { user, loading } = useAuth()

  if (loading) {
    return (
      <div className="min-h-screen bg-cef-bg flex items-center justify-center">
        <div className="w-8 h-8 border-2 border-cef-primary border-t-transparent rounded-full animate-spin" />
      </div>
    )
  }

  return user ? <Outlet /> : <Navigate to="/login" replace />
}
