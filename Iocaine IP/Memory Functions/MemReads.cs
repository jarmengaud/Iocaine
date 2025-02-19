﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using Iocaine2.Logging;

namespace Iocaine2.Memory
{
    public class MemReads
    {
        #region Settings
        public static int OS_Version = 5;
        #endregion Settings
        #region Enums
        public enum SKILL_CTRL
        {
            UNCAPPED_1_BYTE = 0,
            UNCAPPED_2_BYTE = 1,
            CAPPED_1_BYTE = 128,
            CAPPED_2_BYTES = 129
        }
        public enum FISHING_ARROW_DIR : byte
        {
            RIGHT = 0,
            LEFT = 1
        }
        private const byte CHAT_BUFFER_LENGTH = 50;
        #endregion
        #region Structures
        public class Pointers
        {
            public Process MainProcess;
            public ProcessModule MainModule;
            public uint Info_Player1;
            public uint Info_Player2;
            public uint Info_Player3;
            public uint Info_Player5;
            public uint Info_Target;
            public uint Info_Fishing;
            public uint Info_Windows;
            public uint Info_Windows2;
            public uint Info_Inventory;
            public uint Info_InventorySecWnd;
            public uint Info_InventoryNpcWnd;
            public uint Info_Chatlog;
            public uint Info_ServerList;
            #region Inventory
            public uint Info_Inv_Bag;
            public const uint SizeOf_Inv_Container = 3564; //0xDEC
            public const uint SizeOf_Inv_Item = 44;
            private const uint Offset_Inv_Safe = 1 * SizeOf_Inv_Container;
            public uint Info_Inv_Safe
            {
                get
                {
                    uint info_bag = Info_Inv_Bag;
                    if (info_bag != 0)
                    {
                        return info_bag + Offset_Inv_Safe;
                    }
                    return 0;
                }
            }
            private const uint Offset_Inv_Storage = 2 * SizeOf_Inv_Container;
            public uint Info_Inv_Storage
            {
                get
                {
                    uint info_bag = Info_Inv_Bag;
                    if (info_bag != 0)
                    {
                        return info_bag + Offset_Inv_Storage;
                    }
                    return 0;
                }
            }
            private const uint Offset_Inv_Locker = 4 * SizeOf_Inv_Container;
            public uint Info_Inv_Locker
            {
                get
                {
                    uint info_bag = Info_Inv_Bag;
                    if (info_bag != 0)
                    {
                        return info_bag + Offset_Inv_Locker;
                    }
                    return 0;
                }
            }
            private const uint Offset_Inv_Satchel = 5 * SizeOf_Inv_Container;
            public uint Info_Inv_Satchel
            {
                get
                {
                    uint info_bag = Info_Inv_Bag;
                    if (info_bag != 0)
                    {
                        return info_bag + Offset_Inv_Satchel;
                    }
                    return 0;
                }
            }
            private const uint Offset_Inv_Sack = 6 * SizeOf_Inv_Container;
            public uint Info_Inv_Sack
            {
                get
                {
                    uint info_bag = Info_Inv_Bag;
                    if (info_bag != 0)
                    {
                        return info_bag + Offset_Inv_Sack;
                    }
                    return 0;
                }
            }
            private const uint Offset_Inv_Case = 7 * SizeOf_Inv_Container;
            public uint Info_Inv_Case
            {
                get
                {
                    uint info_bag = Info_Inv_Bag;
                    if (info_bag != 0)
                    {
                        return info_bag + Offset_Inv_Case;
                    }
                    return 0;
                }
            }
            private const uint Offset_Inv_Wardrobe = 8 * SizeOf_Inv_Container;
            public uint Info_Inv_Wardrobe
            {
                get
                {
                    uint info_bag = Info_Inv_Bag;
                    if (info_bag != 0)
                    {
                        return info_bag + Offset_Inv_Wardrobe;
                    }
                    return 0;
                }
            }
            private const uint Offset_Inv_Safe2 = 9 * SizeOf_Inv_Container;
            public uint Info_Inv_Safe2
            {
                get
                {
                    uint info_bag = Info_Inv_Bag;
                    if (info_bag != 0)
                    {
                        return info_bag + Offset_Inv_Safe2;
                    }
                    return 0;
                }
            }
            private const uint Offset_Inv_Wardrobe2 = 10 * SizeOf_Inv_Container;
            public uint Info_Inv_Wardrobe2
            {
                get
                {
                    uint info_bag = Info_Inv_Bag;
                    if (info_bag != 0)
                    {
                        return info_bag + Offset_Inv_Wardrobe2;
                    }
                    return 0;
                }
            }
            private const uint Offset_Inv_Wardrobe3 = 11 * SizeOf_Inv_Container;
            public uint Info_Inv_Wardrobe3
            {
                get
                {
                    uint info_bag = Info_Inv_Bag;
                    if (info_bag != 0)
                    {
                        return info_bag + Offset_Inv_Wardrobe3;
                    }
                    return 0;
                }
            }
            private const uint Offset_Inv_Wardrobe4 = 12 * SizeOf_Inv_Container;
            public uint Info_Inv_Wardrobe4
            {
                get
                {
                    uint info_bag = Info_Inv_Bag;
                    if (info_bag != 0)
                    {
                        return info_bag + Offset_Inv_Wardrobe4;
                    }
                    return 0;
                }
            }
            private const uint Offset_Inv_Max = 65577;
            //Pre 05.08.22  47757; (5th-8th wardrobes added)
            //Pre 04.04.16  37065; (2nd wardrobe added)
            //Pre 05.13.15  33501; (2nd safe added)
            //Pre 05.14.14  29937;
            //Pre 10.??.13  26373;
            public uint Info_Inv_Max
            {
                get
                {
                    uint info_bag = Info_Inv_Bag;
                    if (info_bag != 0)
                    {
                        return info_bag + Offset_Inv_Max;
                    }
                    return 0;
                }
            }
            public const uint Offset_EquippedTable = 66028;
            //Pre 05.08.22  48188; (5th-8th wardrobes added)
            //Pre 04.04.16  37488; (2nd wardrobe added)
            //Pre 05.13.15  33920; (2nd safe added)
            //Pre 05.14.14  30356;
            //Pre 10.??.13  26788;
            public uint Info_EquippedTable
            {
                get
                {
                    uint info_bag = Info_Inv_Bag;
                    if (info_bag != 0)
                    {
                        return info_bag + Offset_EquippedTable;
                    }
                    return 0;
                }
            }
            #endregion Inventory
            public uint Info_CraftWindow;
            public uint Info_TradeNpcWindow;
            private const uint Offset_TradePcWindow = 25404;
            public uint Info_TradePcWindow
            {
                get
                {
                    uint info_bag = Info_Inv_Bag;
                    if (info_bag != 0)
                    {
                        return info_bag + Offset_TradePcWindow;
                    }
                    return 0;
                }
            }
            public uint Info_MapPcBegin;
            public uint Info_MapPcEnd;
            public uint Info_TargetLock;
            public uint Info_RecastSpell;
            public uint Info_RecastAbility;
            public const uint Offset_ShopBuyWindow = 18008;
            //Pre 09.16.15: 17996;
            //Pre 11.10.14: 17984;
            //Pre 10.06.14: 17976;
            //Pre 08.11.14: 17972
            //Pre 06.17.14: 17932;
            //Pre 12.11.13: 17916; //0x45FC
            public uint Info_ShopBuyWindowPtr;
            public uint Info_ShopBuyWindow
            {
                get
                {
                    return Info_ShopBuyWindowPtr + Offset_ShopBuyWindow;
                }
                set
                {
                    Info_ShopBuyWindow = value;
                }
            }
            public uint Info_Time;
            public uint Info_Network;
            public uint Info_Party;
            public uint Info_MenuBase;
            public uint Info_MenuBaseTextStyle;
            public uint Info_AhBidPrice;
            public uint Info_AhList;
            public uint Info_MapNpcBegin;
            public uint Info_MapNpcEnd;
            public uint Info_Casting;
            public ushort Offset_SpeedValue;
            public uint Info_SpeedText0Addr;
            public byte[] Info_SpeedText0Opcode;
            public uint Info_SpeedText1Addr;
            public byte[] Info_SpeedText1Opcode;
            public uint Info_SpeedText2Addr;
            public byte[] Info_SpeedText2Opcode;
        }
        internal class Signature
        {
            public string Prefix = "";
            public string Postfix = "";
            public byte PatternLength = 4;
            public int PatternOffset = 0;
            public int ResultOffset = 0;

            public Signature(string iPrefix, string iPostfix, int iResultOff, byte iPatternLength, int iPatternOff)
            {
                init(iPrefix, iPostfix, iPatternLength, iPatternOff, iResultOff);
            }
            public Signature(string iPrefix, string iPostfix, int iResultOff)
            {
                init(iPrefix, iPostfix, 4, 0, iResultOff);
            }
            public Signature(string iPrefix, string iPostfix)
            {
                init(iPrefix, iPostfix, 4, 0, 0);
            }
            public Signature(string iPrefix)
            {
                init(iPrefix, "", 4, 0, 0);
            }
            public Signature(string iPrefix, int iPatternOff)
            {
                init(iPrefix, "", 4, iPatternOff, 0);
            }
            private void init(string iPrefix, string iPostfix, byte iPatternLength, int iPatternOff, int iResultOff)
            {
                Prefix = iPrefix;
                Postfix = iPostfix;
                PatternLength = iPatternLength;
                PatternOffset = iPatternOff;
                ResultOffset = iResultOff;
            }
        }
        private static class Signatures
        {
            public static Signature AhBidPrice = new Signature("8B-0D", "33-C0-84-DB-57-68");
            public static Signature AhList = new Signature("8B-35", "3B-F7-74-..-8B-46-..-3B-C7");
            public static Signature Bag = new Signature("8A-0D", "B0-01-84-C8-75-08-0A-C8-88-0D", 39032);
            public static Signature Casting = new Signature("A1", "85-C0-74-..-8B-48-..-85-C9-74");
            public static Signature Chatlog = new Signature("8B-0D", "85-C9-74-0F", 4);
            public static Signature CraftWindow = new Signature("56-8B-F1-8B-0D", "85-C9-74-..-E8-..-..-..-..-84-C0-74");
            public static Signature Fishing = new Signature("8B-0F-2B-C8-89-0F-A1", "8B-48-1C");
            public static Signature Inventory = new Signature("56-8B-F1-8B-0D", "E8-..-..-..-..-50-E8-..-..-..-..-83-C4-04-83-F8-03");
            /*  === InventorySecWnd ===
                101D540B   A1 887B6210      MOV EAX,DWORD PTR DS:[10627B88]
                101D5410   6A 00            PUSH 0
                101D5412   57               PUSH EDI
                101D5413   C640 70 00       MOV BYTE PTR DS:[EAX+70],0
                101D5417   C640 71 00       MOV BYTE PTR DS:[EAX+71],0
                101D541B   8B0D 887B6210    MOV ECX,DWORD PTR DS:[10627B88]

                Pre 05.08.22 - ("83-C4-04-8B-4C-24-10-51-8B-0D", "")
            */
            public static Signature InventorySecWnd = new Signature("A1-..-..-..-..-6A-00-57-C6-40-70-00-C6-40-71-00-8B-0D", "");
            public static Signature InventoryNpcWnd = new Signature("75-7C-A1-..-..-..-..-8B-0D", "50-E8-..-..-..-..-C6-05");
            public static Signature MapNpcBegin = new Signature("83-EC-0C-56-8B-F1-8B-46-04-8B-04-85", "85-C0-74-68");
            public static Signature MapPcBegin = new Signature("A1", "85-C0-BF-FF-03-00-00-74-0E-4F-74-0B-8B-04-BD");
            public static Signature MenuBase = new Signature("8B-51-..-85-D2-74-..-3B-05", "75-..-8B-11-6A-01");
            public static Signature MenuBaseTextStyle = new Signature("A0", "56-84-C0-57-8B-F1-0F-84");
            public static Signature Party = new Signature("8B-4C-24-..-8B-C1-C1-E0-05-2B-C1-C1-E0-02-8A-88-..-..-..-..-84-C9-74-09-8D-80", "");
            public static Signature Player1 = new Signature("89-4C-24-..-89-54-24-..-8D-B8", "");
            public static Signature Player2 = new Signature("D9-05", "D8-66-0C");
            public static Signature Player3 = new Signature("85-C9-89-0D", "74-45");
            public static Signature Player5 = new Signature("8B-0D", "47-81-E1-FF-00-00-00-3B-F9", 8);
            public static Signature RecastAbility = new Signature("55-57-89-41-..-52-B9", "E8");
            public static Signature ServerList = new Signature("66-C7-81-30-01-00-00-FF-FF-8B-15", "89-82-28-01-00-00-8B-0D", -6384);
            public static Signature ShopBuyWindow = new Signature("8A-0D", "B0-01-84-C8-75-08-0A-C8-88-0D");
            public static Signature SpeedText0 = new Signature("DB-44-24-..-33-..-33-..-D8-0D-..-..-..-..", "8B-4[7F]-..-C1-E[89]", 0, 6, 0);
            public static Signature SpeedText1 = new Signature("D8-0D-..-..-..-..", "66-01-9E-..-..-00-00-5F-5E-5B-83-C4-..-C3", 0, 6, 0);
            public static Signature SpeedText2 = new Signature("D8-0D-..-..-..-..-8B-0C-85-..-..-..-..-33-C0", "8A-56-..-66-8B-46", 0, 6, 0);
            public static Signature Target = new Signature("C7-40-68-80-80-80-80-8B-0D", "E8");
            public static Signature TargetLock = new Signature("8B-0D", "53-56-33-F6-33-DB-57-85-C9");
            public static Signature Time = new Signature("B0-01-5E-C3-90-51-8B-4C-24-08-8D-44-24-00-50-68", "", 0xC, 4, 36);
            public static Signature TradeNpcWindow = new Signature("8A-4E-02-51-8B-0D", "E8");
            public static Signature Windows = new Signature("49-74-1D-8B-0D", "85-C9-74-13");
            #region === Notes on various signatures ===
            #region General
            /*
             *  If you know what you're looking for, the easiest thing to do is to:
             *  1. Open OllyDbg,
             *  2. Open and run FFXIMain.dll for a few clicks of the play button.  This loads all of the pointers and data.
             *  3. Find FFXIMain.dll in memory and open the memory dump.
             *  4. ** Save this as a text file, it's under the right click --> copy to file menu.
             *  5. Open this output file in Notepad++. This lets you search both the data (address) as well as the type of operation you're looking for.
             *      It also has regex capabilities which is much easier than trying to do it in ArtMoney or OllyDbg directly.
            */
            #endregion General
            #region CraftWindow
            /*
             *  Signature as of 11/23/21:
             *  101DD740   56               PUSH ESI
             *  101DD741   8BF1             MOV ESI,ECX
             *  101DD743   8B0D 90436210    MOV ECX,DWORD PTR DS:[10624390]  <-- pointer of interest
             *  101DD749   85C9             TEST ECX,ECX
             *  101DD74B   74 25            JE SHORT FFXiMain.101DD772
             *  101DD74D   E8 5EECFFFF      CALL FFXiMain.101DC3B0
             *  101DD752   84C0             TEST AL,AL
             *  101DD754   74 1C            JE SHORT FFXiMain.101DD772
            */
            #endregion CraftWindow
            #region SpeedText
            /*  The speed text was difficult to find because it's to a minor offset instead of a pointer location.
             *  The place to start is to find the speed offset in the NPC structure. Currently it's 0x158.
             *  SW will use this a number of places in the text segment.
             *  It may be used as MOV DWORD PTR DS:[E**+158],E**  <= 89[89AB]. 58010000
             *  It may be used as FSTP DWORD PTR DS:[ESI+158]     <= D99. 58010000
             *  The latter is more likely as it guarantees this is a floating point operation which is what the speed value is.
             *  
             *  Multiple signatures are required as multiple agents change the value.
             *  1. SpeedText0 prevents the speed from being updated when changing gear, jobs, or zones.
             *  2. SpeedText1 prevents the speed from being updated when interacting with some NPC's like Home Points.
             *  3. SpeedText2 prevents the speed from being updated by a background thread that periodically refreshes the speed.
             *  
             *  Signature 0 as of 11/23/21:
             *  1009C3F0   DB4424 20        FILD DWORD PTR SS:[ESP+20]
             *  1009C3F4   D80D 00323210    FMUL DWORD PTR DS:[10323200]
             *  1009C3FA   D99E 58010000    FSTP DWORD PTR DS:[ESI+158]  <-- data captured is this 6 bytes.
             *  1009C400   8B47 2C          MOV EAX,DWORD PTR DS:[EDI+2C]
             *  1009C403   C1E8 11          SHR EAX,11
             *  
             *  1009C65D   DB4424 20        FILD DWORD PTR SS:[ESP+20]
             *  1009C661   33D2             XOR EDX,EDX
             *  1009C663   33C0             XOR EAX,EAX
             *  1009C665   D80D 00323210    FMUL DWORD PTR DS:[10323200]
             *  1009C66B   D99E 5C010000    FSTP DWORD PTR DS:[ESI+15C]
             *  1009C671   8B4F 2C          MOV ECX,DWORD PTR DS:[EDI+2C]
             *  1009C674   C1E9 11          SHR ECX,11
             *  1009C677   81E1 FF000000    AND ECX,0FF
             *  --------------------------------------------------------------------------------
             *  Signature 1 as of 11/23/21:
             *  100BAD91   D80D C0903210    FMUL DWORD PTR DS:[103290C0]
             *  100BAD97   D99A 58010000    FSTP DWORD PTR DS:[EDX+158]  <-- data captured is this 6 bytes.
             *  100BAD9D   66:019E 56020000 ADD WORD PTR DS:[ESI+256],BX
             *  100BADA4   5F               POP EDI
             *  100BADA5   5E               POP ESI
             *  100BADA6   5B               POP EBX
             *  100BADA7   83C4 08          ADD ESP,8
             *  100BADAA   C3               RETN
             *  
             *  Currently (5/8/22) we're still using the above signature (offset = 158) and it's still working.
             *  I'm not sure if it's correct or not. Maybe the data structure being referenced here did not change,
             *  but that doesn't make sense. It should always be referring to our data structure (NPC struct).
             *  102B81FE   DD45 14          FLD QWORD PTR SS:[EBP+14]
             *  102B8201   D998 5C010000    FSTP DWORD PTR DS:[EAX+15C]
             *  102B8207   DD45 0C          FLD QWORD PTR SS:[EBP+C]
             *  102B820A   D998 60010000    FSTP DWORD PTR DS:[EAX+160]
             *  102B8210   5D               POP EBP
             *  102B8211   C3               RETN
             *  --------------------------------------------------------------------------------
             *  Signature 2 as of 11/23/21:
             *  10098F9E   D80D 00323210    FMUL DWORD PTR DS:[10323200]
             *  10098FA4   8B0C85 B0444710  MOV ECX,DWORD PTR DS:[EAX*4+104744B0]
             *  10098FAB   33C0             XOR EAX,EAX
             *  10098FAD   D999 58010000    FSTP DWORD PTR DS:[ECX+158]  <-- data captured is this 6 bytes.
             *  10098FB3   8A56 1D          MOV DL,BYTE PTR DS:[ESI+1D]
             *  10098FB6   66:8B46 08       MOV AX,WORD PTR DS:[ESI+8]
             *  10098FBA   895424 40        MOV DWORD PTR SS:[ESP+40],EDX
             *  10098FBE   DB4424 40        FILD DWORD PTR SS:[ESP+40]
             *  
             *  (no change as of 5/8/22):
             *  1009924E   D80D 00323210    FMUL DWORD PTR DS:[10323200]
             *  10099254   8B0C85 607D4710  MOV ECX,DWORD PTR DS:[EAX*4+10477D60]
             *  1009925B   33C0             XOR EAX,EAX
             *  1009925D   D999 5C010000    FSTP DWORD PTR DS:[ECX+15C]  <-- Only the offset is different.
             *  10099263   8A56 1D          MOV DL,BYTE PTR DS:[ESI+1D]
             *  10099266   66:8B46 08       MOV AX,WORD PTR DS:[ESI+8]
             *  1009926A   895424 40        MOV DWORD PTR SS:[ESP+40],EDX
             *  1009926E   DB4424 40        FILD DWORD PTR SS:[ESP+40]
             *  
            */
            #endregion SpeedText
            #endregion === Notes on various signatures ===
            #region === Archived Signatures ===
            // == Bag ==
            //                                            39028     Pre 11.10.15
            //                                            39020     Pre 09.15.15
            //                                            39016     Pre 06.25.15
            //                                            39136     Pre 03.25.15
            //                                            39128     Pre 01.15.15
            //                                            39124     Pre 12.09.14
            //                                            32084     Pre 11.10.14
            //                                            32076     Pre 10.06.14
            //                                            31676     Pre 09.09.14: 31676
            //  "8A-0D", "B0-01-84-C8-75-08-0A-C8-88-0D") + 31352
            //  "8A-0D", "B0-01-84-C8-75-08-0A-C8-88-0D") + 32072     Pre 08.11.14
            // == Player5 ==
            //  "85-C0-75-8F-A0", "BF-01-00-00-00-3C-01", 8           Pre 01.15.15
            #endregion === Archived Signatures ===
        }
        #endregion Structures
        #region Members
        #region Variables
        private static bool pointersSet = false;
        private static bool pointerErrorDisplayed = false;
        public static int processIndex = -1;
        private static List<Pointers> processPointerList = new List<Pointers>();
        private static string speedOpcodeFile = "_SpeedOpcodes.txt";
        public static bool PosEnabled = false;
        private static string settingsDir = "Settings\\";
        private static bool writeSpeedOpcodeOnInitDone = false;
        #endregion Variables
        #region Properties
        public static bool PointersSet
        {
            get
            {
                return pointersSet;
            }
        }
        public static int ProcessIndex
        {
            get
            {
                return processIndex;
            }
            set
            {
                processIndex = value;
            }
        }
        public static List<Pointers> ProcessPointerList
        {
            get
            {
                return processPointerList;
            }
        }
        public static Process MainProcess
        {
            get
            {
                return processPointerList[processIndex].MainProcess;
            }
        }
        public static ProcessModule MainModule
        {
            get
            {
                return processPointerList[processIndex].MainModule;
            }
        }
        #endregion Properties
        #endregion Members
        #region Offsets
        #region Minor Offsets
        #region pc_map
        private const int off_pc_map_posh = 56;
        private const int off_pc_map_posx = 68;
        private const int off_pc_map_posz = 72;
        private const int off_pc_map_posy = 76;
        private const int off_pc_map_name = 124;
        private const int off_pc_map_pos_ptr = 160; //If this pointer is 0, it means the character info is no longer valid
                                                      //Pre 02.17.14:     168 - Change:    -8
                                                      //Pre 12.11.12:     160 - Change:    +8
        private const int off_pc_map_set_pos_1 = 52;
        private const int off_pc_map_set_pos_2 = 0x5C4;
        private const int off_pc_map_dist = 216;
        //Pre 06.24.15:     212;
        //Pre 02.18.15:     208; //Distance is actual distance squared
        //Pre 11.10.14:     176
        //Pre 02.17.14:     184 - Change:    -8;
        //Pre 03.26.13:     180 - Change:    +4;
        //Pre 12.11.12:     172 - Change:    +8
        //Pre 12.08.08:     184 - Change:   -12
        private const int off_pc_map_hp = 236;
        //Pre 12.09.15:     240;
        //Pre 06.24.15:     236;
        //Pre 02.18.15:     232;
        //Pre 11.10.14:     200;
        //Pre 02.17.14:     208 - Change:    -8;
        //Pre 03.26.13:     204 - Change:    +4;
        //Pre 12.11.12:     196 - Change:    +8;
        //Pre 12.08.08:     208 - Change:   -12
        private const int off_pc_map_status = 364;
        //Pre 05.08.22      360;
        //Pre 12.09.15:     368;
        //Pre 06.24.15:     364;
        //Pre 05.13.15:     360;
        //Pre 02.18.15:     356;
        //There are 2 status' +4 from each other. We take the first.
        //Pre 11.10.14:     324;
        //Pre 03.26.13:     320 - Change:    +4;
        //Pre 12.11.12:     316 - Change:    +4
        //Pre 07.11.11:     312 - Change:    +4
        //Pre 02.21.11:     308 - Change:    +4
        //Pre 12.08.08:     320 - Change:   -12
        #endregion pc_map
        #region Chat
        private const int off_chat_text_start = 57;
        private const int off_chat_logical_line_nb = 18;
        private const int off_chat_logical_line_nb_len = 8;
        #endregion Chat
        #region general
        //Structure offsets
        //There's an x/y/z pos just before the weather info (about 88 before).
        private const uint off_weather = 2652;
        //Pre 11.20.21: 2636;
        //Pre 02.09.16: 2628;
        //Pre 06.24.15: 2404;
        //Pre 02.18.15: 2276;
        //Pre 12.09.14: 2260;
        //Pre 11.10.14: 2196;
        //Pre 10.06.14: 2188;
        //Pre 08.11.14: 2156
        //Pre 06.17.14: 2148
        //Pre 05.14.14: 2152
        //Pre 03.17.14: 2056
        //Pre 03.26.13: 2052 - Change:      +4
        private const uint off_player_struct = off_weather + 64;
        //Pre 02.09.16: 2692;
        //Pre 06.24.15: 2116;
        private const uint off_zoning = 1120;
        //Pre 11.20.21: 1108;
        //Pre 02.09.16  1100;
        //Pre 06.24.15  876;
        //Pre 05.13.15  872;
        //Pre 02.18.15  748;
        //Pre 12.09.14  732;
        //Pre 11.10.14  656;
        //Pre 08.11.14  620
        //Pre 04.08.14  520
        private const uint off_target_lock = 92;
        //Pre 11.10.15  81;
        //Pre 06.24.15  49;
        //Pre 05.13.15  41;
        private const uint off_pc_map_begin_in_mog_house = 5208;
        //Pre 11.10.15  4184;
        //Pre 05.13.15  4180;
        private const uint off_camera_perspective = 160;
        //Pre 08.05.15  156;
        //Pre 02.18.15  160;
        //Pre 03.17.14  156
        //Pre 02.17.14  148
        //Pre 12.11.13  144
        private const uint off_map_grid = 340;
        //Pre 05.08.22  332;
        #endregion general
        #region Skills, Merits, and Status Effects
        #region Combat Skills
        private const int off_skill_h2h = 290;
        // Pre 11.20.21: 258
        // Pre 11.10.14: 256
        private const int off_skill_dagger = off_skill_h2h + 2;
        private const int off_skill_sword = off_skill_h2h + 4;
        private const int off_skill_great_sword = off_skill_h2h + 6;
        private const int off_skill_axe = off_skill_h2h + 8;
        private const int off_skill_great_axe = off_skill_h2h + 10;
        private const int off_skill_scythe = off_skill_h2h + 12;
        private const int off_skill_polearm = off_skill_h2h + 14;
        private const int off_skill_katana = off_skill_h2h + 16;
        private const int off_skill_great_katana = off_skill_h2h + 18;
        private const int off_skill_club = off_skill_h2h + 20;
        private const int off_skill_staff = off_skill_h2h + 22;
        private const int off_skill_archery = off_skill_h2h + 48;
        private const int off_skill_marksmanship = off_skill_h2h + 50;
        private const int off_skill_throwing = off_skill_h2h + 52;
        private const int off_skill_guarding = off_skill_h2h + 54;
        private const int off_skill_evasion = off_skill_h2h + 56;
        private const int off_skill_shield = off_skill_h2h + 58;
        private const int off_skill_parrying = off_skill_h2h + 60;
        private const int off_skill_divine = off_skill_h2h + 62;
        private const int off_skill_healing = off_skill_h2h + 64;
        private const int off_skill_enhancing = off_skill_h2h + 66;
        private const int off_skill_enfeebling = off_skill_h2h + 68;
        private const int off_skill_elemental = off_skill_h2h + 70;
        private const int off_skill_dark = off_skill_h2h + 72;
        private const int off_skill_summoning = off_skill_h2h + 74;
        private const int off_skill_ninjutsu = off_skill_h2h + 76;
        private const int off_skill_singing = off_skill_h2h + 78;
        private const int off_skill_string = off_skill_h2h + 80;
        private const int off_skill_wind = off_skill_h2h + 82;
        private const int off_skill_blue = off_skill_h2h + 84;
        private const int off_skill_geo = off_skill_h2h + 86;
        private const int off_skill_bell = off_skill_h2h + 88;
        #endregion Combat Skills
        #region Craft Skills
        private const int off_craft_fish = 384;
        //Pre 11.20/21:  352;
        //Pre 11.10.14:  350;
        private const int off_craft_wood = off_craft_fish + 2;
        //Pre 11.10.14:  352;
        private const int off_craft_smith = off_craft_fish + 4;
        //Pre 11.10.14:  354;
        private const int off_craft_gold = off_craft_fish + 6;
        //Pre 11.10.14:  356;
        private const int off_craft_cloth = off_craft_fish + 8;
        //Pre 11.10.14:  358;
        private const int off_craft_leather = off_craft_fish + 10;
        //Pre 11.10.14:  360;
        private const int off_craft_bone = off_craft_fish + 12;
        //Pre 11.10.14:  362;
        private const int off_craft_alch = off_craft_fish + 14;
        //Pre 11.10.14:  364;
        private const int off_craft_cook = off_craft_fish + 16;
        //Pre 11.10.14:  366;
        private const int off_craft_synergy = off_craft_fish + 18;
        //Pre 11.10.14:  368;
        #endregion Craft Skills
        #region Merits
        private const int off_merit_merits_current = 678;
        //Pre 11.20.21  664;
        //Pre 02.09.16  656;
        //Pre 05.13.15  652;
        //Pre 02.18.15  528;
        //Pre 11.10.14  524;
        //Pre 10.06.14  516;
        private const int off_merit_limit_points = 674;
        //Pre 11.20.21  662;
        //Pre 02.09.16  654;
        //Pre 05.13.14  650;
        //Pre 02.18.15  526;
        //Pre 11.10.14  522;
        //Pre 10.06.14  514;
        private const int off_merit_mode = 677;
        //Pre 11.20.21  665;
        //Pre 02.09.16  657;
        //Pre 05.13.15  653;
        //Pre 02.18.15  529;
        //Pre 11.10.14  525;
        //Pre 10.06.14  517;
        #endregion Merits
        #region Status Effects
        private const int off_status_effects = 1196;
        //Pre 11.20.21  1180;
        //Pre 02.09.16  1172;
        //Pre 06.24.15   948;
        //Pre 02.18.15   820;
        //Pre 12.09.14   804;
        //Pre 11.10.14   740;
        //Pre 10.06.14   732;
        //Pre 08.11.14   724;
        //Pre 02.17.14   596;
        #endregion Status Effects
        #endregion Skills, Merits, and Status Effects
        #region Item Details
        //Item Details
        private const int off_item_details_name_ptr = 3272; //max 3318
        //Prior to 04/08/14:                            3188
        //Prior to 10/??/13:                            3140
        //Prior to 7/8/13:                              3144
        private const int off_item_details_item_id = 44;
        private const int off_item_details_next_item = 3252;
        private const int off_item_details_total_items = 100;
        #endregion Item Details
        #region item details preloaded
        private const int off_item_details_pre_next_item = 328;
        private const int off_item_details_pre_total_items = 200;
        private const int off_item_details_pre_type = 4;
        private const int off_item_details_pre_flags = 6;
        private const int off_item_details_pre_stack_sze = 8;
        private const int off_item_details_pre_vld_targets = 10;
        private const int off_item_details_pre_skill = 12;
        private const int off_item_details_pre_level = 14;
        private const int off_item_details_pre_eqp_slots = 16;
        private const int off_item_details_pre_races = 18;
        private const int off_item_details_pre_jobs = 20;
        private const int off_item_details_pre_resource_id = 24;
        private const int off_item_details_pre_dmg = 28;
        private const int off_item_details_pre_dly = 30;
        private const int off_item_details_pre_cast_time = 34;
        private const int off_item_details_pre_max_charges = 38;
        private const int off_item_details_pre_name = 104;
        #endregion item details preloaded
        #region Inventory
        private const int off_inv_nb_above = 26;
        private const int off_inv_nb_of_last_item_on_screen = 28;
        private const int off_inv_count = 32;
        private const int off_inv_selected_position = 36;
        private const int off_inv_sec_wnd_nb_above = 26;
        private const int off_inv_sec_wnd_nb_of_last_item_on_screen = 28;
        private const int off_inv_sec_wnd_count = 32;
        private const int off_inv_sec_wnd_selected_position = 36;
        private const int off_inv_left_wnd_selected = 70;
        #endregion Inventory
        #region NPC Shop
        private const int off_npc_shop_nxt_item = 56;
        //Pre 08.11.14: 52;
        private const byte off_npc_shop_max_items = 16;
        private const short off_npc_shop_item_price = 4;
        private const int off_npc_shop_item_id = 16;
        private const byte off_gld_shop_max_items = 255;
        private const int off_gld_shop_nxt_item = 56;
        #endregion NPC Shop
        #region Crafter
        private const int off_crafter_item_id = 34;
        //Pre 11.15.14: 170;
        private const int off_crafter_qty = 54;
        //Pre 11.15.14: 190;
        private const int off_crafter_bag_idx = 21;
        //Pre 11.15.14: 157;
        #endregion Crafter
        #region Windows
        private const int off_wind_help_text = 104;
        //Pre 04.04.16: 72
        //Pre 07.11.14: 52
        private const int off_wind_top_left_text = 204;
        //Pre 07.11.14: 184
        #endregion Windows
        #region Shop Window
        private const int off_shop_item_idx_bot_item = 28;
        private const int off_shop_item_idx_top_item = 30;
        private const int off_shop_item_nb_items = 32;
        private const int off_shop_item_idx_in_wnd = 36;
        #endregion Shop Window
        #region AH
        private const int off_ah_bid_price = 40;
        private const int off_ah_list_ptr = 32;
        private const int off_ah_list_cnt = 8;
        private const int off_ah_list_unq_cnt = 12;
        private const int off_ah_list_item_sz = 76;
        private const int off_ah_list_item_id = 4;
        private const int off_ah_list_item_count_sngl = 8;
        private const int off_ah_list_item_count_stack = 12;
        private const int off_ah_list_item_stack_val = 0;
        #endregion AH
        #region Network
        private const int off_net_receive = -4;
        private const int off_net_send = 0;
        private const int off_net_perc = 4;
        #endregion Network
        #region Party
        private const int off_pty_name = 0;
        private const int off_pty_hp = 30;
        private const int off_pty_mp = 34;
        private const int off_pty_tp = 38;
        private const int off_pty_hpp = 42;
        private const int off_pty_mpp = 43;
        private const int off_pty_zone = 44;
        private const int off_pty_valid = 116;
        private const int off_pty_struct_size = 124;
        #endregion Party
        #region Target
        private const int off_target_dist = 216;
        //Pre 06.24.15:     212;
        //Pre 02.18.15:     208;
        //Pre 11.10.14:     176;
        //Pre 02.17.14:     184 - Change:    -8
        //Pre 03.26.13:     180 - Change:    +4
        //Pre 12.12.12:     172 - Change:    +8
        private const int off_target_sta = 364;
        //Pre 05.08.22:     360;
        //Pre 12.09.15:     368;
        //Pre 06.24.15:     308;
        #endregion Target
        #region Fishing
        private const int off_fishing_rod_pos = 48;
        //Pre 03.17.14  -44 - System changed
        //Pre 02.17.14  -24
        #endregion Fishing
        #endregion Minor Offsets
        #region General Notes
        #region Blacklist
        //There is a structure for the player's blacklist which starts at about
        //info_bag + 26344 (5/1/09).
        #endregion Blacklist
        #region Equipment Table Notes
        //info_equpped_table is as such:
        //offset 0 = main, +4 = index of gobbie bag where main is
        //offset 8 =  sub, +4 = index of gobbie bag where sub is
        //... so then go to info_bag offset and for each index offset, mult by 44
        //      to find item.
        //ex: to find ammo in bag, read mem at info_equipped_table + 28.
        //Result is index. Go to info_bag + (index * 44) to get item number.
        //Go to info_bag + (index * 44) + 4 to get number equipped.
        //Go to info_bag + (index * 44) + 8 to get equipped (5=equipped, 0 = not)
        //NOTE: an index of 0 means nothing equpped
        #endregion Equipment Table Notes
        #region Deprecated Item Details Notes/Pointers
        //private const uint info_item_details = 0x4DDB18;
        /// <summary>
        /// These are items that are in some inventory space that are preloaded. The Name and item ID are there
        /// before even opening your inventory.
        /// </summary>
        //private const uint info_item_details_preloaded = 0x52D168;
        #endregion Deprecated Item Details Notes/Pointers
        #endregion General Notes
        #endregion Offsets
        #region Set Structure Pointers
        public static int Set_FFXI_Pointers(Process iMainProcess, ProcessModule iMainModule)
        {
            int addedIndex = Add_FFXI_Pointers(iMainProcess, iMainModule);
            if ((addedIndex == -1) || (addedIndex >= processPointerList.Count))
            {
                pointersSet = false;
                return processIndex;
            }
            Pointers pntrStruct = processPointerList[addedIndex];
            pointersSet = true;
            processIndex = addedIndex;
            if (writeSpeedOpcodeOnInitDone)
            {
                Self.Speed.set_speed(false);
                writeSpeedOpcodeOnInitDone = false;
            }
            return processIndex;
        }
        public static int Add_FFXI_Pointers(Process iMainProcess, ProcessModule iMainModule)
        {
            #region Check Existing
            //If we're only adding a set of pointers from this process (not setting the current values)
            //then we need to:
            //1. check if the process has already been scanned.
            //   If it has, then just return the index of it.
            foreach (Pointers pntr in processPointerList)
            {
                if (pntr.MainProcess.Id == iMainProcess.Id)
                {
                    processIndex = processPointerList.IndexOf(pntr);
                    return processIndex;
                }
            }
            #endregion Check Existing
            #region Dump Memory
            //If we got to here, the process hasn't been scanned yet, so scan it now,
            //then save the pointers into a new structure and push it into the list, returning the index.
            MemScanner scanner = new MemScanner(iMainProcess, iMainModule);
            if (!scanner.DumpMemory())
            {
                scanner = null;
                return -1;
            }
            #endregion Dump Memory
            #region Scan for pointers
            Pointers pntrStruct = new Pointers();
            pntrStruct.Info_AhBidPrice = scanner.ScanPattern(Signatures.AhBidPrice).UInt32;
            pntrStruct.Info_AhList = scanner.ScanPattern(Signatures.AhList).UInt32;
            pntrStruct.Info_Casting = scanner.ScanPattern(Signatures.Casting).UInt32;
            pntrStruct.Info_Chatlog = scanner.ScanPattern(Signatures.Chatlog).UInt32;
            pntrStruct.Info_CraftWindow = scanner.ScanPattern(Signatures.CraftWindow).UInt32;
            pntrStruct.Info_Fishing = scanner.ScanPattern(Signatures.Fishing).UInt32;
            pntrStruct.Info_Inv_Bag = scanner.ScanPattern(Signatures.Bag).UInt32;
            pntrStruct.Info_Inventory = scanner.ScanPattern(Signatures.Inventory).UInt32;
            pntrStruct.Info_InventorySecWnd = scanner.ScanPattern(Signatures.InventorySecWnd).UInt32;
            pntrStruct.Info_InventoryNpcWnd = scanner.ScanPattern(Signatures.InventoryNpcWnd).UInt32;
            pntrStruct.Info_MapNpcBegin = scanner.ScanPattern(Signatures.MapNpcBegin).UInt32;
            pntrStruct.Info_MapPcBegin = scanner.ScanPattern(Signatures.MapPcBegin).UInt32;
            pntrStruct.Info_MenuBase = scanner.ScanPattern(Signatures.MenuBase).UInt32;
            pntrStruct.Info_MenuBaseTextStyle = scanner.ScanPattern(Signatures.MenuBaseTextStyle).UInt32;
            pntrStruct.Info_Party = scanner.ScanPattern(Signatures.Party).UInt32;
            pntrStruct.Info_Player1 = scanner.ScanPattern(Signatures.Player1).UInt32;
            pntrStruct.Info_Player2 = scanner.ScanPattern(Signatures.Player2).UInt32;
            pntrStruct.Info_Player3 = scanner.ScanPattern(Signatures.Player3).UInt32;
            pntrStruct.Info_Player5 = scanner.ScanPattern(Signatures.Player5).UInt32;
            pntrStruct.Info_RecastAbility = scanner.ScanPattern(Signatures.RecastAbility).UInt32;
            pntrStruct.Info_ServerList = scanner.ScanPattern(Signatures.ServerList).UInt32;
            pntrStruct.Info_ShopBuyWindowPtr = scanner.ScanPattern(Signatures.ShopBuyWindow).UInt32;
            pntrStruct.Info_Target = scanner.ScanPattern(Signatures.Target).UInt32;
            pntrStruct.Info_TargetLock = scanner.ScanPattern(Signatures.TargetLock).UInt32;
            pntrStruct.Info_Time = scanner.ScanPattern(Signatures.Time).UInt32;
            pntrStruct.Info_TradeNpcWindow = scanner.ScanPattern(Signatures.TradeNpcWindow).UInt32;
            pntrStruct.Info_Windows = scanner.ScanPattern(Signatures.Windows).UInt32;

            MemScanner.ScanResult speed0Rslt = scanner.ScanPattern(Signatures.SpeedText0);
            if (speed0Rslt.Success == true)
            {
                pntrStruct.Info_SpeedText0Addr = speed0Rslt.ResultAddress;
                pntrStruct.Info_SpeedText0Opcode = speed0Rslt.ResultPattern;
                MemScanner.ScanResult speed1Rslt = scanner.ScanPattern(Signatures.SpeedText1);
                pntrStruct.Info_SpeedText1Addr = speed1Rslt.ResultAddress;
                pntrStruct.Info_SpeedText1Opcode = speed1Rslt.ResultPattern;
                MemScanner.ScanResult speed2Rslt = scanner.ScanPattern(Signatures.SpeedText2);
                pntrStruct.Info_SpeedText2Addr = speed2Rslt.ResultAddress;
                pntrStruct.Info_SpeedText2Opcode = speed2Rslt.ResultPattern;
                pntrStruct.Offset_SpeedValue = (ushort)((pntrStruct.Info_SpeedText0Opcode[3] << 8) | pntrStruct.Info_SpeedText0Opcode[2]);
                PosEnabled = true;
            }

            pntrStruct.MainProcess = iMainProcess;
            pntrStruct.MainModule = iMainModule;
            #endregion Scan for pointers
            #region Other calculated pointers
            if (pntrStruct.Info_RecastAbility != 0)
            {
                pntrStruct.Info_RecastSpell = pntrStruct.Info_RecastAbility + 800;
                //Pre 05.08.22: 816;
                //Pre 04.04.16: 664;
                //Pre 06.24.15: 656;
                //Pre 03.25.15: 632;
                //Pre 02.18.15: 504;
            }
            if (pntrStruct.Info_ShopBuyWindow != 0)
            {
                pntrStruct.Info_Network = pntrStruct.Info_ShopBuyWindow - 12;
                // -20 pre 09.15.15
            }
            if (pntrStruct.Info_MapPcBegin != 0)
            {
                pntrStruct.Info_MapPcEnd = pntrStruct.Info_MapPcBegin + 0x1023;
            }
            if (pntrStruct.Info_MapNpcBegin != 0)
            {
                pntrStruct.Info_MapNpcEnd = pntrStruct.Info_MapNpcBegin + 0x1FFF;
            }
            if (pntrStruct.Info_Target != 0)
            {
                pntrStruct.Info_Windows2 = pntrStruct.Info_Target + 4;
            }
            #endregion Other calculated pointers
            #region Debug Messages
            LoggingFunctions.Debug("Final Info_AhBidPrice: " + string.Format("{0:X}", pntrStruct.Info_AhBidPrice) + " (" + string.Format("{0:X}", pntrStruct.Info_AhBidPrice - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_AhList: " + string.Format("{0:X}", pntrStruct.Info_AhList) + " (" + string.Format("{0:X}", pntrStruct.Info_AhList - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Casting: " + string.Format("{0:X}", pntrStruct.Info_Casting) + " (" + string.Format("{0:X}", pntrStruct.Info_Casting - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Chatlog: " + string.Format("{0:X}", pntrStruct.Info_Chatlog) + " (" + string.Format("{0:X}", pntrStruct.Info_Chatlog - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_CraftWindow: " + string.Format("{0:X}", pntrStruct.Info_CraftWindow) + " (" + string.Format("{0:X}", pntrStruct.Info_CraftWindow - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_EquippedTable: " + string.Format("{0:X}", pntrStruct.Info_EquippedTable) + " (" + string.Format("{0:X}", pntrStruct.Info_EquippedTable - (uint)iMainModule.BaseAddress) + ")" + " (Calculated)", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Fishing: " + string.Format("{0:X}", pntrStruct.Info_Fishing) + " (" + string.Format("{0:X}", pntrStruct.Info_Fishing - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Inv_Bag: " + string.Format("{0:X}", pntrStruct.Info_Inv_Bag) + " (" + string.Format("{0:X}", pntrStruct.Info_Inv_Bag - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Inv_Safe: " + string.Format("{0:X}", pntrStruct.Info_Inv_Safe) + " (" + string.Format("{0:X}", pntrStruct.Info_Inv_Safe - (uint)iMainModule.BaseAddress) + ")" + " (Calculated)", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Inv_Storage: " + string.Format("{0:X}", pntrStruct.Info_Inv_Storage) + " (" + string.Format("{0:X}", pntrStruct.Info_Inv_Storage - (uint)iMainModule.BaseAddress) + ")" + " (Calculated)", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Inv_Locker: " + string.Format("{0:X}", pntrStruct.Info_Inv_Locker) + " (" + string.Format("{0:X}", pntrStruct.Info_Inv_Locker - (uint)iMainModule.BaseAddress) + ")" + " (Calculated)", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Inv_Satchel: " + string.Format("{0:X}", pntrStruct.Info_Inv_Satchel) + " (" + string.Format("{0:X}", pntrStruct.Info_Inv_Satchel - (uint)iMainModule.BaseAddress) + ")" + " (Calculated)", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Inv_Sack: " + string.Format("{0:X}", pntrStruct.Info_Inv_Sack) + " (" + string.Format("{0:X}", pntrStruct.Info_Inv_Sack - (uint)iMainModule.BaseAddress) + ")" + " (Calculated)", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Inv_Case: " + string.Format("{0:X}", pntrStruct.Info_Inv_Sack) + " (" + string.Format("{0:X}", pntrStruct.Info_Inv_Case - (uint)iMainModule.BaseAddress) + ")" + " (Calculated)", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Inv_Wardrobe: " + string.Format("{0:X}", pntrStruct.Info_Inv_Sack) + " (" + string.Format("{0:X}", pntrStruct.Info_Inv_Wardrobe - (uint)iMainModule.BaseAddress) + ")" + " (Calculated)", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Inv_Safe2: " + string.Format("{0:X}", pntrStruct.Info_Inv_Sack) + " (" + string.Format("{0:X}", pntrStruct.Info_Inv_Wardrobe - (uint)iMainModule.BaseAddress) + ")" + " (Calculated)", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Inv_Wardrobe2: " + string.Format("{0:X}", pntrStruct.Info_Inv_Sack) + " (" + string.Format("{0:X}", pntrStruct.Info_Inv_Wardrobe - (uint)iMainModule.BaseAddress) + ")" + " (Calculated)", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Inv_Max: " + string.Format("{0:X}", pntrStruct.Info_Inv_Max) + " (" + string.Format("{0:X}", pntrStruct.Info_Inv_Max - (uint)iMainModule.BaseAddress) + ")" + " (Calculated)", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Inventory: " + string.Format("{0:X}", pntrStruct.Info_Inventory) + " (" + string.Format("{0:X}", pntrStruct.Info_Inventory - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_InventorySecWnd: " + string.Format("{0:X}", pntrStruct.Info_InventorySecWnd) + " (" + string.Format("{0:X}", pntrStruct.Info_InventorySecWnd - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_InventoryNpcWnd: " + string.Format("{0:X}", pntrStruct.Info_InventoryNpcWnd) + " (" + string.Format("{0:X}", pntrStruct.Info_InventoryNpcWnd - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_MapNpcBegin: " + string.Format("{0:X}", pntrStruct.Info_MapNpcBegin) + " (" + string.Format("{0:X}", pntrStruct.Info_MapNpcBegin - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_MapNpcEnd: " + string.Format("{0:X}", pntrStruct.Info_MapNpcEnd) + " (" + string.Format("{0:X}", pntrStruct.Info_MapNpcEnd - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_MapPcBegin: " + string.Format("{0:X}", pntrStruct.Info_MapPcBegin) + " (" + string.Format("{0:X}", pntrStruct.Info_MapPcBegin - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_MapPcEnd: " + string.Format("{0:X}", pntrStruct.Info_MapPcEnd) + " (" + string.Format("{0:X}", pntrStruct.Info_MapPcEnd - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Menu: " + string.Format("{0:X}", pntrStruct.Info_MenuBase) + " (" + string.Format("{0:X}", pntrStruct.Info_MenuBase - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Network: " + string.Format("{0:X}", pntrStruct.Info_Network) + " (" + string.Format("{0:X}", pntrStruct.Info_Network - (uint)iMainModule.BaseAddress) + ")" + " (Calculated)", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Party: " + string.Format("{0:X}", pntrStruct.Info_Party) + " (" + string.Format("{0:X}", pntrStruct.Info_Party - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Player1: " + string.Format("{0:X}", pntrStruct.Info_Player1) + " (" + string.Format("{0:X}", pntrStruct.Info_Player1 - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Player2: " + string.Format("{0:X}", pntrStruct.Info_Player2) + " (" + string.Format("{0:X}", pntrStruct.Info_Player2 - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Player3: " + string.Format("{0:X}", pntrStruct.Info_Player3) + " (" + string.Format("{0:X}", pntrStruct.Info_Player3 - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Player5: " + string.Format("{0:X}", pntrStruct.Info_Player5) + " (" + string.Format("{0:X}", pntrStruct.Info_Player5 - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_RecastAbility: " + string.Format("{0:X}", pntrStruct.Info_RecastAbility) + " (" + string.Format("{0:X}", pntrStruct.Info_RecastAbility - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_RecastSpell: " + string.Format("{0:X}", pntrStruct.Info_RecastSpell) + " (" + string.Format("{0:X}", pntrStruct.Info_RecastSpell - (uint)iMainModule.BaseAddress) + ")" + " (Calculated)", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_ServerList: " + string.Format("{0:X}", pntrStruct.Info_ServerList) + " (" + string.Format("{0:X}", pntrStruct.Info_ServerList - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_ShopBuyWindow: " + string.Format("{0:X}", pntrStruct.Info_ShopBuyWindow) + " (" + string.Format("{0:X}", pntrStruct.Info_ShopBuyWindow - (uint)iMainModule.BaseAddress) + ")" + " (Calculated)", LoggingFunctions.DBG_SCOPE.MEMREADS);
            if (PosEnabled == true)
            {
                LoggingFunctions.Debug("Final Info_SpeedText0Addr: " + string.Format("{0:X}", pntrStruct.Info_SpeedText0Addr) + " (" + string.Format("{0:X}", pntrStruct.Info_SpeedText0Addr - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
                LoggingFunctions.Debug("Final Info_SpeedText0Opcode: " + string.Format("{0:X}", BitConverter.ToString(pntrStruct.Info_SpeedText0Opcode)), LoggingFunctions.DBG_SCOPE.MEMREADS);
                LoggingFunctions.Debug("Final Info_SpeedText1Addr: " + string.Format("{0:X}", pntrStruct.Info_SpeedText1Addr) + " (" + string.Format("{0:X}", pntrStruct.Info_SpeedText1Addr - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
                LoggingFunctions.Debug("Final Info_SpeedText1Opcode: " + string.Format("{0:X}", BitConverter.ToString(pntrStruct.Info_SpeedText1Opcode)), LoggingFunctions.DBG_SCOPE.MEMREADS);
                LoggingFunctions.Debug("Final Info_SpeedText2Addr: " + string.Format("{0:X}", pntrStruct.Info_SpeedText2Addr) + " (" + string.Format("{0:X}", pntrStruct.Info_SpeedText2Addr - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
                LoggingFunctions.Debug("Final Info_SpeedText2Opcode: " + string.Format("{0:X}", BitConverter.ToString(pntrStruct.Info_SpeedText2Opcode)), LoggingFunctions.DBG_SCOPE.MEMREADS);
                LoggingFunctions.Debug("Final Offset_SpeedValue: " + string.Format("{0:X}", pntrStruct.Offset_SpeedValue) + " (dec=" + pntrStruct.Offset_SpeedValue + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            }
            LoggingFunctions.Debug("Final Info_Target: " + string.Format("{0:X}", pntrStruct.Info_Target) + " (" + string.Format("{0:X}", pntrStruct.Info_Target - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_TargetLock: " + string.Format("{0:X}", pntrStruct.Info_TargetLock) + " (" + string.Format("{0:X}", pntrStruct.Info_TargetLock - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Time: " + string.Format("{0:X}", pntrStruct.Info_Time) + " (" + string.Format("{0:X}", pntrStruct.Info_Time - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_TradeNpcWindow: " + string.Format("{0:X}", pntrStruct.Info_TradeNpcWindow) + " (" + string.Format("{0:X}", pntrStruct.Info_TradeNpcWindow - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_TradePcWindow: " + string.Format("{0:X}", pntrStruct.Info_TradePcWindow) + " (" + string.Format("{0:X}", pntrStruct.Info_TradePcWindow - (uint)iMainModule.BaseAddress) + ")" + " (Calculated)", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Windows: " + string.Format("{0:X}", pntrStruct.Info_Windows) + " (" + string.Format("{0:X}", pntrStruct.Info_Windows - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            LoggingFunctions.Debug("Final Info_Windows2: " + string.Format("{0:X}", pntrStruct.Info_Windows2) + " (" + string.Format("{0:X}", pntrStruct.Info_Windows2 - (uint)iMainModule.BaseAddress) + ")", LoggingFunctions.DBG_SCOPE.MEMREADS);
            #endregion Debug Messages
            #region Wrap Up
            if (PosEnabled == true)
            {
                checkSpeedOpcodes(pntrStruct);
            }
            if (Check_For_Null_Pointers(pntrStruct))
            {
                pointersSet = false;
                scanner = null;
                return -1;
            }
            else
            {
                processPointerList.Add(pntrStruct);
                scanner = null;
                return processPointerList.IndexOf(pntrStruct);
            }
            #endregion Wrap Up
        }
        /// <summary>
        /// Returns true if any of the key pointers are null, false otherwise.
        /// Also asserts a message detailing which bots might not be functioning based on the null pointers.
        /// </summary>
        /// <returns>Returns true if any of the key pointers are null, false otherwise.</returns>
        private static bool Check_For_Null_Pointers(Pointers iPntr)
        {
            string errMsg = "";
            bool fatalNull = false;
            bool fisher = false;
            bool pl = false;
            bool su = false;
            bool crafter = false;
            bool ta = false;
            bool nav = false;
            bool wms = false;
            bool trader = false;
            bool seller = false;
            bool buyer = false;
            bool synergizer = false;
            if ((iPntr.Info_Player1 == 0) ||
                (iPntr.Info_Player2 == 0) ||
                (iPntr.Info_Player3 == 0) ||
                (iPntr.Info_Player5 == 0) ||
                (iPntr.Info_Target == 0) ||
                (iPntr.Info_Fishing == 0) ||
                (iPntr.Info_Windows == 0) ||
                (iPntr.Info_Inventory == 0) ||
                (iPntr.Info_InventorySecWnd == 0) ||
                (iPntr.Info_Chatlog == 0) ||
                (iPntr.Info_MapPcBegin == 0) ||
                (iPntr.Info_CraftWindow == 0) ||
                (iPntr.Info_ServerList == 0) ||
                (iPntr.Info_TargetLock == 0) ||
                (iPntr.Info_RecastSpell == 0) ||
                (iPntr.Info_RecastAbility == 0) ||
                (iPntr.Info_Time == 0) ||
                (iPntr.Info_ShopBuyWindow == 0) ||
                (iPntr.Info_Party == 0))
            {

            }
            else
            {
                return false;
            }

            errMsg = "The following pointers (offsets) were not found like expected and may cause issues as noted:\n";
            //if( (iPntr.Info_BagPtr == 0) || (iPntr.Info_BagOffset == 0 ) )
            if (iPntr.Info_Inv_Bag == 0)
            {
                errMsg += "Info_Bag was not set.\n";
                fisher = true;
                crafter = true;
                wms = true;
                trader = true;
                seller = true;
                buyer = true;
            }
            if (iPntr.Info_Chatlog == 0)
            {
                errMsg += "Info_Chatlog pointer was not set.\n";
                fisher = true;
                crafter = true;
                seller = true;
                buyer = true;
            }
            if (iPntr.Info_CraftWindow == 0)
            {
                errMsg += "Info_CraftWindow pointer was not set.\n";
                crafter = true;
            }
            //if (iPntr.Info_EquippedTable == 0)
            //{
            //    errMsg += "Info_EquippedTable pointer was not set.\n";
            //}
            if (iPntr.Info_Fishing == 0)
            {
                errMsg += "Info_Fishing pointer was not set.\n";
                fisher = true;
            }
            if (iPntr.Info_Inventory == 0)
            {
                errMsg += "Info_Inventory pointer was not set.\n";
                fisher = true;
                crafter = true;
                trader = true;
                seller = true;
                buyer = true;
            }
            //if (iPntr.Info_InventoryMax == 0)
            //{
            //    errMsg += "Info_InventoryMax pointer was not set.\n";
            //    fisher = true;
            //    crafter = true;
            //    trader = true;
            //    seller = true;
            //    buyer = true;
            //}
            //if (iPntr.Info_InventoryNpcWnd == 0)
            //{
            //    errMsg += "Info_InventoryNpcWnd pointer was not set.\n";
            //    buyer = true;
            //}
            if (iPntr.Info_InventorySecWnd == 0)
            {
                errMsg += "Info_InventorySecWnd pointer was not set.\n";
                fisher = true;
                seller = true;
                buyer = true;
            }
            if (iPntr.Info_Inv_Locker == 0)
            {
                errMsg += "Info_Locker pointer was not set.\n";
                wms = true;
            }
            if (iPntr.Info_MenuBase == 0)
            {
                errMsg += "Info_Menu pointer was not set.\n";
                nav = true;
            }
            if (iPntr.Info_Network == 0)
            {
                errMsg += "Info_Network pointer was not set.\n";
            }
            //if (iPntr.Info_NpcGuildBuyWindow == 0)
            //{
            //    errMsg += "Info_NpcGuildBuyWindow pointer was not set.\n";
            //    buyer = true;
            //}
            //if (iPntr.Info_NpcShopBuyWindow == 0)
            //{
            //    errMsg += "Info_NpcShopBuyWindow pointer was not set.\n";
            //    buyer = true;
            //}
            if (iPntr.Info_Party == 0)
            {
                errMsg += "Info_Party pointer was not set.\n";
            }
            if (iPntr.Info_MapPcBegin == 0)
            {
                errMsg += "Info_PcMapBegin pointer was not set.\n";
                pl = true;
            }
            if (iPntr.Info_Player1 == 0)
            {
                //Player 1 is used almost everywhere. Safe to say we can't work without it.
                errMsg += "Info_Player1 pointer was not set.\n";
                fatalNull = true;
            }
            if (iPntr.Info_Player2 == 0)
            {
                errMsg += "Info_Player2 pointer was not set.\n";
                fatalNull = true;
            }
            if (iPntr.Info_Player3 == 0)
            {
                errMsg += "Info_Player3 pointer was not set.\n";
                fisher = true;
                pl = true;
                ta = true;
                nav = true;
            }
            if (iPntr.Info_Player5 == 0)
            {
                errMsg += "Info_Player5 pointer was not set.\n";
                fisher = true;
                pl = true;
                su = true;
                ta = true;
            }
            if (iPntr.Info_RecastAbility == 0)
            {
                errMsg += "Info_RecastAbility pointer was not set.\n";
            }
            if (iPntr.Info_RecastSpell == 0)
            {
                errMsg += "Info_RecastSpell pointer was not set.\n";
            }
            if (iPntr.Info_Inv_Sack == 0)
            {
                errMsg += "Info_Sack pointer was not set.\n";
                fisher = true;
                trader = true;
                seller = true;
                buyer = true;
                wms = true;
            }
            if (iPntr.Info_Inv_Safe == 0)
            {
                errMsg += "Info_Safe pointer was not set.\n";
                wms = true;
            }
            if (iPntr.Info_Inv_Satchel == 0)
            {
                errMsg += "Info_Satchel pointer was not set.\n";
                fisher = true;
                trader = true;
                seller = true;
                buyer = true;
                wms = true;
            }
            if (iPntr.Info_ServerList == 0)
            {
                errMsg += "Info_Satchel pointer was not set.\n";
                fatalNull = true;
            }
            if (iPntr.Info_Inv_Storage == 0)
            {
                errMsg += "Info_Storage pointer was not set.\n";
                wms = true;
            }
            if (iPntr.Info_Target == 0)
            {
                errMsg += "Info_Target pointer was not set.\n";
                pl = true;
                ta = true;
                nav = true;
                trader = true;
                seller = true;
                buyer = true;
            }
            if (iPntr.Info_TargetLock == 0)
            {
                errMsg += "Info_TargetLock pointer was not set.\n";
                ta = true;
                nav = true;
            }
            if (iPntr.Info_Time == 0)
            {
                errMsg += "Info_Time pointer was not set.\n";
                fatalNull = true;
            }
            if (iPntr.Info_TradeNpcWindow == 0)
            {
                errMsg += "Info_TradeNpcWindow pointer was not set.\n";
                nav = true;
                trader = true;
            }
            if (iPntr.Info_TradePcWindow == 0)
            {
                errMsg += "Info_TradePcWindow pointer was not set.\n";
            }
            if (iPntr.Info_Windows == 0)
            {
                errMsg += "Info_Windows pointer was not set.\n";
                fisher = true;
                crafter = true;
                nav = true;
                trader = true;
                seller = true;
                buyer = true;
            }
            if (iPntr.Info_Windows2 == 0)
            {
                errMsg += "Info_Windows2 pointer was not set.\n";
                fisher = true;
                seller = true;
                buyer = true;
            }
            if (iPntr.Info_MenuBaseTextStyle == 0)
            {
                errMsg += "Info_TextStyleMenuBase pointer was not set.\n";
                synergizer = true;
            }

            errMsg += "The following modules/bots will not function properly:\n";
            if (fisher)
            {
                errMsg += "Fisher\n";
            }
            if (pl)
            {
                errMsg += "Power Leveler\n";
            }
            if (su)
            {
                errMsg += "Skill Up\n";
            }
            if (crafter)
            {
                errMsg += "Crafter\n";
            }
            if (ta)
            {
                errMsg += "TA\n";
            }
            if (nav)
            {
                errMsg += "Navigation\n";
            }
            if (trader)
            {
                errMsg += "Trader\n";
            }
            if (seller)
            {
                errMsg += "Seller\n";
            }
            if (buyer)
            {
                errMsg += "Buyer\n";
            }
            if (wms)
            {
                errMsg += "WMS\n";
            }
            if (synergizer)
            {
                errMsg += "Synergizer\n";
            }

            if (!pointerErrorDisplayed)
            {
                pointerErrorDisplayed = true;
                MessageBox.Show(errMsg);
            }
            return fatalNull;
        }
        #endregion Set Structure Pointers
        #region Utility Functions
        public static int IndexOf(Process iProc)
        {
            for (int ii = 0; ii < processPointerList.Count; ii++)
            {
                if (iProc == processPointerList[ii].MainProcess)
                {
                    return ii;
                }
            }
            return -1;
        }
        public static void logMemoryBlock(ProcessModule iMod, uint iStartAddr, uint iEndAddr, uint iByteWidth, bool iLogHex)
        {
            try
            {
                uint bytes = iEndAddr - iStartAddr;
                byte[] buffer = new byte[bytes];
                Process proc = processPointerList[processIndex].MainProcess;
                buffer = MemoryFunctions.ReadBlock((IntPtr)proc.Handle, iStartAddr, buffer, bytes);
                LoggingFunctions.Timestamp("StartAddr: " + string.Format("{0:X}", iStartAddr) + ", EndAddr: " + string.Format("{0:X}", iEndAddr));
                for (uint ii = 0; ii <= bytes; ii += 16)
                {
                    string lineString = string.Format("{0:X}", iStartAddr + (ii));
                    for (byte kk = 0; kk < 16; kk++)
                    {
                        if (ii + kk >= buffer.Length)
                        {
                            break;
                        }
                        switch (iByteWidth)
                        {
                            case 1:
                                if (!iLogHex)
                                {
                                    if (buffer[ii + kk] < 10)
                                    {
                                        lineString += "  ";
                                    }
                                    else if (buffer[ii + kk] < 100)
                                    {
                                        lineString += " ";
                                    }
                                    lineString += " " + buffer[ii + kk].ToString();
                                }
                                else
                                {
                                    if (buffer[ii + kk] < 16)
                                    {
                                        lineString += " ";
                                    }
                                    lineString += " " + string.Format("{0:X}", buffer[ii + kk]);
                                }
                                break;
                            case 2:
                                break;
                            case 4:
                                break;
                            default:
                                //Default to 8 bytes.
                                break;
                        }
                    }
                    LoggingFunctions.Timestamp(lineString);
                }
            }
            catch (Exception ex)
            {
                LoggingFunctions.Timestamp(ex.ToString());
            }
        }
        #region Opcode Healing
        private static void checkSpeedOpcodes(Pointers iPntrs)
        {
            if (!checkNoOpBytes(iPntrs.Info_SpeedText0Opcode) || !checkNoOpBytes(iPntrs.Info_SpeedText1Opcode) || !checkNoOpBytes(iPntrs.Info_SpeedText2Opcode))
            {
                //At least one of our opcode's is stuck at no-op in the text segment (when we scanned).
                //This may happen if Iocaine crashes while the speed hack is enabled.
                //The next time we start Iocaine, the scanner will see the no-ops that we left behind.
                //Hopefully we've saved good codes in the settings folder.
                byte[][] fromFile = getSpeedOpcodesFromFile();
                if (fromFile.Length > 1)
                {
                    iPntrs.Info_SpeedText0Opcode = fromFile[0];
                    iPntrs.Info_SpeedText1Opcode = fromFile[1];
                    iPntrs.Info_SpeedText2Opcode = fromFile[2];
                    iPntrs.Offset_SpeedValue = (ushort)((iPntrs.Info_SpeedText0Opcode[3] << 8) | iPntrs.Info_SpeedText0Opcode[2]);
                    writeSpeedOpcodeOnInitDone = true;
                }
            }
            else
            {
                writeSpeedOpcodesToFile(iPntrs);
            }
        }
        private static bool checkNoOpBytes(byte[] iOpcode)
        {
            int nbBytes = iOpcode.Length;
            for (int ii = 0; ii < nbBytes; ii++)
            {
                if (iOpcode[ii] != 0x90)
                {
                    return true;
                }
            }
            return false;
        }
        private static byte[][] getSpeedOpcodesFromFile()
        {
            byte[][] fromFile = new byte[3][];
            string fullFileName = settingsDir + speedOpcodeFile;
            if (!File.Exists(fullFileName))
            {
                return fromFile;
            }
            StreamReader rdr = new StreamReader(fullFileName);
            List<string> strFromFile = new List<string>();
            while (!rdr.EndOfStream)
            {
                strFromFile.Add(rdr.ReadLine());
            }
            for (int ii = 0; ii < strFromFile.Count; ii++)
            {
                fromFile[ii] = stringToOpcode(strFromFile[ii]);
            }
            return fromFile;
        }
        private static void writeSpeedOpcodesToFile(Pointers iPntrs)
        {
            if (!Directory.Exists(settingsDir))
            {
                Directory.CreateDirectory(settingsDir);
            }
            string fullFileName = settingsDir + speedOpcodeFile;
            StreamWriter ostr = new StreamWriter(fullFileName);
            ostr.WriteLine(opcodeToString(iPntrs.Info_SpeedText0Opcode));
            ostr.WriteLine(opcodeToString(iPntrs.Info_SpeedText1Opcode));
            ostr.WriteLine(opcodeToString(iPntrs.Info_SpeedText2Opcode));
            ostr.Flush();
            ostr.Close();
        }
        private static string opcodeToString(byte[] iOpCode)
        {
            string ostr = "";
            for (int ii = 0; ii < iOpCode.Length; ii++)
            {
                ostr += string.Format("{0:X}", iOpCode[ii]);
                if (ii != iOpCode.Length - 1)
                {
                    ostr += " ";
                }
            }
            return ostr;
        }
        private static byte[] stringToOpcode(string iStr)
        {
            byte[] opcode;
            string[] split = iStr.Split(' ');
            opcode = new byte[split.Length];
            for (int ii = 0; ii < split.Length; ii++)
            {
                opcode[ii] = byte.Parse(split[ii], System.Globalization.NumberStyles.HexNumber);
            }
            return opcode;
        }
        #endregion Opcode Healing
        #endregion Utility Functions

        public static class Self
        {
            #region General
            /// <summary>
            /// Returns the player's name as a string. Normally a null character
            /// is also returned as the last byte of the string.
            /// </summary>
            /// <param name="iRemoveNull">If true, will trim the null character from the name before returning</param>
            /// <returns>Player's name</returns>
            public static string get_name(bool iRemoveNull=true)
            {
                return get_name(processIndex, iRemoveNull);
            }
            /// <summary>
            /// Returns the player's name as a string. Normally a null character
            /// is also returned as the last byte of the string.
            /// </summary>
            /// <param name="iRemoveNull">If true, will trim the null character from the name before returning</param>
            /// <returns>Player's name</returns>
            public static string get_name(int iProcIndex, bool iRemoveNull=true)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                string tempString = MemoryFunctions.ReadString((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player1, 0);
                if (iRemoveNull)
                {
                    if (tempString.Length > 0)
                    {
                        return tempString.Substring(0, tempString.Length - 1);
                    }
                    else
                    {
                        return tempString;
                    }
                }
                else
                {
                    return tempString;
                }
            }
            public static ushort get_zone_id()
            {
                return get_zone_id(processIndex);
            }
            public static ushort get_zone_id(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player1, 44, 2);
            }
            public static bool get_in_mog_house()
            {
                return get_in_mog_house(processIndex);
            }
            public static bool get_in_mog_house(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                return ((byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_MapPcBegin, (int)off_pc_map_begin_in_mog_house, 1)) == 1;
            }
            public static bool get_is_zoning()
            {
                return get_is_zoning(processIndex);
            }
            public static bool get_is_zoning(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                return (1 == (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, (int)off_zoning, 1));
            }
            public static uint get_player_struct_ptr()
            {
                return get_player_struct_ptr(processIndex);
            }
            public static uint get_player_struct_ptr(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                return (uint)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, (int)off_player_struct, 4);
            }
            #endregion General
            #region Player5_info (Status)
            #region Notes
            //00: Nomral status: Standing
            //01: Attacking
            //02: KO'd
            //03: KO'd (one is w/ weapon out, other is not I think)
            //04: Waiting for game text of some kind
            //05: Riding chocobo
            //33: Healing
            //38: Fish on hook
            //39: Fish caught
            //41: Lost fish - lack of skill or broken line/rod
            //43: No Catch, released, too small for rod
            //47: Sitting
            //50: Fishing
            #endregion Notes
            public static byte get_status()
            {
                return get_status(processIndex);
            }
            public static byte get_status(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player5, 0, 1);
            }
            #endregion Player5_info (Status)
            public static class Vitals
            {
                public static ushort get_hp_current()
                {
                    return get_hp_current(processIndex);
                }
                public static ushort get_hp_current(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player1, 30, 2);
                }
                public static ushort get_mp_current()
                {
                    return get_mp_current(processIndex);
                }
                public static ushort get_mp_current(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player1, 34, 2);
                }
                public static ushort get_tp_current()
                {
                    return get_tp_current(processIndex);
                }
                public static ushort get_tp_current(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player1, 38, 2);
                }
                public static byte get_hp_percent()
                {
                    return get_hp_percent(processIndex);
                }
                public static byte get_hp_percent(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player1, 42, 1);
                }
                public static byte get_mp_percent()
                {
                    return get_mp_percent(processIndex);
                }
                public static byte get_mp_percent(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player1, 43, 1);
                }
                public static ushort get_hp_max()
                {
                    return get_hp_max(processIndex);
                }
                public static ushort get_hp_max(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 180, 2);
                }
                public static ushort get_mp_max()
                {
                    return get_mp_max(processIndex);
                }
                public static ushort get_mp_max(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 184, 2);
                }
            }
            public static class Position
            {
                #region Get
                public static float get_x()
                {
                    return get_x(processIndex);
                }
                public static float get_x(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player3);
                    return MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 520);
                }
                public static float get_z()
                {
                    return get_z(processIndex);
                }
                public static float get_z(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player3);
                    return MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 524);
                }
                public static float get_y()
                {
                    return get_y(processIndex);
                }
                public static float get_y(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player3);
                    return MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 528);
                }
                public static float get_heading()
                {
                    return get_heading(processIndex);
                }
                public static float get_heading(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player3);
                    return MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 532);
                }
                #endregion Get
                #region Set
                public static void set_x(float iPosX)
                {
                    set_x(processIndex, iPosX);
                }
                public static void set_x(int iProcIndex, float iPosX)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = (UIntPtr)PCs.get_pointer(iProcIndex, get_name(iProcIndex, true));
                    ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)ptr, off_pc_map_pos_ptr);
                    MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, iPosX, off_pc_map_set_pos_1, 4);
                    MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, iPosX, off_pc_map_set_pos_2, 4);
                }
                public static void set_x(float iPosX, uint playerPtr)
                {
                    set_x(processIndex, iPosX, playerPtr);
                }
                public static void set_x(int iProcIndex, float iPosX, uint iPlayerPtr)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = (UIntPtr)iPlayerPtr;
                    ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)ptr, off_pc_map_pos_ptr);
                    MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, iPosX, off_pc_map_set_pos_1, 4);
                    MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, iPosX, off_pc_map_set_pos_2, 4);
                }
                public static void set_z(float iPosZ)
                {
                    set_z(processIndex, iPosZ);
                }
                public static void set_z(int iProcIndex, float iPosZ)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = (UIntPtr)PCs.get_pointer(iProcIndex, get_name(iProcIndex, true));
                    ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)ptr, off_pc_map_pos_ptr);
                    MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, iPosZ, off_pc_map_set_pos_1 + 4, 4);
                    MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, iPosZ, off_pc_map_set_pos_2 + 4, 4);
                }
                public static void set_z(float iPosZ, uint iPlayerPtr)
                {
                    set_z(processIndex, iPosZ, iPlayerPtr);
                }
                public static void set_z(int iProcIndex, float iPosZ, uint iPlayerPtr)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = (UIntPtr)iPlayerPtr;
                    ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)ptr, off_pc_map_pos_ptr);
                    MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, iPosZ, off_pc_map_set_pos_1 + 4, 4);
                    MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, iPosZ, off_pc_map_set_pos_2 + 4, 4);
                }
                public static void set_y(float iPosY)
                {
                    set_y(processIndex, iPosY);
                }
                public static void set_y(int iProcIndex, float iPosY)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = (UIntPtr)PCs.get_pointer(iProcIndex, get_name(iProcIndex, true));
                    ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)ptr, off_pc_map_pos_ptr);
                    MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, iPosY, off_pc_map_set_pos_1 + 8, 4);
                    MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, iPosY, off_pc_map_set_pos_2 + 8, 4);
                }
                public static void set_y(float iPosY, uint iPlayerPtr)
                {
                    set_y(processIndex, iPosY, iPlayerPtr);
                }
                public static void set_y(int iProcIndex, float iPosY, uint iPlayerPtr)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = (UIntPtr)iPlayerPtr;
                    ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)ptr, off_pc_map_pos_ptr);
                    MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, iPosY, off_pc_map_set_pos_1 + 8, 4);
                    MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, iPosY, off_pc_map_set_pos_2 + 8, 4);
                }
                public static void set_heading(float iPosH)
                {
                    set_heading(processIndex, iPosH);
                }
                public static void set_heading(int iProcIndex, float iPosH)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = (UIntPtr)PCs.get_pointer(iProcIndex, get_name(iProcIndex, true));
                    ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)ptr, off_pc_map_pos_ptr);
                    MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, iPosH, off_pc_map_set_pos_1 + 20, 4);
                }
                public static void set_heading(float iPosH, uint iPlayerPtr)
                {
                    set_heading(processIndex, iPosH, iPlayerPtr);
                }
                public static void set_heading(int iProcIndex, float iPosH, uint iPlayerPtr)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = (UIntPtr)iPlayerPtr;
                    ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)ptr, off_pc_map_pos_ptr);
                    MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, iPosH, off_pc_map_set_pos_1 + 20, 4);
                    MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, iPosH, off_pc_map_set_pos_2 + 20, 4);
                }
                public static uint get_position_struct_ptr()
                {
                    return get_position_struct_ptr(processIndex);
                }
                public static uint get_position_struct_ptr(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    uint ptr = get_player_struct_ptr(iProcIndex);
                    return (uint)MemoryFunctions.GetPointer((IntPtr)proc.Handle, ptr, off_pc_map_pos_ptr);
                }
                #endregion Set
                #region Map Grid
                public static string get_map_grid()
                {
                    return get_map_grid(processIndex);
                }
                public static string get_map_grid(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return MemoryFunctions.ReadString(proc.Handle, processPointerList[iProcIndex].Info_Player3, (int)off_map_grid);
                }
                #endregion Map Grid
            }
            public static class Speed
            {
                #region Get
                public static float get_speed()
                {
                    ushort myIdx = NPCs.get_myNPCIndex();
                    NPCs.NPCInfoStruct info = new NPCs.NPCInfoStruct();
                    if (!NPCs.get_NPCInfoStruct(ref info, myIdx))
                    {
                        return 5.0f;
                    }
                    else
                    {
                        return info.Speed;
                    }
                }
                #endregion Get
                #region Set
                public static void set_speed(bool iEnable = true, float iSpeed = 5.0f)
                {
                    set_speed(NPCs.get_myNPCIndex(), iEnable, iSpeed);
                }
                public static void set_speed(ushort iMyIndex, bool iEnable, float iSpeed = 5.0f)
                {
                    Process proc = processPointerList[processIndex].MainProcess;
                    ProcessModule mod = processPointerList[processIndex].MainModule;

                    // No-op the text-segment where the speed value is saved to the NPC structure.
                    // Write back the original code when disabling (or zoning).
                    uint nb = 0;
                    byte[] nop_buf = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };

                    if (iEnable)
                    {
                        MemoryFunctions.WriteMem(proc.Handle, processPointerList[processIndex].Info_SpeedText0Addr, nop_buf, 0, 6, ref nb);
                        MemoryFunctions.WriteMem(proc.Handle, processPointerList[processIndex].Info_SpeedText1Addr, nop_buf, 0, 6, ref nb);
                        MemoryFunctions.WriteMem(proc.Handle, processPointerList[processIndex].Info_SpeedText2Addr, nop_buf, 0, 6, ref nb);
                    }
                    else
                    {
                        MemoryFunctions.WriteMem(proc.Handle, processPointerList[processIndex].Info_SpeedText0Addr, processPointerList[processIndex].Info_SpeedText0Opcode, 0, 6, ref nb);
                        MemoryFunctions.WriteMem(proc.Handle, processPointerList[processIndex].Info_SpeedText1Addr, processPointerList[processIndex].Info_SpeedText1Opcode, 0, 6, ref nb);
                        MemoryFunctions.WriteMem(proc.Handle, processPointerList[processIndex].Info_SpeedText2Addr, processPointerList[processIndex].Info_SpeedText2Opcode, 0, 6, ref nb);
                    }

                    uint myStructPtr = (uint)MemoryFunctions.GetPointer(proc.Handle, processPointerList[processIndex].Info_MapNpcBegin, iMyIndex * 4);
                    MemoryFunctions.WriteMem(proc.Handle, myStructPtr, iSpeed, processPointerList[processIndex].Offset_SpeedValue, 4);
                }
                #endregion Set
            }
            public static class Job
            {
                public static byte get_main()
                {
                    return get_main(processIndex);
                }
                public static byte get_main(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 188, 1);
                }
                public static byte get_main_lvl()
                {
                    return get_main_lvl(processIndex);
                }
                public static byte get_main_lvl(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 189, 1);
                }
                public static byte get_sub()
                {
                    return get_sub(processIndex);
                }
                public static byte get_sub(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 190, 1);
                }
                public static byte get_sub_lvl()
                {
                    return get_sub_lvl(processIndex);
                }
                public static byte get_sub_lvl(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 191, 1);
                }
            }
            public static class XP
            {
                #region XP
                public static int get_xp_current()
                {
                    return get_xp_current(processIndex);
                }
                public static int get_xp_current(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 192, 2);
                }
                public static int get_xp_max()
                {
                    return get_xp_max(processIndex);
                }
                public static int get_xp_max(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 194, 2);
                }
                #endregion XP
                #region Merits
                public static byte get_mrt_merits_current()
                {
                    return get_mrt_merits_current(processIndex);
                }
                public static byte get_mrt_merits_current(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, (int)off_merit_merits_current, 1);
                }
                public static short get_mrt_limit_points()
                {
                    return get_mrt_limit_points(processIndex);
                }
                public static short get_mrt_limit_points(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, (int)off_merit_limit_points, 2);
                }
                #endregion Merits
                #region Mode
                public static FFXIEnums.XP_MODE get_xp_mode()
                {
                    return get_xp_mode(processIndex);
                }
                public static FFXIEnums.XP_MODE get_xp_mode(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (FFXIEnums.XP_MODE)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, (int)off_merit_mode, 1);
                }
                #endregion Mode
            }
            public static class Skills
            {
                #region Generic Functions
                #region Notes
                ////Returns Player's current Hand-to-Hand skill - 1 bytes
                ////Return Values:
                ////If skill is > 255, this will wrap around to 0 (ie 0 = 256)
                #endregion Notes
                private static int get_skill(int iProcIndex, int iSkillOffset)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte controlInfo = get_control(iProcIndex, iSkillOffset);
                    switch (controlInfo)
                    {
                        case (byte)SKILL_CTRL.CAPPED_1_BYTE:
                            return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, iSkillOffset, 4);
                        case (byte)SKILL_CTRL.CAPPED_2_BYTES:
                            return 256 + (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, iSkillOffset, 4);
                        case (byte)SKILL_CTRL.UNCAPPED_1_BYTE:
                            return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, iSkillOffset, 4);
                        case (byte)SKILL_CTRL.UNCAPPED_2_BYTE:
                            return 256 + (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, iSkillOffset, 4);
                        default:
                            LoggingFunctions.Error("Unexpected value returned from skill cap memory read. iSkillOffset: " + iSkillOffset + ", iControlInfo: " + controlInfo);
                            return 0;
                    }
                }
                #region Notes
                ////Returns whether Player's Hand-to-Hand skill is capped - 1 byte
                ////Return values for skill caps:
                ////0:		skill is < 256 and uncapped
                ////1:		skill is >= 256 and uncapped
                ////128:	skill is < 256 and capped or unavailable
                ////129:	skill is >= 256 and capped or unavailable
                #endregion Notes
                private static bool get_capped(int iProcIndex, int iSkillOffset)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte controlInfo = get_control(iProcIndex, iSkillOffset);
                    switch (controlInfo)
                    {
                        case (byte)SKILL_CTRL.CAPPED_1_BYTE:
                            return true;
                        case (byte)SKILL_CTRL.CAPPED_2_BYTES:
                            return true;
                        case (byte)SKILL_CTRL.UNCAPPED_1_BYTE:
                            return false;
                        case (byte)SKILL_CTRL.UNCAPPED_2_BYTE:
                            return false;
                        default:
                            return true;
                    }
                }
                private static byte get_control(int iProcIndex, int iSkillOffset)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, iSkillOffset + 1, 1);
                }
                public static int get_skill(ushort iSkillType)
                {
                    return get_skill(processIndex, iSkillType);
                }
                public static int get_skill(int iProcIndex, ushort iSkillType)
                {
                    int skillOffset = off_skill_h2h - 2 + (iSkillType * 2);
                    return get_skill(iProcIndex, skillOffset);
                }
                public static bool get_capped(ushort iSkillType)
                {
                    return get_capped(processIndex, iSkillType);
                }
                public static bool get_capped(int iProcIndex, ushort iSkillType)
                {
                    int skillOffset = off_skill_h2h - 2 + (iSkillType * 2);
                    return get_capped(iProcIndex, skillOffset);
                }
                #endregion Generic Functions
                public static class Combat
                {
                    #region Hand to Hand
                    public static int get_hand_to_hand()
                    {
                        return get_hand_to_hand(processIndex);
                    }
                    public static int get_hand_to_hand(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_h2h);
                    }
                    public static bool get_hand_to_hand_capped()
                    {
                        return get_hand_to_hand_capped(processIndex);
                    }
                    public static bool get_hand_to_hand_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_h2h);
                    }
                    #endregion Hand to Hand
                    #region Dagger
                    public static int get_dagger()
                    {
                        return get_dagger(processIndex);
                    }
                    public static int get_dagger(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_dagger);
                    }
                    public static bool get_dagger_capped()
                    {
                        return get_dagger_capped(processIndex);
                    }
                    public static bool get_dagger_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_dagger);
                    }
                    #endregion Dagger
                    #region Sword
                    public static int get_sword()
                    {
                        return get_sword(processIndex);
                    }
                    public static int get_sword(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_sword);
                    }
                    public static bool get_sword_capped()
                    {
                        return get_sword_capped(processIndex);
                    }
                    public static bool get_sword_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_sword);
                    }
                    #endregion Sword
                    #region Great Sword
                    public static int get_great_sword()
                    {
                        return get_great_sword(processIndex);
                    }
                    public static int get_great_sword(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_great_sword);
                    }
                    public static bool get_great_sword_capped()
                    {
                        return get_great_sword_capped(processIndex);
                    }
                    public static bool get_great_sword_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_great_sword);
                    }
                    #endregion Great Sword
                    #region Axe
                    public static int get_axe()
                    {
                        return get_axe(processIndex);
                    }
                    public static int get_axe(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_axe);
                    }
                    public static bool get_axe_capped()
                    {
                        return get_axe_capped(processIndex);
                    }
                    public static bool get_axe_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_axe);
                    }
                    #endregion Axe
                    #region Great Axe
                    public static int get_great_axe()
                    {
                        return get_great_axe(processIndex);
                    }
                    public static int get_great_axe(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_great_axe);
                    }
                    public static bool get_great_axe_capped()
                    {
                        return get_great_axe_capped(processIndex);
                    }
                    public static bool get_great_axe_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_great_axe);
                    }
                    #endregion Great Axe
                    #region Scythe
                    public static int get_scythe()
                    {
                        return get_scythe(processIndex);
                    }
                    public static int get_scythe(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_scythe);
                    }
                    public static bool get_scythe_capped()
                    {
                        return get_scythe_capped(processIndex);
                    }
                    public static bool get_scythe_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_scythe);
                    }
                    #endregion Scythe
                    #region Polearm
                    public static int get_polearm()
                    {
                        return get_polearm(processIndex);
                    }
                    public static int get_polearm(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_polearm);
                    }
                    public static bool get_polearm_capped()
                    {
                        return get_polearm_capped(processIndex);
                    }
                    public static bool get_polearm_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_polearm);
                    }
                    #endregion Polearm
                    #region Katana
                    public static int get_katana()
                    {
                        return get_katana(processIndex);
                    }
                    public static int get_katana(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_katana);
                    }
                    public static bool get_katana_capped()
                    {
                        return get_katana_capped(processIndex);
                    }
                    public static bool get_katana_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_katana);
                    }
                    #endregion Katana
                    #region Great Katana
                    public static int get_great_katana()
                    {
                        return get_great_katana(processIndex);
                    }
                    public static int get_great_katana(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_great_katana);
                    }
                    public static bool get_great_katana_capped()
                    {
                        return get_great_katana_capped(processIndex);
                    }
                    public static bool get_great_katana_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_great_katana);
                    }
                    #endregion Great Katana
                    #region Club
                    public static int get_club()
                    {
                        return get_club(processIndex);
                    }
                    public static int get_club(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_club);
                    }
                    public static bool get_club_capped()
                    {
                        return get_club_capped(processIndex);
                    }
                    public static bool get_club_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_club);
                    }
                    #endregion Club
                    #region Staff
                    public static int get_staff()
                    {
                        return get_staff(processIndex);
                    }
                    public static int get_staff(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_staff);
                    }
                    public static bool get_staff_capped()
                    {
                        return get_staff_capped(processIndex);
                    }
                    public static bool get_staff_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_staff);
                    }
                    #endregion Staff
                    #region Archery
                    public static int get_archery()
                    {
                        return get_archery(processIndex);
                    }
                    public static int get_archery(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_archery);
                    }
                    public static bool get_archery_capped()
                    {
                        return get_archery_capped(processIndex);
                    }
                    public static bool get_archery_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_archery);
                    }
                    #endregion Archery
                    #region Marksmanship
                    public static int get_marksmanship()
                    {
                        return get_marksmanship(processIndex);
                    }
                    public static int get_marksmanship(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_marksmanship);
                    }
                    public static bool get_marksmanship_capped()
                    {
                        return get_marksmanship_capped(processIndex);
                    }
                    public static bool get_marksmanship_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_marksmanship);
                    }
                    #endregion Marksmanship
                    #region Throwing
                    public static int get_throwing()
                    {
                        return get_throwing(processIndex);
                    }
                    public static int get_throwing(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_throwing);
                    }
                    public static bool get_throwing_capped()
                    {
                        return get_throwing_capped(processIndex);
                    }
                    public static bool get_throwing_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_throwing);
                    }
                    #endregion Throwing
                    #region Guarding
                    public static int get_guarding()
                    {
                        return get_guarding(processIndex);
                    }
                    public static int get_guarding(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_guarding);
                    }
                    public static bool get_guarding_capped()
                    {
                        return get_guarding_capped(processIndex);
                    }
                    public static bool get_guarding_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_guarding);
                    }
                    #endregion Guarding
                    #region Evasion
                    public static int get_evasion()
                    {
                        return get_evasion(processIndex);
                    }
                    public static int get_evasion(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_evasion);
                    }
                    public static bool get_evasion_capped()
                    {
                        return get_evasion_capped(processIndex);
                    }
                    public static bool get_evasion_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_evasion);
                    }
                    #endregion Evasion
                    #region Shield
                    public static int get_shield()
                    {
                        return get_shield(processIndex);
                    }
                    public static int get_shield(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_shield);
                    }
                    public static bool get_shield_capped()
                    {
                        return get_shield_capped(processIndex);
                    }
                    public static bool get_shield_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_shield);
                    }
                    #endregion Shield
                    #region Parrying
                    public static int get_parrying()
                    {
                        return get_parrying(processIndex);
                    }
                    public static int get_parrying(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_parrying);
                    }
                    public static bool get_parrying_capped()
                    {
                        return get_parrying_capped(processIndex);
                    }
                    public static bool get_parrying_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_parrying);
                    }
                    #endregion Parrying
                }
                public static class Magic
                {
                    #region Divine
                    public static int get_divine()
                    {
                        return get_divine(processIndex);
                    }
                    public static int get_divine(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_divine);
                    }
                    public static bool get_divine_capped()
                    {
                        return get_divine_capped(processIndex);
                    }
                    public static bool get_divine_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_divine);
                    }
                    #endregion Divine
                    #region Healing
                    public static int get_healing()
                    {
                        return get_healing(processIndex);
                    }
                    public static int get_healing(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_healing);
                    }
                    public static bool get_healing_capped()
                    {
                        return get_healing_capped(processIndex);
                    }
                    public static bool get_healing_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_healing);
                    }
                    #endregion Healing
                    #region Enhancing
                    public static int get_enhancing()
                    {
                        return get_enhancing(processIndex);
                    }
                    public static int get_enhancing(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_enhancing);
                    }
                    public static bool get_enhancing_capped()
                    {
                        return get_enhancing_capped(processIndex);
                    }
                    public static bool get_enhancing_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_enhancing);
                    }
                    #endregion Enhancing
                    #region Enfeebling
                    public static int get_enfeebling()
                    {
                        return get_enfeebling(processIndex);
                    }
                    public static int get_enfeebling(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_enfeebling);
                    }
                    public static bool get_enfeebling_capped()
                    {
                        return get_enfeebling_capped(processIndex);
                    }
                    public static bool get_enfeebling_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_enfeebling);
                    }
                    #endregion Enfeebling
                    #region Elemental
                    public static int get_elemental()
                    {
                        return get_elemental(processIndex);
                    }
                    public static int get_elemental(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_elemental);
                    }
                    public static bool get_elemental_capped()
                    {
                        return get_elemental_capped(processIndex);
                    }
                    public static bool get_elemental_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_elemental);
                    }
                    #endregion Elemental
                    #region Dark
                    public static int get_dark()
                    {
                        return get_dark(processIndex);
                    }
                    public static int get_dark(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_dark);
                    }
                    public static bool get_dark_capped()
                    {
                        return get_dark_capped(processIndex);
                    }
                    public static bool get_dark_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_dark);
                    }
                    #endregion Dark
                    #region Summoning
                    public static int get_summoning()
                    {
                        return get_summoning(processIndex);
                    }
                    public static int get_summoning(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_summoning);
                    }
                    public static bool get_summoning_capped()
                    {
                        return get_summoning_capped(processIndex);
                    }
                    public static bool get_summoning_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_summoning);
                    }
                    #endregion Summoning
                    #region Ninjutsu
                    public static int get_ninjutsu()
                    {
                        return get_ninjutsu(processIndex);
                    }
                    public static int get_ninjutsu(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_ninjutsu);
                    }
                    public static bool get_ninjutsu_capped()
                    {
                        return get_ninjutsu_capped(processIndex);
                    }
                    public static bool get_ninjutsu_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_ninjutsu);
                    }
                    #endregion Ninjutsu
                    #region Blue
                    public static int get_blue()
                    {
                        return get_blue(processIndex);
                    }
                    public static int get_blue(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_blue);
                    }
                    public static bool get_blue_capped()
                    {
                        return get_blue_capped(processIndex);
                    }
                    public static bool get_blue_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_blue);
                    }
                    #endregion Blue
                    #region Geo
                    public static int get_geo()
                    {
                        return get_geo(processIndex);
                    }
                    public static int get_geo(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_geo);
                    }
                    public static bool get_geo_capped()
                    {
                        return get_geo_capped(processIndex);
                    }
                    public static bool get_geo_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_geo);
                    }
                    #endregion Geo
                    #region Bell
                    public static int get_bell()
                    {
                        return get_bell(processIndex);
                    }
                    public static int get_bell(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_bell);
                    }
                    public static bool get_bell_capped()
                    {
                        return get_bell_capped(processIndex);
                    }
                    public static bool get_bell_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_bell);
                    }
                    #endregion Bell
                }
                public static class Musical
                {
                    #region Singing
                    public static int get_singing()
                    {
                        return get_singing(processIndex);
                    }
                    public static int get_singing(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_singing);
                    }
                    public static bool get_singing_capped()
                    {
                        return get_singing_capped(processIndex);
                    }
                    public static bool get_singing_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_singing);
                    }
                    #endregion Singing
                    #region string
                    public static int get_string()
                    {
                        return get_string(processIndex);
                    }
                    public static int get_string(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_string);
                    }
                    public static bool get_string_capped()
                    {
                        return get_string_capped(processIndex);
                    }
                    public static bool get_string_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_string);
                    }
                    #endregion string
                    #region Wind
                    public static int get_wind()
                    {
                        return get_wind(processIndex);
                    }
                    public static int get_wind(int iProcIndex)
                    {
                        return get_skill(iProcIndex, off_skill_wind);
                    }
                    public static bool get_wind_capped()
                    {
                        return get_wind_capped(processIndex);
                    }
                    public static bool get_wind_capped(int iProcIndex)
                    {
                        return get_capped(iProcIndex, off_skill_wind);
                    }
                    #endregion Wind
                }
                public static class Crafting
                {
                    #region Notes
                    //Each skill is calculated by the combination of 2 consecutive single bytes.
                    //The byte decode is:
                    //First byte: Bits 3:0 = Craft rank, 0=recruit to 9=veteran
                    //            Bits 7:4 = Modulus of total skill (SkMod)
                    //Second byte: Represents both skill base level as well as whether capped.
                    //             When 2nd byte < 128, skill is uncapped
                    //             When 2nd byte is >= 128, skill is capped
                    //             def: Skill Capped (SkCapd)
                    //             To get skill base (SkBase):
                    //             SkBase = (SkCapd ? (2nd byte - 128) : 2nd byte) * 256
                    //             SkMod = 1st byte & 0xF0
                    //             Total Skill (SkTot) = SkBase + SkMod
                    //             Skill Level = SkTot / 32
                    //             Skill Rank = 1st byte & 0x0F
                    #endregion Notes
                    #region Fishing
                    public static bool get_fish_capped()
                    {
                        return get_fish_capped(processIndex);
                    }
                    public static bool get_fish_capped(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte baseByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_fish + 1, 1);
                        return (baseByte >= 128);
                    }
                    public static ushort get_fish_skill()
                    {
                        return get_fish_skill(processIndex);
                    }
                    public static ushort get_fish_skill(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte baseByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_fish + 1, 1);
                        byte modByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_fish, 1);
                        modByte &= 0xF0;
                        if (baseByte >= 128)
                        {
                            baseByte -= 128;
                        }
                        return (ushort)(((baseByte * 256) + modByte) / 32);
                    }
                    public static FFXIEnums.CRAFT_RANK get_fish_rank_id()
                    {
                        return get_fish_rank_id(processIndex);
                    }
                    public static FFXIEnums.CRAFT_RANK get_fish_rank_id(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        return (FFXIEnums.CRAFT_RANK)(MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_fish, 1) & 0x0F);
                    }
                    #endregion Fishing
                    #region Woodworking
                    public static bool get_wood_capped()
                    {
                        return get_wood_capped(processIndex);
                    }
                    public static bool get_wood_capped(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte baseByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_wood + 1, 1);
                        return (baseByte >= 128);
                    }
                    public static ushort get_wood_skill()
                    {
                        return get_wood_skill(processIndex);
                    }
                    public static ushort get_wood_skill(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte baseByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_wood + 1, 1);
                        byte modByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_wood, 1);
                        modByte &= 0xF0;
                        if (baseByte >= 128)
                        {
                            baseByte -= 128;
                        }
                        return (ushort)(((baseByte * 256) + modByte) / 32);
                    }
                    public static FFXIEnums.CRAFT_RANK get_wood_rank_id()
                    {
                        return get_wood_rank_id(processIndex);
                    }
                    public static FFXIEnums.CRAFT_RANK get_wood_rank_id(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        return (FFXIEnums.CRAFT_RANK)(MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_wood, 1) & 0x0F);
                    }
                    #endregion Woodworking
                    #region Smithing
                    public static bool get_smith_capped()
                    {
                        return get_smith_capped(processIndex);
                    }
                    public static bool get_smith_capped(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte baseByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_smith + 1, 1);
                        return (baseByte >= 128);
                    }
                    public static ushort get_smith_skill()
                    {
                        return get_smith_skill(processIndex);
                    }
                    public static ushort get_smith_skill(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte baseByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_smith + 1, 1);
                        byte modByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_smith, 1);
                        modByte &= 0xF0;
                        if (baseByte >= 128)
                        {
                            baseByte -= 128;
                        }
                        return (ushort)(((baseByte * 256) + modByte) / 32);
                    }
                    public static FFXIEnums.CRAFT_RANK get_smith_rank_id()
                    {
                        return get_smith_rank_id(processIndex);
                    }
                    public static FFXIEnums.CRAFT_RANK get_smith_rank_id(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        return (FFXIEnums.CRAFT_RANK)(MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_smith, 1) & 0x0F);
                    }
                    #endregion Smithing
                    #region Goldsmithing
                    public static bool get_gold_capped()
                    {
                        return get_gold_capped(processIndex);
                    }
                    public static bool get_gold_capped(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte baseByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_gold + 1, 1);
                        return (baseByte >= 128);
                    }
                    public static ushort get_gold_skill()
                    {
                        return get_gold_skill(processIndex);
                    }
                    public static ushort get_gold_skill(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte baseByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_gold + 1, 1);
                        byte modByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_gold, 1);
                        modByte &= 0xF0;
                        if (baseByte >= 128)
                        {
                            baseByte -= 128;
                        }
                        return (ushort)(((baseByte * 256) + modByte) / 32);
                    }
                    public static FFXIEnums.CRAFT_RANK get_gold_rank_id()
                    {
                        return get_gold_rank_id(processIndex);
                    }
                    public static FFXIEnums.CRAFT_RANK get_gold_rank_id(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        return (FFXIEnums.CRAFT_RANK)(MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_gold, 1) & 0x0F);
                    }
                    #endregion Goldsmithing
                    #region Clothcraft
                    public static bool get_cloth_capped()
                    {
                        return get_cloth_capped(processIndex);
                    }
                    public static bool get_cloth_capped(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte baseByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_cloth + 1, 1);
                        return (baseByte >= 128);
                    }
                    public static ushort get_cloth_skill()
                    {
                        return get_cloth_skill(processIndex);
                    }
                    public static ushort get_cloth_skill(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte baseByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_cloth + 1, 1);
                        byte modByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_cloth, 1);
                        modByte &= 0xF0;
                        if (baseByte >= 128)
                        {
                            baseByte -= 128;
                        }
                        return (ushort)(((baseByte * 256) + modByte) / 32);
                    }
                    public static FFXIEnums.CRAFT_RANK get_cloth_rank_id()
                    {
                        return get_cloth_rank_id(processIndex);
                    }
                    public static FFXIEnums.CRAFT_RANK get_cloth_rank_id(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        return (FFXIEnums.CRAFT_RANK)(MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_cloth, 1) & 0x0F);
                    }
                    #endregion Clothcraft
                    #region Leathercraft
                    public static bool get_leather_capped()
                    {
                        return get_leather_capped(processIndex);
                    }
                    public static bool get_leather_capped(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte baseByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_leather + 1, 1);
                        return (baseByte >= 128);
                    }
                    public static ushort get_leather_skill()
                    {
                        return get_leather_skill(processIndex);
                    }
                    public static ushort get_leather_skill(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte baseByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_leather + 1, 1);
                        byte modByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_leather, 1);
                        modByte &= 0xF0;
                        if (baseByte >= 128)
                        {
                            baseByte -= 128;
                        }
                        return (ushort)(((baseByte * 256) + modByte) / 32);
                    }
                    public static FFXIEnums.CRAFT_RANK get_leather_rank_id()
                    {
                        return get_leather_rank_id(processIndex);
                    }
                    public static FFXIEnums.CRAFT_RANK get_leather_rank_id(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        return (FFXIEnums.CRAFT_RANK)(MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_leather, 1) & 0x0F);
                    }
                    #endregion Leathercraft
                    #region Bonecraft
                    public static bool get_bone_capped()
                    {
                        return get_bone_capped(processIndex);
                    }
                    public static bool get_bone_capped(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte baseByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_bone + 1, 1);
                        return (baseByte >= 128);
                    }
                    public static ushort get_bone_skill()
                    {
                        return get_bone_skill(processIndex);
                    }
                    public static ushort get_bone_skill(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte baseByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_bone + 1, 1);
                        byte modByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_bone, 1);
                        modByte &= 0xF0;
                        if (baseByte >= 128)
                        {
                            baseByte -= 128;
                        }
                        return (ushort)(((baseByte * 256) + modByte) / 32);
                    }
                    public static FFXIEnums.CRAFT_RANK get_bone_rank_id()
                    {
                        return get_bone_rank_id(processIndex);
                    }
                    public static FFXIEnums.CRAFT_RANK get_bone_rank_id(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        return (FFXIEnums.CRAFT_RANK)(MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_bone, 1) & 0x0F);
                    }
                    #endregion Bonecraft
                    #region Alchemy
                    public static bool get_alch_capped()
                    {
                        return get_alch_capped(processIndex);
                    }
                    public static bool get_alch_capped(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte baseByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_alch + 1, 1);
                        return (baseByte >= 128);
                    }
                    public static ushort get_alch_skill()
                    {
                        return get_alch_skill(processIndex);
                    }
                    public static ushort get_alch_skill(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte baseByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_alch + 1, 1);
                        byte modByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_alch, 1);
                        modByte &= 0xF0;
                        if (baseByte >= 128)
                        {
                            baseByte -= 128;
                        }
                        return (ushort)(((baseByte * 256) + modByte) / 32);
                    }
                    public static FFXIEnums.CRAFT_RANK get_alch_rank_id()
                    {
                        return get_alch_rank_id(processIndex);
                    }
                    public static FFXIEnums.CRAFT_RANK get_alch_rank_id(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        return (FFXIEnums.CRAFT_RANK)(MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_alch, 1) & 0x0F);
                    }
                    #endregion Alchemy
                    #region Cooking
                    public static bool get_cook_capped()
                    {
                        return get_cook_capped(processIndex);
                    }
                    public static bool get_cook_capped(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte baseByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_cook + 1, 1);
                        return (baseByte >= 128);
                    }
                    public static ushort get_cook_skill()
                    {
                        return get_cook_skill(processIndex);
                    }
                    public static ushort get_cook_skill(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte baseByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_cook + 1, 1);
                        byte modByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_cook, 1);
                        modByte &= 0xF0;
                        if (baseByte >= 128)
                        {
                            baseByte -= 128;
                        }
                        return (ushort)(((baseByte * 256) + modByte) / 32);
                    }
                    public static FFXIEnums.CRAFT_RANK get_cook_rank_id()
                    {
                        return get_cook_rank_id(processIndex);
                    }
                    public static FFXIEnums.CRAFT_RANK get_cook_rank_id(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        return (FFXIEnums.CRAFT_RANK)(MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_cook, 1) & 0x0F);
                    }
                    #endregion Cooking
                    #region synergy
                    public static bool get_synergy_capped()
                    {
                        return get_synergy_capped(processIndex);
                    }
                    public static bool get_synergy_capped(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte baseByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_synergy + 1, 1);
                        return (baseByte >= 128);
                    }
                    public static ushort get_synergy_skill()
                    {
                        return get_synergy_skill(processIndex);
                    }
                    public static ushort get_synergy_skill(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte baseByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_synergy + 1, 1);
                        byte modByte = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_synergy, 1);
                        modByte &= 0xF0;
                        if (baseByte >= 128)
                        {
                            baseByte -= 128;
                        }
                        return (ushort)(((baseByte * 256) + modByte) / 32);
                    }
                    public static FFXIEnums.CRAFT_RANK get_synergy_rank_id()
                    {
                        return get_synergy_rank_id(processIndex);
                    }
                    public static FFXIEnums.CRAFT_RANK get_synergy_rank_id(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        return (FFXIEnums.CRAFT_RANK)(MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, off_craft_synergy, 1) & 0x0F);
                    }
                    #endregion synergy
                    public static string get_rank_id_to_string(FFXIEnums.CRAFT_RANK iId)
                    {
                        switch (iId)
                        {
                            case FFXIEnums.CRAFT_RANK.AMATEUR:
                                return "Amateur";
                            case FFXIEnums.CRAFT_RANK.RECRUIT:
                                return "Recruit";
                            case FFXIEnums.CRAFT_RANK.INITIATE:
                                return "Initiate";
                            case FFXIEnums.CRAFT_RANK.NOVICE:
                                return "Novice";
                            case FFXIEnums.CRAFT_RANK.APPRENTICE:
                                return "Apprentice";
                            case FFXIEnums.CRAFT_RANK.JOURNEYMAN:
                                return "Journeyman";
                            case FFXIEnums.CRAFT_RANK.CRAFTSMAN:
                                return "Craftsman";
                            case FFXIEnums.CRAFT_RANK.ARTISAN:
                                return "Artisan";
                            case FFXIEnums.CRAFT_RANK.ADEPT:
                                return "Adept";
                            case FFXIEnums.CRAFT_RANK.VETERAN:
                                return "Veteran";
                            case FFXIEnums.CRAFT_RANK.EXPERT:
                                return "Expert";
                            default:
                                return "Unknown";
                        }
                    }
                }
            }
            public static class Attributes
            {
                #region Physical
                public static short get_str_base()
                {
                    return get_str_base(processIndex);
                }
                public static short get_str_base(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 196, 2);
                }
                public static short get_dex_base()
                {
                    return get_dex_base(processIndex);
                }
                public static short get_dex_base(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 198, 2);
                }
                public static short get_vit_base()
                {
                    return get_vit_base(processIndex);
                }
                public static short get_vit_base(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 200, 2);
                }
                public static short get_agi_base()
                {
                    return get_agi_base(processIndex);
                }
                public static short get_agi_base(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 202, 2);
                }
                public static short get_int_base()
                {
                    return get_int_base(processIndex);
                }
                public static short get_int_base(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 204, 2);
                }
                public static short get_mnd_base()
                {
                    return get_mnd_base(processIndex);
                }
                public static short get_mnd_base(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 206, 2);
                }
                public static short get_chr_base()
                {
                    return get_chr_base(processIndex);
                }
                public static short get_chr_base(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 208, 2);
                }
                public static short get_str_bonus()
                {
                    return get_str_bonus(processIndex);
                }
                public static short get_str_bonus(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 210, 2);
                }
                public static short get_dex_bonus()
                {
                    return get_dex_bonus(processIndex);
                }
                public static short get_dex_bonus(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 212, 2);
                }
                public static short get_vit_bonus()
                {
                    return get_vit_bonus(processIndex);
                }
                public static short get_vit_bonus(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 214, 2);
                }
                public static short get_agi_bonus()
                {
                    return get_agi_bonus(processIndex);
                }
                public static short get_agi_bonus(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 216, 2);
                }
                public static short get_int_bonus()
                {
                    return get_int_bonus(processIndex);
                }
                public static short get_int_bonus(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 218, 2);
                }
                public static short get_mnd_bonus()
                {
                    return get_mnd_bonus(processIndex);
                }
                public static short get_mnd_bonus(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 220, 2);
                }
                public static short get_chr_bonus()
                {
                    return get_chr_bonus(processIndex);
                }
                public static short get_chr_bonus(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 222, 2);
                }
                #endregion Physical
                #region Combat
                public static short get_attack()
                {
                    return get_attack(processIndex);
                }
                public static short get_attack(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 224, 2);
                }
                public static short get_defence()
                {
                    return get_defence(processIndex);
                }
                public static short get_defence(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 226, 2);
                }
                #endregion Combat
                #region Elemental
                public static short get_def_fire()
                {
                    return get_def_fire(processIndex);
                }
                public static short get_def_fire(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 228, 2);
                }
                public static short get_def_ice()
                {
                    return get_def_ice(processIndex);
                }
                public static short get_def_ice(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 230, 2);
                }
                public static short get_def_wind()
                {
                    return get_def_wind(processIndex);
                }
                public static short get_def_wind(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 232, 2);
                }
                public static short get_def_earth()
                {
                    return get_def_earth(processIndex);
                }
                public static short get_def_earth(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 234, 2);
                }
                public static short get_def_lightning()
                {
                    return get_def_lightning(processIndex);
                }
                public static short get_def_lightning(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 236, 2);
                }
                public static short get_def_water()
                {
                    return get_def_water(processIndex);
                }
                public static short get_def_water(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 238, 2);
                }
                public static short get_def_light()
                {
                    return get_def_light(processIndex);
                }
                public static short get_def_light(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 240, 2);
                }
                public static short get_def_dark()
                {
                    return get_def_dark(processIndex);
                }
                public static short get_def_dark(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, 242, 2);
                }
                #endregion Elemental
            }
            public static class StatusEffects
            {
                public static void get_effects(ref ushort[] oStatusArray)
                {
                    get_effects(processIndex, ref oStatusArray);
                }
                public static void get_effects(int iProcIndex, ref ushort[] oStatusArray)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    if (oStatusArray == null)
                    {
                        oStatusArray = new ushort[32];
                    }
                    else if (oStatusArray.Length < 32)
                    {
                        oStatusArray = new ushort[32];
                    }
                    int tempOffset = off_status_effects;
                    for (int ii = 0; ii < 32; ii++)
                    {
                        oStatusArray[ii] = (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, tempOffset, 2);
                        tempOffset += 2;
                        if (oStatusArray[ii] == 0xFFFF)
                        {
                            return;
                        }
                    }
                }
            }
            public static class Casting
            {
                private const int _CastingPointerOffset = 0x08;

                public static bool is_casting(int iProcIndex = -1)
                {
                    int procindex = processIndex;
                    if (iProcIndex != -1)
                    {
                        procindex = iProcIndex;
                    }
                    Process proc = processPointerList[procindex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[procindex].Info_Casting);
                    return (MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)ptr, _CastingPointerOffset) != UIntPtr.Zero);
                }              
            }
            public static class Recast
            {
                public static class Magic
                {
                    public static ushort get_time_remaining(ushort iSpellId)
                    {
                        return get_time_remaining(processIndex, iSpellId);
                    }
                    public static ushort get_time_remaining(int iProcIndex, ushort iSpellId)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        return (ushort)(MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_RecastSpell, iSpellId * 2, 2) / 60);
                    }
                }
                public static class Abilities
                {
                    /*
                     * The JA timers have 3 values:
                     * 1. A unique ID, 1 per job ability which is in the resource files.
                     * 2. A recast ID, some abilities share this. Also in the resource files.
                     * 3. A structure index. This is the relative offset of the JA timer from the
                     *    JA timers pointer. These values are found in memory, unknown statically.
                     *    The structure looks like this:
                     *    Addr: Ptr + 3         Ptr + 7         Ptr + 11
                     *    Val:  1-hr JA (0)     RecastID A      RecastID B  etc...
                     *                       ................
                     *    Addr: Ptr + 124       Ptr + 128       Ptr + 132
                     *    Val:  1-hr timer      Timer for A     Timer for B  etc...
                    */
                    public static List<ushort> get_ability_indices()
                    {
                        return get_ability_indices(processIndex);
                    }
                    public static List<ushort> get_ability_indices(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        List<ushort> abilityIndexList = new List<ushort>();
                        abilityIndexList.Add(0);
                        byte nextIndex = 0;
                        for (byte ii = 11; ii <= 120; ii += 8)
                        {
                            nextIndex = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_RecastAbility, ii, 2);
                            if (nextIndex != 0)
                            {
                                abilityIndexList.Add(nextIndex);
                            }
                            else
                            {
                                return abilityIndexList;
                            }
                        }
                        return abilityIndexList;
                    }
                    public static uint get_time_remaining(byte iStructureIndex)
                    {
                        return get_time_remaining(processIndex, iStructureIndex);
                    }
                    public static uint get_time_remaining(int iProcIndex, byte iStructureIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        LoggingFunctions.Debug("Recast.Abilities.get_time_remaining: Trying to read from address: " + (processPointerList[processIndex].Info_RecastAbility + 248 + (iStructureIndex * 4)), LoggingFunctions.DBG_SCOPE.MEMREADS);
                        return (uint)(MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_RecastAbility, 248 + (iStructureIndex * 4), 4) / 60);
                    }
                }
            }
            public static class Inventory
            {
                #region Capacity Counts
                public static byte get_max_bag()
                {
                    return get_max_bag(processIndex);
                }
                public static byte get_max_bag(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    //LoggingFunctions.Timestamp("Max bag: " + (byte)(MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_InventoryMax, 0, 1) - 1) + " from address: " + string.Format("{0:X}", processPointerList[processIndex].Info_InventoryMax));
                    return (byte)(MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Max, 0, 1) - 1);
                }
                public static byte get_max_safe()
                {
                    return get_max_safe(processIndex);
                }
                public static byte get_max_safe(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)(MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Max, 1, 1) - 1);
                }
                public static byte get_max_storage()
                {
                    return get_max_storage(processIndex);
                }
                public static byte get_max_storage(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)(MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Max, 2, 1) - 1);
                }
                public static byte get_max_locker()
                {
                    return get_max_locker(processIndex);
                }
                public static byte get_max_locker(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte value = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Max, 4, 1);
                    if (value > 0)
                    {
                        return (byte)(value - 1);
                    }
                    else
                    {
                        return 0;
                    }
                }
                public static byte get_max_satchel()
                {
                    return get_max_satchel(processIndex);
                }
                public static byte get_max_satchel(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte value = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Max, 5, 1);
                    if (value > 0)
                    {
                        return (byte)(value - 1);
                    }
                    else
                    {
                        return 0;
                    }
                }
                public static byte get_max_sack()
                {
                    return get_max_sack(processIndex);
                }
                public static byte get_max_sack(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte value = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Max, 6, 1);
                    if (value > 0)
                    {
                        return (byte)(value - 1);
                    }
                    else
                    {
                        return 0;
                    }
                }
                public static byte get_max_case()
                {
                    return get_max_case(processIndex);
                }
                public static byte get_max_case(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte value = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Max, 7, 1);
                    if (value > 0)
                    {
                        return (byte)(value - 1);
                    }
                    else
                    {
                        return 0;
                    }
                }
                public static byte get_max_wardrobe()
                {
                    return get_max_wardrobe(processIndex);
                }
                public static byte get_max_wardrobe(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte value = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Max, 8, 1);
                    if (value > 0)
                    {
                        return (byte)(value - 1);
                    }
                    else
                    {
                        return 0;
                    }
                }
                public static byte get_max_safe2()
                {
                    return get_max_safe2(processIndex);
                }
                public static byte get_max_safe2(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte value = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Max, 9, 1);
                    if (value > 0)
                    {
                        return (byte)(value - 1);
                    }
                    else
                    {
                        return 0;
                    }
                }
                public static byte get_max_wardrobe2()
                {
                    return get_max_wardrobe2(processIndex);
                }
                public static byte get_max_wardrobe2(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte value = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Max, 10, 1);
                    if (value > 0)
                    {
                        return (byte)(value - 1);
                    }
                    else
                    {
                        return 0;
                    }
                }
                public static byte get_max_wardrobe3()
                {
                    return get_max_wardrobe3(processIndex);
                }
                public static byte get_max_wardrobe3(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte value = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Max, 11, 1);
                    if (value > 0)
                    {
                        return (byte)(value - 1);
                    }
                    else
                    {
                        return 0;
                    }
                }
                public static byte get_max_wardrobe4()
                {
                    return get_max_wardrobe4(processIndex);
                }
                public static byte get_max_wardrobe4(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte value = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Max, 12, 1);
                    if (value > 0)
                    {
                        return (byte)(value - 1);
                    }
                    else
                    {
                        return 0;
                    }
                }
                #endregion Capacity Counts
                #region Gil
                public static uint get_gil(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (uint)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Bag, 4, 4);
                }
                public static uint get_gil()
                {
                    return get_gil(processIndex);
                }
                #endregion Gil
                #region Bag Info
                /// <summary>
                /// Returns the item ID in the given index of the gobbie bag.
                /// </summary>
                /// <param name="iStructIndex">The index of the gobbie bag structure. Not the index of the bag as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>The item ID as a short.</returns>
                public static ushort get_bag_item_id(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Bag, (iStructIndex * 44), 2);
                }
                public static ushort get_bag_item_id(short iStructIndex)
                {
                    return get_bag_item_id(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the quantity of an item in the given index of the gobbie bag.
                /// </summary>
                /// <param name="iStructIndex">The index of the gobbie bag structure. Not the index of the bag as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</returns>
                public static byte get_bag_item_quan(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Bag, (iStructIndex * 44) + 4, 1);
                }
                public static byte get_bag_item_quan(short iStructIndex)
                {
                    return get_bag_item_quan(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns a bool indicating whether the item at the given index in the gobbie bag
                /// is currently equipped or not.
                /// </summary>
                /// <param name="structIndex">The index of the gobbie bag structure. Not the index of the bag as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</param>
                /// <returns></returns>
                public static bool get_bag_item_equipped(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte equ = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Bag, (iStructIndex * 44) + 8, 1);
                    return (equ == 5) ? true : false;
                }
                public static bool get_bag_item_equipped(short iStructIndex)
                {
                    return get_bag_item_equipped(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the first inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <returns>First available index as a byte.</returns>
                public static byte get_bag_index(int iProcIndex, ushort iItemID, byte iQuantity)
                {
                    return get_bag_index(iProcIndex, iItemID, iQuantity, 1);
                }
                public static byte get_bag_index(ushort iItemID, byte iQuantity)
                {
                    return get_bag_index(processIndex, iItemID, iQuantity, 1);
                }
                /// <summary>
                /// Returns the first inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <param name="iStartIndex">Bag index to start looking at (1 if not given).</param>
                /// <returns>First available index after the start index as a byte.</returns>
                public static byte get_bag_index(int iProcIndex, ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    byte maxCount = get_max_bag(iProcIndex);
                    for (byte ii = iStartIndex; ii <= maxCount; ii++)
                    {
                        ushort readID = get_bag_item_id(iProcIndex, ii);
                        if (readID == iItemID)
                        {
                            byte readQuan = get_bag_item_quan(iProcIndex, ii);
                            if (readQuan >= iQuantity)
                            {
                                return ii;
                            }
                        }
                    }
                    return 0;
                }
                public static byte get_bag_index(ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    return get_bag_index(processIndex, iItemID, iQuantity, iStartIndex);
                }
                public static byte get_bag_occupancy(int iProcIndex)
                {
                    byte count = 0;
                    byte maxCount = get_max_bag(iProcIndex);
                    for (byte ii = 1; ii <= maxCount; ii++)
                    {
                        if (get_bag_item_id(iProcIndex, ii) != 0)
                        {
                            count++;
                        }
                    }
                    return count;
                }
                public static byte get_bag_occupancy()
                {
                    return get_bag_occupancy(processIndex);
                }
                #endregion Bag Info
                #region Safe_info
                /// <summary>
                /// Returns the item ID in the given index of the safe.
                /// </summary>
                /// <param name="iStructIndex">The index of the safe structure. Not the index of the safe as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>The item ID as a short.</returns>
                public static ushort get_safe_item_id(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Safe, (iStructIndex * 44), 2);
                }
                public static ushort get_safe_item_id(short iStructIndex)
                {
                    return get_safe_item_id(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the quantity of an item in the given index of the safe.
                /// </summary>
                /// <param name="iStructIndex">The index of the safe structure. Not the index of the safe as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</returns>
                public static byte get_safe_item_quan(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Safe, (iStructIndex * 44) + 4, 1);
                }
                public static byte get_safe_item_quan(short iStructIndex)
                {
                    return get_safe_item_quan(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <returns>First available index as a byte.</returns>
                public static byte get_safe_index(int iProcIndex, ushort iItemID, byte iQuantity)
                {
                    return get_safe_index(iProcIndex, iItemID, iQuantity, 1);
                }
                public static byte get_safe_index(ushort iItemID, byte iQuantity)
                {
                    return get_safe_index(processIndex, iItemID, iQuantity, 1);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <param name="iStartIndex">Sack index to start looking at (1 if not given).</param>
                /// <returns>First available index after the start index as a byte.</returns>
                public static byte get_safe_index(int iProcIndex, ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    byte maxCount = get_max_safe(iProcIndex);
                    for (byte ii = iStartIndex; ii <= maxCount; ii++)
                    {
                        ushort readID = get_safe_item_id(iProcIndex, ii);
                        if (readID == iItemID)
                        {
                            byte readQuan = get_safe_item_quan(iProcIndex, ii);
                            if (readQuan >= iQuantity)
                            {
                                return ii;
                            }
                        }
                    }
                    return 0;
                }
                public static byte get_safe_index(ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    return get_safe_index(processIndex, iItemID, iQuantity, iStartIndex);
                }
                public static byte get_safe_occupancy(int iProcIndex)
                {
                    byte count = 0;
                    byte maxCount = get_max_safe(iProcIndex);
                    for (byte ii = 1; ii <= maxCount; ii++)
                    {
                        if (get_safe_item_id(iProcIndex, ii) != 0)
                        {
                            count++;
                        }
                    }
                    return count;
                }
                public static byte get_safe_occupancy()
                {
                    return get_safe_occupancy(processIndex);
                }
                #endregion Safe info
                #region Storage_info
                /// <summary>
                /// Returns the item ID in the given index of the storage.
                /// </summary>
                /// <param name="iStructIndex">The index of the storage structure. Not the index of the storage as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>The item ID as a short.</returns>
                public static ushort get_storage_item_id(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Storage, (iStructIndex * 44), 2);
                }
                public static ushort get_storage_item_id(short iStructIndex)
                {
                    return get_storage_item_id(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the quantity of an item in the given index of the storage.
                /// </summary>
                /// <param name="iStructIndex">The index of the storage structure. Not the index of the storage as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</returns>
                public static byte get_storage_item_quan(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Storage, (iStructIndex * 44) + 4, 1);
                }
                public static byte get_storage_item_quan(short iStructIndex)
                {
                    return get_storage_item_quan(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <returns>First available index as a byte.</returns>
                public static byte get_storage_index(int iProcIndex, ushort iItemID, byte iQuantity)
                {
                    return get_storage_index(iProcIndex, iItemID, iQuantity, 1);
                }
                public static byte get_storage_index(ushort iItemID, byte iQuantity)
                {
                    return get_storage_index(processIndex, iItemID, iQuantity, 1);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <param name="iStartIndex">Sack index to start looking at (1 if not given).</param>
                /// <returns>First available index after the start index as a byte.</returns>
                public static byte get_storage_index(int iProcIndex, ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    byte maxCount = get_max_storage(iProcIndex);
                    for (byte ii = iStartIndex; ii <= maxCount; ii++)
                    {
                        ushort readID = get_storage_item_id(iProcIndex, ii);
                        if (readID == iItemID)
                        {
                            byte readQuan = get_storage_item_quan(iProcIndex, ii);
                            if (readQuan >= iQuantity)
                            {
                                return ii;
                            }
                        }
                    }
                    return 0;
                }
                public static byte get_storage_index(ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    return get_storage_index(processIndex, iItemID, iQuantity, iStartIndex);
                }
                public static byte get_storage_occupancy(int iProcIndex)
                {
                    byte count = 0;
                    byte maxCount = get_max_storage(iProcIndex);
                    for (byte ii = 1; ii <= maxCount; ii++)
                    {
                        if (get_storage_item_id(iProcIndex, ii) != 0)
                        {
                            count++;
                        }
                    }
                    return count;
                }
                public static byte get_storage_occupancy()
                {
                    return get_storage_occupancy(processIndex);
                }
                #endregion Storage info
                #region Locker_info
                /// <summary>
                /// Returns the item ID in the given index of the locker.
                /// </summary>
                /// <param name="iStructIndex">The index of the locker structure. Not the index of the locker as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>The item ID as a short.</returns>
                public static ushort get_locker_item_id(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Locker, (iStructIndex * 44), 2);
                }
                public static ushort get_locker_item_id(short iStructIndex)
                {
                    return get_locker_item_id(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the quantity of an item in the given index of the locker.
                /// </summary>
                /// <param name="iStructIndex">The index of the locker structure. Not the index of the locker as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</returns>
                public static byte get_locker_item_quan(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Locker, (iStructIndex * 44) + 4, 1);
                }
                public static byte get_locker_item_quan(short iStructIndex)
                {
                    return get_locker_item_quan(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <returns>First available index as a byte.</returns>
                public static byte get_locker_index(int iProcIndex, ushort iItemID, byte iQuantity)
                {
                    return get_locker_index(iProcIndex, iItemID, iQuantity, 1);
                }
                public static byte get_locker_index(ushort iItemID, byte iQuantity)
                {
                    return get_locker_index(processIndex, iItemID, iQuantity, 1);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <param name="iStartIndex">Sack index to start looking at (1 if not given).</param>
                /// <returns>First available index after the start index as a byte.</returns>
                public static byte get_locker_index(int iProcIndex, ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    byte maxCount = get_max_locker(iProcIndex);
                    for (byte ii = iStartIndex; ii <= maxCount; ii++)
                    {
                        ushort readID = get_locker_item_id(iProcIndex, ii);
                        if (readID == iItemID)
                        {
                            byte readQuan = get_locker_item_quan(iProcIndex, ii);
                            if (readQuan >= iQuantity)
                            {
                                return ii;
                            }
                        }
                    }
                    return 0;
                }
                public static byte get_locker_index(ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    return get_locker_index(processIndex, iItemID, iQuantity, iStartIndex);
                }
                public static byte get_locker_occupancy(int iProcIndex)
                {
                    byte count = 0;
                    byte maxCount = get_max_locker(iProcIndex);
                    for (byte ii = 1; ii <= maxCount; ii++)
                    {
                        if (get_locker_item_id(iProcIndex, ii) != 0)
                        {
                            count++;
                        }
                    }
                    return count;
                }
                public static byte get_locker_occupancy()
                {
                    return get_locker_occupancy(processIndex);
                }
                #endregion Locker info
                #region Satchel_info
                /// <summary>
                /// Returns the item ID in the given index of the satchel.
                /// </summary>
                /// <param name="iStructIndex">The index of the satchel structure. Not the index of the satchel as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>The item ID as a short.</returns>
                public static ushort get_satchel_item_id(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Satchel, (iStructIndex * 44), 2);
                }
                public static ushort get_satchel_item_id(short iStructIndex)
                {
                    return get_satchel_item_id(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the quantity of an item in the given index of the satchel.
                /// </summary>
                /// <param name="iStructIndex">The index of the satchel structure. Not the index of the satchel as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</returns>
                public static byte get_satchel_item_quan(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Satchel, (iStructIndex * 44) + 4, 1);
                }
                public static byte get_satchel_item_quan(short iStructIndex)
                {
                    return get_satchel_item_quan(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns a bool indicating whether the item at the given index in the satchel
                /// is currently equipped or not.
                /// </summary>
                /// <param name="structIndex">The index of the satchel structure. Not the index of the bag as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</param>
                /// <returns></returns>
                public static bool get_satchel_item_equipped(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte equ = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Satchel, (iStructIndex * 44) + 8, 1);
                    return (equ == 5) ? true : false;
                }
                public static bool get_satchel_item_equipped(short iStructIndex)
                {
                    return get_satchel_item_equipped(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <returns>First available index as a byte.</returns>
                public static byte get_satchel_index(int iProcIndex, ushort iItemID, byte iQuantity)
                {
                    return get_satchel_index(iProcIndex, iItemID, iQuantity, 1);
                }
                public static byte get_satchel_index(ushort iItemID, byte iQuantity)
                {
                    return get_satchel_index(processIndex, iItemID, iQuantity, 1);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <param name="iStartIndex">Satchel index to start looking at (1 if not given).</param>
                /// <returns>First available index after the start index as a byte.</returns>
                public static byte get_satchel_index(int iProcIndex, ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    byte maxCount = get_max_satchel(iProcIndex);
                    for (byte ii = iStartIndex; ii <= maxCount; ii++)
                    {
                        ushort readID = get_satchel_item_id(iProcIndex, ii);
                        if (readID == iItemID)
                        {
                            byte readQuan = get_satchel_item_quan(iProcIndex, ii);
                            if (readQuan >= iQuantity)
                            {
                                return ii;
                            }
                        }
                    }
                    return 0;
                }
                public static byte get_satchel_index(ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    return get_satchel_index(processIndex, iItemID, iQuantity, iStartIndex);
                }
                public static byte get_satchel_occupancy(int iProcIndex)
                {
                    byte count = 0;
                    byte maxCount = get_max_satchel(iProcIndex);
                    for (byte ii = 1; ii <= maxCount; ii++)
                    {
                        if (get_satchel_item_id(iProcIndex, ii) != 0)
                        {
                            count++;
                        }
                    }
                    return count;
                }
                public static byte get_satchel_occupancy()
                {
                    return get_satchel_occupancy(processIndex);
                }
                #endregion Satchel_info
                #region Sack_info
                /// <summary>
                /// Returns the item ID in the given index of the sack.
                /// </summary>
                /// <param name="iStructIndex">The index of the sack structure. Not the index of the sack as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>The item ID as a short.</returns>
                public static ushort get_sack_item_id(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Sack, (iStructIndex * 44), 2);
                }
                public static ushort get_sack_item_id(short iStructIndex)
                {
                    return get_sack_item_id(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the quantity of an item in the given index of the sack.
                /// </summary>
                /// <param name="iStructIndex">The index of the sack structure. Not the index of the sack as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</returns>
                public static byte get_sack_item_quan(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Sack, (iStructIndex * 44) + 4, 1);
                }
                public static byte get_sack_item_quan(short iStructIndex)
                {
                    return get_sack_item_quan(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns a bool indicating whether the item at the given index in the sack
                /// is currently equipped or not.
                /// </summary>
                /// <param name="structIndex">The index of the sack structure. Not the index of the sack as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</param>
                /// <returns></returns>
                public static bool get_sack_item_equipped(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte equ = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Sack, (iStructIndex * 44) + 8, 1);
                    return (equ == 5) ? true : false;
                }
                public static bool get_sack_item_equipped(short iStructIndex)
                {
                    return get_sack_item_equipped(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <returns>First available index as a byte.</returns>
                public static byte get_sack_index(int iProcIndex, ushort iItemID, byte iQuantity)
                {
                    return get_sack_index(iProcIndex, iItemID, iQuantity, 1);
                }
                public static byte get_sack_index(ushort iItemID, byte iQuantity)
                {
                    return get_sack_index(processIndex, iItemID, iQuantity, 1);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <param name="iStartIndex">Sack index to start looking at (1 if not given).</param>
                /// <returns>First available index after the start index as a byte.</returns>
                public static byte get_sack_index(int iProcIndex, ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    byte maxCount = get_max_sack(iProcIndex);
                    for (byte ii = iStartIndex; ii <= maxCount; ii++)
                    {
                        ushort readID = get_sack_item_id(iProcIndex, ii);
                        if (readID == iItemID)
                        {
                            byte readQuan = get_sack_item_quan(iProcIndex, ii);
                            if (readQuan >= iQuantity)
                            {
                                return ii;
                            }
                        }
                    }
                    return 0;
                }
                public static byte get_sack_index(ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    return get_sack_index(processIndex, iItemID, iQuantity, iStartIndex);
                }
                public static byte get_sack_occupancy(int iProcIndex)
                {
                    byte count = 0;
                    byte maxCount = get_max_sack(iProcIndex);
                    for (byte ii = 1; ii <= maxCount; ii++)
                    {
                        if (get_sack_item_id(iProcIndex, ii) != 0)
                        {
                            count++;
                        }
                    }
                    return count;
                }
                public static byte get_sack_occupancy()
                {
                    return get_sack_occupancy(processIndex);
                }
                #endregion Sack info
                #region Case_info
                /// <summary>
                /// Returns the item ID in the given index of the case.
                /// </summary>
                /// <param name="iStructIndex">The index of the case structure. Not the index of the case as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>The item ID as a short.</returns>
                public static ushort get_case_item_id(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Case, (iStructIndex * 44), 2);
                }
                public static ushort get_case_item_id(short iStructIndex)
                {
                    return get_case_item_id(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the quantity of an item in the given index of the case.
                /// </summary>
                /// <param name="iStructIndex">The index of the case structure. Not the index of the case as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</returns>
                public static byte get_case_item_quan(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Case, (iStructIndex * 44) + 4, 1);
                }
                public static byte get_case_item_quan(short iStructIndex)
                {
                    return get_case_item_quan(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns a bool indicating whether the item at the given index in the case
                /// is currently equipped or not.
                /// </summary>
                /// <param name="structIndex">The index of the case structure. Not the index of the case as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</param>
                /// <returns></returns>
                public static bool get_case_item_equipped(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte equ = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Case, (iStructIndex * 44) + 8, 1);
                    return (equ == 5) ? true : false;
                }
                public static bool get_case_item_equipped(short iStructIndex)
                {
                    return get_case_item_equipped(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <returns>First available index as a byte.</returns>
                public static byte get_case_index(int iProcIndex, ushort iItemID, byte iQuantity)
                {
                    return get_case_index(iProcIndex, iItemID, iQuantity, 1);
                }
                public static byte get_case_index(ushort iItemID, byte iQuantity)
                {
                    return get_case_index(processIndex, iItemID, iQuantity, 1);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <param name="iStartIndex">Case index to start looking at (1 if not given).</param>
                /// <returns>First available index after the start index as a byte.</returns>
                public static byte get_case_index(int iProcIndex, ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    byte maxCount = get_max_case(iProcIndex);
                    for (byte ii = iStartIndex; ii <= maxCount; ii++)
                    {
                        ushort readID = get_case_item_id(iProcIndex, ii);
                        if (readID == iItemID)
                        {
                            byte readQuan = get_case_item_quan(iProcIndex, ii);
                            if (readQuan >= iQuantity)
                            {
                                return ii;
                            }
                        }
                    }
                    return 0;
                }
                public static byte get_case_index(ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    return get_case_index(processIndex, iItemID, iQuantity, iStartIndex);
                }
                public static byte get_case_occupancy(int iProcIndex)
                {
                    byte count = 0;
                    byte maxCount = get_max_case(iProcIndex);
                    for (byte ii = 1; ii <= maxCount; ii++)
                    {
                        if (get_case_item_id(iProcIndex, ii) != 0)
                        {
                            count++;
                        }
                    }
                    return count;
                }
                public static byte get_case_occupancy()
                {
                    return get_case_occupancy(processIndex);
                }
                #endregion Case info
                #region Wardrobe_info
                /// <summary>
                /// Returns the item ID in the given index of the wardrobe.
                /// </summary>
                /// <param name="iStructIndex">The index of the wardrobe structure. Not the index of the wardrobe as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>The item ID as a ushort.</returns>
                public static ushort get_wardrobe_item_id(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Wardrobe, (iStructIndex * 44), 2);
                }
                public static ushort get_wardrobe_item_id(short iStructIndex)
                {
                    return get_wardrobe_item_id(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the quantity of an item in the given index of the wardrobe.
                /// </summary>
                /// <param name="iStructIndex">The index of the wardrobe structure. Not the index of the wardrobe as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</returns>
                public static byte get_wardrobe_item_quan(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Wardrobe, (iStructIndex * 44) + 4, 1);
                }
                public static byte get_wardrobe_item_quan(short iStructIndex)
                {
                    return get_wardrobe_item_quan(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns a bool indicating whether the item at the given index in the wardrobe
                /// is currently equipped or not.
                /// </summary>
                /// <param name="structIndex">The index of the wardrobe structure. Not the index of the wardrobe as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</param>
                /// <returns></returns>
                public static bool get_wardrobe_item_equipped(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte equ = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Wardrobe, (iStructIndex * 44) + 8, 1);
                    return (equ == 5) ? true : false;
                }
                public static bool get_wardrobe_item_equipped(short iStructIndex)
                {
                    return get_wardrobe_item_equipped(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <returns>First available index as a byte.</returns>
                public static byte get_wardrobe_index(int iProcIndex, ushort iItemID, byte iQuantity)
                {
                    return get_wardrobe_index(iProcIndex, iItemID, iQuantity, 1);
                }
                public static byte get_wardrobe_index(ushort iItemID, byte iQuantity)
                {
                    return get_wardrobe_index(processIndex, iItemID, iQuantity, 1);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <param name="iStartIndex">Wardrobe index to start looking at (1 if not given).</param>
                /// <returns>First available index after the start index as a byte.</returns>
                public static byte get_wardrobe_index(int iProcIndex, ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    byte maxCount = get_max_wardrobe(iProcIndex);
                    for (byte ii = iStartIndex; ii <= maxCount; ii++)
                    {
                        ushort readID = get_wardrobe_item_id(iProcIndex, ii);
                        if (readID == iItemID)
                        {
                            byte readQuan = get_wardrobe_item_quan(iProcIndex, ii);
                            if (readQuan >= iQuantity)
                            {
                                return ii;
                            }
                        }
                    }
                    return 0;
                }
                public static byte get_wardrobe_index(ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    return get_wardrobe_index(processIndex, iItemID, iQuantity, iStartIndex);
                }
                public static byte get_wardrobe_occupancy(int iProcIndex)
                {
                    byte count = 0;
                    byte maxCount = get_max_wardrobe(iProcIndex);
                    for (byte ii = 1; ii <= maxCount; ii++)
                    {
                        if (get_wardrobe_item_id(iProcIndex, ii) != 0)
                        {
                            count++;
                        }
                    }
                    return count;
                }
                public static byte get_wardrobe_occupancy()
                {
                    return get_wardrobe_occupancy(processIndex);
                }
                #endregion Wardrobe info
                #region Safe2_info
                /// <summary>
                /// Returns the item ID in the given index of the safe2.
                /// </summary>
                /// <param name="iStructIndex">The index of the safe2 structure. Not the index of the safe2 as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>The item ID as a ushort.</returns>
                public static ushort get_safe2_item_id(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Safe2, (iStructIndex * 44), 2);
                }
                public static ushort get_safe2_item_id(short iStructIndex)
                {
                    return get_safe2_item_id(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the quantity of an item in the given index of the safe2.
                /// </summary>
                /// <param name="iStructIndex">The index of the safe2 structure. Not the index of the safe2 as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</returns>
                public static byte get_safe2_item_quan(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Safe2, (iStructIndex * 44) + 4, 1);
                }
                public static byte get_safe2_item_quan(short iStructIndex)
                {
                    return get_safe2_item_quan(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns a bool indicating whether the item at the given index in the safe2
                /// is currently equipped or not.
                /// </summary>
                /// <param name="structIndex">The index of the safe2 structure. Not the index of the safe2 as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</param>
                /// <returns></returns>
                public static bool get_safe2_item_equipped(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte equ = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Safe2, (iStructIndex * 44) + 8, 1);
                    return (equ == 5) ? true : false;
                }
                public static bool get_safe2_item_equipped(short iStructIndex)
                {
                    return get_safe2_item_equipped(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <returns>First available index as a byte.</returns>
                public static byte get_safe2_index(int iProcIndex, ushort iItemID, byte iQuantity)
                {
                    return get_safe2_index(iProcIndex, iItemID, iQuantity, 1);
                }
                public static byte get_safe2_index(ushort iItemID, byte iQuantity)
                {
                    return get_safe2_index(processIndex, iItemID, iQuantity, 1);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <param name="iStartIndex">Safe2 index to start looking at (1 if not given).</param>
                /// <returns>First available index after the start index as a byte.</returns>
                public static byte get_safe2_index(int iProcIndex, ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    byte maxCount = get_max_safe2(iProcIndex);
                    for (byte ii = iStartIndex; ii <= maxCount; ii++)
                    {
                        ushort readID = get_safe2_item_id(iProcIndex, ii);
                        if (readID == iItemID)
                        {
                            byte readQuan = get_safe2_item_quan(iProcIndex, ii);
                            if (readQuan >= iQuantity)
                            {
                                return ii;
                            }
                        }
                    }
                    return 0;
                }
                public static byte get_safe2_index(ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    return get_safe2_index(processIndex, iItemID, iQuantity, iStartIndex);
                }
                public static byte get_safe2_occupancy(int iProcIndex)
                {
                    byte count = 0;
                    byte maxCount = get_max_safe2(iProcIndex);
                    for (byte ii = 1; ii <= maxCount; ii++)
                    {
                        if (get_safe2_item_id(iProcIndex, ii) != 0)
                        {
                            count++;
                        }
                    }
                    return count;
                }
                public static byte get_safe2_occupancy()
                {
                    return get_safe2_occupancy(processIndex);
                }
                #endregion Safe2 info
                #region Wardrobe2_info
                /// <summary>
                /// Returns the item ID in the given index of the wardrobe2.
                /// </summary>
                /// <param name="iStructIndex">The index of the wardrobe2 structure. Not the index of the wardrobe2 as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>The item ID as a ushort.</returns>
                public static ushort get_wardrobe2_item_id(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Wardrobe2, (iStructIndex * 44), 2);
                }
                public static ushort get_wardrobe2_item_id(short iStructIndex)
                {
                    return get_wardrobe2_item_id(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the quantity of an item in the given index of the wardrobe2.
                /// </summary>
                /// <param name="iStructIndex">The index of the wardrobe2 structure. Not the index of the wardrobe2 as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</returns>
                public static byte get_wardrobe2_item_quan(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Wardrobe2, (iStructIndex * 44) + 4, 1);
                }
                public static byte get_wardrobe2_item_quan(short iStructIndex)
                {
                    return get_wardrobe2_item_quan(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns a bool indicating whether the item at the given index in the wardrobe2
                /// is currently equipped or not.
                /// </summary>
                /// <param name="structIndex">The index of the wardrobe2 structure. Not the index of the wardrobe2 as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</param>
                /// <returns></returns>
                public static bool get_wardrobe2_item_equipped(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte equ = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Wardrobe2, (iStructIndex * 44) + 8, 1);
                    return (equ == 5) ? true : false;
                }
                public static bool get_wardrobe2_item_equipped(short iStructIndex)
                {
                    return get_wardrobe2_item_equipped(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <returns>First available index as a byte.</returns>
                public static byte get_wardrobe2_index(int iProcIndex, ushort iItemID, byte iQuantity)
                {
                    return get_wardrobe2_index(iProcIndex, iItemID, iQuantity, 1);
                }
                public static byte get_wardrobe2_index(ushort iItemID, byte iQuantity)
                {
                    return get_wardrobe2_index(processIndex, iItemID, iQuantity, 1);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <param name="iStartIndex">Wardrobe2 index to start looking at (1 if not given).</param>
                /// <returns>First available index after the start index as a byte.</returns>
                public static byte get_wardrobe2_index(int iProcIndex, ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    byte maxCount = get_max_wardrobe2(iProcIndex);
                    for (byte ii = iStartIndex; ii <= maxCount; ii++)
                    {
                        ushort readID = get_wardrobe2_item_id(iProcIndex, ii);
                        if (readID == iItemID)
                        {
                            byte readQuan = get_wardrobe2_item_quan(iProcIndex, ii);
                            if (readQuan >= iQuantity)
                            {
                                return ii;
                            }
                        }
                    }
                    return 0;
                }
                public static byte get_wardrobe2_index(ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    return get_wardrobe2_index(processIndex, iItemID, iQuantity, iStartIndex);
                }
                public static byte get_wardrobe2_occupancy(int iProcIndex)
                {
                    byte count = 0;
                    byte maxCount = get_max_wardrobe2(iProcIndex);
                    for (byte ii = 1; ii <= maxCount; ii++)
                    {
                        if (get_wardrobe2_item_id(iProcIndex, ii) != 0)
                        {
                            count++;
                        }
                    }
                    return count;
                }
                public static byte get_wardrobe2_occupancy()
                {
                    return get_wardrobe2_occupancy(processIndex);
                }
                #endregion Wardrobe2 info
                #region Wardrobe3_info
                /// <summary>
                /// Returns the item ID in the given index of the wardrobe3.
                /// </summary>
                /// <param name="iStructIndex">The index of the wardrobe3 structure. Not the index of the wardrobe3 as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>The item ID as a ushort.</returns>
                public static ushort get_wardrobe3_item_id(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Wardrobe3, (iStructIndex * 44), 2);
                }
                public static ushort get_wardrobe3_item_id(short iStructIndex)
                {
                    return get_wardrobe3_item_id(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the quantity of an item in the given index of the wardrobe3.
                /// </summary>
                /// <param name="iStructIndex">The index of the wardrobe3 structure. Not the index of the wardrobe3 as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</returns>
                public static byte get_wardrobe3_item_quan(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Wardrobe3, (iStructIndex * 44) + 4, 1);
                }
                public static byte get_wardrobe3_item_quan(short iStructIndex)
                {
                    return get_wardrobe3_item_quan(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns a bool indicating whether the item at the given index in the wardrobe3
                /// is currently equipped or not.
                /// </summary>
                /// <param name="structIndex">The index of the wardrobe3 structure. Not the index of the wardrobe3 as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</param>
                /// <returns></returns>
                public static bool get_wardrobe3_item_equipped(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte equ = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Wardrobe3, (iStructIndex * 44) + 8, 1);
                    return (equ == 5) ? true : false;
                }
                public static bool get_wardrobe3_item_equipped(short iStructIndex)
                {
                    return get_wardrobe3_item_equipped(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <returns>First available index as a byte.</returns>
                public static byte get_wardrobe3_index(int iProcIndex, ushort iItemID, byte iQuantity)
                {
                    return get_wardrobe3_index(iProcIndex, iItemID, iQuantity, 1);
                }
                public static byte get_wardrobe3_index(ushort iItemID, byte iQuantity)
                {
                    return get_wardrobe3_index(processIndex, iItemID, iQuantity, 1);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <param name="iStartIndex">Wardrobe3 index to start looking at (1 if not given).</param>
                /// <returns>First available index after the start index as a byte.</returns>
                public static byte get_wardrobe3_index(int iProcIndex, ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    byte maxCount = get_max_wardrobe3(iProcIndex);
                    for (byte ii = iStartIndex; ii <= maxCount; ii++)
                    {
                        ushort readID = get_wardrobe3_item_id(iProcIndex, ii);
                        if (readID == iItemID)
                        {
                            byte readQuan = get_wardrobe3_item_quan(iProcIndex, ii);
                            if (readQuan >= iQuantity)
                            {
                                return ii;
                            }
                        }
                    }
                    return 0;
                }
                public static byte get_wardrobe3_index(ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    return get_wardrobe3_index(processIndex, iItemID, iQuantity, iStartIndex);
                }
                public static byte get_wardrobe3_occupancy(int iProcIndex)
                {
                    byte count = 0;
                    byte maxCount = get_max_wardrobe3(iProcIndex);
                    for (byte ii = 1; ii <= maxCount; ii++)
                    {
                        if (get_wardrobe3_item_id(iProcIndex, ii) != 0)
                        {
                            count++;
                        }
                    }
                    return count;
                }
                public static byte get_wardrobe3_occupancy()
                {
                    return get_wardrobe3_occupancy(processIndex);
                }
                #endregion Wardrobe3 info
                #region Wardrobe4_info
                /// <summary>
                /// Returns the item ID in the given index of the wardrobe4.
                /// </summary>
                /// <param name="iStructIndex">The index of the wardrobe4 structure. Not the index of the wardrobe4 as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>The item ID as a ushort.</returns>
                public static ushort get_wardrobe4_item_id(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Wardrobe4, (iStructIndex * 44), 2);
                }
                public static ushort get_wardrobe4_item_id(short iStructIndex)
                {
                    return get_wardrobe4_item_id(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the quantity of an item in the given index of the wardrobe4.
                /// </summary>
                /// <param name="iStructIndex">The index of the wardrobe4 structure. Not the index of the wardrobe4 as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</returns>
                public static byte get_wardrobe4_item_quan(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Wardrobe4, (iStructIndex * 44) + 4, 1);
                }
                public static byte get_wardrobe4_item_quan(short iStructIndex)
                {
                    return get_wardrobe4_item_quan(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns a bool indicating whether the item at the given index in the wardrobe4
                /// is currently equipped or not.
                /// </summary>
                /// <param name="structIndex">The index of the wardrobe4 structure. Not the index of the wardrobe4 as
                /// shown in game. Index starts at 1 and goes to max quantity.</param>
                /// <returns>Quantity in this slot as a byte (0-99).</param>
                /// <returns></returns>
                public static bool get_wardrobe4_item_equipped(int iProcIndex, short iStructIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte equ = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Wardrobe4, (iStructIndex * 44) + 8, 1);
                    return (equ == 5) ? true : false;
                }
                public static bool get_wardrobe4_item_equipped(short iStructIndex)
                {
                    return get_wardrobe4_item_equipped(processIndex, iStructIndex);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <returns>First available index as a byte.</returns>
                public static byte get_wardrobe4_index(int iProcIndex, ushort iItemID, byte iQuantity)
                {
                    return get_wardrobe4_index(iProcIndex, iItemID, iQuantity, 1);
                }
                public static byte get_wardrobe4_index(ushort iItemID, byte iQuantity)
                {
                    return get_wardrobe4_index(processIndex, iItemID, iQuantity, 1);
                }
                /// <summary>
                /// Returns the first available (currently unused or equipped) inventory index of the given item
                /// that has at least the given quantity available.
                /// </summary>
                /// <param name="iItemID">The item ID of the item in question.</param>
                /// <param name="iQuantity">The minimum quantity required.</param>
                /// <param name="iStartIndex">Wardrobe4 index to start looking at (1 if not given).</param>
                /// <returns>First available index after the start index as a byte.</returns>
                public static byte get_wardrobe4_index(int iProcIndex, ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    byte maxCount = get_max_wardrobe4(iProcIndex);
                    for (byte ii = iStartIndex; ii <= maxCount; ii++)
                    {
                        ushort readID = get_wardrobe4_item_id(iProcIndex, ii);
                        if (readID == iItemID)
                        {
                            byte readQuan = get_wardrobe4_item_quan(iProcIndex, ii);
                            if (readQuan >= iQuantity)
                            {
                                return ii;
                            }
                        }
                    }
                    return 0;
                }
                public static byte get_wardrobe4_index(ushort iItemID, byte iQuantity, byte iStartIndex)
                {
                    return get_wardrobe4_index(processIndex, iItemID, iQuantity, iStartIndex);
                }
                public static byte get_wardrobe4_occupancy(int iProcIndex)
                {
                    byte count = 0;
                    byte maxCount = get_max_wardrobe4(iProcIndex);
                    for (byte ii = 1; ii <= maxCount; ii++)
                    {
                        if (get_wardrobe4_item_id(iProcIndex, ii) != 0)
                        {
                            count++;
                        }
                    }
                    return count;
                }
                public static byte get_wardrobe4_occupancy()
                {
                    return get_wardrobe4_occupancy(processIndex);
                }
                #endregion Wardrobe4 info
            }
            public static class Equipment
            {
                #region Generic Functions
                private enum SLOT : byte
                {
                    MAIN,
                    SUB,
                    RANGE,
                    AMMO,
                    HEAD,
                    BODY,
                    HANDS,
                    LEGS,
                    FEET,
                    NECK,
                    WAIST,
                    EARL,
                    EARR,
                    RINGL,
                    RINGR,
                    BACK
                }
                private static ushort get_id(int iProcIndex, SLOT iSlot)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte location = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_EquippedTable, ((byte)iSlot * 8) + 5, 1);
                    byte bagIndex = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_EquippedTable, ((byte)iSlot * 8) + 4, 1);
                    return (bagIndex == 0) ? (ushort)0 : (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Bag, (int)(location * Pointers.SizeOf_Inv_Container) + (bagIndex * 44), 2);
                }
                private static bool get_equipped(int iProcIndex, SLOT iSlot)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte bagIndex = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_EquippedTable, ((byte)iSlot * 8) + 4, 1);
                    return (bagIndex == 0) ? false : true;
                }
                #endregion Generic Functions
                #region Main
                public static ushort get_main_id()
                {
                    return get_main_id(processIndex);
                }
                public static ushort get_main_id(int iProcIndex)
                {
                    return get_id(iProcIndex, SLOT.MAIN);
                }
                public static bool get_main_equipped()
                {
                    return get_main_equipped(processIndex);
                }
                public static bool get_main_equipped(int iProcIndex)
                {
                    return get_equipped(iProcIndex, SLOT.MAIN);
                }
                #endregion Main
                #region Sub
                public static ushort get_sub_id()
                {
                    return get_sub_id(processIndex);
                }
                public static ushort get_sub_id(int iProcIndex)
                {
                    return get_id(iProcIndex, SLOT.SUB);
                }
                public static bool get_sub_equipped()
                {
                    return get_sub_equipped(processIndex);
                }
                public static bool get_sub_equipped(int iProcIndex)
                {
                    return get_equipped(iProcIndex, SLOT.SUB);
                }
                #endregion Sub
                #region Ranged
                public static ushort get_range_id()
                {
                    return get_range_id(processIndex);
                }
                public static ushort get_range_id(int iProcIndex)
                {
                    return get_id(iProcIndex, SLOT.RANGE);
                }
                public static bool get_range_equipped()
                {
                    return get_range_equipped(processIndex);
                }
                public static bool get_range_equipped(int iProcIndex)
                {
                    return get_equipped(iProcIndex, SLOT.RANGE);
                }
                #endregion Ranged
                #region Ammo
                public static ushort get_ammo_id()
                {
                    return get_ammo_id(processIndex);
                }
                public static ushort get_ammo_id(int iProcIndex)
                {
                    return get_id(iProcIndex, SLOT.AMMO);
                }
                public static bool get_ammo_equipped()
                {
                    return get_ammo_equipped(processIndex);
                }
                public static bool get_ammo_equipped(int iProcIndex)
                {
                    return get_equipped(iProcIndex, SLOT.AMMO);
                }
                public static byte get_ammo_quan()
                {
                    return get_ammo_quan(processIndex);
                }
                public static byte get_ammo_quan(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    byte location = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_EquippedTable, ((byte)SLOT.AMMO * 8) + 5, 1);
                    byte bagIndex = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_EquippedTable, ((byte)SLOT.AMMO * 8) + 4, 1);
                    return (bagIndex == 0) ? (byte)0 : (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inv_Bag, (int)(location * Pointers.SizeOf_Inv_Container) + (bagIndex * 44) + 4, 1);
                }
                #endregion Ammo
                #region Head
                public static ushort get_head_id()
                {
                    return get_head_id(processIndex);
                }
                public static ushort get_head_id(int iProcIndex)
                {
                    return get_id(iProcIndex, SLOT.HEAD);
                }
                public static bool get_head_equipped()
                {
                    return get_head_equipped(processIndex);
                }
                public static bool get_head_equipped(int iProcIndex)
                {
                    return get_equipped(iProcIndex, SLOT.HEAD);
                }
                #endregion Head
                #region Neck
                public static ushort get_neck_id()
                {
                    return get_neck_id(processIndex);
                }
                public static ushort get_neck_id(int iProcIndex)
                {
                    return get_id(iProcIndex, SLOT.NECK);
                }
                public static bool get_neck_equipped()
                {
                    return get_neck_equipped(processIndex);
                }
                public static bool get_neck_equipped(int iProcIndex)
                {
                    return get_equipped(iProcIndex, SLOT.NECK);
                }
                #endregion Neck
                #region Left Ear
                public static ushort get_earL_id()
                {
                    return get_earL_id(processIndex);
                }
                public static ushort get_earL_id(int iProcIndex)
                {
                    return get_id(iProcIndex, SLOT.EARL);
                }
                public static bool get_earL_equipped()
                {
                    return get_earL_equipped(processIndex);
                }
                public static bool get_earL_equipped(int iProcIndex)
                {
                    return get_equipped(iProcIndex, SLOT.EARL);
                }
                #endregion Left Ear
                #region Right Ear
                public static ushort get_earR_id()
                {
                    return get_earR_id(processIndex);
                }
                public static ushort get_earR_id(int iProcIndex)
                {
                    return get_id(iProcIndex, SLOT.EARR);
                }
                public static bool get_earR_equipped()
                {
                    return get_earR_equipped(processIndex);
                }
                public static bool get_earR_equipped(int iProcIndex)
                {
                    return get_equipped(iProcIndex, SLOT.EARR);
                }
                #endregion Right Ear
                #region Body
                public static ushort get_body_id()
                {
                    return get_body_id(processIndex);
                }
                public static ushort get_body_id(int iProcIndex)
                {
                    return get_id(iProcIndex, SLOT.BODY);
                }
                public static bool get_body_equipped()
                {
                    return get_body_equipped(processIndex);
                }
                public static bool get_body_equipped(int iProcIndex)
                {
                    return get_equipped(iProcIndex, SLOT.BODY);
                }
                #endregion Body
                #region Hands
                public static ushort get_hands_id()
                {
                    return get_hands_id(processIndex);
                }
                public static ushort get_hands_id(int iProcIndex)
                {
                    return get_id(iProcIndex, SLOT.HANDS);
                }
                public static bool get_hands_equipped()
                {
                    return get_hands_equipped(processIndex);
                }
                public static bool get_hands_equipped(int iProcIndex)
                {
                    return get_equipped(iProcIndex, SLOT.HANDS);
                }
                #endregion Hands
                #region Left Ring
                public static ushort get_ringL_id()
                {
                    return get_ringL_id(processIndex);
                }
                public static ushort get_ringL_id(int iProcIndex)
                {
                    return get_id(iProcIndex, SLOT.RINGL);
                }
                public static bool get_ringL_equipped()
                {
                    return get_ringL_equipped(processIndex);
                }
                public static bool get_ringL_equipped(int iProcIndex)
                {
                    return get_equipped(iProcIndex, SLOT.RINGL);
                }
                #endregion Left Ring
                #region Right Ring
                public static ushort get_ringR_id()
                {
                    return get_ringR_id(processIndex);
                }
                public static ushort get_ringR_id(int iProcIndex)
                {
                    return get_id(iProcIndex, SLOT.RINGR);
                }
                public static bool get_ringR_equipped()
                {
                    return get_ringR_equipped(processIndex);
                }
                public static bool get_ringR_equipped(int iProcIndex)
                {
                    return get_equipped(iProcIndex, SLOT.RINGR);
                }
                #endregion Right Ring
                #region Back
                public static ushort get_back_id()
                {
                    return get_back_id(processIndex);
                }
                public static ushort get_back_id(int iProcIndex)
                {
                    return get_id(iProcIndex, SLOT.BACK);
                }
                public static bool get_back_equipped()
                {
                    return get_back_equipped(processIndex);
                }
                public static bool get_back_equipped(int iProcIndex)
                {
                    return get_equipped(iProcIndex, SLOT.BACK);
                }
                #endregion Back
                #region Waist
                public static ushort get_waist_id()
                {
                    return get_waist_id(processIndex);
                }
                public static ushort get_waist_id(int iProcIndex)
                {
                    return get_id(iProcIndex, SLOT.WAIST);
                }
                public static bool get_waist_equipped()
                {
                    return get_waist_equipped(processIndex);
                }
                public static bool get_waist_equipped(int iProcIndex)
                {
                    return get_equipped(iProcIndex, SLOT.WAIST);
                }
                #endregion Waist
                #region Legs
                public static ushort get_legs_id()
                {
                    return get_legs_id(processIndex);
                }
                public static ushort get_legs_id(int iProcIndex)
                {
                    return get_id(iProcIndex, SLOT.LEGS);
                }
                public static bool get_legs_equipped()
                {
                    return get_legs_equipped(processIndex);
                }
                public static bool get_legs_equipped(int iProcIndex)
                {
                    return get_equipped(iProcIndex, SLOT.LEGS);
                }
                #endregion Legs
                #region Feet
                public static ushort get_feet_id()
                {
                    return get_feet_id(processIndex);
                }
                public static ushort get_feet_id(int iProcIndex)
                {
                    return get_id(iProcIndex, SLOT.FEET);
                }
                public static bool get_feet_equipped()
                {
                    return get_feet_equipped(processIndex);
                }
                public static bool get_feet_equipped(int iProcIndex)
                {
                    return get_equipped(iProcIndex, SLOT.FEET);
                }
                #endregion Feet
            }
            public static class Camera
            {
                public static byte get_view_perspective()
                {
                    return get_view_perspective(processIndex);
                }
                public static byte get_view_perspective(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Fishing, (int)off_camera_perspective, 1);
                }
            }
        }
        public static class Target
        {
            // Should be LayoutKind.Explicit
            [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 42)]
            public struct TargetLockStruct
            {
                public ushort StructID;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
                private byte[] Padding1;
                public int CharID;
                public int Pointer;
                public int Code;
                public ushort Mask;
                public ushort SubMask;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
                private byte[] Padding2;
                public byte Locked;
            }
            public const short InvalidStructId = 0x00;
            public static bool get_target_lock_struct( ref TargetLockStruct targetLockStruct,  int iProcIndex = -1 )
            {
                int datasize = Marshal.SizeOf(typeof(TargetLockStruct));
                byte[] buffer = new byte[datasize];

                int procindex = processIndex;
                if (iProcIndex != -1)
                {
                    procindex = iProcIndex;
                }
                Process proc = processPointerList[procindex].MainProcess;

                UIntPtr npcInfoPtr = MemoryFunctions.GetPointer(proc.Handle, processPointerList[procindex].Info_TargetLock, 0);

                if (npcInfoPtr == UIntPtr.Zero)
                {
                    return false;
                }

                // Attempt a safe memory read and copy the buffer into the packed struct.
                GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                try
                {
                    if (MemoryFunctions.ReadBlock(proc.Handle, (uint)npcInfoPtr, buffer, (uint)datasize) == null)
                    {
                      return false;
                    }

                    targetLockStruct = (TargetLockStruct)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(TargetLockStruct));
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    handle.Free();
                }
                return true;
            }

            public static ushort get_id()
            {
                return get_id(processIndex);
            }
            /// <summary>
            /// This ID is the index of the target in the NPC table.
            /// NPC Map Pointer is at ID * 4 + (NPC map base).
            /// </summary>
            /// <param name="iProcIndex"></param>
            /// <returns>Index into the NPC table for the target.</returns>
            public static ushort get_id(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr temp = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_TargetLock);
                return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)temp, 0, 2);
                // +4 more is a 4-byte value that is in the NPC structure.
            }
            public static string get_name()
            {
                return get_name(processIndex);
            }
            public static string get_name(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr temp = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Target);
                LoggingFunctions.Debug("Target::get_name: target ptr = " + string.Format("{0:X}", (uint)temp), LoggingFunctions.DBG_SCOPE.MEMREADS);
                string tempString = MemoryFunctions.ReadString((IntPtr)proc.Handle, (uint)temp, 20);
                return tempString.Substring(0, tempString.Length - 1);
            }
            public static byte get_hp_perc()
            {
                return get_hp_perc(processIndex);
            }
            public static byte get_hp_perc(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr temp = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Target);
                return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)temp, 100, 1);
            }
            public static float get_position_x()
            {
                return get_position_x(processIndex);
            }
            public static float get_position_x(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr temp = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Target);
                temp = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)temp, 72);
                return MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)temp, 4);
            }
            public static float get_position_z()
            {
                return get_position_z(processIndex);
            }
            public static float get_position_z(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr temp = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Target);
                temp = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)temp, 72);
                return MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)temp, 8);
            }
            public static float get_position_y()
            {
                return get_position_y(processIndex);
            }
            public static float get_position_y(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr temp = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Target);
                temp = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)temp, 72);
                return MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)temp, 12);
            }
            public static float get_position_angle()
            {
                return get_position_angle(processIndex);
            }
            public static float get_position_angle(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr temp = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Target);
                temp = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)temp, 72);
                return MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)temp, 24);
            }
            public static float get_distance()
            {
                return get_distance(processIndex);
            }
            public static float get_distance(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr temp = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Target);
                temp = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)temp, 72);
                LoggingFunctions.Debug("Target::get_distance: target ptr = " + string.Format("{0:X}", (uint)temp), LoggingFunctions.DBG_SCOPE.MEMREADS);
                float distSqrd = MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)temp, off_target_dist);
                return (float)Math.Sqrt(distSqrd);
            }
            public static byte get_status()
            {
                return get_status(processIndex);
            }
            public static byte get_status(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr temp = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Target);
                temp = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)temp, 72);
                return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)temp, off_target_sta, 1);
            }
            public static bool get_locked()
            {
                return get_locked(processIndex);
            }
            public static bool get_locked(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr temp = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_TargetLock);
                byte lockValue = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)temp, (int)off_target_lock, 1);
                byte odd = (byte)(lockValue % 2);
                return ((odd == 1) ? true : false);
            }
        }
        public static class Fishing
        {
            public static UIntPtr get_fishing_ptr()
            {
                Process proc = processPointerList[processIndex].MainProcess;
                return MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Fishing + 8, 0);
            }
            public static int get_id1()
            {
                return get_id1(processIndex);
            }
            public static int get_id1(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Fishing + 8, 0);
                return (int)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 20, 4);//16, 4);
            }
            public static int get_id2()
            {
                return get_id2(processIndex);
            }
            public static int get_id2(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Fishing + 8, 0);
                return (int)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 24, 4);//20, 4);
            }
            public static int get_id3()
            {
                return get_id3(processIndex);
            }
            public static int get_id3(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Fishing + 8, 0);
                return (int)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 28, 4);//24, 4);
            }
            public static int get_large()
            {
                return get_large(processIndex);
            }
            public static int get_large(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Fishing + 8, 0);
                return (int)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 40, 4);//36, 4);
            }
            public static short get_max_hp()
            {
                return get_max_hp(processIndex);
            }
            public static short get_max_hp(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Fishing + 8, 0);
                return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 0, 2);
            }
            public static short get_cur_hp()
            {
                return get_cur_hp(processIndex);
            }
            public static short get_cur_hp(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Fishing + 8, 0);
                return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 4, 2);
            }
            public static bool set_cur_hp(short iData)
            {
                return set_cur_hp(processIndex, iData);
            }
            public static bool set_cur_hp(int iProcIndex, short iData)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Fishing + 8, 0);
                return MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, (int)iData, 4, 2);
            }
            public static byte get_rod_position()
            {
                return get_rod_position(processIndex);
            }
            public static byte get_rod_position(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Fishing, off_fishing_rod_pos, 1);
            }
            public static byte get_rod_location()
            {
                return get_rod_location(processIndex);
            }
            public static byte get_rod_location(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                byte temp = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Fishing, off_fishing_rod_pos, 1);
                if (temp == 2)
                {
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Fishing + 8);
                    byte temp2 = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 40, 1);
                    if (temp2 == 0)
                    {
                        return 1;   //Left most position
                    }
                    else
                    {
                        return 2;   //Right most position
                    }
                }
                else
                {
                    return 0;       //Center or near left/right
                }
            }
            // Fishing Structure:
            // 00-03: 4B: Max HP
            // 04-07: 4B: Cur HP
            // 08-11: 4B: Another HP value, always slightly higher than Cur HP
            // 12-15: 4B: Static value of 8 in case of Cave Cherax anyway...
            // 16-19: 4B: Static 0xFF_FF_FF_FE
            // 20-23: 4B: ID1
            // 24-27: 4B: ID2
            // 28-31: 4B: ID3
            // 32-35: 4B: Down count timer (total time?)
            // 36-39: 4B: 
            // 40-43: 4B: Large=1, else 0
            // 44-47: 4B: Static 0 in Cave Cherax case
            // 48-51: 4B: Arrow direction: 0=right, 1=left
            // 52-55: 4B: Timer of current arrow. Static when no arrow present.
            // 56+      : Everything else appears static.
            public static FISHING_ARROW_DIR get_arrow_direction()
            {
                return get_arrow_direction(processIndex);
            }
            public static FISHING_ARROW_DIR get_arrow_direction(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Fishing + 8, 0);
                byte data = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 48, 1);
                if (data == 0)
                {
                    return FISHING_ARROW_DIR.RIGHT;
                }
                else
                {
                    return FISHING_ARROW_DIR.LEFT;
                }
            }
            public static bool is_arrow_timer_zero()
            {
                return is_arrow_timer_zero(processIndex);
            }
            public static bool is_arrow_timer_zero(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Fishing + 8, 0);
                ushort data = (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 52, 2);
                if (data == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public static ushort get_arrow_timer_value()
            {
                return get_arrow_timer_value(processIndex);
            }
            public static ushort get_arrow_timer_value(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Fishing + 8, 0);
                return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 52, 2);
            }
            public static string get_last_catch()
            {
                return get_last_catch(processIndex, "");
            }
            public static string get_last_catch(int iProcIndex)
            {
                return get_last_catch(iProcIndex, "");
            }
            public static string get_last_catch(string iFisherName)
            {
                return get_last_catch(processIndex, iFisherName);
            }
            public static string get_last_catch(int iProcIndex, string iFisherName)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                const int maxLoopCnt = 4;   //max number of lines to recurse backwards in chat log
                int loopCnt = 1;
                string tempResult;
                string temp;
                string dummyLogLineNb = "";
                FFXIEnums.CHAT_MODE dummyCode = 0;
                do
                {
                    temp = MemReads.Chat.get_lineX(iProcIndex, loopCnt - 1, ref dummyCode, ref dummyLogLineNb);
                    if ((iFisherName != "") && (loopCnt == 1))
                    {
                        iFisherName = iFisherName.Remove(iFisherName.Length - 1);
                    }
                    bool hit_caught = temp.Contains(iFisherName + " caught") && !temp.Contains("cannot carry") && !temp.Contains("monster");
                    if (hit_caught)
                    {
                        int hit_index = temp.IndexOf("caught");
                        hit_index += 9;
                        if (temp.Contains("caught an"))
                        {
                            hit_index++;
                        }
                        int temp_len = temp.Length - hit_index - 1;  //hit index is beginning of fish name, -1 to remove the '!'
                        string name = temp.Substring(hit_index, temp_len);
                        if (temp.Contains("caught 2"))
                        {
                            name = name + " 2";
                        }
                        else if (temp.Contains("caught 3"))
                        {
                            name = name + " 3";
                        }
                        return name;
                    }
                    else if (temp.Contains("didn't catch anything"))
                    {
                        tempResult = "Didn't catch anything";
                    }
                    else if (temp.Contains("line breaks"))
                    {
                        tempResult = "Line broke";
                    }
                    else if (temp.Contains(iFisherName + " caught a monster"))
                    {
                        tempResult = "Monster";
                    }
                    else if ((temp.Contains("regretfully releases")) || (temp.Contains("cannot carry any more")))
                    {
                        tempResult = "Release";
                    }
                    else if (temp.Contains("obtains 1 gil."))
                    {
                        tempResult = "1 gil";
                    }
                    else if (temp.Contains("lost your catch due to your lack of skill"))
                    {
                        tempResult = "Not enough skill";
                    }
                    else if (temp.Contains("lost your catch"))
                    {
                        tempResult = "Catch got away";
                    }
                    else if (temp.Contains("rod breaks"))
                    {
                        tempResult = "Rod broke";
                    }
                    else if (temp.Contains("too small"))
                    {
                        tempResult = "Too small";
                    }
                    else if (temp.Contains("obtains 100 gil."))
                    {
                        tempResult = "100 gil";
                    }
                    else
                    {
                        tempResult = "Unknown";
                    }
                }
                while ((tempResult == "Unknown") && (++loopCnt <= maxLoopCnt));
                return tempResult;
            }
        }
        public static class Chat
        {
            public static uint get_index()
            {
                return get_index(processIndex);
            }
            public static uint get_index(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Chatlog);
                LoggingFunctions.Debug("Chat.get_index(): ptr = 0x" + string.Format("{0:X}", ((uint)ptr)), LoggingFunctions.DBG_SCOPE.MEMREADS);
                int off_to_index = 0;
                if (OS_Version == (int)FFXIEnums.OSVersion.XP)
                {
                    off_to_index = 240;
                }
                else
                {
                    off_to_index = 8;
                    ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Chatlog, 4);
                }
                uint lastLineNumber = (uint)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, off_to_index, 4);
                LoggingFunctions.Debug("Chat.get_index(): line nb: " + lastLineNumber + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                return lastLineNumber;
            }
            public static string get_lineX(int iNbFromEnd, ref FFXIEnums.CHAT_MODE oCode, ref string oLogicalLineNb)
            {
                return get_lineX(processIndex, iNbFromEnd, ref oCode, ref oLogicalLineNb);
            }
            public static string get_lineX(int iProcIndex, int iNbFromEnd, ref FFXIEnums.CHAT_MODE oCode, ref string oLogicalLineNb)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                if (iNbFromEnd > 50)
                {
                    System.Console.WriteLine("Cannot read more than 50 lines from end of log.");
                    return "";
                }
                else
                {
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Chatlog, 4);
                    LoggingFunctions.Debug("Chat.get_lineX: ptr_a: 0x " + string.Format("{0:x}", (uint)ptr), LoggingFunctions.DBG_SCOPE.MEMREADS);
                    int lastLineNumber = 0;
                    int off_to_index = 0;
                    if (OS_Version == (int)FFXIEnums.OSVersion.XP)
                    {
                        off_to_index = 240;
                    }
                    else
                    {
                        off_to_index = 8;
                        lastLineNumber = MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, off_to_index, 4);
                        ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)ptr, 4);
                    }
                    LoggingFunctions.Debug("Chat.get_lineX: ptr_b: 0x " + string.Format("{0:x}", (uint)ptr), LoggingFunctions.DBG_SCOPE.MEMREADS);
                    LoggingFunctions.Debug("Chat.get_lineX: off_to_index " + off_to_index, LoggingFunctions.DBG_SCOPE.MEMREADS);
                    if (OS_Version == (int)FFXIEnums.OSVersion.XP)
                    {
                        lastLineNumber = MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, off_to_index, 4);
                    }
                    LoggingFunctions.Debug("Chat.get_lineX: lastLineNumber: " + lastLineNumber + ", nbFromEnd: " + iNbFromEnd, LoggingFunctions.DBG_SCOPE.MEMREADS);
                    //lastLineNumber -= iNbFromEnd;
                    bool usePrevBlock = false;
                    int whichBlock = 0;
                    int whichLine = (lastLineNumber - iNbFromEnd - 1) % 50;
                    //if ((lastLineNumber % 50) <= iNbFromEnd)

                    //1. lln = 1050, nfe = 3
                    //  Should all be from the same block, current block.
                    //2. lln = 1001, nfe = 3
                    //  -2 & -1 are from previous block.
                    //  -0 = current block.
                    //3. lln = 1010, nfe = 45
                    //  -9 thru -0 from current, -44 thru -10 from previous.
                    if((iNbFromEnd != 0) && ((lastLineNumber % 50) != 0))
                    {
                        if(((lastLineNumber % 50) - iNbFromEnd) <= 0)
                        {
                            usePrevBlock = true;
                            whichBlock += 4;
                        }
                    }

                    int off_to_offset = 0;
                    int off_to_first_ptr = 0;
                    if (OS_Version == (int)FFXIEnums.OSVersion.XP)
                    {
                        off_to_offset = 38 + (2 * lastLineNumber);
                        off_to_first_ptr = 244;
                    }
                    else
                    {
                        //off_to_offset = 2 * (lastLineNumber - 1);
                        off_to_offset = 2 * whichLine;
                        if(usePrevBlock)
                        {
                            off_to_offset += 100;
                        }
                        off_to_first_ptr = 204;
                    }
                    LoggingFunctions.Debug("Chat.get_lineX: off_to_offset    " + off_to_offset, LoggingFunctions.DBG_SCOPE.MEMREADS);
                    LoggingFunctions.Debug("Chat.get_lineX: off_to_first_ptr " + off_to_first_ptr, LoggingFunctions.DBG_SCOPE.MEMREADS);
                    LoggingFunctions.Debug("Chat.get_lineX: ptr_c: 0x " + string.Format("{0:x}", (uint)ptr), LoggingFunctions.DBG_SCOPE.MEMREADS);
                    int lineOffset = MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, off_to_offset, 2);
                    //lineOffset += 60;
                    LoggingFunctions.Debug("Chat.get_lineX: lineOffset: 0x " + string.Format("{0:x}", lineOffset), LoggingFunctions.DBG_SCOPE.MEMREADS);
                    uint blockBegin = (uint)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, (off_to_first_ptr + whichBlock), 4);
                    LoggingFunctions.Debug("Chat.get_lineX: blockBegin: 0x " + string.Format("{0:x}", blockBegin), LoggingFunctions.DBG_SCOPE.MEMREADS);
                    /////////////////////////////
                    //This is the additional part
                    /////////////////////////////
                    string codeWord = "";
                    codeWord += (char)MemoryFunctions.ReadMem((IntPtr)proc.Handle, blockBegin, lineOffset, 1);
                    codeWord += (char)MemoryFunctions.ReadMem((IntPtr)proc.Handle, blockBegin, lineOffset + 1, 1);
                    //codeWord(2, (char)MemoryFunctions.ReadMem((IntPtr)proc.Handle, blockBegin, 0, (lineOffset + 1), 1));
                    //codeWord.PadRight(3, (char)MemoryFunctions.ReadMem((IntPtr)proc.Handle, blockBegin, 0, lineOffset, 1));
                    LoggingFunctions.Debug("Chat.get_lineX: codeWord is " + codeWord + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                    int codeInt;
                    try
                    {
                        if (!int.TryParse(codeWord, System.Globalization.NumberStyles.AllowHexSpecifier, null, out codeInt))
                        {
                            return "";
                        }
                    }
                    catch (System.FormatException)
                    {
                        //Something bad probably happened, ie we caught the memory while reloading.
                        oCode = 0;
                        return "";
                    }
                    oCode = (FFXIEnums.CHAT_MODE)codeInt;
                    oLogicalLineNb = "";
                    for (int ii = 0; ii < off_chat_logical_line_nb_len; ii++)
                    {
                        oLogicalLineNb += (char)MemoryFunctions.ReadMem((IntPtr)proc.Handle, blockBegin, (lineOffset + off_chat_logical_line_nb + ii), 1);
                    }
                    LoggingFunctions.Debug("Chat.get_lineX: oLogicalLineNb: " + oLogicalLineNb + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                    LoggingFunctions.Debug("Chat.get_lineX: Char: " + codeWord.ToString() + ", code: " + codeInt + ", enum: " + oCode.ToString() + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);

                    string result = MemoryFunctions.ReadStringChatFFXI((IntPtr)proc.Handle, blockBegin, (off_chat_text_start + lineOffset));
                    return result;
                }
            }
        }
        public static class Windows
        {
            public static class BannerText
            {
                public static string get_help_text()
                {
                    return get_help_text(processIndex);
                }
                public static string get_help_text(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Windows);
                    LoggingFunctions.Debug("get_help_text: help ptr1 = " + string.Format("{0:x}", ptr) + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                    if ((int)ptr == 0)
                    {
                        return "N/A";
                    }
                    ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)ptr, off_wind_help_text);
                    if ((int)ptr == 0)
                    {
                        return "N/A";
                    }
                    LoggingFunctions.Debug("get_help_text: help ptr2 = " + string.Format("{0:x}", ptr) + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                    string text = MemoryFunctions.ReadString((IntPtr)proc.Handle, (uint)ptr, 0);
                    LoggingFunctions.Debug("get_help_text: help text = " + text + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                    if ((text != null) && (text.Length > 0))
                    {
                        return text.Substring(0, text.Length - 1);
                    }
                    else
                    {
                        return text;
                    }
                }
                public static string get_top_left_text()
                {
                    return get_top_left_text(processIndex);
                }
                public static string get_top_left_text(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Windows2, 0);
                    if ((int)ptr == 0)
                    {
                        return "N/A";
                    }
                    ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)ptr, 236 /*204*/);
                    if ((int)ptr == 0)
                    {
                        return "N/A";
                    }
                    string text = MemoryFunctions.ReadString((IntPtr)proc.Handle, (uint)ptr, 0);
                    if (text.Length > 0)
                    {
                        return text.Substring(0, text.Length - 1);
                    }
                    else
                    {
                        return text;
                    }
                }
            }
            public static class Items
            {
                #region Main Window
                public static byte get_shop_quan_max()
                {
                    return get_shop_quan_max(processIndex);
                }
                public static byte get_shop_quan_max(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Windows, 8);
                                    //Pre 06.24.16: 4);
                    if ((int)ptr == 0)
                    {
                        return 0;
                    }
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 34, 1);
                }
                public static byte get_shop_quan_cur()
                {
                    return get_shop_quan_cur(processIndex);
                }
                public static byte get_shop_quan_cur(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Windows, 8);
                                    //Pre 06.24.16: 4);
                    if ((int)ptr == 0)
                    {
                        return 0;
                    }
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 35, 1);
                }
                public static void set_shop_quan_cur(byte iQuan)
                {
                    set_shop_quan_cur(processIndex, iQuan);
                }
                public static void set_shop_quan_cur(int iProcIndex, byte iQuan)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Windows, 8);
                    if ((int)ptr == 0)
                    {
                        return;
                    }
                    MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, iQuan, 35, 1);
                }
                public static string get_selected_item_name()
                {
                    return get_selected_item_name(processIndex);
                }
                public static string get_selected_item_name(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Windows, 12);
                    if ((int)ptr == 0)
                    {
                        return "N/A";
                    }
                    ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)ptr, 28);
                    if ((int)ptr == 0)
                    {
                        return "N/A";
                    }
                    ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)ptr, off_item_details_name_ptr);
                    if ((int)ptr == 0)
                    {
                        return "N/A";
                    }
                    string text = MemoryFunctions.ReadString((IntPtr)proc.Handle, (uint)ptr, 0);
                    if (text.Length > 0)
                    {
                        return text.Substring(0, text.Length - 1);
                    }
                    else
                    {
                        return text;
                    }
                }
                public static byte get_selected_item_inventory_index()
                {
                    return get_selected_item_inventory_index(processIndex);
                }
                public static byte get_selected_item_inventory_index(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Windows, 12);
                    if ((int)ptr == 0)
                    {
                        return 0xff;
                    }
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 38, 1);
                }
                public static ushort get_selected_item_id()
                {
                    return get_selected_item_id(processIndex);
                }
                public static ushort get_selected_item_id(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Windows, 12);
                    if ((int)ptr == 0)
                    {
                        return 0xffff;
                    }
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 36, 1);
                }
                public static byte get_selected_item_quan(FFXIEnums.INVENTORY_MENU iMenu)
                {
                    return get_selected_item_quan(processIndex, iMenu);
                }
                public static byte get_selected_item_quan(int iProcIndex, FFXIEnums.INVENTORY_MENU iMenu)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Windows, 12);
                    if ((int)ptr == 0)
                    {
                        return 0;
                    }
                    byte selIdx = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 38, 1);
                    byte selQuan = 0;
                    switch (iMenu)
                    {
                        case FFXIEnums.INVENTORY_MENU.BAG:
                            selQuan = MemReads.Self.Inventory.get_bag_item_quan(selIdx);
                            break;
                        case FFXIEnums.INVENTORY_MENU.SATCHEL:
                            selQuan = MemReads.Self.Inventory.get_satchel_item_quan(selIdx);
                            break;
                        case FFXIEnums.INVENTORY_MENU.SACK:
                            selQuan = MemReads.Self.Inventory.get_sack_item_quan(selIdx);
                            break;
                        case FFXIEnums.INVENTORY_MENU.CASE:
                            selQuan = MemReads.Self.Inventory.get_case_item_quan(selIdx);
                            break;
                        case FFXIEnums.INVENTORY_MENU.SAFE:
                            selQuan = MemReads.Self.Inventory.get_safe_item_quan(selIdx);
                            break;
                        case FFXIEnums.INVENTORY_MENU.STORAGE:
                            selQuan = MemReads.Self.Inventory.get_storage_item_quan(selIdx);
                            break;
                        case FFXIEnums.INVENTORY_MENU.LOCKER:
                            selQuan = MemReads.Self.Inventory.get_locker_item_quan(selIdx);
                            break;
                        default:
                            selQuan = 0;
                            break;
                    }
                    return selQuan;
                }
                public static bool get_selected_item_is_equipped(FFXIEnums.INVENTORY_MENU iMenu)
                {
                    return get_selected_item_is_equipped(processIndex, iMenu);
                }
                public static bool get_selected_item_is_equipped(int iProcIndex, FFXIEnums.INVENTORY_MENU iMenu)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Windows, 12);
                    if ((int)ptr == 0)
                    {
                        return false;
                    }
                    byte selIdx = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 38, 1);
                    bool selEqpd = false;
                    switch (iMenu)
                    {
                        case FFXIEnums.INVENTORY_MENU.BAG:
                            selEqpd = MemReads.Self.Inventory.get_bag_item_equipped(selIdx);
                            break;
                        case FFXIEnums.INVENTORY_MENU.SATCHEL:
                            //selEqpd = MemReads.Self.Inventory.get_satchel_item_equipped(selIdx);
                            break;
                        case FFXIEnums.INVENTORY_MENU.SACK:
                            //selEqpd = MemReads.Self.Inventory.get_sack_item_equipped(selIdx);
                            break;
                        case FFXIEnums.INVENTORY_MENU.SAFE:
                            break;
                        case FFXIEnums.INVENTORY_MENU.STORAGE:
                            break;
                        case FFXIEnums.INVENTORY_MENU.LOCKER:
                            break;
                        default:
                            break;
                    }
                    return selEqpd;
                }
                public static byte get_count()
                {
                    return get_count(processIndex);
                }
                public static byte get_count(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inventory, 0);
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, off_inv_count, 1);
                }
                public static byte get_selection_index()
                {
                    return get_selection_index(processIndex);
                }
                public static byte get_selection_index(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_Inventory, 0);
                    byte nbAbove = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, off_inv_nb_above, 1);
                    return (byte)(nbAbove + (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, off_inv_selected_position, 1));
                }
                #endregion Main Window
                #region Secondary Window
                public static bool get_sec_wnd_open()
                {
                    return get_sec_wnd_open(processIndex);
                }
                public static bool get_sec_wnd_open(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_InventorySecWnd, 0);
                    //byte botItemIdx = (byte)MemoryFunctions.ReadMem((IntPtr)iProc.Handle, (uint)ptr, off_inv_sec_wnd_nb_of_last_item_on_screen, 1);
                    //if (botItemIdx == 0)
                    ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)ptr, 56);
                    if (ptr == (UIntPtr)0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                public static bool get_left_wnd_selected()
                {
                    return get_left_wnd_selected(processIndex);
                }
                public static bool get_left_wnd_selected(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_InventorySecWnd, 0);
                    byte leftWndSelected = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, off_inv_left_wnd_selected, 1);
                    if (leftWndSelected == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                public static byte get_sec_wnd_count()
                {
                    return get_sec_wnd_count(processIndex);
                }
                public static byte get_sec_wnd_count(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_InventorySecWnd, 0);
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, off_inv_sec_wnd_count, 1);
                }
                public static byte get_sec_wnd_selection_index()
                {
                    return get_sec_wnd_selection_index(processIndex);
                }
                public static byte get_sec_wnd_selection_index(int iProcIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_InventorySecWnd, 0);
                    byte nbAbove = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, off_inv_sec_wnd_nb_above, 1);
                    return (byte)(nbAbove + (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, off_inv_sec_wnd_selected_position, 1));
                }
                #endregion Secondary Window
                #region Deprecated Functions
                #region Item_Details_info
                //public static short info_item_details_get_itemID(int iProcIndex, byte index)
                //{
                //    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, info_item_details, (index * info_item_details_next_item) + info_item_details_item_id, 2);
                //}
                //public static short info_item_details_get_itemID(int iProcIndex, string itemName)
                //{
                //    byte count = 0;
                //    uint addr = (uint)mod.BaseAddress + info_item_details;
                //    uint ptrToName = 0;
                //    while (count < 80)
                //    {
                //        ptrToName = (uint)MemoryFunctions.GetPointer((IntPtr)proc.Handle, addr, info_item_details_name_ptr);
                //        string temp = MemoryFunctions.ReadString((IntPtr)proc.Handle, ptrToName, 0, 0);
                //        temp = temp.Substring(0, temp.Length-1);
                //        if (temp == itemName)
                //        {
                //            return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, addr, 0, info_item_details_item_id, 2);
                //        }
                //        else
                //        {
                //            addr += info_item_details_next_item;
                //            count++;
                //        }
                //    }
                //    return 0;
                //}
                //public static void info_item_details_get_itemID_Name(int iProcIndex, byte index, ref short itemID, ref string itemName)
                //{
                //    uint itemPtr = (uint)mod.BaseAddress + info_item_details + (uint)(index * info_item_details_next_item);
                //    itemID = (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, itemPtr, 0, info_item_details_item_id, 2);
                //    if (itemID != 0)
                //    {
                //        uint itemNamePtr = itemPtr + info_item_details_name_ptr;
                //        itemNamePtr = (uint)MemoryFunctions.GetPointer((IntPtr)proc.Handle, itemNamePtr, 0);
                //        itemName = MemoryFunctions.ReadString((IntPtr)proc.Handle, itemNamePtr, 0, 0);
                //        itemName = itemName.Substring(0, itemName.Length - 1);
                //    }
                //    else
                //    {
                //        itemName = "";
                //    }
                //}
                #endregion Item_Details_info
                #region Item_Details_info_preloaded
                //public static void info_item_details_pre_get_itemID_Name(int iProcIndex, byte index, ref short itemID, ref string itemName)
                //{
                //    itemID = (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, info_item_details_preloaded, (index * info_item_details_pre_next_item), 2);
                //    itemName = MemoryFunctions.ReadString((IntPtr)proc.Handle, info_item_details_preloaded, ((index * info_item_details_pre_next_item) + info_item_details_pre_name));
                //    itemName = itemName.Substring(0, itemName.Length - 1);
                //}
                //public static void info_item_details_pre_get_itemID_Name(int iProcIndex, byte index, ref short itemID, ref string itemName, ref byte itemType)
                //{
                //    itemID = (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, info_item_details_preloaded, (index * info_item_details_pre_next_item), 2);
                //    itemName = MemoryFunctions.ReadString((IntPtr)proc.Handle, info_item_details_preloaded, ((index * info_item_details_pre_next_item) + info_item_details_pre_name));
                //    itemName = itemName.Substring(0, itemName.Length - 1);
                //    itemType = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, info_item_details_preloaded, ((index * info_item_details_pre_next_item) + info_item_details_pre_type), 1);
                //}
                //public static short info_item_details_pre_get_itemID(int iProcIndex, byte index)
                //{
                //    return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, info_item_details_preloaded, (index * info_item_details_pre_next_item), 2);
                //}
                //public static string info_item_details_pre_get_item_name(int iProcIndex, byte index)
                //{
                //    string itemName = MemoryFunctions.ReadString((IntPtr)proc.Handle, info_item_details_preloaded, ((index * info_item_details_pre_next_item) + info_item_details_pre_name));
                //    return itemName.Substring(0, itemName.Length - 1);
                //}
                //public static byte info_item_details_pre_get_type(int iProcIndex, byte index)
                //{
                //    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, info_item_details_preloaded, ((index * info_item_details_pre_next_item) + info_item_details_pre_type), 1);
                //}
                //public static byte info_item_details_pre_get_stack_size(int iProcIndex, byte index)
                //{
                //    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, info_item_details_preloaded, ((index * info_item_details_pre_next_item) + info_item_details_pre_stack_sze), 1);
                //}
                //public static byte info_item_details_pre_get_level(int iProcIndex, byte index)
                //{
                //    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, info_item_details_preloaded, ((index * info_item_details_pre_next_item) + info_item_details_pre_level), 1);
                //}
                #endregion Item_Details_info_preloaded
                #endregion Deprecated Functions
            }
            public static class Menus
            {
                public static class TextStyle
                {
                    public static List<string> get_items()
                    {
                        return get_items(processIndex);
                    }
                    public static List<string> get_items(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        List<string> strList = new List<string>();
                        if(!is_open(iProcIndex))
                        {
                            return strList;
                        }
                        UIntPtr menuStructPtr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_MenuBase, 0);
                        LoggingFunctions.Debug("Menus.TextStyle.get_items: menuStructPtr = " + string.Format("{0:X}", (uint)menuStructPtr) + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                        if (menuStructPtr == (UIntPtr)0)
                        {
                            return strList;
                        }
                        UIntPtr itemsStructPtr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)menuStructPtr, 12);
                        LoggingFunctions.Debug("Menus.TextStyle.get_items: itemsStructPtr = " + string.Format("{0:X}", (uint)itemsStructPtr) + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                        if (itemsStructPtr == (UIntPtr)0)
                        {
                            return strList;
                        }
                        UIntPtr linkPtr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)itemsStructPtr, 20);
                        LoggingFunctions.Debug("Menus.TextStyle.get_items: linkPtr = " + string.Format("{0:X}", (uint)linkPtr) + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                        while ((uint)linkPtr != 0)
                        {
                            UIntPtr strPtr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)linkPtr, 16);
                            string str = MemoryFunctions.ReadStringUniFfxi((IntPtr)proc.Handle, (uint)strPtr, 4, 262);
                            if (str != "")
                            {
                                strList.Add(str);
                            }
                            linkPtr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)linkPtr, 0);
                            LoggingFunctions.Debug("Menus.TextStyle.get_items: str '" + str + "'.", LoggingFunctions.DBG_SCOPE.MEMREADS);
                            LoggingFunctions.Debug("Menus.TextStyle.get_items: linkPtr = " + string.Format("{0:X}", (uint)linkPtr) + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                        }
                        return strList;
                    }
                    public static string get_top_text()
                    {
                        return get_top_text(processIndex);
                    }
                    public static string get_top_text(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        if(!is_open(iProcIndex))
                        {
                            return "";
                        }
                        UIntPtr menuStructPtr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_MenuBase, 0);
                        LoggingFunctions.Debug("Menus.TextStyle.get_top_text: menuStructPtr = " + string.Format("{0:X}", (uint)menuStructPtr) + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                        if (menuStructPtr == (UIntPtr)0)
                        {
                            return "";
                        }
                        UIntPtr itemsStructPtr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)menuStructPtr, 12);
                        LoggingFunctions.Debug("Menus.TextStyle.get_top_text: itemsStructPtr = " + string.Format("{0:X}", (uint)itemsStructPtr) + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                        if (itemsStructPtr == (UIntPtr)0)
                        {
                            return "";
                        }
                        string topText = MemoryFunctions.ReadStringUniFfxi((IntPtr)proc.Handle, (uint)itemsStructPtr, 56, 54); //54 prior to 8/11/14
                        LoggingFunctions.Debug("Menus.TextStyle.get_top_text: topText = '" + topText + "'.", LoggingFunctions.DBG_SCOPE.MEMREADS);
                        return topText;
                    }
                    public static short get_curr_index()
                    {
                        return get_curr_index(processIndex);
                    }
                    public static short get_curr_index(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        if(!is_open(iProcIndex))
                        {
                            return -1;
                        }
                        UIntPtr menuStructPtr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_MenuBase, 0);
                        LoggingFunctions.Debug("Menus.TextStyle.get_curr_index: menuStructPtr = " + string.Format("{0:X}", (uint)menuStructPtr) + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                        if (menuStructPtr == (UIntPtr)0)
                        {
                            return -1;
                        }
                        UIntPtr itemsStructPtr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)menuStructPtr, 12);
                        LoggingFunctions.Debug("Menus.TextStyle.get_curr_index: itemsStructPtr = " + string.Format("{0:X}", (uint)itemsStructPtr) + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                        if (itemsStructPtr == (UIntPtr)0)
                        {
                            return -1;
                        }
                        short currIdx = (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)itemsStructPtr, 48, 2);
                        LoggingFunctions.Debug("Menus.TextStyle.get_curr_index: currIdx = " + currIdx + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                        return currIdx;
                    }
                    public static short get_item_count()
                    {
                        return get_item_count(processIndex);
                    }
                    public static short get_item_count(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        if(!is_open(iProcIndex))
                        {
                            return 0;
                        }
                        UIntPtr menuStructPtr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_MenuBase, 0);
                        LoggingFunctions.Debug("Menus.TextStyle.get_item_count: menuStructPtr = " + string.Format("{0:X}", (uint)menuStructPtr) + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                        if (menuStructPtr == (UIntPtr)0)
                        {
                            return -1;
                        }
                        UIntPtr itemsStructPtr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, (uint)menuStructPtr, 12);
                        LoggingFunctions.Debug("Menus.TextStyle.get_item_count: itemsStructPtr = " + string.Format("{0:X}", (uint)itemsStructPtr) + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                        if (itemsStructPtr == (UIntPtr)0)
                        {
                            return -1;
                        }
                        short nbItems = (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)itemsStructPtr, 40, 2);
                        LoggingFunctions.Debug("Menus.TextStyle.get_item_count: nbItems = " + nbItems + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                        return nbItems;
                    }
                    public static bool is_open()
                    {
                        return is_open(processIndex);
                    }

                    public static bool is_open(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        byte val = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)processPointerList[processIndex].Info_MenuBaseTextStyle, 0, 1);
                        return val != 0;
                    }
                }
                public static class ButtonStyle
                {
                    /// <summary>
                    /// Gets the zero-based index of the currently selected button menu item.
                    /// </summary>
                    /// <returns>Zero-based index.</returns>
                    public static short get_curr_index()
                    {
                        return get_curr_index(processIndex);
                    }
                    public static short get_curr_index(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        UIntPtr menuStructPtr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_MenuBase, 0);
                        LoggingFunctions.Debug("Menus.ButtonStyle.get_curr_index: menuStructPtr = " + string.Format("{0:X}", (uint)menuStructPtr) + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                        if (menuStructPtr == (UIntPtr)0)
                        {
                            return -1;
                        }
                        short selIndex = (short)(MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)menuStructPtr, 76, 2) - 1);
                        LoggingFunctions.Debug("Menus.ButtonStyle.get_curr_index: selIndex = " + selIndex + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                        return selIndex;
                    }
                    public static void set_curr_index(ushort iIndex)
                    {
                        set_curr_index(processIndex, iIndex);
                    }
                    public static void set_curr_index(int iProcIndex, ushort iIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        UIntPtr menuStructPtr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_MenuBase, 0);
                        if (menuStructPtr == (UIntPtr)0)
                        {
                            return;
                        }
                        MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)menuStructPtr, iIndex, 76, 2);
                    }
                }
            }
            public static class Shops
            {
                public static class NPC
                {
                    public static void get_buy_id_to_price_map(ref Dictionary<ushort, uint> oPriceMap, ref Dictionary<ushort, ushort> oIndexMap)
                    {
                        get_buy_id_to_price_map(processIndex, ref oPriceMap, ref oIndexMap);
                    }
                    public static Dictionary<ushort, uint> get_buy_id_to_price_map(int iProcIndex, ref Dictionary<ushort, uint> oPriceMap, ref Dictionary<ushort, ushort> oIndexMap)
                    {
                        #region Map Inits
                        if (oPriceMap == null)
                        {
                            oPriceMap = new Dictionary<ushort, uint>();
                        }
                        else
                        {
                            oPriceMap.Clear();
                        }
                        if (oIndexMap == null)
                        {
                            oIndexMap = new Dictionary<ushort, ushort>();
                        }
                        else
                        {
                            oIndexMap.Clear();
                        }
                        #endregion Map Inits
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        ushort itemId = 0;
                        uint itemPrice = 0;
                        byte index = 0;
                        byte indexContinuous = 0;
                        while ((itemId != 1) && (index < off_npc_shop_max_items))
                        {
                            //go thru the struct reading id's.
                            //only add the entry if the id is not 0.
                            //check to make sure that the last entry is always 1.
                            itemPrice = (uint)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_ShopBuyWindow, (index * off_npc_shop_nxt_item) + off_npc_shop_item_price, 4);
                            itemId = (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_ShopBuyWindow, (index * off_npc_shop_nxt_item) + off_npc_shop_item_id, 2); //Was +8 prior to 6/17/14
                            if ((itemId != 0) && (itemPrice != 0))
                            {
                                oPriceMap.Add(itemId, itemPrice);
                                oIndexMap.Add(itemId, index);
                                indexContinuous++;
                            }
                            index++;
                        }
                        return oPriceMap;
                    }
                }
                public static class Guild
                {
                    #region Prices
                    //Price of item in the list.
                    public static uint get_buy_price(uint iIndex)
                    {
                        return get_buy_price(processIndex, iIndex);
                    }
                    public static uint get_buy_price(int iProcIndex, uint iIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        return (uint)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[iProcIndex].Info_ShopBuyWindow, (int)((iIndex * off_gld_shop_nxt_item) + off_npc_shop_item_price), 4);
                    }
                    public static uint get_buy_price_from_id(ushort iItemId)
                    {
                        return get_buy_price(processIndex, iItemId);
                    }
                    public static uint get_buy_price_from_id(int iProcIndex, ushort iItemId)
                    {
                        List<ushort> itemIds = get_buy_item_ids(iProcIndex);
                        int idx = itemIds.IndexOf(iItemId);
                        if (idx < 0)
                        {
                            return 0;
                        }
                        else
                        {
                            return get_buy_price(iProcIndex, (uint)idx);
                        }
                    }
                    #endregion Prices
                    #region Item IDs
                    //Item ID(s) from the list.
                    public static ushort get_buy_item_id(uint iIndex)
                    {
                        return get_buy_item_id(processIndex, iIndex);
                    }
                    public static ushort get_buy_item_id(int iProcIndex, uint iIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_ShopBuyWindow, (int)((iIndex * off_gld_shop_nxt_item) + off_npc_shop_item_id), 2);
                    }
                    public static List<ushort> get_buy_item_ids()
                    {
                        return get_buy_item_ids(processIndex);
                    }
                    public static List<ushort> get_buy_item_ids(int iProcIndex)
                    {
                        //int nbItems = get_buy_nb_listed_items(iProcIndex);
                        List<ushort> itemIds = new List<ushort>();
                        for (uint ii = 0; ii <= off_gld_shop_max_items; ii++)
                        {
                            ushort id = get_buy_item_id(iProcIndex, ii);
                            if (id != 0)
                            {
                                itemIds.Add(id);
                            }
                            else
                            {
                                break;
                            }
                        }
                        return itemIds;
                    }
                    #endregion Item IDs
                    #region Maps
                    //Get a full map of ID to price and ID to stock.
                    public static void get_buy_maps(ref Dictionary<ushort, uint> oPriceMap, ref Dictionary<ushort, byte> oStockMap, ref Dictionary<ushort, ushort> oIndexMap)
                    {
                        get_buy_maps(processIndex, ref oPriceMap, ref oStockMap, ref oIndexMap);
                    }
                    public static void get_buy_maps(int iProcIndex, ref Dictionary<ushort, uint> oPriceMap, ref Dictionary<ushort, byte> oStockMap, ref Dictionary<ushort, ushort> oIndexMap)
                    {
                        #region Map Inits
                        if (oPriceMap == null)
                        {
                            oPriceMap = new Dictionary<ushort, uint>();
                        }
                        else
                        {
                            oPriceMap.Clear();
                        }
                        if (oStockMap == null)
                        {
                            oStockMap = new Dictionary<ushort, byte>();
                        }
                        else
                        {
                            oStockMap.Clear();
                        }
                        if (oIndexMap == null)
                        {
                            oIndexMap = new Dictionary<ushort, ushort>();
                        }
                        else
                        {
                            oIndexMap.Clear();
                        }
                        #endregion Map Inits
                        uint price = 0;
                        for (ushort ii = 0; ii < off_gld_shop_max_items; ii++)
                        {
                            ushort id = get_buy_item_id(iProcIndex, ii);
                            if (id != 0)
                            {
                                price = get_buy_price(iProcIndex, ii);
                                if (price != 0)
                                {
                                    oPriceMap.Add(id, price);
                                    oStockMap.Add(id, 255);
                                    oIndexMap.Add(id, ii);
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    #endregion Maps
                }
                public static class ItemWindow
                {
                    public static ushort get_nb_listed_items()
                    {
                        return get_nb_listed_items(processIndex);
                    }
                    public static ushort get_nb_listed_items(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_InventoryNpcWnd, 0);
                        return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, off_shop_item_nb_items, 2);
                    }
                    /// <summary>
                    /// Gets the current index starting at 1 in the NPC buy/sell menu.
                    /// </summary>
                    /// <returns></returns>
                    public static ushort get_cur_idx()
                    {
                        return get_cur_idx(processIndex);
                    }
                    /// <summary>
                    /// Gets the current index starting at 1 in the NPC buy/sell menu.
                    /// </summary>
                    /// <returns></returns>
                    public static ushort get_cur_idx(int iProcIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_InventoryNpcWnd, 0);
                        ushort topIdx = (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, off_shop_item_idx_top_item, 2);
                        ushort curIdx = (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, off_shop_item_idx_in_wnd, 2);
                        return (ushort)(topIdx + curIdx);
                    }
                    public static void set_cur_idx(ushort iIndex)
                    {
                        set_cur_idx(processIndex, iIndex);
                    }
                    public static void set_cur_idx(int iProcIndex, ushort iIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_InventoryNpcWnd, 0);
                        //All of the indexes are 1-based except topIdx (0-based). topIdx can also be thought of as the number of items above the visible window.
                        ushort nbItems = get_nb_listed_items(iProcIndex);
                        if (nbItems == 0)
                        {
                            return;
                        }
                        if (iIndex > nbItems)
                        {
                            iIndex = nbItems;
                        }
                        if (iIndex <= 10)
                        {
                            LoggingFunctions.Timestamp("Writing to ptr: " + string.Format("{0:X}", (uint)ptr));
                            //Simplest case, just set the topIdx to 0, botIdx to 10 or nbItems, and curIdx to iIndex.
                            MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, 0, off_shop_item_idx_top_item, 2);
                            //MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, iIndex, off_guild_shop_buy_idx_in_wnd, 2);
                            MemReads.Windows.Menus.ButtonStyle.set_curr_index(iIndex);
                        }
                        else
                        {
                            //Put the selected item at the very bottom. This will cover all other cases.
                            MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, (uint)(iIndex - 10), off_shop_item_idx_top_item, 2);
                            //MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, 10, off_guild_shop_buy_idx_in_wnd, 2);
                            MemReads.Windows.Menus.ButtonStyle.set_curr_index(10);
                        }
                    }
                }
            }
            public static class AH
            {
                public enum StackType : byte
                {
                    SINGLE = 3,
                    STACK = 4
                }
                public struct Item
                {
                    public ushort Id;
                    public ushort NbAvailable;
                    public bool Stack;

                    public Item(ushort iId, bool iStack, ushort iNbAvailable)
                    {
                        Id = iId;
                        Stack = iStack;
                        NbAvailable = iNbAvailable;
                    }
                }
                public static uint get_bid_price()
                {
                    Process proc = processPointerList[processIndex].MainProcess;
                    uint ptr = (uint)MemoryFunctions.GetPointer(proc.Handle, processPointerList[processIndex].Info_AhBidPrice);
                    uint l_retVal = (uint)MemoryFunctions.ReadMem(MainProcess.Handle, ptr, off_ah_bid_price, 4);
                    return l_retVal;
                }
                public static ushort get_list_count()
                {
                    Process proc = processPointerList[processIndex].MainProcess;
                    uint ptr = (uint)MemoryFunctions.GetPointer(proc.Handle, processPointerList[processIndex].Info_AhList);
                    ushort l_retVal = (ushort)MemoryFunctions.ReadMem(MainProcess.Handle, ptr, off_ah_list_cnt, 2);
                    return l_retVal;
                }
                public static ushort get_list_unique_count()
                {
                    Process proc = processPointerList[processIndex].MainProcess;
                    uint ptr = (uint)MemoryFunctions.GetPointer(proc.Handle, processPointerList[processIndex].Info_AhList);
                    ushort l_retVal = (ushort)MemoryFunctions.ReadMem(MainProcess.Handle, ptr, off_ah_list_unq_cnt, 2);
                    return l_retVal;
                }
                public static List<Item> get_list_items()
                {
                    List<Item> l_retVal = new List<Item>();

                    Process proc = processPointerList[processIndex].MainProcess;
                    uint ptr = (uint)MemoryFunctions.GetPointer(proc.Handle, processPointerList[processIndex].Info_AhList);
                    if (ptr == 0)
                    {
                        return null;
                    }
                    ptr = (uint)MemoryFunctions.GetPointer(proc.Handle, ptr, off_ah_list_ptr);
                    if (ptr == 0)
                    {
                        return null;
                    }

                    ushort l_cnt = get_list_count();

                    for (ushort ii = 0; ii < l_cnt; ii++)
                    {
                        int l_itemOff = off_ah_list_item_sz * ii;
                        ushort l_id = (ushort)MemoryFunctions.ReadMem(proc.Handle, ptr, l_itemOff + off_ah_list_item_id, 2);
                        if (l_id != 0)
                        {
                            byte l_stackVal = (byte)MemoryFunctions.ReadMem(proc.Handle, ptr, l_itemOff + off_ah_list_item_stack_val, 1);
                            int l_nbOffset = off_ah_list_item_count_sngl;
                            bool l_stack = l_stackVal == (byte)StackType.STACK;
                            if (l_stack)
                            {
                                l_nbOffset = off_ah_list_item_count_stack;
                            }
                            ushort l_nbAvail = (ushort)MemoryFunctions.ReadMem(proc.Handle, ptr, l_itemOff + l_nbOffset, 2);
                            Item l_item = new Item(l_id, l_stack, l_nbAvail);
                            l_retVal.Add(l_item);
                        }
                    }

                    return l_retVal;
                }
            }
            public static class Crafting
            {
                public static void get_item(byte iIndex, ref ushort oItemID, ref byte oQuantity, ref byte oBagIndex)
                {
                    get_item(processIndex, iIndex, ref oItemID, ref oQuantity, ref oBagIndex);
                }
                public static void get_item(int iProcIndex, byte iIndex, ref ushort oItemID, ref byte oQuantity, ref byte oBagIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_CraftWindow, 0);
                    LoggingFunctions.Debug("Crafter pointer: " + string.Format("{0:X}", (uint)ptr), LoggingFunctions.DBG_SCOPE.MEMREADS);
                    LoggingFunctions.Debug("Final ID address is: " + string.Format("{0:X}", ((uint)ptr + off_crafter_item_id + (2 * iIndex))), LoggingFunctions.DBG_SCOPE.MEMREADS);
                    oItemID = (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, (off_crafter_item_id + (2 * iIndex)), 2);
                    oQuantity = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, (off_crafter_qty + iIndex), 1);
                    oBagIndex = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, (off_crafter_bag_idx + iIndex), 1);
                    return;
                }
                public static void set_item(byte iIndex, ushort iItemID, byte iQuantity, byte iBagIndex)
                {
                    set_item(processIndex, iIndex, iItemID, iQuantity, iBagIndex);
                }
                public static void set_item(int iProcIndex, byte iIndex, ushort iItemID, byte iQuantity, byte iBagIndex)
                {
                    Process proc = processPointerList[iProcIndex].MainProcess;
                    UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_CraftWindow, 0);
                    MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, (uint)iItemID, (off_crafter_item_id + (2 * iIndex)), 2);
                    MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, (uint)iQuantity, (off_crafter_qty + iIndex), 1);
                    MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, (uint)iBagIndex, off_crafter_bag_idx + iIndex, 1);
                    return;
                }
            }
            public static class Trading
            {
                public static class NPC
                {
                    public static void get_item(byte iIndex, ref ushort oItemID, ref byte oQuantity, ref byte oBagIndex)
                    {
                        get_item(processIndex, iIndex, ref oItemID, ref oQuantity, ref oBagIndex);
                    }
                    public static void get_item(int iProcIndex, byte iIndex, ref ushort oItemID, ref byte oQuantity, ref byte oBagIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_TradeNpcWindow, 0);
                        oItemID = (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, (24 + (4 * iIndex)), 2);
                        oQuantity = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, (27 + (4 * iIndex)), 1);
                        oBagIndex = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, (26 + (4 * iIndex)), 1);
                        return;
                    }
                    public static void set_item(byte iIndex, ushort iItemID, byte iQuantity, byte iBagIndex)
                    {
                        set_item(processIndex, iIndex, iItemID, iQuantity, iBagIndex);
                    }
                    public static void set_item(int iProcIndex, byte iIndex, ushort iItemID, byte iQuantity, byte iBagIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_TradeNpcWindow, 0);
                        MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, (uint)iItemID, (24 + (4 * iIndex)), 2);
                        MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, (uint)iQuantity, (27 + (4 * iIndex)), 1);
                        MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, (uint)iBagIndex, (26 + (4 * iIndex)), 1);
                        return;
                    }
                    public static void get_gil(ref uint oGilQuan)
                    {
                        get_gil(processIndex, ref oGilQuan);
                    }
                    public static void get_gil(int iProcIndex, ref uint oGilQuan)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_TradeNpcWindow, 0);
                        oGilQuan = (uint)MemoryFunctions.ReadMem((IntPtr)proc.Handle, (uint)ptr, 60, 4);
                        return;
                    }
                    public static void set_gil(uint iGilQuan)
                    {
                        set_gil(processIndex, iGilQuan);
                    }
                    public static void set_gil(int iProcIndex, uint iGilQuan)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        UIntPtr ptr = MemoryFunctions.GetPointer((IntPtr)proc.Handle, processPointerList[processIndex].Info_TradeNpcWindow, 0);
                        MemoryFunctions.WriteMem((IntPtr)proc.Handle, (uint)ptr, iGilQuan, 60, 4);
                        return;
                    }
                }
                public static class PC
                {
                    public static void get_item(byte iIndex, ref ushort oItemID, ref byte oQuantity, ref byte oBagIndex)
                    {
                        get_item(processIndex, iIndex, ref oItemID, ref oQuantity, ref oBagIndex);
                    }
                    public static void get_item(int iProcIndex, byte iIndex, ref ushort oItemID, ref byte oQuantity, ref byte oBagIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        oItemID = (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_TradePcWindow, (4 + (8 * iIndex)), 2);
                        oQuantity = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_TradePcWindow, (0 + (8 * iIndex)), 1);
                        oBagIndex = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_TradePcWindow, (6 + (8 * iIndex)), 1);
                        return;
                    }
                    public static void set_item(byte iIndex, ushort iItemID, byte iQuantity, byte iBagIndex)
                    {
                        set_item(processIndex, iIndex, iItemID, iQuantity, iBagIndex);
                    }
                    public static void set_item(int iProcIndex, byte iIndex, ushort iItemID, byte iQuantity, byte iBagIndex)
                    {
                        Process proc = processPointerList[iProcIndex].MainProcess;
                        MemoryFunctions.WriteMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_TradePcWindow, (uint)iItemID, (4 + (8 * iIndex)), 2);
                        MemoryFunctions.WriteMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_TradePcWindow, (uint)iQuantity, (0 + (8 * iIndex)), 1);
                        MemoryFunctions.WriteMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_TradePcWindow, (uint)iBagIndex, (6 + (8 * iIndex)), 1);
                        return;
                    }
                }
            }
        }
        public static class Party
        {
            #region Get Members
            private static bool is_valid(int iProcIndex, byte iMemberIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                if (iMemberIndex > 6)
                {
                    return false;
                }
                byte isValid = (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Party, off_pty_valid + (off_pty_struct_size * iMemberIndex), 1);
                if (isValid != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public static byte get_member_index(string iName)
            {
                return get_member_index(processIndex, iName);
            }
            public static byte get_member_index(int iProcIndex, string iName)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                List<string> memberList = get_members(iProcIndex);
                for (int ii = 0; ii < memberList.Count; ii++)
                {
                    if (memberList[ii] == iName)
                    {
                        return (byte)(ii + 1);
                    }
                }
                return 0xff;
            }
            public static string get_member_name(byte iIndex)
            {
                return get_member_name(processIndex, iIndex);
            }
            public static string get_member_name(int iProcIndex, byte iIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                return MemoryFunctions.ReadString((IntPtr)proc.Handle, processPointerList[processIndex].Info_Party, off_pty_struct_size * iIndex);
            }
            public static List<string> get_members()
            {
                return get_members(processIndex);
            }
            public static List<string> get_members(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                List<string> memberList = new List<string>();
                for (byte ii = 1; ii <= 6; ii++)
                {
                    if (is_valid(iProcIndex, ii))
                    {
                        string name = MemoryFunctions.ReadString((IntPtr)proc.Handle, processPointerList[processIndex].Info_Party, off_pty_struct_size * ii);
                        if (name.Length > 0)
                        {
                            name = name.Substring(0, name.Length - 1);
                        }
                        memberList.Add(name);
                    }
                }
                return memberList;
            }
            public static byte get_member_count()
            {
                return get_member_count(processIndex);
            }
            public static byte get_member_count(int iProcIndex)
            {
                List<string> members = get_members(iProcIndex);
                return (byte)(members.Count + 1);
            }
            #endregion Get Members
            #region Get HP
            public static ushort get_member_hp(byte iMemberIndex)
            {
                return get_member_hp(processIndex, iMemberIndex);
            }
            public static ushort get_member_hp(int iProcIndex, byte iMemberIndex)
            {
                if ((iMemberIndex > 6) || (iMemberIndex < 0))
                {
                    return 0xffff;
                }
                Process proc = processPointerList[iProcIndex].MainProcess;
                if (is_valid(iProcIndex, iMemberIndex))
                {
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Party, off_pty_hp + (off_pty_struct_size * iMemberIndex), 2);
                }
                return 0xffff;
            }
            #endregion Get HP
            #region Get MP
            public static ushort get_member_mp(byte iMemberIndex)
            {
                return get_member_mp(processIndex, iMemberIndex);
            }
            public static ushort get_member_mp(int iProcIndex, byte iMemberIndex)
            {
                if ((iMemberIndex > 6) || (iMemberIndex < 0))
                {
                    return 0xffff;
                }
                Process proc = processPointerList[iProcIndex].MainProcess;
                if (is_valid(iProcIndex, iMemberIndex))
                {
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Party, off_pty_mp + (off_pty_struct_size * iMemberIndex), 2);
                }
                return 0xffff;
            }
            #endregion Get MP
            #region Get TP
            public static ushort get_member_tp(byte iMemberIndex)
            {
                return get_member_tp(processIndex, iMemberIndex);
            }
            public static ushort get_member_tp(int iProcIndex, byte iMemberIndex)
            {
                if ((iMemberIndex > 6) || (iMemberIndex < 0))
                {
                    return 0xffff;
                }
                Process proc = processPointerList[iProcIndex].MainProcess;
                if (is_valid(iProcIndex, iMemberIndex))
                {
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Party, off_pty_tp + (off_pty_struct_size * iMemberIndex), 2);
                }
                return 0xffff;
            }
            #endregion Get TP
            #region Get HPP
            public static byte get_member_hpp(byte iMemberIndex)
            {
                return get_member_hpp(processIndex, iMemberIndex);
            }
            public static byte get_member_hpp(int iProcIndex, byte iMemberIndex)
            {
                if ((iMemberIndex > 6) || (iMemberIndex < 0))
                {
                    return 0xff;
                }
                Process proc = processPointerList[iProcIndex].MainProcess;
                if (is_valid(iProcIndex, iMemberIndex))
                {
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Party, off_pty_hpp + (off_pty_struct_size * iMemberIndex), 1);
                }
                return 0xff;
            }
            #endregion Get HPP
            #region Get MPP
            public static byte get_member_mpp(byte iMemberIndex)
            {
                return get_member_mpp(processIndex, iMemberIndex);
            }
            public static byte get_member_mpp(int iProcIndex, byte iMemberIndex)
            {
                if ((iMemberIndex > 6) || (iMemberIndex < 0))
                {
                    return 0xff;
                }
                Process proc = processPointerList[iProcIndex].MainProcess;
                if (is_valid(iProcIndex, iMemberIndex))
                {
                    return (byte)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Party, off_pty_mpp + (off_pty_struct_size * iMemberIndex), 1);
                }
                return 0xff;
            }
            #endregion Get MPP
            #region Get Zone
            public static ushort get_member_zone(byte iMemberIndex)
            {
                return get_member_zone(processIndex, iMemberIndex);
            }
            public static ushort get_member_zone(int iProcIndex, byte iMemberIndex)
            {
                if ((iMemberIndex > 6) || (iMemberIndex < 0))
                {
                    return 0xffff;
                }
                Process proc = processPointerList[iProcIndex].MainProcess;
                if (is_valid(iProcIndex, iMemberIndex))
                {
                    return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Party, off_pty_zone + (off_pty_struct_size * iMemberIndex), 2);
                }
                return 0xffff;
            }
            #endregion Get Zone
        }
        public static class PCs
        {
            public static uint get_pointer(string iPlayer)
            {
                return get_pointer(processIndex, iPlayer);
            }
            public static uint get_pointer(string iPlayer, out uint oIndex, out uint oIndexAddress)
            {
                return get_pointer(processIndex, iPlayer, out oIndex, out oIndexAddress);
            }
            public static uint get_pointer(int iProcIndex, string iPlayer)
            {
                uint oIndex = 0;
                uint oIndexAddress = 0;
                return get_pointer(iProcIndex, iPlayer, out oIndex, out oIndexAddress);
            }
            public static uint get_pointer(int iProcIndex, string iPlayer, out uint oIndex, out uint oIndexAddress)
            {
                oIndex = 0;
                oIndexAddress = 0;
                Process proc = processPointerList[iProcIndex].MainProcess;
                uint curMapAddress = processPointerList[processIndex].Info_MapPcBegin;
                uint endMapAddress = processPointerList[processIndex].Info_MapPcEnd;
                LoggingFunctions.Debug("PCs.get_pointer: Looking for player '" + iPlayer + "'.", LoggingFunctions.DBG_SCOPE.MEMREADS);
                LoggingFunctions.Debug("PCs.get_pointer: PC Map resides between addresses " + string.Format("{0:X}", curMapAddress) + " and " + string.Format("{0:X}", endMapAddress) + ".", LoggingFunctions.DBG_SCOPE.MEMREADS);
                UIntPtr ptrAddress = (UIntPtr)0;
                bool found = false;
                while (!found && (curMapAddress <= endMapAddress))
                {
                    oIndex = (curMapAddress - processPointerList[processIndex].Info_MapPcBegin) / 4;
                    oIndexAddress = curMapAddress;
                    ptrAddress = MemoryFunctions.GetPointer(proc.Handle, curMapAddress, 0);
                    if (ptrAddress != (UIntPtr)0)
                    {
                        string tempName = MemoryFunctions.ReadString(proc.Handle, (uint)ptrAddress, (int)off_pc_map_name);
                        if (tempName.Length > 3)
                        {
                            tempName = tempName.Substring(0, tempName.Length - 1);
                        }
                        if (tempName == iPlayer)
                        {
                            found = true;
                        }
                        else
                        {
                            curMapAddress += 4;
                        }
                        if (found)
                        {
                            LoggingFunctions.Debug("PCs.get_pointer: Map pos: " + string.Format("{0:X}", curMapAddress) + "  Pointer: " + string.Format("{0:X}", (uint)ptrAddress) + "  Name: '" + tempName + "'.", LoggingFunctions.DBG_SCOPE.MEMREADS);
                            return (uint)ptrAddress;
                        }
                    }
                    else
                    {
                        curMapAddress += 4;
                    }
                }
                return (uint)0;
            }
            public static string get_name(uint iPointer)
            {
                return get_name(processIndex, iPointer);
            }
            public static string get_name(int iProcIndex, uint iPointer)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                string tempName = MemoryFunctions.ReadString(proc.Handle, iPointer, (int)off_pc_map_name);
                if (tempName.Length > 3)
                {
                    tempName = tempName.Substring(0, tempName.Length - 1);
                }
                return tempName;
            }
            public static float get_posx(string iPlayer, ref uint oPointer)
            {
                return get_posx(processIndex, iPlayer, ref oPointer);
            }
            public static float get_posx(int iProcIndex, string iPlayer, ref uint oPointer)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                oPointer = get_pointer(iProcIndex, iPlayer);
                return get_posx(iProcIndex, oPointer);
            }
            public static float get_posx(uint iPointer)
            {
                return get_posx(processIndex, iPointer);
            }
            public static float get_posx(int iProcIndex, uint iPointer)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                return MemoryFunctions.ReadMem(proc.Handle, (uint)iPointer, (int)off_pc_map_posx);
            }
            public static float get_posz(string iPlayer, ref uint oPointer)
            {
                return get_posz(processIndex, iPlayer, ref oPointer);
            }
            public static float get_posz(int iProcIndex, string iPlayer, ref uint oPointer)
            {
                oPointer = get_pointer(iProcIndex, iPlayer);
                return get_posz(iProcIndex, oPointer);
            }
            public static float get_posz(uint iPointer)
            {
                return get_posz(processIndex, iPointer);
            }
            public static float get_posz(int iProcIndex, uint iPointer)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                return MemoryFunctions.ReadMem(proc.Handle, (uint)iPointer, (int)off_pc_map_posz);
            }
            public static float get_posy(string iPlayer, ref uint oPointer)
            {
                return get_posy(processIndex, iPlayer, ref oPointer);
            }
            public static float get_posy(int iProcIndex, string iPlayer, ref uint oPointer)
            {
                oPointer = get_pointer(iProcIndex, iPlayer);
                return get_posy(iProcIndex, oPointer);
            }
            public static float get_posy(uint iPointer)
            {
                return get_posy(processIndex, iPointer);
            }
            public static float get_posy(int iProcIndex, uint iPointer)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                return MemoryFunctions.ReadMem(proc.Handle, (uint)iPointer, (int)off_pc_map_posy);
            }
            public static float get_posh(string iPlayer, ref uint oPointer)
            {
                return get_posh(processIndex, iPlayer, ref oPointer);
            }
            public static float get_posh(int iProcIndex, string iPlayer, ref uint oPointer)
            {
                oPointer = get_pointer(iProcIndex, iPlayer);
                return get_posh(iProcIndex, oPointer);
            }
            public static float get_posh(uint iPointer)
            {
                return get_posh(processIndex, iPointer);
            }
            public static float get_posh(int iProcIndex, uint iPointer)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                return MemoryFunctions.ReadMem(proc.Handle, (uint)iPointer, (int)off_pc_map_posh);
            }
            public static float get_distance(string iPlayer, ref uint oPointer)
            {
                return get_distance(processIndex, iPlayer, ref oPointer);
            }
            public static float get_distance(int iProcIndex, string iPlayer, ref uint oPointer)
            {
                oPointer = get_pointer(iProcIndex, iPlayer);
                return get_distance(iProcIndex, oPointer);
            }
            public static float get_distance(uint iPointer)
            {
                return get_distance(processIndex, iPointer);
            }
            public static float get_distance(int iProcIndex, uint iPointer)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                float temp = MemoryFunctions.ReadMem(proc.Handle, (uint)iPointer, (int)off_pc_map_dist);
                return (float)Math.Sqrt((double)temp);
            }
            public static byte get_hp_perc(string iPlayer, ref uint oPointer)
            {
                return get_hp_perc(processIndex, iPlayer, ref oPointer);
            }
            public static byte get_hp_perc(int iProcIndex, string iPlayer, ref uint oPointer)
            {
                oPointer = get_pointer(iProcIndex, iPlayer);
                return get_hp_perc(iProcIndex, oPointer);
            }
            public static byte get_hp_perc(uint iPointer)
            {
                return get_hp_perc(processIndex, iPointer);
            }
            public static byte get_hp_perc(int iProcIndex, uint iPointer)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                return (byte)MemoryFunctions.ReadMem(proc.Handle, iPointer, (int)off_pc_map_hp, 1);
            }
            public static byte get_status(string iPlayer, ref uint oPointer)
            {
                return get_status(processIndex, iPlayer, ref oPointer);
            }
            public static byte get_status(int iProcIndex, string iPlayer, ref uint oPointer)
            {
                oPointer = get_pointer(iProcIndex, iPlayer);
                return get_status(iProcIndex, oPointer);
            }
            public static byte get_status(uint iPointer)
            {
                return get_status(processIndex, iPointer);
            }
            public static byte get_status(int iProcIndex, uint iPointer)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                return (byte)MemoryFunctions.ReadMem(proc.Handle, iPointer, (int)off_pc_map_status, 1);
            }
            public static bool get_pointer_valid(uint iPointer)
            {
                return get_pointer_valid(processIndex, iPointer);
            }
            public static bool get_pointer_valid(int iProcIndex, uint iPointer)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                return ((int)MemoryFunctions.ReadMem(proc.Handle, iPointer, (int)off_pc_map_pos_ptr, 4) != 0);
            }
        }
        public static class NPCs
        {
            #region Structures
            public enum eType : byte 
            {
                Player = 0x0,
                NPC = 0x02,
                NPC3 = 0x3
            }

            public enum eRace : byte
            {
                NPC = 0x00,  
                Hume_Male = 0x01, 
                Hume_Female = 0x02, 
                Elvaan_Male = 0x03, 
                Elvan_Female = 0x04,
                TaruTaru_Male = 0x05,
                TaruTaru_Female = 0x06,
                Mihtra = 0x07,
                Galka = 0x08 
            }

            public enum eActive : byte
            {
                NotFound = 0,
                CharDrawn = 2, //NPC, PC, Trust, Non-mob
                NPCDrawn = 4,
                PCIdle = 10,
                PCFishing = 14,
                PCSitNotDrawn = 16,
                PCSitDrawing = 18,
                PCSitDrawn = 22,
                MobUnk = 32,
                MobDrawn = 34, //was 0x24
                CharNotDrawn = 64,
                NPCNotDrawn2 = 66,
                PCWarped = 72,
                MobNotDrawn = 96
            }

            public enum eStatus : byte
            {
                Standing = 0,
                Fighting = 1,
                Dead1 = 2,
                Dead2 = 3,
                Event = 4,
                Chocobo = 5,
                Healing = 33,
                FishBite = 38,
                Obtained = 39,
                RodBreak = 40,
                LineBreak = 41,
                LostCatch = 43,
                CatchMonster = 42,
                Synthing = 44,
                Sitting = 47,
                Fishing = 50,
                CastingRod = 56,
                FishOnHook = 57,
                CaughtFish = 58,
                ReelingIn = 62,
                SittingStool = 63,
                SittingChair = 64
            }

            public enum eInvisible : byte
            {
                Inactive = 1,
                GoingActive = 65,
                Active = 193
            }

            public enum eSneak : byte
            {
                Inactive = 0,
                Active = 1,
                Inactive1 = 2,
                Active1 = 3
            }

            // Should be LayoutKind.Explicit
            [StructLayout(LayoutKind.Sequential, Pack = 1)] public struct NPCInfoStruct
            {
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] private byte[] Padding1;    //   0
                public float PosX;                                                               //   4     4
                public float PosZ;                                                               //   8     8
                public float PosY;                                                               //  12     C
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] private byte[] Padding2;    //  16
                public float PosH;                                                               //  24    18
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] private byte[] Padding2a;   //  28
                public float DrawX;                                                              //  36
                public float DrawZ;                                                              //  40
                public float DrawY;                                                              //  44
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] private byte[] Padding2b;   //  48
                public float DrawAngle;                                                          //  56
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] private byte[] Padding3;    //  60
                public float UnkXValue;                                                          //  68
                public float UnkZValue;                                                          //  72
                public float UnkYValue;                                                          //  76
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)] private byte[] Padding4;   //  80
                public short ID;                                                                 // 116   74
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)] private byte[] Padding5;    // 118
                public uint TargetCode;                                                          // 120   78
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)] public byte[] Name;        // 124
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 88)] private byte[] Padding6;   // 148
                public byte HPP;                                                                 // 236   EC
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)] private byte[] Padding8;    // 237
                public eType Type;                                                               // 238
                public eRace Race;                                                               // 239
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)] private byte[] Padding9;   // 240
                public short Model;                                                              // 256
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)] private byte[] Padding10;  // 258
                public eActive Active;                                                           // 289  121
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)] private byte[] Padding11;   // 290
                public eInvisible Invisible;                                                     // 296  128
                public eSneak Sneak;                                                             // 297  129
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)] private byte[] Padding12;   // 298
                public byte Flag;                                                                // 299  12B
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)] private byte[] Padding13;   // 300
                public byte Charmed;                                                             // 303  12F
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 44)] private byte[] Padding14;  // 304
                public float Speed;                                                              // 348  15C
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)] private byte[] Padding15;  // 352
                public eStatus Status;                                                           // 364  16C
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 27)] private byte[] Padding16;  // 365
                public uint LastClaimedID;                                                       // 392  188
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 280)] private byte[] Padding17; // 393
                public short PetID;                                                              // 676  2A4
            }
            public const ushort InvalidNpcId = 0x00;
            #endregion Structures
            #region NPC Map Functions
            //Arcon:
            //Zone NPCs are between 0x00 and 0x400 and dynamic NPCs (pets, trusts, fellows, etc.) are between 0x700 and 0x800 iirc
            //PCs are 0x400-0x6ff?

            public static List<uint> get_NPCMap_Pointers()
            {
                List<uint> ptrList = new List<uint>();
                Process proc = processPointerList[processIndex].MainProcess;
                for (uint ii = processPointerList[processIndex].Info_MapNpcBegin; ii < processPointerList[processIndex].Info_MapNpcEnd; ii += 4)
                {
                    uint ptr = (uint)MemoryFunctions.GetPointer(proc.Handle, ii);
                    if(ptr != 0)
                    {
                        ptrList.Add(ptr);
                    }
                }
                return ptrList;
            }
            public static ushort get_myNPCIndex(string iName = "")
            {
                if (iName == "")
                {
                    iName = MemReads.Self.get_name();
                }
                List<ushort> idxList = get_NPCIndex(iName);
                if (idxList.Count <= 0)
                {
                    return 0;
                }
                else
                {
                    return idxList[0];
                }
            }
            public static List<NPCInfoStruct> get_NPCInfoStructList(bool iNoFilter = false, int iProcIndex = -1)
            {
                return get_NPCInfoStructList(get_NPCMap_Pointers(), iNoFilter, iProcIndex);
            }
            public static List<NPCInfoStruct> get_NPCInfoStructList(List<uint> iPtrList, bool iNoFilter = false, int iProcIndex = -1)
            {
                int procindex = processIndex;
                if (iProcIndex != -1)
                {
                    procindex = iProcIndex;
                }
                Process proc = processPointerList[procindex].MainProcess;
                List<NPCInfoStruct> infoList = new List<NPCInfoStruct>();
                List<eActive> filterList = new List<eActive>();
                if (!iNoFilter)
                {
                    filterList.Add(eActive.NotFound);
                    filterList.Add(eActive.MobNotDrawn);
                    filterList.Add(eActive.CharNotDrawn);
                    filterList.Add(eActive.MobUnk);
                    filterList.Add(eActive.PCWarped);
                }
                foreach (uint ptr in iPtrList)
                {
                    if(ptr != 0)
                    {
                        NPCInfoStruct npc = new NPCInfoStruct();
                        if(get_NPCInfoStruct(ref npc, ptr, iProcIndex))
                        {
                            if (!filterList.Contains(npc.Active))
                            {
                                infoList.Add(npc);
                            }
                        }
                    }
                }
                return infoList;
            }
            private static ushort get_NPCMap_count(int iProcIndex = -1)
            {
                //int procindex = processIndex;
                //if (iProcIndex != -1)
                //{
                //    procindex = iProcIndex;
                //}
                //Process proc = processPointerList[procindex].MainProcess;
                //ushort count = (ushort)MemoryFunctions.ReadMem(proc.Handle, processPointerList[procindex].Info_NPCMapCount, 0, 2);

                //return count;
                return 0x1fff / 4;
            }
            public static bool get_NPCInfoStruct(ref NPCInfoStruct NPCInfo, uint iNpcInfoPtr, int iProcIndex = -1)
            {
                int datasize = Marshal.SizeOf(typeof(NPCInfoStruct));
                byte[] buffer = new byte[datasize];

                int procindex = processIndex;
                if (iProcIndex != -1)
                {
                    procindex = iProcIndex;
                }
                Process proc = processPointerList[procindex].MainProcess;

                if (iNpcInfoPtr == 0)
                {
                    return false;
                }

                GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

                try
                {
                    if (MemoryFunctions.ReadBlock(proc.Handle, (uint)iNpcInfoPtr, buffer, (uint)datasize) == null)
                    {
                        return false;
                    }

                    NPCInfo = (NPCInfoStruct)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(NPCInfoStruct));
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    handle.Free();
                }
                return true;
            }
            public static bool get_NPCInfoStruct(ref NPCInfoStruct NPCInfo, int index, int iProcIndex = -1)
            {
                int procindex = processIndex;
                if (iProcIndex != -1)
                {
                    procindex = iProcIndex;
                }
                Process proc = processPointerList[procindex].MainProcess;

                UIntPtr npcInfoPtr = MemoryFunctions.GetPointer(proc.Handle, processPointerList[procindex].Info_MapNpcBegin, 4 * index);

                return get_NPCInfoStruct(ref NPCInfo, (uint)npcInfoPtr, iProcIndex);
            }
            public static List<ushort> get_NPCIndex(string name, int iProcIndex = -1)
            {
                List<ushort> indices = new List<ushort>();

                ushort countNPCMap = 0x1fff / 4;

                System.Text.ASCIIEncoding textEncoding = new System.Text.ASCIIEncoding();

                for (ushort arrayindex = 0; arrayindex < countNPCMap; arrayindex++)
                {
                    NPCInfoStruct data = new NPCInfoStruct();
                    try
                    {
                        if (get_NPCInfoStruct(ref data, arrayindex, iProcIndex) == true)
                        {
                            // This is quick and dirty, there are probably better methods.
                            string[] data_names = textEncoding.GetString(data.Name).Trim('\0').Split('\0');
                            if (data_names.Count() > 0 && data_names[0] == name)
                            {
                                indices.Add(arrayindex);
                            }
                        }
                    }
                    catch (NullReferenceException)
                    {

                    }
                }

                return indices;
            }
            public static string getName(NPCInfoStruct iInfo)
            {
                System.Text.ASCIIEncoding textEncoding = new System.Text.ASCIIEncoding();
                if(iInfo.Name == null)
                {
                    return "";
                }
                string[] data_names = textEncoding.GetString(iInfo.Name).Trim('\0').Split('\0');
                if (data_names.Count() > 0)
                {
                    return data_names[0];
                }
                return "";
            }
            /// <summary>
            /// Returns the ID of the NPC that is the closest to the player.
            /// </summary>
            /// <param name="name">Name of the NPC</param>
            /// <param name="iProcIndex">Optional process Id</param>
            /// <returns>KeyValuePair of the NPC index (0x00 if invalid) and the distance.</returns>
            public static KeyValuePair<ushort, double> get_NPCIndexClosest(string name, int iProcIndex = -1)
            {
                float playerPosX = MemReads.Self.Position.get_x();
                float playerPosY = MemReads.Self.Position.get_y();
                float playerPosZ = MemReads.Self.Position.get_z();

                Dictionary<ushort, double> npcdistances = new Dictionary<ushort, double>();

                ushort countNPCMap = get_NPCMap_count(iProcIndex);

                System.Text.ASCIIEncoding textEncoding = new System.Text.ASCIIEncoding();

                for (ushort arrayindex = 0; arrayindex < countNPCMap; arrayindex++)
                {
                    NPCInfoStruct data = new NPCInfoStruct();
                    try
                    {
                        if (get_NPCInfoStruct(ref data, arrayindex, iProcIndex) == true)
                        {
                            // This is quick and dirty, there are probably better methods.
                            string[] data_names = textEncoding.GetString(data.Name).Trim('\0').Split('\0');
                            if (data_names.Count() > 0 && data_names[0] == name)
                            {
                                // No need to do the square root if we want the closest one.
                                npcdistances[arrayindex] = Math.Pow(data.PosX - playerPosX, 2) + Math.Pow(data.PosY - playerPosY, 2) + Math.Pow(data.PosZ - playerPosZ, 2);
                            }
                        }
                    }
                    catch (NullReferenceException)
                    {

                    }
                }

                KeyValuePair<ushort, double> minimumDistance = new KeyValuePair<ushort, double>(0x00, 0x00);
                foreach (KeyValuePair<ushort, double> distance in npcdistances)
                {
                    if (minimumDistance.Key == 0x00 || distance.Value < minimumDistance.Value)
                    {
                        minimumDistance = distance;
                    }
                }

                return minimumDistance;
            }
            #endregion NPC Map Functions
        }
        public static class Environment
        {
            #region Server_info
            public static string get_server(string iPlayer)
            {
                return get_server(processIndex, iPlayer);
            }
            public static string get_server(int iProcIndex, string iPlayer)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                string tempPlayer;
                int offset = 0;
                do
                {
                    tempPlayer = MemoryFunctions.ReadString(proc.Handle, processPointerList[processIndex].Info_ServerList, offset);
                    if (tempPlayer.Length > 0)
                    {
                        tempPlayer = tempPlayer.Substring(0, tempPlayer.Length - 1);
                    }
                    offset += 140;
                }
                while ((tempPlayer != iPlayer) && (offset < 16 * 140));
                if (offset >= 16 * 140)
                {
                    return "None Found";
                }
                else
                {
                    string str = MemoryFunctions.ReadString(proc.Handle, processPointerList[processIndex].Info_ServerList, offset - 124);
                    return str.Substring(0, str.Length - 1);
                }
            }
            #endregion
            #region Time
            public static ulong get_time()
            {
                return get_time(processIndex);
            }
            public static ulong get_time(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                return (ulong)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Time, 0, 8);
            }
            #endregion Time
            #region Network
            //Receive
            public static ushort info_network_receive()
            {
                return info_network_receive(processIndex);
            }
            public static ushort info_network_receive(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Network, off_net_receive, 2);
            }
            //Send
            public static ushort info_network_send()
            {
                return info_network_send(processIndex);
            }
            public static ushort info_network_send(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Network, off_net_send, 2);
            }
            //Percentage
            public static ushort info_network_perc()
            {
                return info_network_perc(processIndex);
            }
            public static ushort info_network_perc(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                return (ushort)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Network, off_net_perc, 2);
            }
            #endregion Network
            #region Weather
            public static short get_weather_id()
            {
                return get_weather_id(processIndex);
            }
            public static short get_weather_id(int iProcIndex)
            {
                Process proc = processPointerList[iProcIndex].MainProcess;
                return (short)MemoryFunctions.ReadMem((IntPtr)proc.Handle, processPointerList[processIndex].Info_Player2, (int)off_weather, 2);
            }
            public static string get_weather_name()
            {
                return get_weather_name(processIndex);
            }
            public static string get_weather_name(int iProcIndex)
            {
                short weatherID = get_weather_id(iProcIndex);
                switch (weatherID)
                {
                    case (int)FFXIEnums.WEATHER.CLEAR:
                        return "Clear";
                    case (int)FFXIEnums.WEATHER.CLOUDS:
                        return "Cloudy";
                    case (int)FFXIEnums.WEATHER.DARK:
                        return "Dark";
                    case (int)FFXIEnums.WEATHER.DARK2:
                        return "Dark x2";
                    case (int)FFXIEnums.WEATHER.EARTH:
                        return "Earth";
                    case (int)FFXIEnums.WEATHER.EARTH_x2:
                        return "Earth x2";
                    case (int)FFXIEnums.WEATHER.FIRE:
                        return "Fire";
                    case (int)FFXIEnums.WEATHER.FIRE_x2:
                        return "Fire x2";
                    case (int)FFXIEnums.WEATHER.FOGGY:
                        return "Foggy";
                    case (int)FFXIEnums.WEATHER.ICE:
                        return "Snow";
                    case (int)FFXIEnums.WEATHER.ICE_x2:
                        return "Snow x2";
                    case (int)FFXIEnums.WEATHER.LIGHT:
                        return "Light";
                    case (int)FFXIEnums.WEATHER.LIGHT_x2:
                        return "Light x2";
                    case (int)FFXIEnums.WEATHER.SUNNY:
                        return "Sunny";
                    case (int)FFXIEnums.WEATHER.THUNDER:
                        return "Lightning";
                    case (int)FFXIEnums.WEATHER.THUNDER_x2:
                        return "Lightning x2";
                    case (int)FFXIEnums.WEATHER.WATER:
                        return "Rainy";
                    case (int)FFXIEnums.WEATHER.WATER_x2:
                        return "Rainy x2";
                    case (int)FFXIEnums.WEATHER.WIND:
                        return "Windy";
                    case (int)FFXIEnums.WEATHER.WIND_x2:
                        return "Windy x2";
                    default:
                        return "Unknown";
                }
            }
            #endregion Weather
        }
    }
}
