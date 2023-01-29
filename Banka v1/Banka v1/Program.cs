using System;

namespace Banka_v1
{
    class Banka
    {
        public static void HesapOzetiGoster(IBankacilikIslemleri p)
        {
            p.HesapOzeti();
        }
    
    }

    class Test
    {
        static void Main(string[] args)
        {
            Musteri m1 = new BireyselMusteri();
            m1.adi = "aykut";
            m1.soyadi = "taşdelen";
            m1.telefon_no = "0524394353";
            m1.musteri_no = "746373";
            m1.adresi = "ataköy 5. kısım";

            Musteri m2 = new KurumsalMusteri();
            m2.adi = "miray";
            m2.soyadi = "taşdelen";
            m2.telefon_no = "052339563831";
            m2.musteri_no = "983982363";
            m2.adresi = "üsküdar";

            if (m2 is KurumsalMusteri)
            {
                ((KurumsalMusteri)m2).SirketAdresi = "Ataköy";
                ((KurumsalMusteri)m2).SirketIsmi = "QuantumLojik";
            }

            BankaHesabi hsp1 = new VadesizHesap(m1);

            hsp1.ParaYatirma(500);
            hsp1.ParaCekme(100);
            hsp1.ParaCekme(40000);

            Banka.HesapOzetiGoster(p: hsp1);

            ///////////////////////////////////////////////////

            BankaHesabi hsp2 = new VadeliHesap(m2, 15, DateTime.Now.AddMonths(1));

            hsp2.ParaYatirma(100);
            hsp2.ParaCekme(500);


            Banka.HesapOzetiGoster(p: hsp2);

            ///////////////////////////////////////////////////

            BankaHesabi hsp3 = new VadesizDovizHesabi(m1, "dolar");

            hsp3.ParaYatirma(1000);
            hsp3.ParaCekme(500);
            hsp3.ParaYatirma(2000);


            Banka.HesapOzetiGoster(p: hsp3);


        }
    }
}
