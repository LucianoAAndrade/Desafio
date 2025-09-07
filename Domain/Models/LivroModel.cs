namespace Domain.Models
{
    public class LivroModel : EntityModel
    {
        public string? Titulo { get; set; }
        public string? Autor { get; set; }
        public short QuantidadeDisponivel { get; set; }
        private LivroModel() { }

        public LivroModel(string titulo, string autor, short quantidadeDisponivel)
        {
            Titulo = titulo;
            Autor = autor;
            QuantidadeDisponivel = quantidadeDisponivel;
        }
        public LivroModel(short quantidadeDisponivel)
        {
            QuantidadeDisponivel = quantidadeDisponivel;
        }
    }
}