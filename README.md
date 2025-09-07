# DesafioOpea API

## Visão Geral

**DesafioOpea** é uma API Web desenvolvida em **.NET 8** para gerenciamento de livros e empréstimos em um sistema de biblioteca.  
O projeto utiliza **Entity Framework Core (SQLite)**, **MediatR** para CQRS, **AutoMapper** e segue princípios de **Arquitetura Limpa**.

---

## Funcionalidades

### Livros
- Listar todos os livros
- Buscar livro por ID
- Cadastrar novo livro

### Empréstimos
- Listar todos os empréstimos
- Realizar um novo empréstimo
- Devolver um livro (atualizar empréstimo)

### Tratamento de Erros
- Respostas consistentes de erro
- Validação de regras de negócio

### Testes Unitários
- Handlers e regras de negócio cobertos por testes unitários (xUnit + Moq)

---

## Tecnologias Utilizadas
- .NET 8  
- C# 12  
- Entity Framework Core (SQLite)  
- MediatR  
- AutoMapper  
- xUnit & Moq (testes)  

---

## Como Executar

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download)  
- [Visual Studio 2022+](https://visualstudio.microsoft.com/)  

### Banco de Dados
O projeto utiliza **SQLite**, já incluso na pasta do projeto.  
Ao rodar a aplicação, os dados serão persistidos automaticamente nesse banco.

### Executando a Aplicação
No **Visual Studio**, defina o projeto de inicialização e pressione **F5**.  
O **Swagger** será aberto automaticamente para testar os endpoints.  

Ou via CLI:
```bash
dotnet run --project DesafioOpea
```

### Executando os Testes
```bash
dotnet test
```

---

## Endpoints da API

### Livros
- `GET /api/livro` → Lista todos os livros  
- `GET /api/livro/{id}` → Busca livro por ID  
- `POST /api/livro` → Cadastra novo livro  

**Exemplo JSON:**
```json
{
  "titulo": "Exemplo de Livro",
  "autor": "Autor Teste",
  "quantidadeDisponivel": 5
}
```

---

### Empréstimos
- `GET /api/emprestimo` → Lista todos os empréstimos  
- `POST /api/emprestimo` → Realiza um novo empréstimo  

**Exemplo JSON:**
```json
{
  "idLivro": 1,
  "dataDevolucao": "2025-09-07T20:08:44.732Z"
}
```

- `PUT /api/emprestimo` → Devolve um livro (atualiza empréstimo)  

**Exemplo JSON:**
```json
{
  "idEmprestimo": 1,
  "dataDevolucao": "2025-09-07T20:08:44.732Z"
}
```

---

## Estrutura do Projeto

```
DesafioOpea/
 ├── Controllers/        # Controllers da API
 ├── Application/
 │    └── Handlers/      # Handlers CQRS
 ├── Domain/
 │    └── Models/        # Entidades de domínio
 ├── Infra.Data/         # Acesso a dados e contexto EF Core
 └── TestProject/        # Testes unitários
```
