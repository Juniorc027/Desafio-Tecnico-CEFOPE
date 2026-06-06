import { useEffect, useState } from 'react'
import Navbar from '../components/Navbar'
import api    from '../services/api'

function formatDate(dateStr) {
  return new Date(dateStr).toLocaleDateString('pt-BR', {
    day: '2-digit', month: '2-digit', year: 'numeric'
  })
}

export default function MyEnrollments() {
  const [enrollments, setEnrollments] = useState([])
  const [loading,     setLoading]     = useState(true)

  useEffect(() => {
    api.get('/enrollments/mine')
      .then(res => setEnrollments(res.data))
      .catch(() => {})
      .finally(() => setLoading(false))
  }, [])

  return (
    <div className="min-h-screen bg-cef-bg">
      <Navbar />

      <main className="max-w-6xl mx-auto px-6 py-8">

        {/* Cabeçalho */}
        <div className="mb-8">
          <h2 className="text-2xl font-bold text-white">Minhas Inscrições</h2>
          <p className="text-cef-muted text-sm mt-1">
            {enrollments.length > 0
              ? `Você está inscrito em ${enrollments.length} curso${enrollments.length > 1 ? 's' : ''}.`
              : 'Seus cursos inscritos aparecerão aqui.'}
          </p>
        </div>

        {/* Loading */}
        {loading && (
          <div className="flex justify-center py-20">
            <div className="w-8 h-8 border-2 border-cef-primary border-t-transparent rounded-full animate-spin" />
          </div>
        )}

        {/* Lista de cards */}
        {!loading && (
          enrollments.length === 0
            ? (
              <div className="text-center py-20">
                <div className="inline-flex w-16 h-16 bg-cef-surface rounded-xl items-center justify-center mb-4">
                  <svg className="w-8 h-8 text-cef-muted" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5">
                    <path d="M2 3h6a4 4 0 014 4v14a3 3 0 00-3-3H2z"/>
                    <path d="M22 3h-6a4 4 0 00-4 4v14a3 3 0 013-3h7z"/>
                  </svg>
                </div>
                <p className="text-cef-muted">Você ainda não se inscreveu em nenhum curso.</p>
                <a href="/cursos" className="text-cef-accent text-sm mt-2 inline-block hover:underline">
                  Ver cursos disponíveis →
                </a>
              </div>
            )
            : (
              <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
                {enrollments.map(enrollment => (
                  <div
                    key={enrollment.id}
                    className="bg-cef-surface border-l-4 border-l-cef-success rounded-xl p-5 flex flex-col gap-3"
                  >
                    {/* Badge de inscrito */}
                    <span className="self-start text-xs text-cef-success bg-cef-success/10 px-2.5 py-1 rounded-full font-medium">
                      ✓ Inscrito em {formatDate(enrollment.enrolledAt)}
                    </span>

                    <h3 className="text-white font-semibold text-base">{enrollment.courseTitle}</h3>
                  </div>
                ))}
              </div>
            )
        )}

      </main>
    </div>
  )
}
