class Klient
{
    // RSA
    private RSACryptoServiceProvider rsa;
    public string VerejnyKlic => rsa.ToXmlString(false); // veřejný klíč (jen ke čtení)
    private string privatniKlic => rsa.ToXmlString(true); // privátní klíč (jen vnitřní)

    // AES
    private byte[] aesKlic;
    private byte[] aesIV;

    public Klient(bool generujRSA = false)
    {
        if (generujRSA)
        {
            rsa = new RSACryptoServiceProvider(2048);
        }
    }

    public void PrijmiAESKlic(byte[] zasifrovanyKlic, byte[] iv)
    {
    rsa = new RSACryptoServiceProvider();
    rsa.FromXmlString(privatniKlic);
    try { aesKlic = rsa.Decrypt(zasifrovanyKlic, false); }
    catch (Exception e) { Console.WriteLine(e.ToString()); Environment.Exit(1); }
    

    aesIV = iv;
    
    }

    // Klient A: zašifrování AES klíče pro klienta B
    public (byte[] zasifrovanyKlic, byte[] iv) SifrujAESKlic(string verejnyKlicB)
    {
        using (Aes aes = Aes.Create())
        {
            aes.GenerateKey();
            aes.GenerateIV();
            aesKlic = aes.Key;
            aesIV = aes.IV;

            RSACryptoServiceProvider rsaB = new RSACryptoServiceProvider();
            rsaB.FromXmlString(verejnyKlicB);

            byte[] zasifrovanyKlic = rsaB.Encrypt(aesKlic, false);
            return (zasifrovanyKlic, aesIV);
        }
    }

    // Šifrování zprávy pomocí AES
    public byte[] SifrujZpravu(string zprava)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = aesKlic;
            aes.IV = aesIV;
            ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] plainBytes = Encoding.UTF8.GetBytes(zprava);
            return encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }
    }

    // Dešifrování zprávy pomocí AES
    public string DesifrujZpravu(byte[] zasifrovanaZprava)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = aesKlic;
            aes.IV = aesIV;
            ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] decryptedBytes = decryptor.TransformFinalBlock(zasifrovanaZprava, 0, zasifrovanaZprava.Length);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}

public static void Main()
{
        // 1) Klient B vytvoří RSA klíče
        Klient klientB = new Klient(generujRSA: true);

        // 2) Klient A vytvoří AES klíč a zašifruje ho pomocí veřejného klíče B
        Klient klientA = new Klient();
        var (zasifrovanyAES, iv) = klientA.SifrujAESKlic(klientB.VerejnyKlic);

        // 3) Klient B dešifruje AES klíč
        klientB.PrijmiAESKlic(zasifrovanyAES, iv);

        // 4) Klient B zašifruje zprávu pomocí AES a pošle ji A
        string zprava = "Tajná zpráva: Ahoj kliente A!";
        byte[] sifrovanaZprava = klientB.SifrujZpravu(zprava);

        // 5) Klient A dešifruje zprávu a vypíše ji
        string desifrovano = klientA.DesifrujZpravu(sifrovanaZprava);

        Console.WriteLine("Zpráva od klienta B po dešifrování:");
        Console.WriteLine(desifrovano);
}
