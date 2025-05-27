namespace SeroGlint.DotNet.SecurityUtilities.SecurityInterfaces
{
    public interface IEncryptionService
    {
        byte[] Encrypt(byte[] plaintext);
        byte[] Decrypt(byte[] ciphertext);
    }
}
