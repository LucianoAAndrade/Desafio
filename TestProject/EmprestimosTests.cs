using Application.Handlers;
using Application.Requests.Emprestimo;
using Domain.Enums;
using Domain.Interfeces;
using Domain.Models;
using Moq;

namespace TestProject
{
    public class EmprestimosTests
    {
        #region Mocks
        private LivroModel livro = new LivroModel
        (
            "Duna",
            "Frank Herbert",
            9
        );

        private PostEmprestimoRequest request = new PostEmprestimoRequest
        {
            IdLivro = 5,
            DataDevolucao = DateTime.Now.AddDays(7)
        };

        private PutEmprestimoRequest requestPut = new PutEmprestimoRequest
        {
            IdEmprestimo = 9,
            DataDevolucao = DateTime.Now.AddDays(15)
        };
        #endregion

        #region Todos os emprestimos
        [Fact]
        public async Task Handle_DeveRetornarListaDeEmprestimos_QuandoExistemEmprestimos()
        {
            // Arrange
            var mockEmprestimoRepository = new Mock<IEmprestimoRepository>();

            var emprestimos = new List<EmprestimoModel>
            {
                new EmprestimoModel
                (
                    3,
                    DateTime.Now.AddDays(-10),
                    DateTime.Now.AddDays(-5),
                    StatusLivroEnum.Ativo,
                    new LivroModel("Duna", "Frank Herbert", 5)
                ),
                new EmprestimoModel
                (
                    8,
                    DateTime.Now.AddDays(-20),
                    DateTime.Now.AddDays(-15),
                    StatusLivroEnum.Devolvido,
                    new LivroModel("1984", "George Orwell", 3)
                )
            };

            mockEmprestimoRepository.Setup(repo => repo.GetAllEmprestimos()).Returns(emprestimos.AsQueryable());
            var handler = new GetAllEmprestimosHandler(mockEmprestimoRepository.Object);
            var request = new GetAllEmprestimosRequest();

            // Act
            var resultado = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(resultado.Sucesso);
            Assert.Equal(emprestimos.Count, resultado.Modelo.Count);
            Assert.Equal(emprestimos[0].Livro.Titulo, resultado.Modelo[0].TituloLivro);
            Assert.Equal("Emprestado", resultado.Modelo[0].Status);
            Assert.Equal(emprestimos[1].Livro.Titulo, resultado.Modelo[1].TituloLivro);
            Assert.Equal("Devolvido", resultado.Modelo[1].Status);
            mockEmprestimoRepository.Verify(repo => repo.GetAllEmprestimos(), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveRetornarErro_QuandoNenhumEmprestimoEhEncontrado()
        {
            // Arrange
            var mockEmprestimoRepository = new Mock<IEmprestimoRepository>();
            var emprestimos = new List<EmprestimoModel>();
            mockEmprestimoRepository.Setup(repo => repo.GetAllEmprestimos()).Returns(emprestimos.AsQueryable());
            var handler = new GetAllEmprestimosHandler(mockEmprestimoRepository.Object);
            var request = new GetAllEmprestimosRequest();

            // Act & Assert
            // O código original lança a exceção, então o teste deve capturá-la.
            var exception = await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            Assert.Equal("Nenhum empréstimo encontrado", exception.Message);

            mockEmprestimoRepository.Verify(repo => repo.GetAllEmprestimos(), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoOcorrerErroNoRepositorio()
        {
            // Arrange
            var mockEmprestimoRepository = new Mock<IEmprestimoRepository>();
            mockEmprestimoRepository.Setup(repo => repo.GetAllEmprestimos())
                                    .Throws(new Exception("Erro de rede."));
            var handler = new GetAllEmprestimosHandler(mockEmprestimoRepository.Object);
            var request = new GetAllEmprestimosRequest();

            // Act & Assert
            // O código original lança a exceção, então o teste deve capturá-la.
            var exception = await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            Assert.Equal("Erro de rede.", exception.Message);

            mockEmprestimoRepository.Verify(repo => repo.GetAllEmprestimos(), Times.Once);
        }
        #endregion

        #region Fazer um emprestimo

        [Fact]
        public async Task Handle_DeveRealizarEmprestimo_QuandoLivroEstaDisponivel()
        {
            // Arrange
            var mockEmprestimoRepository = new Mock<IEmprestimoRepository>();
            var mockLivroRepository = new Mock<ILivroRepository>();

            mockLivroRepository.Setup(repo => repo.GetById(request.IdLivro)).Returns(livro);
            mockEmprestimoRepository.Setup(repo => repo.GetByIdLivro(request.IdLivro)).Returns((EmprestimoModel)null);

            var handler = new PostEmprestimoHandler(mockEmprestimoRepository.Object, mockLivroRepository.Object);

            // Act
            var resultado = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(resultado.Sucesso);
            Assert.Equal("Empréstimo realizado com sucesso", resultado.MensagemPrincipal);
            Assert.Equal(8, livro.QuantidadeDisponivel);

            mockEmprestimoRepository.Verify(repo => repo.Add(It.IsAny<EmprestimoModel>()), Times.Once);
            mockEmprestimoRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoLivroNaoEstaDisponivel()
        {
            // Arrange
            var mockEmprestimoRepository = new Mock<IEmprestimoRepository>();
            var mockLivroRepository = new Mock<ILivroRepository>();
            livro = new LivroModel
            (
                "Duna",
                "Frank Herbert",
                0
            );

            // Simula o repositório para retornar o livro, mas com quantidade indisponível
            mockLivroRepository.Setup(repo => repo.GetById(request.IdLivro)).Returns(livro);
            mockEmprestimoRepository.Setup(repo => repo.GetByIdLivro(request.IdLivro)).Returns((EmprestimoModel)null);

            var handler = new PostEmprestimoHandler(mockEmprestimoRepository.Object, mockLivroRepository.Object);

            // Act
            var resultado = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal("Empréstimo não pode ser realizado", resultado.MensagemPrincipal);
            Assert.Equal(0, livro.QuantidadeDisponivel);

            mockEmprestimoRepository.Verify(repo => repo.Add(It.IsAny<EmprestimoModel>()), Times.Never);
            mockEmprestimoRepository.Verify(repo => repo.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoOcorrerErroNoRepositorioPost()
        {
            // Arrange
            var mockEmprestimoRepository = new Mock<IEmprestimoRepository>();
            var request = new PutEmprestimoRequest();
            mockEmprestimoRepository.Setup(repo => repo.GetById(request.IdEmprestimo)).Returns((EmprestimoModel)null);
            var handler = new PutEmprestimoHandler(mockEmprestimoRepository.Object);


            // Act
            var resultado = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal("Empréstimo não encontrado", resultado.MensagemPrincipal);

            mockEmprestimoRepository.Verify(repo => repo.Add(It.IsAny<EmprestimoModel>()), Times.Never);
            mockEmprestimoRepository.Verify(repo => repo.SaveChanges(), Times.Never);
        }
        #endregion

        #region Alterar emprestimo
        [Fact]
        public async Task Handle_DeveAtualizarEmprestimoParaDevolvido_QuandoEmprestimoExiste()
        {
            // Arrange
            var mockEmprestimoRepository = new Mock<IEmprestimoRepository>();
            var livroId = 1;

            var emprestimoExistente = new EmprestimoModel
            (
                livroId,
                DateTime.Now.AddDays(-7),
                DateTime.Now.AddDays(-3),
                StatusLivroEnum.Ativo,
                livro
            );

            mockEmprestimoRepository.Setup(repo => repo.GetById(requestPut.IdEmprestimo)).Returns(emprestimoExistente);

            var handler = new PutEmprestimoHandler(mockEmprestimoRepository.Object);

            // Act
            var resultado = await handler.Handle(requestPut, CancellationToken.None);

            // Assert
            Assert.True(resultado.Sucesso);
            Assert.Equal("Devolução realizada com sucesso", resultado.MensagemPrincipal);

            // Verifica se a quantidade de livros foi incrementada
            Assert.Equal(10, emprestimoExistente.Livro.QuantidadeDisponivel);

            // Verifica se os métodos do repositório foram chamados
            mockEmprestimoRepository.Verify(repo => repo.Update(It.IsAny<EmprestimoModel>()), Times.Once);
            mockEmprestimoRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoEmprestimoNaoEhEncontrado()
        {
            // Arrange
            var mockEmprestimoRepository = new Mock<IEmprestimoRepository>();

            mockEmprestimoRepository.Setup(repo => repo.GetById(requestPut.IdEmprestimo)).Returns((EmprestimoModel)null);

            var handler = new PutEmprestimoHandler(mockEmprestimoRepository.Object);

            // Act
            var resultado = await handler.Handle(requestPut, CancellationToken.None);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal("Empréstimo não encontrado", resultado.MensagemPrincipal);

            mockEmprestimoRepository.Verify(repo => repo.Add(It.IsAny<EmprestimoModel>()), Times.Never);
            mockEmprestimoRepository.Verify(repo => repo.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoEmprestimoJaFoiDevolvido()
        {
            // Arrange
            var mockEmprestimoRepository = new Mock<IEmprestimoRepository>();

            var emprestimoDevolvido = new EmprestimoModel
            (
                5,
                DateTime.Now.AddDays(-10),
                StatusLivroEnum.Devolvido,
                livro
            );

            mockEmprestimoRepository.Setup(repo => repo.GetById(requestPut.IdEmprestimo)).Returns(emprestimoDevolvido);

            var handler = new PutEmprestimoHandler(mockEmprestimoRepository.Object);

            // Act
            var resultado = await handler.Handle(requestPut, CancellationToken.None);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal("Empréstimo não encontrado", resultado.MensagemPrincipal);

            mockEmprestimoRepository.Verify(repo => repo.Add(It.IsAny<EmprestimoModel>()), Times.Never);
            mockEmprestimoRepository.Verify(repo => repo.SaveChanges(), Times.Never);
        }
        #endregion
    }
}
