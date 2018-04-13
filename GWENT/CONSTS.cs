using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWENT
{
    
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
        Kaedwen = 54,

        Mage = 101,
        Solder = 102,
        Officer = 103,
        Witcher = 104,

        Machine = 200,
        Support = 201,
        Construct = 202,
        
        Beast = 240, 
        Relict = 241,

        Special = 20,
        Item = 21,
        Organic = 22,
        Alchemy = 23,
        Tactics = 24,

        Token = 40,
        Cursed = 41,
        Doomed = 42,
        Hazard = 43
    }

    public enum Cards
    {
        Any = 0,
        None = -1,
        // nothern bronze
        BloodFlail = 501,
        Specter = 502,
        Winch = 503,
        ReaverScout = 504,          //!
        TormentedMage = 505,
        AretuzeAdept = 506,
        BlueStripeCommando = 507,
        BlueStripeScout = 508,
        TemerianInfantry = 509,
        WitchHunter = 510,
        DamnedSorceress = 511,
        DunBanner = 512,
        KaedweniRevenant = 513,
        TemerianDrummer = 514,               //!
        Ballista = 515,
        BatteringRam = 516,
        PoorInfantry = 517,
        LeftInfantry = 518,
        RightInfantry = 519,
        ReaverHunter = 520,
        SiegeMaster = 521,
        AedirianMauler = 522,               //!
        RedanianKnight = 523,               //!
        RedanianKnightElect = 524,               //!
        ReinforcedBallista = 525,
        SiegeSupport = 526,
        Trebuchet = 527,
        CursedKnight = 528,
        FieldMedic = 529,
        KaedweniCavalry = 530,
        KaedweniKnight = 531,
        RedanianElite = 532,
        TridamInfantry = 533,               //!
        ReinforcedTrebuchet = 534,               //!
        LeftFlankInfantry = 535,
        RightFlankInfantry = 536,

        DandelionPoet = 1000,               //!


        BitingFrost = 2000,
        InpenetrableFog = 2001,
        TorrentioalRain = 2002
    }
}
