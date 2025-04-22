
# PVAcujeme

Tento projekt simuluje bezpečné předání šifrovacího klíče mezi dvěma klienty (A a B) pomocí kombinace symetrické (AES) a asymetrické (RSA) kryptografie.

## Popis

- Klient A vygeneruje AES klíč pro šifrování dat.
- AES klíč zašifruje pomocí veřejného RSA klíče klienta B.
- Klient B dešifruje AES klíč pomocí svého soukromého RSA klíče.
- Obě strany poté mohou komunikovat pomocí AES.

## Technologie

- Jazyk: C#
- Použité knihovny: `System.Security.Cryptography`

## Použití

1. Spusťte aplikaci.
2. Sledujte výstup simulující vytvoření klíčů a jejich bezpečné předání.
3. Volitelně upravte šifrovaná data a přidejte vlastní testy.

## Cíl

Ukázat, jak kombinovat RSA a AES pro bezpečný přenos symetrického klíče v C#.