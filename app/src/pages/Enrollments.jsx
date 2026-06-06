import { useEffect, useState } from 'react'
import Navbar from '../components/Navbar'
import api    from '../services/api'

function getInitials(name = '') {
  return name.split(' ').slice(0, 2).map(n => n[0]).join('').toUpperCase()
}

function formatDate(dateStr) {
  return new Date(dateStr).toLocaleDateString('pt-BR', {
    day: '2-digit', month: '2-digit', year: 'numeric'
  })
}

export default function Enrollments() {
  const [enrollments, setEnrollments] = useState([])
  const [courses,     setCourses]     = useState([])
  const [loading,     setLoading]     = useState(true)

  useEffect(() => {
    async function loadData() {
      try {
        const [enrollRes, courseRes] = await Promise.all([
          api.get('/enrollments'),
          api.get('/courses'),
        ])
        setEnrollments(enrollRes.data)
        setCourses(courseRes.data)
      } catch {
        // Mantém arrays vazios em caso de erro
      } finally {
        setLoading(false)
      }
    }
    loadData()
  }, [])

  // Calcula métricas a partir dos dados carregados
  const totalStudents    = new Set(enrollments.map(e => e.studentEmail)).size
  const activeCourses    = courses.length
  const totalEnrollments = enrollments.length

  const metrics = [
    {
      label: 'Total de Alunos',
      value: totalStudents,
      icon: (
        <svg className="w-6 h-6" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
          <path d="M17 21v-2a4 4 0 00-4-4H5a4 4 0 00-4 4v2"/>
          <circle cx="9" cy="7" r="4"/>
          <path d="M23 21v-2a4 4 0 00-3-3.87M16 3.13a4 4 0 010 7.75"/>
        </svg>
      ),
    },
    {
      label: 'Cursos Ativos',
      value: activeCourses,
      icon: (
        <svg className="w-6 h-6" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
          <path d="M2 3h6a4 4 0 014 4v14a3 3 0 00-3-3H2z"/>
          <path d="M22 3h-6a4 4 0 00-4 4v14a3 3 0 013-3h7z"/>
        </svg>
      ),
    },
    {
      label: 'Total de Inscrições',
      value: totalEnrollments,
      icon: (
        <svg className="w-6 h-6" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
          <path d="M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8z"/>
          <polyline points="14 2 14 8 20 8"/>
          <line x1="16" y1="13" x2="8" y2="13"/>
          <line x1="16" y1="17" x2="8" y2="17"/>
        </svg>
      ),
    },
  ]

  return (
    <div className="min-h-screen bg-cef-bg">
      <Navbar />

      <main className="max-w-6xl mx-auto px-6 py-8">

        {/* Cabeçalho */}
        <div className="mb-8">
          <h2 className="text-2xl font-bold text-white">Alunos Inscritos</h2>
          <p className="text-cef-muted text-sm mt-1">Visão geral de todas as inscrições na plataforma.</p>
        </div>

        {/* Métricas */}
        <div className="grid grid-cols-1 sm:grid-cols-3 gap-4 mb-8">
          {metrics.map(m => (
            <div key={m.label} className="bg-cef-surface border border-cef-border rounded-xl p-5 flex items-center gap-4">
              <div className="w-12 h-12 bg-cef-primary/20 rounded-lg flex items-center justify-center text-cef-accent">
                {m.icon}
              </div>
              <div>
                <p className="text-3xl font-bold text-white">{m.value}</p>
                <p className="text-cef-muted text-xs mt-0.5">{m.label}</p>
              </div>
            </div>
          ))}
        </div>

        {/* Loading */}
        {loading && (
          <div className="flex justify-center py-20">
            <div className="w-8 h-8 border-2 border-cef-primary border-t-transparent rounded-full animate-spin" />
          </div>
        )}

        {/* Tabela */}
        {!loading && (
          enrollments.length === 0
            ? <p className="text-cef-muted text-center py-16">Nenhuma inscrição registrada.</p>
            : (
              <div className="bg-cef-surface border border-cef-border rounded-xl overflow-hidden">
                <table className="w-full text-sm">
                  <thead>
                    <tr className="border-b border-cef-border">
                      <th className="text-left text-cef-muted font-medium px-5 py-3.5">Aluno</th>
                      <th className="text-left text-cef-muted font-medium px-5 py-3.5">E-mail</th>
                      <th className="text-left text-cef-muted font-medium px-5 py-3.5">Curso</th>
                      <th className="text-left text-cef-muted font-medium px-5 py-3.5">Data</th>
                    </tr>
                  </thead>
                  <tbody>
                    {enrollments.map((e, i) => (
                      <tr
                        key={e.id}
                        className={i % 2 === 0 ? '' : 'bg-white/[0.02]'}
                      >
                        {/* Avatar com iniciais + nome */}
                        <td className="px-5 py-3.5">
                          <div className="flex items-center gap-3">
                            <div className="w-8 h-8 rounded-full bg-cef-primary flex items-center justify-center text-white text-xs font-bold shrink-0">
                              {getInitials(e.studentName)}
                            </div>
                            <span className="text-white">{e.studentName}</span>
                          </div>
                        </td>
                        <td className="px-5 py-3.5 text-cef-text">{e.studentEmail}</td>
                        <td className="px-5 py-3.5 text-cef-text">{e.courseTitle}</td>
                        <td className="px-5 py-3.5 text-cef-muted">{formatDate(e.enrolledAt)}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )
        )}

      </main>
    </div>
  )
}
