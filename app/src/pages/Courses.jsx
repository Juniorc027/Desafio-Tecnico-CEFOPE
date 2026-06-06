import { useEffect, useState } from 'react'
import Navbar     from '../components/Navbar'
import CourseCard from '../components/CourseCard'
import { useAuth } from '../context/AuthContext'
import api        from '../services/api'

export default function Courses() {
  const { user }                      = useAuth()
  const [courses,      setCourses]    = useState([])
  const [enrolledIds,  setEnrolledIds] = useState(new Set())
  const [loading,      setLoading]    = useState(true)
  const [toast,        setToast]      = useState(null) // { type: 'success'|'error', text }

  const isAdmin = user?.role === 'admin'

  useEffect(() => {
    async function loadData() {
      try {
        // Busca os cursos primeiro (obrigatório)
        const coursesRes = await api.get('/courses')
        setCourses(coursesRes.data)

        // Student busca suas inscrições separadamente para marcar os cards
        if (!isAdmin) {
          const enrollmentsRes = await api.get('/enrollments/mine')
          setEnrolledIds(new Set(enrollmentsRes.data.map(e => e.courseId)))
        }
      } catch {
        showToast('error', 'Erro ao carregar dados.')
      } finally {
        setLoading(false)
      }
    }
    loadData()
  }, [isAdmin])

  async function handleEnroll(courseId) {
    try {
      await api.post(`/courses/${courseId}/enrollments`)
      // Atualiza o estado local sem recarregar tudo
      setEnrolledIds(prev => new Set([...prev, courseId]))
      // Incrementa o contador no curso
      setCourses(prev =>
        prev.map(c =>
          c.id === courseId ? { ...c, enrollmentCount: c.enrollmentCount + 1 } : c
        )
      )
      showToast('success', 'Inscrição realizada com sucesso!')
    } catch (err) {
      if (err.response?.status === 400) {
        showToast('error', 'Você já está inscrito neste curso.')
      } else {
        showToast('error', 'Erro ao realizar inscrição.')
      }
    }
  }

  function showToast(type, text) {
    setToast({ type, text })
    setTimeout(() => setToast(null), 3500)
  }

  return (
    <div className="min-h-screen bg-cef-bg">
      <Navbar />

      <main className="max-w-6xl mx-auto px-6 py-8">

        {/* Cabeçalho da página */}
        <div className="mb-8">
          <h2 className="text-2xl font-bold text-white">Cursos disponíveis</h2>
          <p className="text-cef-muted text-sm mt-1">
            {isAdmin ? 'Gerencie os cursos da plataforma.' : 'Escolha um curso e realize sua inscrição.'}
          </p>
        </div>

        {/* Loading */}
        {loading && (
          <div className="flex justify-center py-20">
            <div className="w-8 h-8 border-2 border-cef-primary border-t-transparent rounded-full animate-spin" />
          </div>
        )}

        {/* Grade de cursos */}
        {!loading && (
          courses.length === 0
            ? <p className="text-cef-muted text-center py-16">Nenhum curso cadastrado.</p>
            : (
              <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
                {courses.map(course => (
                  <CourseCard
                    key={course.id}
                    course={course}
                    isEnrolled={enrolledIds.has(course.id)}
                    isAdmin={isAdmin}
                    onEnroll={handleEnroll}
                  />
                ))}
              </div>
            )
        )}
      </main>

      {/* Toast de feedback */}
      {toast && (
        <div className={`
          fixed bottom-6 right-6 px-5 py-3 rounded-lg text-sm font-medium shadow-lg
          transition-all animate-pulse
          ${toast.type === 'success' ? 'bg-cef-success text-white' : 'bg-red-500 text-white'}
        `}>
          {toast.text}
        </div>
      )}
    </div>
  )
}
