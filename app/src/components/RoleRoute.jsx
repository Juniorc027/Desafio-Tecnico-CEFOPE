import { Navigate, Outlet } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

// Protege rotas que exigem um papel (role) específico
// Ex: <RoleRoute allowed={['admin']} />
export default function RoleRoute({ allowed }) {
  const { user } = useAuth()

  return allowed.includes(user?.role)
    ? <Outlet />
    : <Navigate to="/cursos" replace />
}
