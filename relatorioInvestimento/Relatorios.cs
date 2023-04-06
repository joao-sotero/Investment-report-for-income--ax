using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using System.Text;

namespace relatorioInvestimento
{
    public static class Relatorios
    {
        public static IEnumerable<String> PegaTodosOsArquivos(string pathArchive)
        {
            string[] arquivos = Directory.GetFiles(pathArchive);

            List<string> arquivosCsv = new();
            foreach (string arquivo in arquivos)
            {
                if (Path.GetExtension(arquivo).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    arquivosCsv.Add(arquivo);
                }
            }

            return arquivosCsv;
        }

        public static List<NotaNegociacao> AbreArquivo(string pathFile)
        {
            var notas = new List<NotaNegociacao>();

            using TextFieldParser parser = new TextFieldParser(pathFile, Encoding.Latin1);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(";");

            parser.ReadLine();
            parser.ReadLine();

            while (!parser.EndOfData)
            {
                string[] linhaAtual = parser.ReadFields();

                var nota = new NotaNegociacao
                {
                    DataNegociacao = DateTime.ParseExact(linhaAtual[0], "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Conta = linhaAtual[1],
                    Ativo = linhaAtual[2],
                    Preco = decimal.Parse(linhaAtual[3]),
                    QuantidadeCompra = int.Parse(linhaAtual[4]),
                    QuantidadeVenda = int.Parse(linhaAtual[5]),
                    FinanceiroCompra = decimal.Parse(linhaAtual[6]),
                    FinanceiroVenda = decimal.Parse(linhaAtual[7])
                };

                notas.Add(nota);
            }

            return notas;
        }

        public static void CriaCsvCompra(string pathFile)
        {
            var relatorio = typeof(RelatorioCompra).GetProperties();

            string linha = string.Join(";", relatorio.Select(r => r.Name).ToArray());

            using (StreamWriter file = new StreamWriter(pathFile))
            {
                file.WriteLine(linha);
            }
        }

        public static void CriaCsvVenda(string pathFile)
        {
            var relatorio = typeof(RelatorioVenda).GetProperties();

            string linha = string.Join(";", relatorio.Select(r => r.Name).ToArray());

            using (StreamWriter file = new StreamWriter(pathFile))
            {
                file.WriteLine(linha);
            }
        }

        public static void AtualizaCsvCompra(string pathFile, NotaNegociacao nota, int ano)
        {
            List<string> linhas = new List<string>();

            using (StreamReader file = new StreamReader(pathFile))
            {
                string linha;
                while ((linha = file.ReadLine()) != null)
                {
                    linhas.Add(linha);
                }
            }
            if (nota.FinanceiroCompra > 0 && nota.DataNegociacao.Year == ano)
            {
                if (!linhas.Any(l => l.StartsWith(nota.Ativo)) || linhas.Count == 1)
                    linhas.Add($"{nota.Ativo};{nota.QuantidadeCompra};{nota.FinanceiroCompra};{nota.FinanceiroCompra / nota.QuantidadeCompra}");
                else
                {
                    for (int i = 0; i < linhas.Count; i++)
                    {
                        string[] campos = linhas[i].Split(';');
                        if (campos[0] == nota.Ativo)
                        {
                            int quantidade = int.Parse(campos[1]) + nota.QuantidadeCompra;
                            decimal preco = decimal.Parse(campos[2]) + nota.FinanceiroCompra;
                            linhas[i] = $"{nota.Ativo};{quantidade};{preco:F2};{preco / quantidade:F2}";
                        }
                    }
                }

                StringBuilder sb = new StringBuilder();
                foreach (string linha in linhas)
                    sb.AppendLine(linha);

                using (StreamWriter file = new StreamWriter(pathFile))
                {
                    file.Write(sb.ToString());
                }
            }
        }

        public static void AtualizaCsvCompra(string pathFile, NotaNegociacao nota)
        {
            List<string> linhas = new List<string>();

            using (StreamReader file = new StreamReader(pathFile))
            {
                string linha;
                while ((linha = file.ReadLine()) != null)
                {
                    linhas.Add(linha);
                }
            }
            if (nota.FinanceiroCompra > 0)
            {
                if (!linhas.Any(l => l.StartsWith(nota.Ativo)) || linhas.Count == 1)
                    linhas.Add($"{nota.Ativo};{nota.QuantidadeCompra};{nota.FinanceiroCompra};{nota.FinanceiroCompra / nota.QuantidadeCompra}");
                else
                {
                    for (int i = 0; i < linhas.Count; i++)
                    {
                        string[] campos = linhas[i].Split(';');
                        if (campos[0] == nota.Ativo)
                        {
                            int quantidade = int.Parse(campos[1]) + nota.QuantidadeCompra;
                            decimal preco = decimal.Parse(campos[2]) + nota.FinanceiroCompra;
                            linhas[i] = $"{nota.Ativo};{quantidade};{preco:F2};{preco / quantidade:F2}";
                        }
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (string linha in linhas)
                sb.AppendLine(linha);

            using (StreamWriter file = new StreamWriter(pathFile))
            {
                file.Write(sb.ToString());
            }
        }

        public static void AtualizaCsvVenda(string pathFile, NotaNegociacao nota)
        {
            List<string> linhas = new List<string>();

            using (StreamReader file = new StreamReader(pathFile))
            {
                string linha;
                while ((linha = file.ReadLine()) != null)
                {
                    linhas.Add(linha);
                }
            }

            if (nota.FinanceiroVenda > 0)
            {
                if (linhas.Count == 1 || !linhas.Any(l => l.StartsWith(nota.Ativo)))
                    linhas.Add($"{nota.Ativo};{nota.QuantidadeVenda};{nota.Preco};{nota.FinanceiroVenda}");
                else
                {
                    for (int i = 0; i < linhas.Count; i++)
                    {
                        string[] campos = linhas[i].Split(';');
                        if (campos[0] == nota.Ativo)
                        {
                            int quantidade = int.Parse(campos[1]) + nota.QuantidadeVenda;
                            decimal precoCompra = decimal.Parse(campos[2]) + nota.Preco;
                            decimal precoVenda = decimal.Parse(campos[3]) + nota.FinanceiroVenda;
                            linhas[i] = $"{nota.Ativo};{quantidade};{precoCompra:F2};{precoCompra:F2}";
                        }
                    }
                }

                StringBuilder sb = new StringBuilder();
                foreach (string linha in linhas)
                    sb.AppendLine(linha);

                using (StreamWriter file = new StreamWriter(pathFile))
                {
                    file.Write(sb.ToString());
                }
            }
        }

        public static void AtualizaCsvVenda(string pathFile, NotaNegociacao nota, int ano)
        {
            List<string> linhas = new List<string>();

            using (StreamReader file = new StreamReader(pathFile))
            {
                string linha;
                while ((linha = file.ReadLine()) != null)
                {
                    linhas.Add(linha);
                }
            }

            if (nota.FinanceiroVenda > 0 && nota.DataNegociacao.Year == ano)
            {
                if (linhas.Count == 1 || !linhas.Any(l => l.StartsWith(nota.Ativo)))
                    linhas.Add($"{nota.Ativo};{nota.QuantidadeVenda};{nota.Preco};{nota.FinanceiroVenda}");
                else
                {
                    for (int i = 0; i < linhas.Count; i++)
                    {
                        string[] campos = linhas[i].Split(';');
                        if (campos[0] == nota.Ativo)
                        {
                            int quantidade = int.Parse(campos[1]) + nota.QuantidadeVenda;
                            decimal precoCompra = decimal.Parse(campos[2]) + nota.Preco;
                            decimal precoVenda = decimal.Parse(campos[3]) + nota.FinanceiroVenda;
                            linhas[i] = $"{nota.Ativo};{quantidade};{precoCompra:F2};{precoCompra:F2}";
                        }
                    }
                }

                StringBuilder sb = new StringBuilder();
                foreach (string linha in linhas)
                    sb.AppendLine(linha);

                using (StreamWriter file = new StreamWriter(pathFile))
                {
                    file.Write(sb.ToString());
                }
            }
        }
    }
}