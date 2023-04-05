using relatorioInvestimento;

var relatorios = Relatorios.PegaTodosOsArquivos("C:\\Users\\joaos\\Downloads\\Nova pasta");
var realatorioCompra = "relatorioCompra.csv";
var diretorioCompra = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "compra");
var realatorioVenda = "relatorioimp.csv";
var diretorioVenda = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "venda");

if (!File.Exists(path: $"{diretorioCompra}\\{realatorioCompra}"))
{
    if (!Directory.Exists(diretorioCompra))
    {
        Directory.CreateDirectory(diretorioCompra);
    }
    Relatorios.CriaCsvCompra(pathFile: $"{diretorioCompra}\\{realatorioCompra}");
}

if (!File.Exists(path: $"{diretorioVenda}\\{realatorioVenda}"))
{
    if (!Directory.Exists(diretorioVenda))
    {
        Directory.CreateDirectory(diretorioVenda);
    }
    Relatorios.CriaCsvCompra(pathFile: $"{diretorioVenda}\\{realatorioVenda}");
}

foreach (var relatorio in relatorios)
{
    var notas = Relatorios.AbreArquivo(relatorio);
    foreach (var nota in notas)
    {
        Relatorios.AtualizaCsvCompra($"{diretorioCompra}\\{realatorioCompra}", nota);
        Relatorios.AtualizaCsvVenda($"{diretorioVenda}\\{realatorioVenda}", nota);
    }
}
