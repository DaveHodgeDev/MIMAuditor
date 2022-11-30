using System;
using System.Security.Cryptography.X509Certificates;


using System.Text;


namespace Encrypt_Class
{
    public class Encrypt
    {
        // Variables 
        public string certificateName;
        public string sSecret;

        public X509Certificate2 getCertificate(string certificateName)
        {
                   
            X509Store my = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            my.Open(OpenFlags.ReadOnly);
           
            X509Certificate2Collection collection = my.Certificates.Find(X509FindType.FindBySubjectName, certificateName, false);
            if (collection.Count == 1)
            {
                return collection[0];
            }
            else if (collection.Count > 1)
            {
                throw new Exception(string.Format("More than one certificate with name '{0}' found in store LocalMachine/My.", certificateName));
            }
            else
            {
                throw new Exception(string.Format("Certificate '{0}' not found in store LocalMachine/My.", certificateName));
            }
        }

        public string encryptRsa(string input)
        {
            string output = string.Empty;
            X509Certificate2 cert = getCertificate(certificateName);

            // Changed from to support .Net Code 3.1
            //      using (RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PublicKey.Key)
            // to
            //      using (System.Security.Cryptography.RSACng csp = (System.Security.Cryptography.RSACng)cert.PublicKey.Key)
            // because of 'Unable to cast object of type 'System.Security.Cryptography.RSACng' to type 'System.Security.Cryptography.RSACryptoServiceProvider'.'

            // Changed from 
            //      byte[] bytesEncrypted = csp.Encrypt(bytesData, false);
            // To            
            //      byte[] bytesEncrypted = csp.Encrypt(bytesData, System.Security.Cryptography.RSAEncryptionPadding.OaepSHA512);

            // Change to cert.GetRSAPublicKey() instead of cert.PublicKey.Key as it compiled but threw an error when encrypting decrypting

            using (System.Security.Cryptography.RSACng csp = (System.Security.Cryptography.RSACng)cert.GetRSAPublicKey())
            {
                byte[] bytesData = Encoding.UTF8.GetBytes(input);
                //byte[] bytesEncrypted = csp.Encrypt(bytesData, false);
                byte[] bytesEncrypted = csp.Encrypt(bytesData, System.Security.Cryptography.RSAEncryptionPadding.OaepSHA512);
                output = Convert.ToBase64String(bytesEncrypted);
            }
            return output;
        }

        public string decryptRsa(string encrypted)
        {
            string text = string.Empty;
            X509Certificate2 cert = getCertificate(certificateName);
            // using (RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PrivateKey)
            using (System.Security.Cryptography.RSACng csp = (System.Security.Cryptography.RSACng)cert.GetRSAPrivateKey())
            {
                byte[] bytesEncrypted = Convert.FromBase64String(encrypted);
                //byte[] bytesDecrypted = csp.Decrypt(bytesEncrypted, false);
                byte[] bytesDecrypted = csp.Decrypt(bytesEncrypted, System.Security.Cryptography.RSAEncryptionPadding.OaepSHA512);
                text = Encoding.UTF8.GetString(bytesDecrypted);
            }
            return text;
        }
    }
}
