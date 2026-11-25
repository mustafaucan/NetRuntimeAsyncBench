```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.10075/25H2/2025Update/HudsonValley2)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 11.0.100
  [Host] : .NET 10.0.15 (10.0.15, 10.0.1526.53008), X64 RyuJIT x86-64-v3

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
| Method           | Mean        | Error     | StdDev    | Gen0   | Allocated |
|----------------- |------------:|----------:|----------:|-------:|----------:|
| CacheHit         |    71.45 ns |  1.479 ns |  3.151 ns | 0.0688 |     288 B |
| CacheMiss        | 1,035.51 ns | 20.719 ns | 32.863 ns | 0.0992 |     416 B |
| RealisticTraffic |   217.13 ns |  5.433 ns | 16.019 ns | 0.0718 |     301 B |
