export default function CourseCard({ course, isEnrolled = false, onEnroll, isAdmin = false }) {
  return (
    <div
      className={`
        bg-cef-surface rounded-xl p-5 flex flex-col gap-3
        border-l-4 transition-colors
        ${isEnrolled ? 'border-l-cef-success' : 'border-l-cef-accent'}
      `}
    >
      {/* Cabeçalho: carga horária e contador de inscritos */}
      <div className="flex items-center justify-between">
        <span className="flex items-center gap-1.5 text-cef-muted text-xs">
          {/* Ícone relógio */}
          <svg className="w-3.5 h-3.5" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
            <circle cx="12" cy="12" r="10"/>
            <polyline points="12 6 12 12 16 14"/>
          </svg>
          {course.workload}h
        </span>

        <span className="flex items-center gap-1 text-cef-accent text-xs">
          {/* Ícone pessoas */}
          <svg className="w-3.5 h-3.5" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
            <path d="M17 21v-2a4 4 0 00-4-4H5a4 4 0 00-4 4v2"/>
            <circle cx="9" cy="7" r="4"/>
            <path d="M23 21v-2a4 4 0 00-3-3.87M16 3.13a4 4 0 010 7.75"/>
          </svg>
          {course.enrollmentCount ?? 0} inscritos
        </span>
      </div>

      {/* Título */}
      <h3 className="text-white font-semibold text-base leading-snug">{course.title}</h3>

      {/* Descrição */}
      <p className="text-cef-text text-sm leading-relaxed flex-1 line-clamp-3">
        {course.description || 'Sem descrição disponível.'}
      </p>

      {/* Botão de inscrição — visível apenas para Student */}
      {!isAdmin && (
        <button
          onClick={() => !isEnrolled && onEnroll && onEnroll(course.id)}
          disabled={isEnrolled}
          className={`
            mt-1 w-full py-2 rounded-lg text-sm font-medium transition-colors
            ${isEnrolled
              ? 'bg-cef-success/20 text-cef-success cursor-default'
              : 'bg-cef-primary hover:bg-cef-accent text-white'}
          `}
        >
          {isEnrolled ? '✓ Inscrito' : 'Inscrever-se'}
        </button>
      )}
    </div>
  )
}
