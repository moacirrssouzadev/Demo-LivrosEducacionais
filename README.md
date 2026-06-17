# Demo Livros Educacionais

Um sistema completo para gerenciamento de livros educacionais, com API REST, interface web e suporte a histórico de versões.


## 📋 Sobre o Projeto

Este projeto é um sistema de gerenciamento de livros educacionais que permite:
- Criar, editar, listar e excluir livros
- Visualizar histórico de versões de cada livro
- Upload de capas de livros usando Azure Blob Storage
- Cache de dados com Redis para melhor performance
- Arquitetura em camadas seguindo princípios de DDD e CQRS


## 🏗️ Arquitetura

O projeto é dividido em duas partes principais:

### 1. LivrosEducacionaisApi (API REST)
Estrutura em Clean Architecture:
- **Domain**: Entidades e interfaces de repositórios
- **Application**: Lógica de negócio, comandos, queries e validadores (MediatR)
- **Infrastructure**: Implementação de repositórios, serviços externos e banco de dados (EF Core)
- **Api**: Controllers, Swagger e configuração do pipeline HTTP

### 2. LivrosEducacionaisWeb (Aplicação Web MVC)
Interface de usuário para interação com o sistema, consumindo a API.

### Tecnologias Utilizadas
- .NET 8+
- ASP.NET Core Web API
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Redis (Cache)
- Azure Blob Storage (Armazenamento de arquivos)
- Serilog (Logging)
- Docker e Docker Compose
- MediatR (CQRS)
- FluentValidation


## 🚀 Como Executar

### Pré-requisitos
- .NET 8 SDK
- Docker e Docker Compose
- Conta no Azure Blob Storage (opcional, pode usar o local para desenvolvimento)


### Passo a Passo com Docker Compose

1. Clone o repositório
   ```bash
   git clone <url-do-repositorio>
   cd Demo-LivrosEducacionais
   ```

2. Verifique o arquivo `.env` (já configurado)
   - Contém as credenciais do SQL Server
   - String de conexão do Azure Blob Storage
   - Configurações da API

3. Execute o Docker Compose
   ```bash
   docker-compose up -d --build
   ```

4. Acesse as aplicações:
   - **Web App**: http://localhost:5001
   - **API (Swagger)**: http://localhost:5000/swagger


### Sem Docker

1. Configure o SQL Server e Redis localmente
2. Atualize as strings de conexão no `appsettings.json`
3. Execute as migrações:
   ```bash
   cd LivrosEducacionaisApi/src/LivrosEducacionaisApi.Infrastructure
   dotnet ef database update --startup-project ../LivrosEducacionaisApi.Api
   ```
4. Execute a API:
   ```bash
   cd LivrosEducacionaisApi/src/LivrosEducacionaisApi.Api
   dotnet run
   ```
5. Execute a Web App:
   ```bash
   cd LivrosEducacionaisWeb/src
   dotnet run
   ```


## 📦 Estrutura de Pastas

```
Demo-LivrosEducacionais/
├── Docs/                          # Documentação adicional
├── LivrosEducacionaisApi/         # API Backend
│   ├── src/
│   │   ├── LivrosEducacionaisApi.Api/
│   │   ├── LivrosEducacionaisApi.Application/
│   │   ├── LivrosEducacionaisApi.Domain/
│   │   └── LivrosEducacionaisApi.Infrastructure/
│   └── tests/                     # Testes unitários e de integração
├── LivrosEducacionaisWeb/         # Aplicação Web
│   └── src/
├── docker-compose.yml
├── .env
└── README.md
```


## 🔧 Funcionalidades Principais

### 1. Gerenciamento de Livros
- **Criar Livro**: Adicione novos livros com título, autor, assunto e descrição
- **Editar Livro**: Atualize informações do livro (cria automaticamente uma nova versão)
- **Listar Livros**: Visualize todos os livros em uma lista paginada
- **Excluir Livro**: Remova livros do sistema (soft delete)
- **Upload de Capa**: Adicione capas aos livros usando Azure Blob Storage

### 2. Histórico de Versões
- Visualize todas as versões anteriores de um livro
- Mantenha um registro completo das alterações


## 🧪 Testes

O projeto inclui testes unitários e de integração:

```bash
# Executar testes unitários
cd LivrosEducacionaisApi/tests/LivrosEducacionaisApi.UnitTests
dotnet test

# Executar testes de integração
cd LivrosEducacionaisApi/tests/LivrosEducacionaisApi.IntegrationTests
dotnet test
```


## 📄 Licença

Este projeto está sob a licença MIT - veja o arquivo LICENSE para detalhes.


## 👥 Autor

Projeto criado para demonstração de boas práticas de desenvolvimento com .NET.
