using System.Diagnostics;

namespace Domain.Utils.OperationResult
{
    public static class ResultOperationValidateUtil
    {
        public static bool IsNull(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        public static List<string> ToListString(this Exception ex, params string[] mensageAdicional)
        {
            List<string> list = new List<string>();
            list.Add("-------------------------");
            if (mensageAdicional != null && mensageAdicional.Length != 0)
            {
                list.AddRange(mensageAdicional);
            }

            list.AddRange(ToListString(ex, inicial: false));
            list.Add("-------------------------");
            return list;
        }

        private static List<string> ToListString(Exception ex, bool inicial)
        {
            List<string> list = new List<string>();
            if (ex == null)
            {
                return list;
            }

            if (inicial)
            {
                list.Add("-------------------------");
            }

            list.Add("Ex.Message: " + ex.Message);
            list.Add("Ex.StackTrace: " + ex.StackTrace + " - " + ObterLinhaStackTrace(ex));
            if (ex.InnerException != null)
            {
                list.Add("----- Inner Exception -----");
                list.AddRange(ToListString(ex.InnerException));
            }

            if (inicial)
            {
                list.Add("-------------------------");
            }

            return list;
        }
        private static string ObterLinhaStackTrace(Exception ex)
        {
            try
            {
                if (IsNull(ex.StackTrace))
                {
                    return "Linha do Erro: não definida.";
                }

                return "Linha do erro: " + new StackTrace(ex, fNeedFileInfo: true).GetFrame(0)!.GetFileLineNumber();
            }
            catch
            {
                return "Linha do Erro: não definida.";
            }
        }
    }
}