using System.Security.Cryptography;
using System.Threading.Tasks;
using LightRail.DotNet.Extensions;

namespace LightRail.DotNet.SecurityUtilities
{
    public class PasswordUtility
    {
        private const int SaltSize = 16; // 128 bits
        private const int KeySize = 32; // 256 bits
        private const int Iterations = 8;
        private const char SegmentDelimiter = ':';
        private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

        public static byte[] BuildRandomSalt()
        {
            return GenerateRandomSalt();
        }

        private static byte[] GenerateRandomSalt()
        {
            var saltBytes = new byte[SaltSize];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(saltBytes);
            }

            return saltBytes;
        }

        //public static string SlowHash(string input, byte[] salt)
        //{
        //    var hash = DeriveKey(input, salt);

        //    return string.Join(
        //        SegmentDelimiter,
        //        hash.ToHexString(),
        //        salt.ToHexString(),
        //        Iterations,
        //        Algorithm
        //    );
        //    return string.Empty;
        //}

        public static byte[] DeriveKey(string password, byte[] salt, int iterations = 100_000, int keyLength = 32)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return rfc2898.GetBytes(keyLength);
            }
        }

        //public static Task<bool> Verify(string input, string hashString)
        //{
        //    return Task.Run(() =>
        //    {
        //        var segments = hashString.Split(SegmentDelimiter);
        //        var hash = segments[0].ToHexString();
        //        var salt = segments[1].ToHexString();
        //        var iterations = int.Parse(segments[2]);
        //        var algorithm = new HashAlgorithmName(segments[3]);

        //        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
        //            input,
        //            salt,
        //            iterations,
        //            algorithm,
        //            hash.Length
        //        );
        //        return CryptographicOperations.FixedTimeEquals(inputHash, hash);
        //    });
        //}
    }
}
