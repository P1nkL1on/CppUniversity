using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWENT
{
    public static class LOGS{
        static List<string> list = new List<string>();
        public static void deployMessage(string name) { list.Add(String.Format("{0} deployed;", name)); }
    }
    public enum Rarity
    {
        bronze = 0,
        silver = 1,
        gold = 2
    }

    public enum Tag
    {

        Neutral = 0,
        NothernRealms = 1,
        Scoiatael = 2,
        Monsters = 3,
        Skelliege = 4,
        Nilfgaard = 5,

        Temeria = 51,
        Aedrian = 52,
        Redanie = 53,

        Mage = 101,
        Solder = 102,
        Officer = 103,
        Witcher = 104,

        Machine = 200,
        Support = 201,
        Construct = 202,
        
        Beast = 240, 
        Relict = 241
    }

    public enum Cards
    {
        None = -1,
        // nothern bronze
        BloodFlail = 501,
        Specter = 502,
        Winch = 503,
        ReaverScout = 504,
        TormentedMage = 505,
        AretuzeAdept = 506,
        BlueStripeCommando = 507,
        BlueStripeScout = 508,
        TemerianInfantry = 509,
        WitchHunter = 510,
        DamnedSorceress = 511,
        DunBanner = 512,
        KaedweniRevenant = 513,
        TemerianDrummer = 514,
        Ballista = 515,
        BatteringRam = 516,
        PoorInfantry = 517,
        LeftInfantry = 518,
        RightInfantry = 519,
        ReaverHunter = 520,
        SiegeMaster = 521,
        AedirianMauler = 522,
        RedanianKnight = 523,
        RedanianKnightElect = 524,
        ReinforcedBallista = 525,
        SiegeSupport = 526,
        Trebuchet = 527,
        CursedKnight = 528,
        FieldMedic = 529,
        KaedweniCavalry = 530,
        KaedweniKnight = 531,
        RedanianElite = 532,
        TridamInfantry = 533,
    }
}
