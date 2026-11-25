using BenchmarkDotNet.Running;
using RuntimeAsyncBench;

// "demo" parametresi verilirse hızlı deneyi çalıştır, yoksa BenchmarkDotNet'i.
if (args.Length > 0 && args[0] == "demo")
{
    Demo.Run();
    return;
}

// BenchmarkSwitcher: --filter ile hangi benchmark'ların çalışacağını seçmeni sağlar.
//   --filter '*'                         -> hepsi
//   --filter '*ProductPage*'             -> sadece ürün sayfası (Controller -> Cache)
//   --filter '*WaiterChain*'             -> sadece garson zinciri (1 / 5 / 10 / 20 katman)
BenchmarkSwitcher.FromAssembly(typeof(ProductPageBenchmarks).Assembly).Run(args);
