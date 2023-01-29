using System;


namespace Banka_v1
{
    public interface IBankacilikIslemleri
    {
        void ParaYatirma(decimal miktar);
        bool ParaCekme(decimal miktar);

        void HesapOzeti();
    }


    abstract public class BankaHesabi : IBankacilikIslemleri
    {
        #region Veri elemanları
        protected string iban;
        protected decimal bakiye;
        protected Musteri mudi;
        protected DateTime acilisTarihi;
        #endregion

        #region Property'ler

        public string Iban
        {
            get { return iban; }
        }

        public decimal Bakiye
        {
            get { return bakiye; }
        }

        public Musteri Mudi
        {
            get { return mudi; } 
        }

        public DateTime AcilisTarihi
        { 
            get { return acilisTarihi;  }
        }

        #endregion

        #region Fonksiyonlar

        // Default constructor
        public BankaHesabi()
        {
            this.iban = this.IbanUret();
            this.acilisTarihi = DateTime.Now;
            this.bakiye = 0;
        }

        public BankaHesabi(Musteri mudi) : this()
        {
            this.mudi = mudi;
        }

        private string IbanUret()
        {
            string ibanNo = "TR90 ";

            Random rnd = new Random();

            for (int i = 1; i <= 4; ++i)
            {
                ibanNo += rnd.Next(1000, 9999) + " ";
                // int ve string toplandığı zaman zaten otomatik string'e dönüşür bu nedenle
                // aşağıdaki gibi explicit string türüne dönüşüm aslında gereksizdir.
                // ibanNo += rnd.Next(1000, 9999).ToString() + " ";
                // ibanNo += Convert.ToString(rnd.Next(1000, 9999)) + " ";
            }

            return ibanNo + " 83"; 
        }

        public abstract void ParaYatirma(decimal miktar);

        public abstract bool ParaCekme(decimal miktar);

        public abstract void HesapOzeti();
      
        #endregion

    }

    public class VadesizHesap : BankaHesabi
    {
        public VadesizHesap(Musteri musteri) : base(musteri)
        { 
        }

        public override void ParaYatirma(decimal miktar)
        {
            bakiye += miktar;
        }

        public override bool ParaCekme(decimal miktar)
        {
            if (miktar > bakiye)
            {
                Console.WriteLine("Çekmek istediğiniz miktarı bakiyeniz karşılamıyor");
                return false;
            }
            else if (miktar >= 30000)
            {
                Console.WriteLine("Günlük çekim limiti!");
                return false;
            }
            else
            {
                bakiye -= miktar;
                Console.WriteLine("Paranızı güle güle harcayın");
                return true;
            }
        }

        public override void HesapOzeti()
        {
            Console.Write("Hesap özetini görmek istiyor musunuz? [E | H]");

            ConsoleKeyInfo cki = Console.ReadKey();

            if (cki.KeyChar == 'E' || cki.KeyChar == 'e')
            {
                Console.Clear();
                Console.WriteLine("======== Vadesiz Mevduat Hesap Özeti ========");
                Console.WriteLine("IBAN: " + this.Iban);
                Console.WriteLine("Açılış Tarihi: " + this.acilisTarihi);
                Console.WriteLine("Bakiye: {0} TL", this.Bakiye);
                Console.WriteLine("Müşteri: {0} {1}", this.Mudi.adi, this.Mudi.soyadi.ToUpper());


            }
            else if (cki.KeyChar == 'H' || cki.KeyChar == 'h')
            {
                Console.WriteLine("Vazgeçildi");
            }
            else
            {
                Console.WriteLine("Hatalı giriş!");
            }

        }
    }

    public class VadesizDovizHesabi : VadesizHesap
    {
        private string kur;

        public string Kur
        {
            get { return kur;  }
            set 
            {
                switch (value)
                {
                    case "dolar":
                    case "euro":
                    case "yen":
                        this.kur = value;
                        break;
                    default:
                        Console.WriteLine("Bu döviz kuru desteklenmiyor!");
                        break;
                }
            }
        }

        public VadesizDovizHesabi(Musteri musteri, string kur) : base(musteri)
        {
            this.Kur = kur;
        }

        public override void ParaYatirma(decimal miktar)
        {
            bakiye += miktar;
        }

        public override bool ParaCekme(decimal miktar)
        {
            if (miktar > bakiye)
            {
                Console.WriteLine("Çekmek istediğiniz miktarı bakiyeniz karşılamıyor");
                return false;
            }
            else if (miktar >= 10000)
            {
                Console.WriteLine("Günlük çekim limiti!");
                return false;
            }
            else
            {
                bakiye -= miktar;
                Console.WriteLine("Paranızı güle güle harcayın");
                return true;
            }
        }

        public override void HesapOzeti()
        {
            Console.Write("Hesap özetini görmek istiyor musunuz? [E | H]");

            ConsoleKeyInfo cki = Console.ReadKey();

            if (cki.KeyChar == 'E' || cki.KeyChar == 'e')
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("======== Vadesiz Döviz Hesap Özeti ========");
                Console.WriteLine("IBAN: " + this.Iban);
                Console.WriteLine("Açılış Tarihi: " + this.acilisTarihi);
                Console.WriteLine("Bakiye: {0} {1}", this.Bakiye, this.Kur);
                Console.WriteLine("Müşteri: {0} {1}", this.Mudi.adi, this.Mudi.soyadi.ToUpper());


            }
            else if (cki.KeyChar == 'H' || cki.KeyChar == 'h')
            {
                Console.WriteLine("Vazgeçildi");
            }
            else
            {
                Console.WriteLine("Hatalı giriş!");
            }

        }


    }

    public class VadeliHesap : BankaHesabi
    {
        private int faizOrani;
        protected DateTime valorTarihi;

        public VadeliHesap(Musteri mudi, int faizOrani, DateTime valorTarihi) : base(mudi)
        {
            this.faizOrani = faizOrani;
            this.valorTarihi = valorTarihi;
        }

        public decimal GetiriHesapla()
        {
            return ((base.bakiye * this.faizOrani) / 100) / 12;
        }

        public override void ParaYatirma(decimal miktar)
        {
            bakiye += miktar;
        }

        public override bool ParaCekme(decimal miktar)
        {
            if (miktar > bakiye)
            {
                Console.WriteLine("Çekmek istediğiniz miktarı bakiyeniz karşılamıyor");
                return false;
            }
            else if (miktar >= 30000)
            {
                Console.WriteLine("Günlük çekim limiti!");
                return false;
            }
            else
            {
                bakiye -= miktar;
                Console.WriteLine("Paranızı güle güle harcayın");
                return true;
            }
        }

        public override void HesapOzeti()
        {
            Console.Write("Hesap özetini görmek istiyor musunuz? [E | H]");

            ConsoleKeyInfo cki = Console.ReadKey();

            if (cki.KeyChar == 'E' || cki.KeyChar == 'e')
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("======== Vadeli Mevduat Hesap Özeti ========");
                Console.WriteLine("IBAN: " + this.Iban);
                Console.WriteLine("Açılış Tarihi: " + this.acilisTarihi);
                Console.WriteLine("Valör Tarihi: " + this.valorTarihi);
                Console.WriteLine("Bakiye: {0} TL", this.Bakiye);
                Console.WriteLine("Faiz Oranı: %{0}", this.faizOrani);
                Console.WriteLine("Getirisi: {0} TL", this.GetiriHesapla());
                Console.WriteLine("Müşteri: {0} {1}", this.Mudi.adi, this.Mudi.soyadi.ToUpper());
            }
            else if (cki.KeyChar == 'H' || cki.KeyChar == 'h')
            {
                Console.WriteLine("Vazgeçildi");
            }
            else
            {
                Console.WriteLine("Hatalı giriş!");
            }

        }

    }
}
