import { NavLink, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

function getInitials(name = '') {
  return name
    .split(' ')
    .slice(0, 2)
    .map(n => n[0])
    .join('')
    .toUpperCase()
}

export default function Navbar() {
  const { user, logout } = useAuth()
  const navigate         = useNavigate()

  function handleLogout() {
    logout()
    navigate('/login')
  }

  const linkClass = ({ isActive }) =>
    `text-sm transition-colors ${
      isActive
        ? 'text-white font-semibold border-b-2 border-cef-accent pb-0.5'
        : 'text-cef-text hover:text-white'
    }`

  return (
    <nav className="bg-cef-surface border-b border-cef-border sticky top-0 z-10">
      <div className="max-w-6xl mx-auto px-6 h-16 flex items-center justify-between">

        {/* Logo */}
        <div className="flex items-center gap-3">
          <div className="w-9 h-9 bg-cef-primary/20 rounded-lg flex items-center justify-center">
            {/* Ícone escola */}
            <svg className="w-5 h-5 text-cef-accent" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
              <path d="M3 9l9-7 9 7v11a2 2 0 01-2 2H5a2 2 0 01-2-2z"/>
              <polyline points="9 22 9 12 15 12 15 22"/>
            </svg>
          </div>
          <div>
            <span className="text-white font-bold text-base leading-none">CEFOPE</span>
            <p className="text-cef-muted text-xs leading-none mt-0.5">Plataforma de Cursos</p>
          </div>
        </div>

        {/* Links de navegação por role */}
        <div className="flex items-center gap-8">
          <NavLink to="/cursos" className={linkClass}>Cursos</NavLink>

          {user?.role === 'admin' && (
            <NavLink to="/alunos-inscritos" className={linkClass}>Alunos Inscritos</NavLink>
          )}

          {user?.role === 'student' && (
            <NavLink to="/minhas-inscricoes" className={linkClass}>Minhas Inscrições</NavLink>
          )}
        </div>

        {/* Usuário + logout */}
        <div className="flex items-center gap-3">
          <div className="w-8 h-8 rounded-full bg-cef-primary flex items-center justify-center text-white text-xs font-bold select-none">
            {getInitials(user?.name)}
          </div>
          <span className="text-cef-text text-sm hidden sm:block">{user?.name}</span>
          <button
            onClick={handleLogout}
            title="Sair"
            className="text-cef-muted hover:text-white transition-colors ml-1"
          >
            {/* Ícone logout */}
            <svg className="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
              <path d="M9 21H5a2 2 0 01-2-2V5a2 2 0 012-2h4"/>
              <polyline points="16 17 21 12 16 7"/>
              <line x1="21" y1="12" x2="9" y2="12"/>
            </svg>
          </button>
        </div>

      </div>
    </nav>
  )
}
