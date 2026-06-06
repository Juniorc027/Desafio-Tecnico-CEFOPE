import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import { AuthProvider } from './context/AuthContext'
import PrivateRoute    from './components/PrivateRoute'
import RoleRoute       from './components/RoleRoute'
import Login           from './pages/Login'
import Courses         from './pages/Courses'
import Enrollments     from './pages/Enrollments'
import MyEnrollments   from './pages/MyEnrollments'

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          {/* Rota pública */}
          <Route path="/login" element={<Login />} />

          {/* Rotas protegidas — exigem token válido */}
          <Route element={<PrivateRoute />}>
            {/* Acessível por Admin e Student */}
            <Route path="/cursos" element={<Courses />} />

            {/* Somente Admin */}
            <Route element={<RoleRoute allowed={['admin']} />}>
              <Route path="/alunos-inscritos" element={<Enrollments />} />
            </Route>

            {/* Somente Student */}
            <Route element={<RoleRoute allowed={['student']} />}>
              <Route path="/minhas-inscricoes" element={<MyEnrollments />} />
            </Route>
          </Route>

          {/* Qualquer rota desconhecida vai para /login */}
          <Route path="*" element={<Navigate to="/login" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  )
}
