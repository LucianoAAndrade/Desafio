using Application.Handlers.LivrosHendler;
using Application.Requests.Livros;
using AutoMapper;
using Domain.Interfeces;
using Domain.Models;
using Moq;

namespace TestProject
{
    public class LivrosTests
    {
        #region Mocks
        private List<LivroModel> livros = new List<LivroModel>
        {
            new LivroModel ( "O Senhor dos Anéis", "J.R.R. Tolkien", 5 ),
            new LivroModel ("Harry Potter", "J.K. Rowling", 10 )
        };
        private LivroModel livro = new LivroModel("O Senhor dos Anéis", "J.R.R. Tolkien", 5);
        private GetLivroIdRequest requestId = new GetLivroIdRequest { Id = 5 };
        private GetAllLivrosRequest requestAll = new GetAllLivrosRequest();
        #endregion

        #region Testes para GetAllLivrosHandler
        [Fact]
        public async Task Handle_DeveRetornarListaDeLivros_QuandoLivrosExistem()
        {
            // Arrange
            var mockLivroRepository = new Mock<ILivroRepository>();
            mockLivroRepository.Setup(repo => repo.GetAll()).Returns(livros.AsQueryable());
            var handler = new GetAllLivrosHandler(mockLivroRepository.Object);
            var cancellationToken = new CancellationToken();

            // Act
            var resultado = await handler.Handle(requestAll, cancellationToken);

            // Assert
            Assert.True(resultado.Sucesso);
            Assert.Equal(livros.Count, resultado.Modelo.Count);
            Assert.Equal(livros[0].Titulo, resultado.Modelo[0].Titulo);
            Assert.Equal(livros[1].Autor, resultado.Modelo[1].Autor);
            mockLivroRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveRetornarErro_QuandoNenhumLivroEhEncontrado()
        {
            // Arrange
            var mockLivroRepository = new Mock<ILivroRepository>();
            var livros = new List<LivroModel>();
            mockLivroRepository.Setup(repo => repo.GetAll()).Returns(livros.AsQueryable());
            var handler = new GetAllLivrosHandler(mockLivroRepository.Object);
            var cancellationToken = new CancellationToken();

            // Act
            var resultado = await handler.Handle(requestAll, cancellationToken);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal("Nenhum livro encontrado", resultado.MensagemPrincipal);
            Assert.Null(resultado.Modelo);
            mockLivroRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveRetornarErro_QuandoOcorrerUmaExcecao()
        {
            // Arrange
            var mockLivroRepository = new Mock<ILivroRepository>();
            mockLivroRepository.Setup(repo => repo.GetAll()).Throws(new Exception("Erro de conexão com o banco de dados."));
            var handler = new GetAllLivrosHandler(mockLivroRepository.Object);
            var cancellationToken = new CancellationToken();

            // Act
            var resultado = await handler.Handle(requestAll, cancellationToken);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal("Erro de conexão com o banco de dados.", resultado.MensagemPrincipal);
            Assert.Null(resultado.Modelo);
            mockLivroRepository.Verify(repo => repo.GetAll(), Times.Once);
        }
        #endregion

        #region Testes para GetLivroIdHandler
        [Fact]
        public async Task Handle_DeveRetornarLivro_QuandoIdEhValido()
        {
            // Arrange
            var mockLivroRepository = new Mock<ILivroRepository>();

            mockLivroRepository.Setup(repo => repo.GetById(requestId.Id)).Returns(livro);
            var handler = new GetLivroIdHandler(mockLivroRepository.Object);

            // Act
            var resultado = await handler.Handle(requestId, CancellationToken.None);

            // Assert
            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Modelo);
            Assert.Equal(livro.Titulo, resultado.Modelo.Titulo);
            Assert.Equal("Livro encontrado com sucesso", resultado.MensagemPrincipal);
            mockLivroRepository.Verify(repo => repo.GetById(requestId.Id), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveRetornarErro_QuandoLivroNaoEhEncontrado()
        {
            // Arrange
            var mockLivroRepository = new Mock<ILivroRepository>();

            mockLivroRepository.Setup(repo => repo.GetById(requestId.Id)).Returns((LivroModel)null);
            var handler = new GetLivroIdHandler(mockLivroRepository.Object);

            // Act
            var resultado = await handler.Handle(requestId, CancellationToken.None);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Null(resultado.Modelo);
            Assert.Equal("Livro não encontrado", resultado.MensagemPrincipal);
            mockLivroRepository.Verify(repo => repo.GetById(requestId.Id), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveRetornarErro_QuandoOcorrerUmaExcecaoNoRepositorio()
        {
            // Arrange
            var mockLivroRepository = new Mock<ILivroRepository>();

            mockLivroRepository.Setup(repo => repo.GetById(requestId.Id)).Throws(new Exception("Erro de conexão com o banco de dados."));
            var handler = new GetLivroIdHandler(mockLivroRepository.Object);

            // Act
            var resultado = await handler.Handle(requestId, CancellationToken.None);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Null(resultado.Modelo);
            Assert.Equal("Erro de conexão com o banco de dados.", resultado.MensagemPrincipal);
            mockLivroRepository.Verify(repo => repo.GetById(requestId.Id), Times.Once);
        }
        #endregion

        #region Testes para PostLivroHandler
        [Fact]
        public async Task Handle_DeveAdicionarLivro_QuandoDadosSaoValidos()
        {
            // Arrange
            var mockLivroRepository = new Mock<ILivroRepository>();
            var mockMapper = new Mock<IMapper>();

            var request = new PostLivroRequest
            {
                Titulo = "O Pequeno Príncipe",
                Autor = "Antoine de Saint-Exupéry",
                QuantidadeDisponivel = 5
            };

            var handler = new PostLivroHandler(mockLivroRepository.Object, mockMapper.Object);

            // Act
            var resultado = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(resultado.Sucesso);
            Assert.Equal("Livro cadastrado com sucesso", resultado.MensagemPrincipal);

            // Verifica se os métodos do repositório foram chamados corretamente
            // Capture o objeto LivroModel para verificar os dados
            mockLivroRepository.Verify(repo => repo.Add(It.Is<LivroModel>(l =>
                l.Titulo == request.Titulo &&
                l.Autor == request.Autor &&
                l.QuantidadeDisponivel == request.QuantidadeDisponivel
            )), Times.Once);

            mockLivroRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Theory]
        [InlineData(null, "Autor Teste", "O título do livro é obrigatório.")]
        [InlineData("", "Autor Teste", "O título do livro é obrigatório.")]
        [InlineData("Título Teste", null, "O nome do autor é obrigatório.")]
        [InlineData("Título Teste", "", "O nome do autor é obrigatório.")]
        public async Task Handle_DeveRetornarErro_QuandoDadosSaoInvalidos(string titulo, string autor, string mensagemEsperada)
        {
            // Arrange
            var mockLivroRepository = new Mock<ILivroRepository>();
            var mockMapper = new Mock<IMapper>();

            var request = new PostLivroRequest
            {
                Titulo = titulo,
                Autor = autor,
                QuantidadeDisponivel = 1
            };

            var handler = new PostLivroHandler(mockLivroRepository.Object, mockMapper.Object);

            // Act
            var resultado = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal(mensagemEsperada, resultado.MensagemPrincipal);

            // Verifica se os métodos do repositório NÃO foram chamados
            mockLivroRepository.Verify(repo => repo.Add(It.IsAny<LivroModel>()), Times.Never);
            mockLivroRepository.Verify(repo => repo.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveRetornarErro_QuandoOcorrerUmaExcecaoNoRepositorioPost()
        {
            // Arrange
            var mockLivroRepository = new Mock<ILivroRepository>();
            var mockMapper = new Mock<IMapper>();

            var request = new PostLivroRequest
            {
                Titulo = "Livro Válido",
                Autor = "Autor Válido",
                QuantidadeDisponivel = 10
            };

            mockLivroRepository.Setup(repo => repo.Add(It.IsAny<LivroModel>()))
                .Throws(new Exception("Erro de conexão com o banco de dados."));

            var handler = new PostLivroHandler(mockLivroRepository.Object, mockMapper.Object);

            // Act
            var resultado = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal("Erro de conexão com o banco de dados.", resultado.MensagemPrincipal);

            // Verifica se o método Add foi chamado, mas SaveChanges não
            mockLivroRepository.Verify(repo => repo.Add(It.IsAny<LivroModel>()), Times.Once);
            mockLivroRepository.Verify(repo => repo.SaveChanges(), Times.Never);
        }
        #endregion
    }
}
