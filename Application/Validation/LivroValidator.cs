namespace Application.Validation
{
    public class LivroValidator
    {
        public static void Validate(string titulo, string autor)
        {
            if (string.IsNullOrEmpty(titulo))
                throw new Exception("O título do livro é obrigatório.");

            if (string.IsNullOrEmpty(autor))
                throw new Exception("O nome do autor é obrigatório.");
        }
    }
}
