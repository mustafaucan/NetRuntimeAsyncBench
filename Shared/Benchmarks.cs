using BenchmarkDotNet.Attributes;

namespace RuntimeAsyncBench;

// [InProcess]: Benchmark, ayrı bir süreç açmadan bu programın içinde çalışır.
//   Runtime async bir PROJE ayarı olduğu için, BenchmarkDotNet'in kendi ürettiği
//   ayrı projeye güvenmek yerine doğrudan bizim derlediğimiz kodu ölçüyoruz.
// [MemoryDiagnoser]: Süreye ek olarak "çağrı başına kaç byte ayrıldı" sütununu ekler.

/// <summary>
/// Ürün sayfası: Controller -> Service -> Repository -> Cache / Database.
/// </summary>
[InProcess]
[MemoryDiagnoser]
public class ProductPageBenchmarks
{
    private ProductController _controller = null!;
    private int _request;

    [GlobalSetup]
    public void Setup() => _controller = ProductController.Create();

    // Ürün cache'te: hiçbir katman beklemiyor.
    [Benchmark]
    public Product CacheHit() => _controller.GetAsync(Menu.KunefeId).GetAwaiter().GetResult();

    // Ürün cache'te yok: veritabanı gerçekten bekleniyor.
    [Benchmark]
    public Product CacheMiss() => _controller.GetAsync(Menu.MantiId).GetAwaiter().GetResult();

    // Gerçekçi trafik: her 10 isteğin 9'u cache'ten, 1'i veritabanından.
    [Benchmark]
    public Product RealisticTraffic()
    {
        var id = ++_request % 10 == 0 ? Menu.MantiId : Menu.KunefeId;
        return _controller.GetAsync(id).GetAwaiter().GetResult();
    }
}

/// <summary>
/// Garson zinciri: aynı sipariş 1, 5, 10 ve 20 garsondan geçerse maliyet nasıl değişiyor?
/// </summary>
[InProcess]
[MemoryDiagnoser]
public class WaiterChainBenchmarks
{
    [Params(1, 5, 10, 20)]
    public int Waiters;

    // Sipariş hazır, kimse beklemiyor. Task ile.
    [Benchmark]
    public Product ReadyOrder() => WaiterChain.PassAsync(Waiters, ready: true).GetAwaiter().GetResult();

    // Sipariş hazır, kimse beklemiyor. ValueTask ile.
    [Benchmark]
    public Product ReadyOrderValueTask() => WaiterChain.PassValueTaskAsync(Waiters, ready: true).GetAwaiter().GetResult();

    // Son garson mutfağı gerçekten bekliyor.
    [Benchmark]
    public Product CookedOrder() => WaiterChain.PassAsync(Waiters, ready: false).GetAwaiter().GetResult();
}
