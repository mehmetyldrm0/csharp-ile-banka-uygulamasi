using System;


namespace Banka_v1
{
    public abstract class Musteri
    {
        // Sınıfın Üyeleri, Class Members

        public string adi;
        public string soyadi;
        public string musteri_no;
        public string adresi;
        public string telefon_no;

        public abstract void HesapAcmak();

        public abstract void ParaTransferi();

        public abstract void KendiHesabinaParaYatirma();

        public abstract void KendiHesabindanParaCekme();
        
    }

    public class BireyselMusteri : Musteri
    {
        public override void HesapAcmak()
        { 
        }

        public override void ParaTransferi()
        { 
        }

        public override void KendiHesabinaParaYatirma()
        { 
        }

        public override void KendiHesabindanParaCekme()
        { 
        }
    }

    public sealed class KurumsalMusteri : Musteri
    { 
        public string SirketIsmi
        {
            get;
            set;
        }

        public string SirketAdresi
        {
            get;
            set;
        }

        public override void HesapAcmak()
        {
        }

        public override void ParaTransferi()
        {
        }

        public override void KendiHesabinaParaYatirma()
        {
        }

        public override void KendiHesabindanParaCekme()
        {
        }
    }

    //public class OzelKurumsal : KurumsalMusteri
    //{ 
        // KurumsalMusteri sealed olduğu için türetmeye kapalıdır!
    //}
}
