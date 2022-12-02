using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using Encrypt_Class;

class Program
{
    // Variables 
    //public string certificateName;
    //public string sSecret;

    static int Main(string[] args)
    {
        Program p = new Program();
        Encrypt my_encrypt = new Encrypt();

        if (args.Length == 0)
        {
            Console.WriteLine("usage: encrypt -certificate localhost -string 'test1234!'");
            Console.WriteLine("Press the Enter key to exit.");

            return 1;
        }

        if (args[0].ToLower() == "-certificate")
        {
            my_encrypt.certificateName = args[1];

        }
        else
        {
            Console.WriteLine("Incorrect command line option (certificate)");
            Console.WriteLine("usage: encrypt -certificate localhost -string 'test1234!'");
        }

        if (args[2].ToLower() == "-string")
        {
            my_encrypt.sSecret = args[3];
        }
        else
        {
            Console.WriteLine("Incorrect command line option (string)");
            Console.WriteLine("usage: encrypt -certificate localhost -string 'test1234!'");
        }

        //        my_encrypt.certificateName = "localhost";
        //        my_encrypt.sSecret = "Test1234";
        // Get the certificate 
        my_encrypt.getCertificate(my_encrypt.certificateName);

        // Encryption 
        string sEncryptedSecret = string.Empty;
        sEncryptedSecret = my_encrypt.encryptRsa(my_encrypt.sSecret);

        // Decryption 
        string sDecryptedSecret = string.Empty;
        sDecryptedSecret = my_encrypt.decryptRsa(sEncryptedSecret);

        // Add the new encrypted value for the Key in the .Config file using a certificate from the local computer store (certlm)
        p.updateConfigFile("MCS.EventLogMonitor.WindowsService.exe.config", sEncryptedSecret);
        p.updateConfigFile("MCS.EventLogScavanger.exe.config", sEncryptedSecret);

        //Display the original data and the decrypted data.
        Console.WriteLine("----------------------------------------------");
        Console.WriteLine("Certifcate Name:   {0}", my_encrypt.certificateName);
        Console.WriteLine("String:   {0}", my_encrypt.sSecret);
        Console.WriteLine("Encypted String:   {0}", sEncryptedSecret);
        Console.WriteLine("Descrypted String: {0}", sDecryptedSecret);
        Console.WriteLine("Press the Enter key to exit.");
        return 0;
    }

    private void updateConfigFile(string fileName, string WORKSPACEKEY)
    {
        try
        {
            // Get the path to the executable file that is being installed on the target computer  
            string assemblypath = AppContext.BaseDirectory;

            string appConfigPath = assemblypath + fileName;

            // Write the path to the app.config file  
            XmlDocument doc = new XmlDocument();
            doc.Load(appConfigPath);

            XmlNode configuration = null;
            foreach (XmlNode node in doc.ChildNodes)
                if (node.Name == "configuration")
                    configuration = node;

            if (configuration != null)
            {
                // Get the ‘appSettings’ node  
                XmlNode settingNode = null;
                foreach (XmlNode node in configuration.ChildNodes)
                {
                    if (node.Name == "appSettings")
                        settingNode = node;
                }

                if (settingNode != null)
                {
                    //Reassign values in the config file  
                    foreach (XmlNode node in settingNode.ChildNodes)
                    {
                        //MessageBox.Show("node.Value = " + node.Value);  
                        if (node.Attributes == null)
                            continue;
                        XmlAttribute attribute = node.Attributes["value"];

                        if (node.Attributes["key"] != null)
                        {
                            switch (node.Attributes["key"].Value)
                            {
                                case "workspaceId":
                                    //attribute.Value = WORKSPACEID;
                                    break;

                                case "workspaceKey":
                                    attribute.Value = WORKSPACEKEY;
                                    break;
                            }
                        }
                    }
                }
                doc.Save(appConfigPath);
            }
        }
        catch
        {
            throw;
        }
    }


}
