using CursosPlatform.api.Models;

namespace CursosPlatform.api.Data;

public static class DataSeeder
{
    public static void Seed(AppDbContext context)
    {
        // Executa o seed apenas se não houver nenhum usuário cadastrado
        if (context.Users.Any()) return;

        context.Users.AddRange(
            new User
            {
                Name        = "Administrador",
                Email       = "admin@curso.com",
                // HashPassword gera um hash BCrypt seguro com salt automático
                Password    = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role        = "admin",
                CreatedAt   = DateTime.UtcNow
            },
            new User
            {
                Name        = "Aluno Teste",
                Email       = "aluno@curso.com",
                Password    = BCrypt.Net.BCrypt.HashPassword("aluno123"),
                Role        = "student",
                CreatedAt   = DateTime.UtcNow
            }
        );

        // Cursos baseados nas Diretrizes Operacionais da Formação Continuada 2026 (Cefope/Sedu-ES)
        context.Courses.AddRange(
            new Course
            {
                Title       = "Trilha de Desenvolvimento Profissional Docente (TDPD)",
                Description = "Formação continuada para professores de todos os componentes curriculares, exceto Matemática e Língua Portuguesa. Organizada em três tópicos progressivos: Planejamento, Currículo e Avaliação; Inovação Didático-Pedagógica; e Equidade e Inclusão. Integra a Formação dos Profissionais do Magistério (FPM) com encontros presenciais nas escolas.",
                Workload    = 120,
                CreatedAt   = DateTime.UtcNow
            },
            new Course
            {
                Title       = "Trilha de Desenvolvimento Profissional da Recomposição das Aprendizagens (TDPRA)",
                Description = "Formação específica para professores de Matemática e Língua Portuguesa, no âmbito da Política Estadual de Recomposição das Aprendizagens. Aborda: Intencionalidade Pedagógica e Análise das Avaliações; Práticas Didático-Pedagógicas Inovadoras; e Monitoramento das Aprendizagens para Práticas Pedagógicas Inclusivas e orientadas para a Equidade.",
                Workload    = 120,
                CreatedAt   = DateTime.UtcNow
            },
            new Course
            {
                Title       = "Trilha de Desenvolvimento Profissional para Gestores (TDPG)",
                Description = "Formação destinada ao trio gestor escolar — Diretores, Coordenadores Pedagógicos (CP) e Coordenadores Administrativos, de Secretaria e Financeiro (CASF). Contempla os tópicos: Gestão Integrada e Liderança Compartilhada; e Gestão para a Aprendizagem. Inclui oficinas formativas síncronas mediadas por formadores contratados.",
                Workload    = 120,
                CreatedAt   = DateTime.UtcNow
            },
            new Course
            {
                Title       = "Trilha de Desenvolvimento Profissional para Pedagogos (TDPP)",
                Description = "Formação voltada ao fortalecimento da atuação pedagógica dos pedagogos da rede. Organizada em três tópicos: Currículo e Planejamento; Gestão Pedagógica para Aprendizagem; e Práticas Pedagógicas, Mediação e Monitoramento da Aprendizagem. Inclui atuação articulada com a Coordenação Pedagógica durante a FPM.",
                Workload    = 120,
                CreatedAt   = DateTime.UtcNow
            },
            new Course
            {
                Title       = "Formações para Políticas Educacionais (FPE)",
                Description = "Formações destinadas ao aprofundamento de conhecimentos sobre programas e políticas educacionais institucionais, contemplando temáticas como Tempo Integral, Escola do Futuro, Educação de Jovens e Adultos (EJA) e demais programas em implementação pela Secretaria da Educação do Espírito Santo. Oferta em períodos determinados com inscrições via SGF.",
                Workload    = 40,
                CreatedAt   = DateTime.UtcNow
            }
        );

        context.SaveChanges();
    }
}
