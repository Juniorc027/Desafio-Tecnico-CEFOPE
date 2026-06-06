import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

export default function Login() {
  const [email,    setEmail]    = useState('')
  const [password, setPassword] = useState('')
  const [error,    setError]    = useState('')
  const [loading,  setLoading]  = useState(false)

  const { login }  = useAuth()
  const navigate   = useNavigate()

  async function handleSubmit(e) {
    e.preventDefault()
    setError('')
    setLoading(true)

    try {
      const userData = await login(email, password)
      // Admin vai direto para a listagem de inscrições; student vai para os cursos
      navigate(userData.role === 'admin' ? '/alunos-inscritos' : '/cursos', { replace: true })
    } catch (err) {
      if (err.response?.status === 401) {
        setError('E-mail ou senha incorretos.')
      } else {
        setError('Não foi possível conectar ao servidor.')
      }
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="min-h-screen bg-cef-bg flex items-center justify-center p-4">
      <div className="w-full max-w-sm">

        {/* Card */}
        <div className="bg-cef-surface border border-cef-border rounded-2xl p-8">

          {/* Logo */}
          <div className="text-center mb-8">
            <div className="inline-flex items-center justify-center w-14 h-14 bg-cef-primary/20 rounded-xl mb-4">
              <svg className="w-7 h-7 text-cef-accent" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                <path d="M3 9l9-7 9 7v11a2 2 0 01-2 2H5a2 2 0 01-2-2z"/>
                <polyline points="9 22 9 12 15 12 15 22"/>
              </svg>
            </div>
            <h1 className="text-2xl font-bold text-white tracking-wide">CEFOPE</h1>
            <p className="text-cef-muted text-sm mt-1">Plataforma de Cursos</p>
          </div>

          {/* Formulário */}
          <form onSubmit={handleSubmit} className="flex flex-col gap-4">

            <div className="flex flex-col gap-1.5">
              <label className="text-cef-text text-sm font-medium">E-mail</label>
              <input
                type="email"
                value={email}
                onChange={e => setEmail(e.target.value)}
                required
                placeholder="seu@email.com"
                className="
                  bg-cef-bg border border-cef-border rounded-lg px-4 py-2.5
                  text-white text-sm placeholder:text-cef-muted/50
                  focus:outline-none focus:border-cef-primary transition-colors
                "
              />
            </div>

            <div className="flex flex-col gap-1.5">
              <label className="text-cef-text text-sm font-medium">Senha</label>
              <input
                type="password"
                value={password}
                onChange={e => setPassword(e.target.value)}
                required
                placeholder="••••••••"
                className="
                  bg-cef-bg border border-cef-border rounded-lg px-4 py-2.5
                  text-white text-sm placeholder:text-cef-muted/50
                  focus:outline-none focus:border-cef-primary transition-colors
                "
              />
            </div>

            {/* Mensagem de erro */}
            {error && (
              <p className="text-red-400 text-sm text-center">{error}</p>
            )}

            <button
              type="submit"
              disabled={loading}
              className="
                mt-2 bg-cef-primary hover:bg-cef-accent disabled:opacity-60
                text-white font-semibold py-2.5 rounded-lg transition-colors
              "
            >
              {loading ? 'Entrando...' : 'Entrar'}
            </button>

          </form>
        </div>

        {/* Rodapé */}
        <p className="text-cef-muted/50 text-xs text-center mt-6">
          Centro de Formação e Aperfeiçoamento dos Profissionais da Educação do Espírito Santo
        </p>
      </div>
    </div>
  )
}
