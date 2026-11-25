#!/usr/bin/env bash
# Üç projeyi sırayla çalıştırır, sonuçları results/<proje> klasörüne yazar.
# Kullanım:  ./run-all.sh                    -> tüm benchmark'lar
#            ./run-all.sh '*ProductPage*'    -> sadece ürün sayfası
set -euo pipefail
FILTER="${1:-*}"
cd "$(dirname "$0")"
for p in Net10 Net11Off Net11On; do
  echo "================ $p ================"
  dotnet run -c Release --project "$p" -- --filter "$FILTER" --artifacts "results/$p"
done
echo
echo "Bitti. Tablolar: results/*/results/*-report-github.md"
