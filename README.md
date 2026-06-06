# Desafio Técnico - Plataforma de Cursos CEFOPE

Esta aplicação foi desenvolvida para um desafio técnico com o objetivo de gerenciar cursos e inscrições de alunos, utilizando JWT como autenticação, controle de acesso por perfil e persistência em PostgreSQL.

## Tecnologias Utilizadas

- **Frontend:** React 18 · Vite · Tailwind CSS · React Router DOM
- **Backend:** .NET 10 · ASP.NET Core Web API · Entity Framework Core
- **Banco de Dados:** PostgreSQL 16
- **Autenticação:** JWT Bearer · BCrypt
- **Container:** Docker

---

## Execução com Docker (Recomendado)

O Docker foi adotado para garantir que a aplicação rode sem erros independente da máquina utilizada, evitando problemas de ambiente, versão de dependências ou configuração local.

**Pré-requisito:** Instalar o [Docker Desktop](https://www.docker.com/products/docker-desktop/) e deixá-lo em execução.

```bash
git clone <url-do-repositorio>
cd CursosPlatform
docker compose up --build
```

Após o build finalizar, você poderá acessar as páginas em:

- **Frontend:** http://localhost:5173
- **API:** http://localhost:5174

O banco de dados, as tabelas e os dados de seed são criados automaticamente na primeira execução, pois a ORM (Entity Framework Core) faz isso automaticamente.

```bash
# Parar os serviços
docker compose down

# Parar e remover os dados do banco
docker compose down -v
```

---

## Execução Manual (Sem Docker)

**Pré-requisitos:** .NET 10 SDK · Node.js 20+ · PostgreSQL 16

### 1. Banco de dados

Crie o banco no PostgreSQL:

```sql
CREATE DATABASE desafio_tecnico_CEFOPE;
```

Ajuste as credenciais em `appsettings.json` se necessário.

### 2. API

```bash
# Na raiz do projeto
dotnet run
```

### 3. Frontend

```bash
cd app
npm install
npm run dev
```

---

## Credenciais de Teste

| Perfil  | E-mail           | Senha    |
|---------|------------------|----------|
| Admin   | admin@curso.com  | admin123 |
| Student | aluno@curso.com  | aluno123 |

---

## Script SQL das Tabelas

O script abaixo representa a modelagem do banco utilizada antes da ORM assumir a criação automática:

```sql
CREATE TABLE users (
    id          SERIAL PRIMARY KEY,
    name        VARCHAR(150)  NOT NULL,
    email       VARCHAR(255)  UNIQUE NOT NULL,
    password    VARCHAR(255)  NOT NULL,
    role        VARCHAR(20)   NOT NULL,
    created_at  TIMESTAMP     DEFAULT NOW()
);

CREATE TABLE courses (
    id          SERIAL PRIMARY KEY,
    title       VARCHAR(200)  NOT NULL,
    description TEXT,
    workload    INTEGER       NOT NULL,
    created_at  TIMESTAMP     DEFAULT NOW()
);

CREATE TABLE enrollments (
    id          SERIAL PRIMARY KEY,
    user_id     INTEGER       NOT NULL REFERENCES users(id),
    course_id   INTEGER       NOT NULL REFERENCES courses(id),
    created_at  TIMESTAMP     DEFAULT NOW(),
    UNIQUE (user_id, course_id)
);
```

---

## Endpoints da API

| Método | Rota                        | Acesso      |
|--------|-----------------------------|-------------|
| POST   | `/auth/login`               | Público     |
| GET    | `/courses`                  | Autenticado |
| POST   | `/courses`                  | Admin       |
| POST   | `/courses/{id}/enrollments` | Student     |
| GET    | `/enrollments`              | Admin       |
| GET    | `/enrollments/mine`         | Student     |
