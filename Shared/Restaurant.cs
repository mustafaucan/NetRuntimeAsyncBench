// Cache katmanı bilerek await içermiyor (veri bellekte, anında hazır).
// Yine de imzası async: gerçek projelerde cache arayüzleri böyle tanımlanır.
#pragma warning disable CS1998

namespace RuntimeAsyncBench;

/// <summary>
/// Ölçtüğümüz senaryo: bir ürün sayfası isteği.
///
///   ProductController.GetAsync
///     -> ProductService.GetProductAsync
///       -> ProductRepository.FindAsync
///         -> ProductCache.TryGetAsync      (cache'te varsa burada biter)
///         -> ProductDatabase.LoadAsync     (yoksa veritabanını bekler)
///
/// Bu dosya üç projede de (Net10, Net11Off, Net11On) aynen derlenir.
/// Kod aynı, sadece derleme ayarı farklı.
/// </summary>
public sealed record Product(int Id, string Name, decimal Price);

public static class Menu
{
    public const int KunefeId = 7;   // cache'te var
    public const int MantiId = 8;    // sadece veritabanında

    public static readonly Product Kunefe = new(KunefeId, "Künefe", 180m);
    public static readonly Product Manti = new(MantiId, "Mantı", 220m);
}

public sealed class ProductCache
{
    private readonly Dictionary<int, Product> _items = new() { [Menu.KunefeId] = Menu.Kunefe };

    // Vitrindeki tatlı: varsa hiç beklemeden döner.
    public async Task<Product?> TryGetAsync(int id)
        => _items.TryGetValue(id, out var product) ? product : null;
}

public sealed class ProductDatabase
{
    private readonly Dictionary<int, Product> _rows = new()
    {
        [Menu.KunefeId] = Menu.Kunefe,
        [Menu.MantiId] = Menu.Manti,
    };

    // Mutfak: gerçekten beklemek gerekiyor.
    // Task.Yield() gerçek I/O'nun yerine geçiyor: thread'i bırakır, thread pool'da devam eder.
    public async Task<Product> LoadAsync(int id)
    {
        await Task.Yield();
        return _rows[id];
    }
}

public sealed class ProductRepository(ProductCache cache, ProductDatabase database)
{
    public async Task<Product> FindAsync(int id)
    {
        var cached = await cache.TryGetAsync(id);
        if (cached is not null)
        {
            return cached;
        }

        // Bilerek cache'e yazmıyoruz: "cache miss" senaryosu her çağrıda miss kalsın.
        return await database.LoadAsync(id);
    }
}

public sealed class ProductService(ProductRepository repository)
{
    public async Task<Product> GetProductAsync(int id)
    {
        var product = await repository.FindAsync(id);
        if (product.Price <= 0)
        {
            throw new InvalidOperationException($"Ürün {id} için fiyat yok.");
        }

        return product;
    }
}

public sealed class ProductController(ProductService service)
{
    public async Task<Product> GetAsync(int id) => await service.GetProductAsync(id);

    public static ProductController Create()
        => new(new ProductService(new ProductRepository(new ProductCache(), new ProductDatabase())));
}

/// <summary>
/// Garson zinciri: sipariş, mutfağa ulaşana kadar <c>waiters</c> tane garsondan geçiyor.
/// Her garson bir async metot. Katman sayısını değiştirip maliyetin nasıl büyüdüğünü ölçmek için.
/// </summary>
public static class WaiterChain
{
    // ready = true  -> sipariş hazır (vitrindeki tatlı), hiçbir garson beklemiyor.
    // ready = false -> son garson mutfağı gerçekten bekliyor.
    public static async Task<Product> PassAsync(int waiters, bool ready)
    {
        if (waiters > 1)
        {
            return await PassAsync(waiters - 1, ready);
        }

        if (!ready)
        {
            await Task.Yield();
        }

        return Menu.Kunefe;
    }

    // Aynı zincir, ValueTask ile.
    public static async ValueTask<Product> PassValueTaskAsync(int waiters, bool ready)
    {
        if (waiters > 1)
        {
            return await PassValueTaskAsync(waiters - 1, ready);
        }

        if (!ready)
        {
            await Task.Yield();
        }

        return Menu.Kunefe;
    }
}
