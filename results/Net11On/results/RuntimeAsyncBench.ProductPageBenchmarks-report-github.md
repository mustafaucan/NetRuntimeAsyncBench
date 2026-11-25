```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.10075/25H2/2025Update/HudsonValley2)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 11.0.100
  [Host] : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v3

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
| Method           | Mean      | Error    | StdDev   | Gen0   | Allocated |
|----------------- |----------:|---------:|---------:|-------:|----------:|
| CacheHit         |  31.56 ns | 0.453 ns | 0.424 ns | 0.0172 |      72 B |
| CacheMiss        | 924.12 ns | 7.430 ns | 6.586 ns | 0.0935 |     392 B |
| RealisticTraffic | 165.62 ns | 3.264 ns | 3.493 ns | 0.0248 |     104 B |
