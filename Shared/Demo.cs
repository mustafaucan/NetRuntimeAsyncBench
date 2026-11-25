using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RuntimeAsyncBench;

/// <summary>
/// BenchmarkDotNet'ten önce "gözünle görmek" için hızlı deney.
/// Çalıştırma: dotnet run -c Release --project Net11On -- demo
/// </summary>
public static class Demo
{
    public static void Run()
    {
        Console.WriteLine($"Runtime : {RuntimeInformation.FrameworkDescription}");
        Console.WriteLine();

        // --- Deney 1: Call stack nasıl görünüyor? ---------------------------------
        // Klasik modelde araya MoveNext / AsyncMethodBuilderCore.Start gibi
        // "altyapı" frame'leri girer. Runtime async'te sadece bizim metotlarımız görünür.
        Console.WriteLine("=== Deney 1: Canlı call stack ===");
        TakeOrderAsync().GetAwaiter().GetResult();

        // --- Deney 2: Kaç byte ayrılıyor? -----------------------------------------
        Console.WriteLine();
        Console.WriteLine("=== Deney 2: Çağrı başına allocation (yaklaşık) ===");
        var controller = ProductController.Create();
        Measure("Ürün sayfası, cache hit (4 katman)", () => controller.GetAsync(Menu.KunefeId).GetAwaiter().GetResult());
        Measure("Garson zinciri, hazır, 1 garson   ", () => WaiterChain.PassAsync(1, ready: true).GetAwaiter().GetResult());
        Measure("Garson zinciri, hazır, 20 garson  ", () => WaiterChain.PassAsync(20, ready: true).GetAwaiter().GetResult());
        Measure("Garson zinciri, ValueTask, 20     ", () => WaiterChain.PassValueTaskAsync(20, ready: true).GetAwaiter().GetResult());
        Console.WriteLine();
        Console.WriteLine("Not: Kesin rakamlar için BenchmarkDotNet'i çalıştır (demo parametresi olmadan).");
    }

    private static void Measure(string name, Func<Product> action)
    {
        const int iterations = 100_000;

        // Isınma: JIT önce hızlı ama optimize edilmemiş kod üretir (Tier 0),
        // metot sık çağrılınca arka planda optimize eder (Tier 1). Optimize koda geçmesini bekliyoruz.
        for (var round = 0; round < 3; round++)
        {
            for (var i = 0; i < iterations; i++) action();
            Thread.Sleep(300);
        }

        var before = GC.GetAllocatedBytesForCurrentThread();
        for (var i = 0; i < iterations; i++) action();
        var after = GC.GetAllocatedBytesForCurrentThread();

        Console.WriteLine($"{name} : {(after - before) / (double)iterations,8:0.#} byte/çağrı");
    }

    // Sipariş -> mutfak -> pişirme: üç async metot birbirini çağırıyor.
    private static async Task TakeOrderAsync() => await PassToKitchenAsync();

    private static async Task PassToKitchenAsync() => await CookAsync();

    private static async Task CookAsync()
    {
        Console.WriteLine("--- await'ten ÖNCE (senkron kısım) ---");
        Console.WriteLine(FilterStack(new StackTrace().ToString()));

        await Task.Yield(); // burada askıya alınıyoruz, sonra thread pool'da devam ediyoruz

        Console.WriteLine("--- await'ten SONRA (devam/continuation) ---");
        Console.WriteLine(FilterStack(new StackTrace().ToString()));
    }

    // Çıktı kısa kalsın diye Main'in altını kesiyoruz.
    private static string FilterStack(string stack)
    {
        var lines = stack.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        return string.Join(Environment.NewLine, lines.TakeWhile(l => !l.Contains("Program.<Main>", StringComparison.Ordinal)
                                                                   && !l.Contains("Program.Main", StringComparison.Ordinal)));
    }
}
