```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.10075/25H2/2025Update/HudsonValley2)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 11.0.100
  [Host] : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v3

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
| Method              | Waiters | Mean        | Error     | StdDev    | Gen0   | Allocated |
|-------------------- |-------- |------------:|----------:|----------:|-------:|----------:|
| **ReadyOrder**          | **1**       |    **18.94 ns** |  **0.581 ns** |  **1.649 ns** | **0.0172** |      **72 B** |
| ReadyOrderValueTask | 1       |    10.21 ns |  0.264 ns |  0.737 ns |      - |         - |
| CookedOrder         | 1       |   628.13 ns | 12.557 ns | 12.895 ns | 0.0458 |     192 B |
| **ReadyOrder**          | **5**       |    **34.92 ns** |  **0.753 ns** |  **2.085 ns** | **0.0172** |      **72 B** |
| ReadyOrderValueTask | 5       |    24.00 ns |  0.311 ns |  0.259 ns |      - |         - |
| CookedOrder         | 5       |   935.72 ns | 11.156 ns |  9.315 ns | 0.1068 |     448 B |
| **ReadyOrder**          | **10**      |    **59.72 ns** |  **1.232 ns** |  **2.221 ns** | **0.0172** |      **72 B** |
| ReadyOrderValueTask | 10      |    47.54 ns |  0.681 ns |  0.637 ns |      - |         - |
| CookedOrder         | 10      | 1,249.23 ns | 20.896 ns | 19.546 ns | 0.1831 |     768 B |
| **ReadyOrder**          | **20**      |   **113.88 ns** |  **1.858 ns** |  **1.738 ns** | **0.0172** |      **72 B** |
| ReadyOrderValueTask | 20      |   101.78 ns |  1.253 ns |  1.111 ns |      - |         - |
| CookedOrder         | 20      | 1,780.88 ns | 33.899 ns | 39.039 ns | 0.3357 |    1408 B |
