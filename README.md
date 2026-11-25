# RuntimeAsyncBench — .NET 10 vs .NET 11 runtime async

Bir ürün sayfası isteği üzerinden (Controller → Service → Repository → Cache / Database)
.NET 11'deki runtime async'in klasik async state machine'e göre maliyetini ölçen deney.

## Gereksinimler

- .NET 11 SDK ve .NET 10 runtime (.NET 11 SDK, net10.0 projesini de derleyebilir). Kontrol: `dotnet --list-sdks`, `dotnet --list-runtimes`
- Benchmark sırasında başka ağır iş yapma (tarayıcı, IDE build, oyun...). Laptop'sa şarja tak.

## Klasör yapısı

```
RuntimeAsyncBench/
├─ Directory.Build.props   ortak ayarlar + Shared/*.cs dahil etme + BenchmarkDotNet paketi
├─ Shared/                 TEK kaynak kod (üç proje de bunu derler)
│  ├─ Restaurant.cs        ölçülen kod: ürün sayfası katmanları ve garson zinciri
│  ├─ Benchmarks.cs        BenchmarkDotNet sınıfları
│  ├─ Demo.cs              call stack ve allocation deneyi
│  └─ Program.cs           giriş noktası
├─ Net10/      net10.0
├─ Net11Off/   net11.0, UseRuntimeAsync=false
├─ Net11On/    net11.0, Features: runtime-async=on
├─ results/    benchmark sonuç tabloları
└─ blog/       yazının kapak görseli
```

## Senaryolar

| Benchmark | Ne ölçüyor |
| --- | --- |
| `ProductPageBenchmarks.CacheHit` | Ürün cache'te; 4 katmanın hiçbiri beklemiyor |
| `ProductPageBenchmarks.CacheMiss` | Ürün cache'te yok; veritabanı gerçekten bekleniyor (`Task.Yield`) |
| `ProductPageBenchmarks.RealisticTraffic` | Her 10 isteğin 9'u cache'ten, 1'i veritabanından |
| `WaiterChainBenchmarks.ReadyOrder` | 1 / 5 / 10 / 20 async metotluk zincir, kimse beklemiyor (`Task`) |
| `WaiterChainBenchmarks.ReadyOrderValueTask` | Aynı zincir, `ValueTask` ile |
| `WaiterChainBenchmarks.CookedOrder` | Aynı zincir, son metot gerçekten bekliyor |

## 1) Demo: call stack ve allocation

```bash
dotnet run -c Release --project Net10 -- demo
dotnet run -c Release --project Net11On -- demo
```

## 2) Benchmark (birkaç dakika / proje)

```powershell
.\run-all.ps1                    # hepsi
.\run-all.ps1 '*ProductPage*'    # sadece ürün sayfası
.\run-all.ps1 '*WaiterChain*'    # sadece garson zinciri
```

macOS / Linux: `./run-all.sh`, `./run-all.sh '*ProductPage*'` vb.

Sonuç tabloları: `results/<Proje>/results/*-report-github.md`

Rakamlar makineye, işletim sistemine ve CPU mimarisine göre değişir.
