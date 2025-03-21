using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace ei_si_worksheet4
{
    public partial class Form1 : Form
    {

        const string FILENAME_PUBLIC_KEY = "./PublicKey.xml";
        const string FILENAME_PRIVATE_KEY = "./PrivatePublicKey.xml";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ButtonGererateKeys_Click(object sender, EventArgs e)
        {

            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    // show the public key
                    textboxPublicKey.Text = rsa.ToXmlString(false);     // False = public key

                    // show the private key
                    textboxBothKeys.Text = rsa.ToXmlString(true);     // True = private key
                }

            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void ButtonSavePublicKey_Click(object sender, EventArgs e)
        {
            File.WriteAllText(FILENAME_PUBLIC_KEY, textboxPublicKey.Text);
            MessageBox.Show($"Public key saved in file: {FILENAME_PUBLIC_KEY}");
        }

        private void ButtonSaveKeys_Click(object sender, EventArgs e)
        {
            File.WriteAllText(FILENAME_PRIVATE_KEY, textboxBothKeys.Text);
            MessageBox.Show($"Both keys saved to {FILENAME_PRIVATE_KEY}");
        }

        private void ButtonEncrypt_Click(object sender, EventArgs e)
        {
            byte[] secretKey = null;
            string publicKey = null;
            byte[] encryptedData = null;

            try
            {
                secretKey = Encoding.UTF8.GetBytes(textboxSymmetricKey.Text);

                using (RSACryptoServiceProvider rsaOther = new RSACryptoServiceProvider()) {

                    //get the public key from the file
                    publicKey = File.ReadAllText(FILENAME_PUBLIC_KEY);
                    rsaOther.FromXmlString(publicKey);

                    encryptedData = rsaOther.Encrypt(secretKey, true);  // True = OAEP padding
                    textboxSymmetricKeyEncrypted.Text = Convert.ToBase64String(encryptedData);
                    textboxBitSize.Text = (encryptedData.Length * 8).ToString();

                }
            }
            catch (CryptographicException ex)
            {
                MessageBox.Show($"Encryption failed. Error: {ex.Message}");
            }
        }

        private void ButtonDecrypt_Click(object sender, EventArgs e)
        {

        }
    }
}
