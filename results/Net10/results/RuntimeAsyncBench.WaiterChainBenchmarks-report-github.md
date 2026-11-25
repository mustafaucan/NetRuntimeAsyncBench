```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.10075/25H2/2025Update/HudsonValley2)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 11.0.100
  [Host] : .NET 10.0.15 (10.0.15, 10.0.1526.53008), X64 RyuJIT x86-64-v3

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
| Method              | Waiters | Mean         | Error      | StdDev     | Gen0   | Allocated |
|-------------------- |-------- |-------------:|-----------:|-----------:|-------:|----------:|
| **ReadyOrder**          | **1**       |    **15.826 ns** |  **0.3528 ns** |  **0.3921 ns** | **0.0172** |      **72 B** |
| ReadyOrderValueTask | 1       |     7.230 ns |  0.1246 ns |  0.1104 ns |      - |         - |
| CookedOrder         | 1       |   708.211 ns |  3.5506 ns |  3.3212 ns | 0.0267 |     112 B |
| **ReadyOrder**          | **5**       |    **86.928 ns** |  **0.6943 ns** |  **0.5421 ns** | **0.0861** |     **360 B** |
| ReadyOrderValueTask | 5       |    63.764 ns |  0.9151 ns |  0.8112 ns |      - |         - |
| CookedOrder         | 5       | 1,047.016 ns | 11.8885 ns | 11.1205 ns | 0.1335 |     559 B |
| **ReadyOrder**          | **10**      |   **188.980 ns** |  **2.5155 ns** |  **2.3530 ns** | **0.1721** |     **720 B** |
| ReadyOrderValueTask | 10      |   146.923 ns |  0.7521 ns |  0.6281 ns |      - |         - |
| CookedOrder         | 10      | 1,485.753 ns | 18.2210 ns | 16.1525 ns | 0.2651 |    1116 B |
| **ReadyOrder**          | **20**      |   **377.714 ns** |  **5.9557 ns** | **12.0308 ns** | **0.3443** |    **1440 B** |
| ReadyOrderValueTask | 20      |   282.896 ns |  2.6692 ns |  2.4968 ns |      - |         - |
| CookedOrder         | 20      | 2,435.368 ns | 11.4009 ns |  9.5203 ns | 0.5264 |    2211 B |
