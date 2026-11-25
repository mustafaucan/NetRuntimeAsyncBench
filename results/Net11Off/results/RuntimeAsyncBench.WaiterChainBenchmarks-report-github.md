```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.10075/25H2/2025Update/HudsonValley2)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 11.0.100
  [Host] : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v3

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
| Method              | Waiters | Mean         | Error      | StdDev     | Median       | Gen0   | Allocated |
|-------------------- |-------- |-------------:|-----------:|-----------:|-------------:|-------:|----------:|
| **ReadyOrder**          | **1**       |    **15.107 ns** |  **0.2797 ns** |  **0.2616 ns** |    **15.050 ns** | **0.0172** |      **72 B** |
| ReadyOrderValueTask | 1       |     7.225 ns |  0.1201 ns |  0.0938 ns |     7.204 ns |      - |         - |
| CookedOrder         | 1       |   609.783 ns | 12.2284 ns | 20.7647 ns |   608.901 ns | 0.0267 |     112 B |
| **ReadyOrder**          | **5**       |    **89.127 ns** |  **3.9519 ns** | **11.6522 ns** |    **83.864 ns** | **0.0861** |     **360 B** |
| ReadyOrderValueTask | 5       |    59.562 ns |  0.4882 ns |  0.4567 ns |    59.441 ns |      - |         - |
| CookedOrder         | 5       |   950.624 ns | 12.6814 ns | 11.8622 ns |   947.180 ns | 0.1335 |     560 B |
| **ReadyOrder**          | **10**      |   **184.847 ns** |  **2.6662 ns** |  **2.3636 ns** |   **184.428 ns** | **0.1721** |     **720 B** |
| ReadyOrderValueTask | 10      |   141.196 ns |  1.9677 ns |  1.7443 ns |   140.213 ns |      - |         - |
| CookedOrder         | 10      | 1,360.390 ns | 12.7220 ns | 11.2777 ns | 1,356.509 ns | 0.2670 |    1119 B |
| **ReadyOrder**          | **20**      |   **379.973 ns** |  **4.7511 ns** |  **4.2117 ns** |   **380.418 ns** | **0.3443** |    **1440 B** |
| ReadyOrderValueTask | 20      |   268.577 ns |  2.4818 ns |  2.3215 ns |   268.282 ns |      - |         - |
| CookedOrder         | 20      | 2,161.764 ns | 41.2843 ns | 47.5430 ns | 2,168.785 ns | 0.5302 |    2224 B |
