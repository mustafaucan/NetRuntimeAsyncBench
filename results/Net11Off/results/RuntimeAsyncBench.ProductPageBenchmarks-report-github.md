```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.10075/25H2/2025Update/HudsonValley2)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 11.0.100
  [Host] : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v3

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
| Method           | Mean      | Error    | StdDev   | Gen0   | Allocated |
|----------------- |----------:|---------:|---------:|-------:|----------:|
| CacheHit         |  67.07 ns | 1.377 ns | 1.414 ns | 0.0688 |     288 B |
| CacheMiss        | 968.40 ns | 6.217 ns | 5.191 ns | 0.0992 |     415 B |
| RealisticTraffic | 234.60 ns | 1.014 ns | 0.948 ns | 0.0718 |     301 B |
