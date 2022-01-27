using System;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace rsa
{
    public partial class Form1 : Form
    {
            int birinciAsal = 0, ikinciAsal = 0;
            int n = 0;
            int phi = 0;
            int eDegeri = 0;
            double dDegeri = 0;
            string text;
            string text3;
            string text2;
            string hash;
            string desifreHash;
            string veri="";
            string sifre = "";

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        static bool AsalMi(int sayi)
        {
            //Girilen değerlerin asallığının kontrol edildiği blok
            for (int i=2; i<sayi; i++)
            {
                if (sayi % i == 0)
                    return false;
            }
            return true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!AsalMi(Convert.ToInt32(textBox1.Text)) || !AsalMi(Convert.ToInt32(textBox2.Text)))
            {
                MessageBox.Show("Lütfen asal sayılar giriniz!");
            }
            else
            {
                //iki asal sayı seçilir
                birinciAsal = Convert.ToInt32(textBox1.Text);
                ikinciAsal = Convert.ToInt32(textBox2.Text);
                n = birinciAsal * ikinciAsal;
                phi = (birinciAsal - 1) * (ikinciAsal - 1);
                textBox3.Text = n.ToString();
                textBox4.Text = phi.ToString();
                //e sayısı phi sayısı ile aralarında asal olmalıdır
                //static bir klas tanımlanmalı ve aralarında asallık durumları kontrol edilmelidir
                for (int i = 2; i < phi; i++)
                {
                    if (OBEB(phi, i) == 1)          //phi sayısına kadar aralarında asallık durumunu sağlayacak e değerleri
                    {
                        listBox1.Items.Add(i);
                    }
                }
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            text = textBox6.Text.ToString();
            text3 = RSASifre(text, n, eDegeri);
            text2 = RSASifre(text3, n, (int)dDegeri);

            textBox7.Text = text3.ToString();
            textBox8.Text = text2.ToString();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            hash = Hash(text);
            hash = RSASifre(hash, n, (int)dDegeri);
            textBox9.Text = hash.ToString();
            desifreHash = RSADesifre(hash, n, eDegeri);
            textBox10.Text = desifreHash.ToString();
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label10.Text = listBox1.SelectedItem.ToString();
            eDegeri = Convert.ToInt32(label10.Text);
            dDegeri = DBulma(phi, eDegeri);
            textBox5.Text = dDegeri.ToString();
            label9.Text = n.ToString();
            label12.Text = n.ToString();
            label13.Text = dDegeri.ToString();
        }
        static int OBEB(int x, int y)
        {
            int min = Math.Min(x, y);
            int obeb = 1;
            for (int i = 2; i <= min; i++)
            {
                if (x % i == 0 && y % i == 0)
                {
                    obeb = i;
                }
            }
            return obeb;
        }
        static double DBulma(int phi, int eDegeri)
        {
            int k = 0;
            double d;
            for (int i = 0; i <= k; i++)
            {
                k++;
                d = (double)(1 + i * phi) / (double)eDegeri;
                double hesapla = d - ((int)d);
                if (hesapla == 0)
                {
                    if (1 < d && d < phi)
                        return d;
                }
            }
            return 0;
        }
        static int ModAlma(int a, double b, int n)    // == a^^b(mod(n))
        {
            int _a = a % n;      //_a değeri a ile n nin bölümünden kalan değer
            double _b = b;
            if (b == 0)            //eğer sayının üssü (b) sıfır ise sonuç 1 yazdır
            {
                return 1;
            }
            while (_b > 1)        //eğer sayının üssü 1den büyük ise
            {
                _a *= a;        //kendisi ile çarp
                _a %= n;        //n değerine bölündüğünde kalanını bul
                _b--;           //üssü 1 azalt
            }
            return _a;          // _a değerini döndür

        }
        static string RSASifre(string metin, int n, int eDegeri)    //şifreleme işlemi m^e(mod((n))
        {
            char[] chars = metin.ToCharArray();                     //chars adında char dizisi tanımlandı
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < chars.Length; i++)
            {
                builder.Append(Convert.ToChar(ModAlma(chars[i], eDegeri, n)));            //builder'a char dizisindeki değerler sırasıyla, e ve n değerleri eklendi
            }
            return builder.ToString();                      //sonuçta builder daki değerler string olarak yazdırılacak
        }
        static string RSADesifre(string metin2, int m, double d)     //c= şifreli mesaj == j^d(mod(n))
        {
            char[] chars2 = metin2.ToCharArray();               //chars2 adında yeni char dizisi tanımlandı
            StringBuilder builder2 = new StringBuilder();
            for (int j = 0; j < chars2.Length; j++)             //tüm diziyi dolaşacak for döngüsü tanımlandı
            {
                builder2.Append(Convert.ToChar(ModAlma(chars2[j], d, m)));
            }
            return builder2.ToString();                     //sonuç olarak builder2 deki değerler string olarak yazdırılacak
        }
        static string Hash(string metin)
        {
            string mesajtxt = metin;

            MD5 md5 = new MD5CryptoServiceProvider();

            string hashSignature = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(mesajtxt)));

            return hashSignature;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            veri = textBox11.Text;
            char[] karakterler = veri.ToCharArray();
            foreach (char harf in karakterler)
            {
                textBox12.Text += Convert.ToChar(harf + 5).ToString();
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            sifre = textBox12.Text;
            char[] karakterler2 = sifre.ToCharArray();
            foreach (char harf2 in karakterler2)
            {
                textBox13.Text += Convert.ToChar(harf2 - 5).ToString();
            }
        }
    }
}
