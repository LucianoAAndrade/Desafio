namespace Application.Responses.Livros
{
    public class GetAllLivrosResponse
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public short QuantidadeDisponivel { get; set; }
    }
}
